using Hukiry.UI;
using System;
using System.Text;
using UnityEngine;

namespace Hukiry.Editor.Tool
{
    public class LuaUIBuilder : UIBuilder
	{
		//数据模板
		private const string Lua_InfoTemplate = "Lua_InfoTemplate";
		private const string Lua_DataTemplate = "Lua_DataTemplate";
		//网络模板
		private const string Lua_NetToServerTemplate = "Lua_NetToServerTemplate";
		private const string Lua_NetToClientTemplate = "Lua_NetToClientTemplate";

		//视图模板
		private const string Lua_ControlTemplate = "Lua_ControlTemplate";
		private const string Lua_ViewTemplate = "Lua_ViewTemplate";
		//panel视图模板
		private const string Lua_PanelTemplate = "Lua_PanelTemplate";
		private const string Lua_PanelControlTemplate = "Lua_PanelControlTemplate";

		//item 视图控制
		private const string Lua_ItemTemplate = "Lua_ItemTemplate";
		private const string Lua_ItemControlTemplate = "Lua_ItemControlTemplate";

		public LuaUIBuilder(GameObject go, string ext) : base(go, ext)
		{
			SetSavePath(UIPathConfig.UIViewPath);
		}

		public override void GenerateUI()
		{
			StringBuilder init = new StringBuilder();
			foreach (Transform tran in GetChildrenTransform())
			{
				string name = (tran.name.Substring(0, 1).ToLower() + tran.name.Substring(1));
				string tag = tran.gameObject.tag;
				if (CheckTag(tag))
				{
					if (name.Contains(" "))
					{
						throw new Exception(name + " Contains Space! Please Check!");
					}

					if (TagLayer.UI_Control.Contains(tag))
					{
						if (tag == typeof(GameObject).Name)
						{
							init.Append("	---@type UnityEngine.GameObject\n");
							init.Append($"	self.{name}Go = self.transform:Find(\"{GetHierarchy(tran)}\").gameObject\n");
						}
						else if (tag == typeof(Transform).Name)
						{
							init.Append("	---@type UnityEngine.Transform\n");
							init.Append($"	self.{name}TF = self.transform:Find(\"{GetHierarchy(tran)}\")\n");
						}
						else
						{
							if (tag == typeof(HukirySupperText).Name)//自定义类
							{
								init.Append("	---@type Hukiry.HukirySupperText\n");
							}
							else if (tag == typeof(AtlasImage).Name)//自定义类
							{
								init.Append("	---@type Hukiry.UI.AtlasImage\n");
							}
							else if (tag == typeof(UIProgressbarMask).Name)//自定义类
							{
								init.Append("	---@type Hukiry.UI.UIProgressbarMask\n");
							}
							else if (tag == typeof(SpriteRenderer).Name)//自定义类
							{
								init.Append("	---@type UnityEngine.SpriteRenderer\n");
							}
							else if (tag == typeof(CanvasGroup).Name)//自定义类
							{
								init.Append("	---@type UnityEngine.CanvasGroup\n");
							}
							else if (tag == typeof(MeshGraphic).Name)//自定义类
							{
								init.Append("	---@type Hukiry.UI.MeshGraphic\n");
							}
							else if (tag == typeof(UnityEngine.UI.Dropdown).Name)//自定义类
							{
								init.Append("	---@type Hukiry.UI.UIDropdown\n");
								init.Append($"	self.{name} = self.transform:Find(\"{GetHierarchy(tran)}\"):GetComponent(\"UIDropdown\")\n");
								goto DROP_DOWN;
							}
							else
							{
								init.Append($"	---@type UnityEngine.UI.{tag}\n");
							}
							init.Append($"	self.{name} = self.transform:Find(\"{GetHierarchy(tran)}\"):GetComponent(\"{tag}\")\n");
							DROP_DOWN:;
						}
					}
				}
			}

			string prefabName = GetUIClassName();
			if (this.IsSceneUITag())//保存场景Item
			{
				SetSavePath("LuaScript/Game/Scene");
				this.SaveItemTemplate(prefabName, "ItemControl", Lua_ItemControlTemplate, init, true);
				this.SaveItemTemplate(prefabName, "Item", Lua_ItemTemplate);
			}
			else if (IsItemTag())//保存Item
			{
				this.SaveItemTemplate(prefabName, "ItemControl", Lua_ItemControlTemplate, init, true);
				this.SaveItemTemplate(prefabName, "Item", Lua_ItemTemplate);
			}
			else if (IsItemAndDataTag())//保存Item和数据
			{
				this.SaveItemTemplate(prefabName, "ItemControl", Lua_ItemControlTemplate, init, true);
				this.SaveItemTemplate(prefabName, "Item", Lua_ItemTemplate);
				this.Save(prefabName, "Info", Lua_InfoTemplate, "Data");
			}
			else if (IsLuaViewTag() || IsPanelTag())//保存View和Panel
			{

				if (IsLuaViewTag())
				{
					this.Save(prefabName, "Control", Lua_ControlTemplate, init, true);
					this.Save(prefabName, "View", Lua_ViewTemplate);
				}
				else
				{
					this.Save(prefabName, "PanelControl", Lua_PanelControlTemplate, init, true);
					this.Save(prefabName, "Panel", Lua_PanelTemplate);
				}
			}
			else if (IsLuaViewAndDataTag())//保存View和数据
			{
				this.Save(prefabName, "Control", Lua_ControlTemplate, init, true);
				this.Save(prefabName, "View", Lua_ViewTemplate);
				this.Save(prefabName, "DataMgr", Lua_DataTemplate);
			}
			else//保存视图全部（视图+数据+网络）
			{
				this.Save(prefabName, "Control", Lua_ControlTemplate, init, true);
				this.Save(prefabName, "View", Lua_ViewTemplate);
				this.Save(prefabName, "DataMgr", Lua_DataTemplate);
				if (LuaTemplateAsset.Instance.isExportNet)
				{
					this.Save(prefabName, "ToServer", Lua_NetToServerTemplate, "Net");
					this.Save(prefabName, "ToClient", Lua_NetToClientTemplate, "Net");
				}
			}

			UnityEditor.AssetDatabase.Refresh();
		}

