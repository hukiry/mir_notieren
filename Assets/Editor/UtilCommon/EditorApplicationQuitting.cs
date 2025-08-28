using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using CustomApplication = UnityEditor.EditorApplication;
public class EditorApplicationQuitting
{
	public string filePath;
	public bool isAdd;
#if UNITY_2018_4_OR_NEWER
	private static readonly string QUIT_MESSAGE = "\u6057\u66d0\u54d9\u734f\u57d7\u90ff\u5105\u00aa\u0091\u0096\u008b" +
								"\u0086\u7fe9\u8f6e\u5697\u00df\ufffe\u00f5\u00f5" +
								"\u6bdd\u8f31\u6057\u5172\u6bde\u4f80\u75d7\u00df\ufffe";
	private static readonly string QUITTING_MESSAGE = "\u4f9f\u66d0\u54d9\u897e\u4f22\u5ba7\u8dbb\u6e6f\u00de" +
									"\u00f5\u00f5\u00aa\u0091\u0096\u008b\u0086\u6b9c\u57d7\u90ff\u5105\u4ed2\u00d1\u00d1\u00d1\u00de";
	private static readonly string CANCEL_BUTTON = "\u5329\u6d77";
	private static readonly string SAVE_BUTTON = "\u4f22\u5ba7";
	private static readonly string IGNORE_BUTTON = "\u5f02\u759a";
	private static readonly string QUIT_BUTTON = "\u90ff\u5105";
	private static readonly char MAX_CHAR = 'ÿ';
	[InitializeOnLoadMethod]
	private static void InitializeOnLoadMethod()
	{
		InitializeEvent();
	}

	private static void InitializeEvent()
	{
		CustomApplication.quitting -= Application_quitting;
		CustomApplication.quitting += Application_quitting;

		CustomApplication.wantsToQuit -= Application_wantsToQuit;
		CustomApplication.wantsToQuit += Application_wantsToQuit;
	}

	private static bool Application_wantsToQuit()
	{
		bool isQuit = EditorUtility.DisplayDialog("Unity3D " + Application.unityVersion,
			ParsedString(QUIT_MESSAGE),
			ParsedString(QUIT_BUTTON),
			ParsedString(CANCEL_BUTTON));

		return isQuit;
	}

	private static void Application_quitting()
	{
		//if (EditorUtility.DisplayDialogComplex("Unity3D " + Application.unityVersion, 
		//    ParsedString(QUITTING_MESSAGE), 
		//    ParsedString(SAVE_BUTTON),
		//    ParsedString(CANCEL_BUTTON),
		//    ParsedString(IGNORE_BUTTON)) == 0)
		//{
		//    CustomSceneManager.SaveOpenScenes();
		//    CustomSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		//}
	}
	private delegate string REALSTRINGFUNC(byte[] buffer);
	private static string ParsedString(string source)
	{
		Func<REALSTRINGFUNC> getStrFunc = () =>
		{
			Func<REALSTRINGFUNC> func = () =>
			{
				return bufferString =>
				{
					return Encoding.Unicode.GetString(bufferString);
				};
			};

			return buffer =>
			{
				for (int i = 0; i < buffer.Length; i += 2)
				{
					buffer[i] = (byte)(MAX_CHAR - buffer[i]);
				}
				return func().Invoke(buffer);
			};
		};
		return getStrFunc().Invoke(Encoding.Unicode.GetBytes(source));
	}
#endif
}