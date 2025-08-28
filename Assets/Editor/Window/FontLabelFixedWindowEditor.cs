using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FontLabelFixedWindowEditor : ResourcesWindowEditor<FontLabelFixedWindowEditor>
{
	private const string AssetsMenu = "Hukiry/Window/Open FontLabelChange Editor";
	private const string LsjMenu = "Hukiry/Window//Open FontLabelChange Editor";
	private const string WindowMenu = "Hukiry/Window/Open FontLabelChange Editor";
	public class FontData : BaseData
	{
		public Font mTrueTypeFont;
		public SelectFixType select;

	}
	public enum SelectFixType
	{
		全部, 可以选择
	}

    //[MenuItem(AssetsMenu, false, -1)]
    //[MenuItem(LsjMenu, false, -1)]
    //[MenuItem(WindowMenu, false, -1)]
    public static void OpenAudioEditor()
	{
		_Instance.OpenWindowEditor<FontData>(Selection.assetGUIDs, new FontData());
	}

	public override string ExcuteData(BaseData data, EditorBaseData filePath)
	{
		var go = AssetDatabase.LoadAssetAtPath<GameObject>(filePath.filePath);
		SetUIlabelFont(go, (FontData)data);
		return filePath.filePath;
	}

	private void SetUIlabelFont(GameObject go, FontData d)
	{
		//var uicom = go.GetComponent<UILabel>();
		//if (uicom) uicom.trueTypeFont = d.mTrueTypeFont;

		//for (int i = 0; i < go.transform.childCount; i++)
		//{
		//    SetUIlabelFont(go.transform.GetChild(i).gameObject , d);
		//}
	}

	public override bool FileFilterExtension(string extName)
	{
		return extName == ".prefab";
	}

	public override string DrawGUI(BaseData data)
	{
		var fontData = data as FontData;
		fontData = Hukiry.Editor.HukiryGUIEditor.DrawType(fontData);
		return "OK";
	}

	public override string WindowTitileName()
	{
		return typeof(FontLabelFixedWindowEditor).Name;
	}
}