		//保存View和Panel
		private void Save(string prefabName, string postfix, string templateName, StringBuilder initStr = null, bool isControl = false)
		{
			string content = ReadTemplateString(Hukiry.HukiryUtilEditor.FindAssetPath<TextAsset>(templateName));
			content = content.Replace("{#class#}", prefabName);
			content = content.Replace("{#Author#}", "Hukiry");
			content = content.Replace("{#dirName#}", this.parentDirName);
			content = content.Replace("{#DateTime#}", GetNowTimer());
			if (isControl)
			{
				content = content.Replace("{#init#}", initStr.ToString());
				SaveControlFile(this.parentDirName, content, prefabName + postfix);
			}
			else
			{
				SaveOtherFile(this.parentDirName, content, prefabName + postfix);
			}
		}

		/// <summary>
		/// 保存数据和网络
		/// </summary>
		/// <param name="prefabName">预制件名</param>
		/// <param name="postfix">预制名+后缀名</param>
		/// <param name="templateName">模板名</param>
		/// <param name="dirName">目录名</param>
		private void Save(string prefabName, string postfix, string templateName, string dirName)
		{
			string content = ReadTemplateString(Hukiry.HukiryUtilEditor.FindAssetPath<TextAsset>(templateName));
			content = content.Replace("{#class#}", prefabName);
			content = content.Replace("{#Author#}", "Hukiry");
			content = content.Replace("{#dirName#}", this.parentDirName);
			content = content.Replace("{#DateTime#}", GetNowTimer());
			SaveOtherFile(this.parentDirName + "/" + dirName, content, prefabName + postfix);
		}

		//保存Item视图
		private void SaveItemTemplate(string prefabName, string postfix, string templateName, StringBuilder initStr = null, bool isControl = false)
		{
			string content = ReadTemplateString(Hukiry.HukiryUtilEditor.FindAssetPath<TextAsset>(templateName));
			content = content.Replace("{#class#}", prefabName);
			content = content.Replace("{#Author#}", "Hukiry");
			content = content.Replace("{#dirName#}", this.parentDirName);
			content = content.Replace("{#DateTime#}", GetNowTimer());
			if (this.IsSceneUITag())
			{
				if (isControl)
				{
					content = content.Replace("{#init#}", initStr.ToString());
					SaveControlFile(this.parentDirName, content, prefabName + postfix);
				}
				else
				{
					SaveOtherFile(this.parentDirName, content, prefabName + postfix);
				}
			}
			else
			{
				if (isControl)
				{
					content = content.Replace("{#init#}", initStr.ToString());
					SaveControlFile(this.parentDirName + "/Item", content, prefabName + postfix);
				}
				else
				{
					SaveOtherFile(this.parentDirName + "/Item", content, prefabName + postfix);
				}
			}
		}
	}
}
