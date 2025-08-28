using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
/*
 * Description:Auto add to Manager tag;
 * Author:hukiry
 * Date:2019-02-20
 * CSharpUIBuilder Template path:   Editor/UITemplate/C#_UI_Template.txt        save path:Script/UI/Generate/
 * LuaUIBuilder Template path:      Editor/UITemplate/Lua_UI_Template.txt       save path:E:\U_Projec\pc_main\LsjProduct\Client\LuaScript/Generate
 */

namespace Hukiry.Editor.Tool
{
	public class GenUITools
	{
		[MenuItem("Assets/Generate UI View")]
		public static void CreateUIView()
		{
			if (Selection.activeGameObject != null)
			{
				BuildViewClass(Selection.activeGameObject);
			}
			else
			{
				string[] ids = Selection.assetGUIDs;

				int index = 0;
				foreach (string id in ids)
				{
					string dir = AssetDatabase.GUIDToAssetPath(id);
					if (dir.IndexOf(".") < 0)
					{
						string root = Application.dataPath + dir.Substring(dir.IndexOf("/"));
						string[] files = Directory.GetFiles(root, "*.prefab", SearchOption.AllDirectories);
						if (files.Length > 0)
						{
							foreach (string file in files)
							{
								string path = file.Substring(file.IndexOf("Assets"));
								GameObject obj = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
								BuildViewClass(obj);
							}
						}
						index++;
						EditorUtility.DisplayProgressBar("Generate", dir, index / (float)ids.Length);

					}
				}
			}

			EditorUtility.ClearProgressBar();
			AssetDatabase.Refresh();
		}

		private static void BuildViewClass(GameObject go, bool isOnlyView = false, bool isOnlyControl = false)
		{
			if (null != go)
			{
				UIBuilder builder = BuildClassFactory(go);

				if (builder != null)
				{
					builder.GenerateUI();
					if (builder.IsSceneUITag())
					{
						SaveSceneConifgFile(builder);
					}
					else
					{
						SaveSingleConfigFile(builder, "Data", "DataMgr", true);
						SaveSingleConfigFile(builder, "Net", "ToServer");
						SaveUIViewConfigFile(builder);
					}
				}
			}
		}

		private static UIBuilder BuildClassFactory(GameObject go)
		{
			string rootTag = go.tag;
			if (rootTag.StartsWith("UI_Lua"))
			{
				return new LuaUIBuilder(go, ".lua");
			}
			else if (rootTag == TagLayer.UI_CSharp)
			{
				return new CSharpUIBuilder(go, ".cs");
			}
			else
			{
				throw new Exception("Please Define Prefab Tag !");
			}
		}


		private const string TemplateNet = @"local _{FileName}
---@return {FileName}
function {ConfigName}.{DirName}()
    if not _{FileName} then
        _{FileName} = require('Game.UI.{FilePath}')
    end
    return _{FileName}
end";

		private const string TemplateView = @"
---@return {FileName}
function {ConfigName}.{DirName}()
     return {ConfigName}.ImportClass('{DirName}','Game.UI.{FilePath}')
end";

