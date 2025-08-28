#if UNITY_EDITOR

using Hukiry;
using System;
using UnityEngine;

public class InputTest : MonoBehaviour
{
	public static void Initialize() => DontDestroyOnLoad(new GameObject(nameof(InputTest), typeof(InputTest)));

	KeyCode[] keyCodes;
	private int length;
	void Start()
	{
		var enumNames = typeof(KeyCode).GetEnumNames();
		keyCodes = new KeyCode[enumNames.Length];
		for (int i = 0; i < keyCodes.Length; i++)
		{
			if (Enum.TryParse(enumNames[i], out KeyCode keyCode))
			{
				if ((int)keyCode < 300)
				{
					keyCodes[i] = keyCode;
				}
			}
		}

        for (KeyCode code = KeyCode.Alpha0; code <= KeyCode.F12; code++)
        {
			if ((code >= KeyCode.F1 && code <= KeyCode.F12) ||
			(code >= KeyCode.Alpha0 && code <= KeyCode.Alpha9))
			{
				HukiryEventDispatch.Instance.RegisterEvent(code, CallLua);
			}
		}

		length = keyCodes.Length;
	}

	void CallLua(object[] objs)
	{
		KeyCode key = (KeyCode)objs[0];
#if ENABLE_LUA
		LuaManager.instance.CallFunction("Test_" + key.ToString());
#endif
	}

	// Update is called once per frame
	void Update()
	{
		for (int i = 0; i < length; i++)
		{
			if (Input.GetKeyDown(keyCodes[i]))
			{
				HukiryEventDispatch.Instance.FireEvent(keyCodes[i], keyCodes[i]);
			}
		}
	}
}
#endif
