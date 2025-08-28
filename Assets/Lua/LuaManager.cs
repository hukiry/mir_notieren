using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System;
using UnityEngine.SceneManagement;
using System.IO;
using Hukiry.Socket;
using Hukiry.Http;
using Hukiry;

public class LuaManager:MonoBehaviour
{
	private static LuaManager m_Instance;
	public static LuaManager instance
	{
		get { return m_Instance; }
	}

	private LuaState m_LuaState;
	private LuaFunction m_OnApplicationPause;
	private LuaFunction m_OnApplicationQuit;
	private LuaTable m_luaMainTable;
	protected LuaLooper loop = null;
	private LuaFunction debug_traceback;


	/// <summary>
	/// lua脚本存储器
	/// </summary>
	public Dictionary<string, byte[]> dicLuaScripts = new Dictionary<string, byte[]>();

	//ab文件
	protected Dictionary<string, AssetBundle> luaABdic = new Dictionary<string, AssetBundle>();

	public void Awake()
	{
		m_Instance = this;
		
	}

	public IEnumerator InitLuaFile()
	{
#if !UNITY_EDITOR || ASSETBUNDLE_TEST
		int len = AssetBundleConifg.DirLuaNames.Length;
		for (int i = 0; i < len; i++)
		{
			yield return AddBundle($"{AssetBundleConifg.LuaOutFile}/{AssetBundleConifg.DirLuaNames[i]}.ab");
		}
#endif
		m_LuaState = new LuaState();
		OpenLibs();//打开库
		m_LuaState.LuaSetTop(0);
		LuaBinder.Bind(m_LuaState);//将导出的C#，注册到lua中
		DelegateFactory.Init();//委托方法的注册
		LuaCoroutine.Register(m_LuaState, this);//协程注册
		m_LuaState.Start();

		//时间添加 tolua/event.lua > function Update(deltaTime, unscaledDeltaTime)
		loop = gameObject.AddComponent<LuaLooper>();
		loop.luaState = m_LuaState;

		 //配置文件引用所有的类UnityEngine.GameObject
		 m_LuaState.DoFile("Game.Main");
		m_luaMainTable = m_LuaState.GetTable("Main");
		//m_luaMainTable?.GetLuaFunction("OnApplicationPause");
		m_OnApplicationPause = m_LuaState.GetFunction("Main.OnApplicationPause");
		m_OnApplicationQuit = m_LuaState.GetFunction("Main.OnApplicationQuit");
		yield return null;
	}

	public void EnableGame()
	{
		LogManager.Log("开始 lua 脚本...");
        //Logger
        m_Instance?.CallLuaFunction("Main", "Start", "");//开始Lua脚本代码
    }

	private void OnDestroy()
	{
		if (m_LuaState != null)
		{
			m_luaMainTable?.GetLuaFunction("OnApplicationQuit")?.Call();
			DetachProfiler();
			LuaState state = m_LuaState;
			m_LuaState = null;

			if (loop != null)
			{
				loop.Destroy();
				loop = null;
			}

			if (debug_traceback != null)
			{
				debug_traceback.Dispose();
				debug_traceback = null;
			}

			state.Dispose();
			m_Instance = null;
		}
	}

    public void ApplicationPause(bool pause, int seconds)
    {
		m_OnApplicationPause?.Call(m_luaMainTable, pause, seconds);
	}

	public void ApplicationQuit()
	{
		m_OnApplicationQuit?.Call(m_luaMainTable);
	}

