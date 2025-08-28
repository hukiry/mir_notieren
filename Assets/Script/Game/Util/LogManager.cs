using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace UnityEngine
{
    public class LogManager
	{
		/// <summary>
		/// 最大日志缓存
		/// </summary>
		private const int MAX_BUFF_SIZE = 1024 * 2;
		/// <summary>
		/// 缓存日志
		/// </summary>
		private static StringBuilder Buffer = new StringBuilder(MAX_BUFF_SIZE);
		/// <summary>
		/// 日志文件
		/// </summary>
		private static string LogFileName;
		/// <summary>
		/// 当前的文件操作
		/// </summary>
		private static StreamWriter Writer;
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="path"></param>
		/// <param name="logLevel"></param>

		public static void Init(string path)
		{
			if (Writer != null) return;
			LogManager.LogFileName = Path.Combine(path, "Log.txt");
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			LogManager.Writer = new StreamWriter(LogFileName);
			LogManager.RegisterLogCallBack(HandleLogCallback);
		}

		/// <summary>
		/// 应用退出
		/// </summary>

		public static void OnApplicationQuit()
		{
			LogManager.SaveLogFile("应用程序结束", true);
			LogManager.Writer?.Close();
			Application.logMessageReceived -= HandleLogCallback;
			AppDomain.CurrentDomain.UnhandledException -= HandleUnCatchException;
		}

		/// <summary>
		/// 注册日志监听
		/// </summary>
		public static void RegisterLogCallBack(Application.LogCallback callBack)
		{
			Application.logMessageReceived += callBack;
			AppDomain.CurrentDomain.UnhandledException += HandleUnCatchException;
		}

		/// <summary>
		/// 处理Debug打印出来的
		/// </summary>
		static void HandleLogCallback(string logString, string stackTrace, LogType type)
		{
			string logMsg = string.Format("{0}\r\n{1}", logString, stackTrace);
			LogManager.SaveLogFile(logMsg, type != LogType.Log);
		}

		/// <summary>
		/// 处理未处理的异常信息
		/// </summary>
		static void HandleUnCatchException(object sender, UnhandledExceptionEventArgs e)
		{
			LogManager.SaveLogFile(e.ToString(), true);
		}

		/// <summary>
		/// 保存日志
		/// </summary>
		public static void SaveLogFile(string msg, bool force = false)
		{
			Buffer.AppendFormat("{0}\r\n", msg);

			if (force || Buffer.Length > MAX_BUFF_SIZE)
			{
				LogManager.Writer?.Write(Buffer?.ToString());

				Buffer.Length = 0;

				LogManager.Writer?.Flush();
			}
		}

		public static void Log(params object[] message)
		{
#if !RELEASE
			if (message != null)
				{
					string currentMessage = string.Empty;
					for (int i = 0; i < message.Length; i++)
					{
						if (message[i] != null)
							currentMessage += message[i].ToString() + "\t";
					}
					UnityEngine.Debug.Log(currentMessage);
				}
#endif
		}

		public static void LogColor(string color, params object[] message)
		{
#if !RELEASE
			if (message != null)
				{
					string currentMessage = string.Empty;
					for (int i = 0; i < message.Length; i++)
					{
						if (message[i] != null)
							currentMessage += message[i].ToString() + "\t";
					}
					UnityEngine.Debug.Log("<color=" + color.ToLower() + ">" + currentMessage + "</color>");
				}
#endif
		}

		public static void LogObject(string color, UnityEngine.Object obj, params object[] message)
		{
#if UNITY_EDITOR
			if (message != null)
			{
				string currentMessage = string.Empty;
				for (int i = 0; i < message.Length; i++)
				{
					if (message[i] != null)
						currentMessage += message[i].ToString() + "\t";
				}
				UnityEngine.Debug.Log("<color=" + color.ToLower() + ">" + currentMessage + "</color>", obj);
			}
#endif
		}
		public static void LogError(params object[] message)
		{
			if (message != null)
			{
				string currentMessage = string.Empty;
				for (int i = 0; i < message.Length; i++)
				{
					if (message[i] != null)
						currentMessage += message[i].ToString() + "\t";
				}
				UnityEngine.Debug.LogError(currentMessage);
			}
		}

		public static void LogWarning(params object[] message)
		{
#if !RELEASE
			if (message != null)
			{
				string currentMessage = string.Empty;
				for (int i = 0; i < message.Length; i++)
				{
					if (message[i] != null)
						currentMessage += message[i].ToString() + "\t";
				}
				UnityEngine.Debug.LogWarning(currentMessage);
			}
#endif
		}

		public static void LogException(params object[] message)
		{
#if !RELEASE
			LogWarning(message);
#endif
		}

	}


#if UNITY_EDITOR
	public class ConsoleInformation
    {
        public static void GetCodeLineNum(string infoText)
        {
            StackTrace st = new StackTrace(true);
            StackFrame[] frams = st.GetFrames();
            string outString = string.Empty;
            for (int framsIndex = frams.Length - 1; framsIndex >= 0; framsIndex--)
            {
                StackFrame item = frams[framsIndex];
                MethodInfo methodInfo = item.GetMethod() as MethodInfo;
                outString += PrintConsole(frams.Length - framsIndex, item, methodInfo);
            }
            LogManager.LogColor("red", infoText,outString);
        }

        private static string PrintConsole(int framsIndex, StackFrame item, MethodInfo methodInfo)
        {
            string parameterString = GetMethodInfoString(methodInfo);
            string classNameAndLineNumber = GetClassNameAndLineNumber(item, methodInfo);

            string str = string.Format("{3}-> {0}\n    Function【{1}】\n    FileName {2} ",
                classNameAndLineNumber,
                parameterString,
                item.GetFileName(),
                framsIndex);

            return str + "\n";
        }

        private static string GetClassNameAndLineNumber(StackFrame item, MethodInfo methodInfo)
        {
            return string.Format("ClassName【{0}.cs】 LineNumber：{1}", methodInfo.ReflectedType.FullName, item.GetFileLineNumber());
        }

        private static string GetMethodInfoString(MethodInfo methodInfo)
        {
            //获取修饰符
            string[] strArray = methodInfo.Attributes.ToString().Split(',');
            string Modification = string.Empty;
            for (int i = 1; i < strArray.Length - 1; i++)
            {
                Modification += strArray[i].ToLower();
            }
            //获取方法返回类型和方法名称
            string methedName = string.Format("{0} {1}", methodInfo.ReturnType.Name.ToLower(), methodInfo.Name);

            //获取方法参数列表
            string parameterString = string.Empty;
            ParameterInfo[] parameterInfos = methodInfo.GetParameters();
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                ParameterInfo parameterInfo = parameterInfos[i];
                parameterString += string.Format("{0} {1}", parameterInfo.ParameterType.FullName,
                    parameterInfo.Name);
            }

            return string.Format("{0} {1}({2})", Modification, methedName, parameterString);
        }
    }
#endif
}