		private const string templateHead = @"---@type table<string,table>
local list = {}
---导入lua
---@param className string
---@param classPath string
function {ConfigName}.ImportClass(className, classPath)
    if list[className] == nil then
        list[className] = require(classPath).New()
        list[className]:InitData()
    end
    return list[className]
end

---切换账号时登录
function {ConfigName}.InitData()
    for i, v in pairs(list) do
        v:InitData()
    end
end

---退出游戏时清空
function {ConfigName}.ClearData()
    list = {}
end";
		//数据配置，UI配置，网络配置
		private static void SaveSingleConfigFile(UIBuilder builder, string configFix, string fileNameFix, bool isViewData = false)
		{
			string TemplateExp = "";
			string configName = "Single" + configFix;
			List<string> content = new List<string>();
			content.Add("---");
			content.Add("---Update Time:" + UIBuilder.GetNowTimer());
			content.Add("---Author:Hukiry");
			content.Add("---");
			content.Add("");
			content.Add($"---@class {configName}");
			content.Add($"{configName} = " + "{}");
			content.Add("");
			if (isViewData)
			{
				TemplateExp = TemplateView;
				content.Add(templateHead.Replace("{ConfigName}", configName));
				content.Add("");
			}
			else
			{
				TemplateExp = TemplateNet;
			}

			string[] dirs = Directory.GetDirectories(builder.saveDirPath);
			foreach (var item in dirs)
			{
				DirectoryInfo di = new DirectoryInfo(item);
				string[] files = Directory.GetFiles(item, "*.lua", SearchOption.AllDirectories);
				foreach (var file in files)
				{
					string fileName = Path.GetFileNameWithoutExtension(file);
					if (fileName.EndsWith(fileNameFix))
					{
						string filePath = file.Replace('\\', '/').Replace("LuaScript/Game/UI/", "").Replace('/', '.');
						filePath = filePath.Substring(0, filePath.Length - 4);
						string template = TemplateExp.Replace("{FileName}", fileName);
						template = template.Replace("{ConfigName}", configName);
						template = template.Replace("{FilePath}", filePath);
						template = template.Replace("{DirName}", di.Name);
						content.Add(template);
						break;
					}
				}

			}
			builder.SaveConfigFile(content.ToArray(), configName);
			AssetDatabase.Refresh();
		}


		private static bool IsViewFix(string fileName)
		{
			return fileName.EndsWith("View") || fileName.EndsWith("Item") || fileName.EndsWith("Panel");
		}
		/// <summary>
		/// view,Panel, 后缀为item
		/// </summary>
		private static void SaveUIViewConfigFile(UIBuilder builder)
		{
			string configName = "UIConfigView";
			List<string> content = new List<string>();
			content.Add("---");
			content.Add("---Update Time:" + UIBuilder.GetNowDay());
			content.Add("---Author:Hukiry");
			content.Add("---");
			content.Add("");

			List<string> ViewID = new List<string>();
			ViewID.Add("---@class ViewID");
			ViewID.Add("ViewID = {");
			List<string> UIConfigView = new List<string>();
			UIConfigView.Add("---@class UIConfigView");
			UIConfigView.Add("UIConfigView = {");

			List<string> UIItemType = new List<string>();
			UIItemType.Add("---@class UIItemType");
			UIItemType.Add("UIItemType = {");
			List<string> ItemPoolRule = new List<string>();
			ItemPoolRule.Add("---@class ItemPoolRule");
			ItemPoolRule.Add("ItemPoolRule = {");

			List<string> UIPanelType = new List<string>();
			UIPanelType.Add("---UI面板类型");
			UIPanelType.Add("---@class UIPanelType");
			UIPanelType.Add("UIPanelType = {");
			List<string> UIPanelRule = new List<string>();
			UIPanelRule.Add("---切换页签动态加载的面板");
			UIPanelRule.Add("---@class UIPanelRule");
			UIPanelRule.Add("UIPanelRule = {");


			string[] dirs = Directory.GetDirectories(builder.saveDirPath);
			foreach (var item in dirs)
			{
				DirectoryInfo di = new DirectoryInfo(item);
				string[] files = Directory.GetFiles(item, "*.lua", SearchOption.AllDirectories);
				foreach (var file in files)
				{
					string fileName = Path.GetFileNameWithoutExtension(file);
					///配置对话框，和窗口
					if (IsViewFix(fileName) && File.Exists(file))
					{
						string scriptPath = file.Replace('\\', '/').Replace("LuaScript/", "");
						scriptPath = scriptPath.Substring(0, scriptPath.Length - 4);
						string viewIDName = fileName.EndsWith("Panel") ? fileName.Substring(0, fileName.Length - 5) : fileName.Substring(0, fileName.Length - 4);
						string resPath = Hukiry.HukiryUtilEditor.FindAssetPath<GameObject>(fileName);
						if (string.IsNullOrEmpty(resPath)) continue;
						resPath = resPath.Replace('\\', '/').Replace("Assets/ResourcesEx/", "");
						resPath = resPath.Substring(0, resPath.Length - ".prefab".Length);

						if (fileName.EndsWith("Item"))//配置Item
						{
							UIItemType.Add($"	---@type {fileName}");
							UIItemType.Add($"	{fileName} = {(UIItemType.Count - 1) / 2},");
							ItemPoolRule.Add($"	[UIItemType.{fileName}] = " + "{" +
								$" itemClass = \"{scriptPath}\", resPath = \"{resPath}\"" + "},");
						}
						else if (fileName.EndsWith("Panel"))
						{
							UIPanelType.Add($"	---@type {fileName}");
							UIPanelType.Add($"	{fileName} = {(UIPanelType.Count - 1) / 2},");
							UIPanelRule.Add($"	[UIPanelType.{fileName}] = " + "{" +
								$" itemClass = \"{scriptPath}\", resPath = \"{resPath}\"" + "},");
						}
						else
						{
							ViewID.Add($"	---@type {fileName}");
							ViewID.Add($"	{viewIDName} = {(ViewID.Count - 1) / 2},");
							UIConfigView.Add($"	[ViewID.{viewIDName}] = " + "{" +
								$" classPath = \"{scriptPath}\", resPath = \"{resPath}\"" + "},");
						}
					}
				}
			}

			ViewID.Add("}\n");
			UIConfigView.Add("}\n");
			UIItemType.Add("}\n");
			ItemPoolRule.Add("}\n");
			UIPanelType.Add("}\n");
			UIPanelRule.Add("}\n");

			content.AddRange(ViewID);
			content.AddRange(UIConfigView);
			content.AddRange(UIItemType);
			content.AddRange(ItemPoolRule);
			content.AddRange(UIPanelType);
			content.AddRange(UIPanelRule);

			builder.SaveConfigFile(content.ToArray(), configName);
			AssetDatabase.Refresh();
		}

