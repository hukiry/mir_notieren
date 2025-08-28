using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizedLanguage : MonoBehaviour
{
	private static Dictionary<MonoBehaviour, Action> mulList = new Dictionary<MonoBehaviour, Action>();

	public string id;
	public bool IsCharp = false;

	private Text uiText;
	void Awake()
	{
		Refresh();
	}

    private void Start(){}

    void OnEnable()
	{
		mulList[this] = this.Refresh;
		Refresh();
	}

    private void OnDisable()
    {
		if (mulList.ContainsKey(this))
		{
			mulList[this] = null;
			mulList.Remove(this);
		}
	}

    public void Refresh()
	{
		string result = id ?? string.Empty;

		if (IsCharp)
		{
			//result =GetText(id);
		}
		else
		{
			if (string.IsNullOrEmpty(id))
			{
				return;
			}
#if ENABLE_LUA
			result = LuaManager.instance?.GetLuaLanguageText(id);
#endif
		}

		if (!string.IsNullOrEmpty(id))
		{
			if (uiText == null)
			{
				uiText = GetComponent<Text>();
				if (uiText == null)
				{
					LogManager.LogError(this.gameObject.name);
					Destroy(this);
					return;
				}
			}
			uiText.text = string.IsNullOrEmpty(result) ? id.ToString() : result;
		}
	}

	public static void RefreshChangeLanguage()
	{
        foreach (var item in mulList)
        {
			if (item.Value != null)
			{
				item.Value();
			}
        }
	}
}
