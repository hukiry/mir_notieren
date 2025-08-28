using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace Hukiry.Editor.Tool
{
	public abstract class UIBuilder
	{
		protected string content = string.Empty;
		public string saveDirPath;
		private Transform[] childrenTransform;
		public string fileSuffix;
		private GameObject rootGameObject = null;
		private string RootPrefabName = null;
		public string parentDirName = string.Empty;

		public UIBuilder(GameObject go, string ext)
		{
	
			string DealDirPath(string parentDirName, string parentPath, List<string> sameNames)
			{
				//获取二级父目录路径
				parentPath = parentPath.Substring(0, parentPath.Length - parentDirName.Length - 1);
				//获取二级父目录名
				var parentName = parentPath.Substring(parentPath.LastIndexOf('\\') + 1);
				//parentDirName 一级父目录名
				if (!parentName.ToLower().Equals("ui")&& !parentDirName.ToLower().Equals("scene"))
				{
					if (sameNames.Contains(parentDirName))
					{
						return DealDirPath(parentName, parentPath, sameNames);
					}
					else
					{
						//获取的父目录拼接在前面
						return DealDirPath(parentName, parentPath, sameNames) + "/" + parentDirName;
					}
				}
				
				return parentDirName;
			}

			this.fileSuffix = ext;
			rootGameObject = go;
			childrenTransform = go.GetComponentsInChildren<Transform>(true);

			string path = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(go)).TrimEnd('/');
			parentDirName = path.Substring(path.LastIndexOf('\\') + 1);
			var sameNameList = new List<string>();
			if (rootGameObject.name.EndsWith("Item")) sameNameList.Add("Item");
			parentDirName = DealDirPath(parentDirName, path, sameNameList);

			//特殊处理场景部分
			if (UIPathConfig.DirNames.ContainsKey(parentDirName))
			{
				parentDirName = UIPathConfig.DirNames[parentDirName];
			}

			if (rootGameObject != null)
			{
				if (rootGameObject.name.EndsWith("Panel"))
				{
					RootPrefabName = rootGameObject.name.Substring(0, rootGameObject.name.Length - 5);
				}
				else if (rootGameObject.name.EndsWith("View") || rootGameObject.name.EndsWith("Item"))
				{
					RootPrefabName = rootGameObject.name.Substring(0, rootGameObject.name.Length - 4);
				}
				else
				{
					RootPrefabName = rootGameObject.name;
				}
			}
			LogManager.LogColor("pink", RootPrefabName, childrenTransform.Length);
		}

		protected bool IsItemTag()
		{
			return rootGameObject != null && rootGameObject.tag == TagLayer.UI_LuaTemplate;
		}

		protected bool IsItemAndDataTag()
		{
			return rootGameObject != null && rootGameObject.tag == TagLayer.UI_LuaDataTemplate;
		}

		protected bool IsLuaViewTag()
		{
			return rootGameObject != null && rootGameObject.tag == TagLayer.UI_LuaView;
		}

		protected bool IsLuaViewAndDataTag()
		{
			return rootGameObject != null && rootGameObject.tag == TagLayer.UI_LuaViewData;
		}

		protected bool IsPanelTag()
		{
			return rootGameObject != null && rootGameObject.tag == TagLayer.UI_LuaPanel;
		}

		public bool IsSceneUITag()
		{
			return rootGameObject != null && rootGameObject.tag == TagLayer.UIScene_LuaView;
		}

		protected bool CheckTag(string tag)
		{
			return tag != string.Empty && tag != TagLayer.Untagged;
		}

		protected Transform[] GetChildrenTransform()
		{
			return childrenTransform;
		}

		protected void SetSavePath(string path)
		{
			saveDirPath = path;

			string genPath = Path.GetFullPath(this.saveDirPath);

			if (!Directory.Exists(this.saveDirPath))
			{
				Directory.CreateDirectory(genPath);
			}
		}

		protected virtual string GetClassName()
		{
			if (RootPrefabName != null)
			{
				return RootPrefabName + "Control";
			}
			return "";
		}

		protected string GetUIClassName()
		{
			if (RootPrefabName != null)
			{
				return RootPrefabName;
			}
			return "";
		}

		public abstract void GenerateUI();

		//保存其他逻辑脚本
		protected void SaveOtherFile(string dirName, string content, string className)
		{
			string genPath = Path.Combine(saveDirPath, dirName);
			string fileName = Path.Combine(genPath, className + fileSuffix);

			if (!Directory.Exists(genPath))
			{
				Directory.CreateDirectory(genPath);
			}

			if (!File.Exists(fileName))
			{
				using (FileStream writer = File.Open(fileName, FileMode.OpenOrCreate))
				{
					byte[] fileData = Encoding.UTF8.GetBytes(content);
					writer.Write(fileData, 0, fileData.Length);
				}
			}
		}

		protected void SaveControlFile(string dirName, string content, string className)
		{
			string genPath = Path.Combine(saveDirPath, dirName);
			string fileName = Path.Combine(genPath, className + fileSuffix);

			if (!Directory.Exists(genPath))
			{
				Directory.CreateDirectory(genPath);
			}

			using (FileStream writer = File.Open(fileName, FileMode.Create))
			{
				byte[] fileData = Encoding.UTF8.GetBytes(content);
				writer.Write(fileData, 0, fileData.Length);
			}

		}

		public void SaveConfigFile(string[] content, string className)
		{
			string genPath = saveDirPath;
			string fileName = Path.Combine(genPath, className + fileSuffix);

			if (!Directory.Exists(genPath))
			{
				Directory.CreateDirectory(genPath);
			}

			File.WriteAllLines(fileName, content);
		}

		//读取模板
		protected string ReadTemplateString(string filePath)
		{
			using (StreamReader reader = new StreamReader(filePath))
			{
				return reader.ReadToEnd();
			}
		}

		//获取查找层级
		protected string GetHierarchy(Transform obj)
		{
			if (obj == null)
				return "";
			string path = obj.name;

			while (obj.parent != null)
			{
				obj = obj.parent;
				path = obj.name + "/" + path;
			}

			path = path.Substring(path.IndexOf("/") + 1);
			return path;
		}

		public static string GetNowTimer()
		{
			DateTime time = DateTime.Now;
			return string.Format("{0}-{1}-{2}", time.Year.ToString().PadLeft(2, '0'), time.Month.ToString().PadLeft(2, '0'),
				time.Day.ToString().PadLeft(2, '0'));
		}

		public static string GetNowDay()
		{
			DateTime time = DateTime.Now;
			return string.Format("{0}-{1}-{2}", time.Year.ToString(), time.Month.ToString(), time.Day.ToString());
		}
	}
}