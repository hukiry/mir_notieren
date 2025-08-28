using System;
using System.IO;
using System.Text;
using UnityEngine;
namespace Hukiry.Editor.Tool
{
	public class CSharpUIBuilder : UIBuilder
	{

		private const string Message_Template = "C#_UIMessage_Template";
		private const string View_Template = "C#_UIView_Template";
		private const string Control_Template = "C#_UIControl_Template";
		public CSharpUIBuilder(GameObject go, string ext) : base(go, ext)
		{
			SetSavePath("Scripts/UI/Generate");
		}

		public override void GenerateUI()
		{

			StringBuilder init = new StringBuilder();
			StringBuilder param = new StringBuilder();

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
						param.Append(string.Format("    public {0} m_{1};\n", tag, name));
						if (tag == typeof(GameObject).Name)
						{
							init.Append($"		{name}Go = transform.Find(\"{GetHierarchy(tran)}\").gameObject;\n");
						}
						else if (tag == typeof(Transform).Name)
						{
							init.Append($"		{name}TF = transform.Find(\"{GetHierarchy(tran)}\");\n");
						}
						else
						{
							init.Append($"		{name} = transform.Find(\"{GetHierarchy(tran)}\").GetComponent<{tag}>();\n");
						}
					}
				}
			}

		
			content = ReadTemplateString(Hukiry.HukiryUtilEditor.FindAssetPath<TextAsset>("C#_UI_Template"));
			content = content.Replace("{#class#}", GetClassName());
			content = content.Replace("{#param#}", param.ToString());
			content = content.Replace("{#init#}", init.ToString());

			string lastClassName = GetUIClassName();
			//if (!isOnlyView)
			//{
			//	content = ReadTemplateString(Path.Combine(Application.dataPath, "Scripts/Editor/UITemplate/C#_UIView_Template.txt"));
			//	SaveOtherFile("Scripts/UI", content.Replace("{#class#}", lastClassName), lastClassName);
			//	//大型游戏
			//}

			//if (isOnlyControl)
			//{
			//	content = ReadTemplateString(Path.Combine(Application.dataPath, "Scripts/Editor/UITemplate/C#_UIControl_Template.txt"));
			//	lastClassName = GetUIClassName().Replace("View", "Control");
			//	if (!lastClassName.Contains("Control"))
			//	{
			//		lastClassName += "Control";
			//	}
			//	SaveOtherFile("Scripts/UI/Control", content.Replace("{#class#}", lastClassName), lastClassName);
			//}

		}
	}
}