		private static void SaveSceneConifgFile(UIBuilder builder)
		{
			string configName = "ScenePoolRule";
			List<string> content = new List<string>();
			content.Add("---");
			content.Add("---Update Time:" + UIBuilder.GetNowDay());
			content.Add("---Author:Hukiry");
			content.Add("---");
			content.Add("");

			List<string> UIItemType = new List<string>();
			UIItemType.Add("---@class SceneItemType");
			UIItemType.Add("SceneItemType = {");
			List<string> ItemPoolRule = new List<string>();
			ItemPoolRule.Add("---@class ScenePoolRule");
			ItemPoolRule.Add("ScenePoolRule = {");



			string[] dirs = Directory.GetDirectories(builder.saveDirPath);
			foreach (var item in dirs)
			{
				DirectoryInfo di = new DirectoryInfo(item);
				string[] files = Directory.GetFiles(item, "*.lua", SearchOption.AllDirectories);
				foreach (var file in files)
				{
					string fileName = Path.GetFileNameWithoutExtension(file);
					///配置对话框，和窗口
					if (IsViewFix(fileName) && File.Exists(file))
					{
						string scriptPath = file.Replace('\\', '/').Replace("LuaScript/", "");
						scriptPath = scriptPath.Substring(0, scriptPath.Length - 4);
						string resPath = Hukiry.HukiryUtilEditor.FindAssetPath<GameObject>(fileName);
						if (string.IsNullOrEmpty(resPath)) continue;
						resPath = resPath.Replace('\\', '/').Replace("Assets/ResourcesEx/", "");
						resPath = resPath.Substring(0, resPath.Length - ".prefab".Length);

						if (fileName.EndsWith("Item"))//配置Item
						{
							UIItemType.Add($"	---@type {fileName}");
							UIItemType.Add($"	{fileName} = {(UIItemType.Count - 1) / 2},");
							ItemPoolRule.Add($"	[SceneItemType.{fileName}] = " + "{" +
								$" luaClass = \"{scriptPath}\", resPath = \"{resPath}\"" + "},");
						}
					}
				}
			}

			UIItemType.Add("}\n");
			ItemPoolRule.Add("}\n");

			content.AddRange(UIItemType);
			content.AddRange(ItemPoolRule);

			string fileName2 = Path.Combine(UIPathConfig.ScenePoolPath, configName + ".lua");

			if (!Directory.Exists(UIPathConfig.ScenePoolPath))
			{
				Directory.CreateDirectory(UIPathConfig.ScenePoolPath);
			}

			File.WriteAllLines(fileName2, content);
			AssetDatabase.Refresh();
		}
	}
}