	private void OpenLibs()
	{
		m_LuaState.OpenLibs(LuaDLL.luaopen_pb);
		m_LuaState.OpenLibs(LuaDLL.luaopen_struct);
		m_LuaState.OpenLibs(LuaDLL.luaopen_lpeg);

		OpenCJson();

		if (LuaConst.openLuaSocket)
		{
			OpenLuaSocket();
		}

#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
        luaState.OpenLibs(LuaDLL.luaopen_bit);
#endif
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LuaOpen_Socket_Core(IntPtr L)
	{
		return LuaDLL.luaopen_socket_core(L);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LuaOpen_Mime_Core(IntPtr L)
	{
		return LuaDLL.luaopen_mime_core(L);
	}

	protected void OpenCJson()
	{
		m_LuaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
		m_LuaState.OpenLibs(LuaDLL.luaopen_cjson);
		m_LuaState.LuaSetField(-2, "cjson");

		m_LuaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
		m_LuaState.LuaSetField(-2, "cjson.safe");
	}

	protected void OpenLuaSocket()
	{
		LuaConst.openLuaSocket = true;

		m_LuaState.BeginPreLoad();
		m_LuaState.RegFunction("socket.core", LuaOpen_Socket_Core);
		m_LuaState.RegFunction("mime.core", LuaOpen_Mime_Core);
		m_LuaState.EndPreLoad();
	}

	LuaTable profiler = null;

	public void AttachProfiler()
	{
		if (profiler == null)
		{
			profiler = m_LuaState.Require<LuaTable>("UnityEngine.Profiler");
			profiler.Call("start", profiler);
		}
	}
	public void DetachProfiler()
	{
		if (profiler != null)
		{
			profiler.Call("stop", profiler);
			profiler.Dispose();
			LuaProfiler.Clear();
		}
	}



	public string GetDebugTraceback()
	{
		if(debug_traceback==null)
			debug_traceback = m_LuaState.GetFunction("debug.traceback");

		return debug_traceback?.Invoke<string>();
	}



	public LuaFunction GetFunction(string funName)
	{
		return m_LuaState.GetFunction(funName);
	}

	public void CallLuaFunction<T>(string className,string funcName, params T[] objs) where T :class/* where K : class*/
	{
		LuaTable table =m_LuaState.GetTable(className);
		if (table != null)
		{
			LuaFunction func = table.GetLuaFunction(funcName);
			if (func != null)
			{
				if (objs != null&& objs.Length>0)
				{
					(T t1,T t2, T t3, T t4, T t5) = (default, default, default, default, default);
					if (objs.Length >= 5)	   (t1, t2, t3, t4, t5) = (objs[0], objs[1], objs[2], objs[3], objs[4]);
					else if (objs.Length == 4) (t1, t2, t3, t4) = (objs[0], objs[1], objs[2], objs[3]);
					else if (objs.Length == 3) (t1, t2, t3) = (objs[0], objs[1], objs[2]);
					else if (objs.Length == 2) (t1, t2) = (objs[0], objs[1]);
					else t1 = objs[0];
					func.Call(table, t1, t2, t3, t4, t5);
				}
				else
				{
					func.Call(table);
				}
				func.Dispose();
				func = null;
			}
			else
			{
				LogManager.LogError($"在此{className}类中找不到：{funcName}");
			}
		}
		else {
			LogManager.LogError("Lua类名找不到：", className);
		}
	}


	public string GetLuaLanguageText(string id)
	{
		int.TryParse(id, out int result);
		return GetLuaLanguageText(result);
	}

	public string GetLuaLanguageText(int result)
	{
		return CallFunction<int, string>("GetLanguageText", result);
	}


	#region Lua 初始化，加载，释放
	//获取lua数据
	public byte[] GetLuaFile(string fileShortPath)
	{
		
		byte[] buffer = null;
#if !UNITY_EDITOR || ASSETBUNDLE_TEST
		fileShortPath = fileShortPath.Replace('\\', '/').ToLower();
		TextAsset luaCode = GetTextAsset(fileShortPath);
		if (luaCode != null)
		{
			buffer = luaCode.bytes;
			Resources.UnloadAsset(luaCode);
		}
		else
		{
			LogManager.Log(fileShortPath + "============================");
		}
#else
		//编辑模式
		string luaPath = LuaFileUtils.Instance.FindFile(fileShortPath);
		if (File.Exists(luaPath))
		{
			buffer = File.ReadAllBytes(luaPath);
		}
		else
		{
			LogManager.LogColor("red", "编辑模式:", fileShortPath);
		}
#endif

		return buffer;
	}


	private TextAsset GetTextAsset(string fileShortPath)
	{
		if (fileShortPath.EndsWith(".lua")) fileShortPath = fileShortPath.Replace(".lua", ".bytes");
		else fileShortPath = fileShortPath + ".bytes";

		foreach (var zipFile in luaABdic.Values)
		{
			string shortPath = $"Assets/{AssetBundleConifg.LuaOutFile}/" + fileShortPath;
			TextAsset luaCode = zipFile.LoadAsset<TextAsset>(shortPath);
			if (luaCode == null)
			{ 		
				int len = AssetBundleConifg.DirLuaNames.Length;
				for (int i = 0; i < len; i++)
				{
					shortPath = $"Assets/{AssetBundleConifg.LuaOutFile}/{AssetBundleConifg.DirLuaNames[i]}/" + fileShortPath;
					luaCode = zipFile.LoadAsset<TextAsset>(shortPath);
					if (luaCode != null)
					{
						break;
					}
				}

			}

			if (luaCode != null)
			{
				return luaCode;
			}
			
		}
		LogManager.LogColor("yellow", fileShortPath);
		return null;
	}

	//添加ab文件
	private IEnumerator AddBundle(string bundleName)
	{
		//缓存路径加载
		string cachePath =	Path.Combine(AssetBundleConifg.AppCachePath, bundleName.ToLower());
		if (File.Exists(cachePath))
		{
            try
            {
                var buffer = File.ReadAllBytes(cachePath);
                var bytes = Hukiry.HukiryUtil.CodeByte(buffer);
                AssetBundle bundle = AssetBundle.LoadFromMemory(bytes);
                if (bundle != null)
                {
                    luaABdic[bundleName.ToLower()] = bundle;
                    LogManager.Log("====缓存路径，添加成功 add bundle======" + bundleName + "    : " + bundle.ToString());
                }
            }
            catch (Exception ex)
            {
				LogManager.Log(cachePath,"====异常 add fail======" + ex.ToString() );
			}
			yield return 0;
		}
		else
		{
			//包内路径加载
			string filepath = AssetBundleConifg.WWWPrefix + Path.Combine(Application.streamingAssetsPath, $"{AssetBundleConifg.AbFile}/{bundleName.ToLower()}");
			HttpLocalRequest uwrGet = new HttpLocalRequest(filepath, (isSuccess, msg, buffer) =>
			{
				if (isSuccess)
				{
					var bytes = Hukiry.HukiryUtil.CodeByte(buffer);
					AssetBundle bundle = AssetBundle.LoadFromMemory(bytes);
					if (bundle != null)
					{
						luaABdic[bundleName.ToLower()] = bundle;
						LogManager.Log("===包内，添加成功 add bundle======" + bundleName + "    : " + bundle.ToString());
					}
				}
				else
				{
					LogManager.Log("====包内，加载本地失败 add bundle======" + bundleName + "    : ");
				}
			});
		}
		yield return 0;
    }


	#endregion

	#region 无返回值的泛型函数
	public void CallFunction(string className, string funcName)
	{
		LuaTable table = m_LuaState.GetTable(className);
		if (table != null)
		{
			LuaFunction func = table.GetLuaFunction(funcName);
			if (func != null)
			{
				func.Call(table);
				func.Dispose();
				func = null;
			}
			else
			{
				LogManager.LogError($"在此{className}类中找不到：{funcName}");
			}
		}
		else
		{
			LogManager.LogError("Lua类名找不到：", className);
		}
	}

	public void CallFunction<T1, T2, T3>(string classDotFuncName, T1 t1, T2 t2, T3 t3, bool isLogMiss = true)
	{
		m_LuaState.Call(classDotFuncName, t1, t2, t3, isLogMiss);
	}
	public void CallFunction<T1, T2>(string classDotFuncName, T1 t1, T2 t2, bool isLogMiss = true)
	{
		m_LuaState.Call(classDotFuncName, t1, t2, isLogMiss);
	}
	public void CallFunction<T1>(string classDotFuncName, T1 t1, bool isLogMiss = true)
	{
		m_LuaState.Call(classDotFuncName, t1, isLogMiss);
	}
	public void CallFunction(string classDotFuncName, bool isLogMiss = true)
	{
		m_LuaState.Call(classDotFuncName, isLogMiss);
	}

	public Result CallFunction<T1, T2, T3, Result>(string classDotFuncName, T1 t1, T2 t2, T3 t3, bool isLogMiss = true)
	{
		return m_LuaState.Invoke<T1, T2, T3, Result>(classDotFuncName, t1, t2, t3, isLogMiss);
	}
	public Result CallFunction<T1, T2, Result>(string classDotFuncName, T1 t1, T2 t2, bool isLogMiss = true)
	{
		return m_LuaState.Invoke<T1, T2, Result>(classDotFuncName, t1, t2, isLogMiss);
	}
	public Result CallFunction<T, Result>(string classDotFuncName, T paramT, bool isLogMiss = true)
	{
		return m_LuaState.Invoke<T, Result>(classDotFuncName, paramT, isLogMiss);
	}

	public Result CallFunction<Result>(string classDotFuncName, bool isLogMiss = true)
	{
		return m_LuaState.Invoke<Result>(classDotFuncName, isLogMiss);
	}
#endregion
}
