using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Hukiry.Editor
{
    /// <summary>
    /// 游戏视图分辨率
    /// </summary>
    public class GameViewResolution
    {
        private readonly GameViewSizeGroupType gameViewSizeGroup;
        private List< GameResolutionSize> m_Resolution = null;
        private object GameViewSizesMenuItemProvider;
        private object m_GameViewSizeGroup;
        private int BuiltinCount;
        private List<GameResolutionSize> m_Builtin;
        public GameViewResolution(GameViewSizeGroupType viewSizeGroupType)
        {
            this.gameViewSizeGroup = viewSizeGroupType;
        }

        /// <summary>
        /// 输出内部分辨率
        /// </summary>
        /// <param name="fileOutPath"></param>
        public void OutInputInterResolution(string fileOutPath)
        {
            List<string> line = new List<string>();
            this.GetGameViewBuiltinAndCustom().Builtin.ForEach(p=> {
                line.Add(p.ToLine());
            });

            File.WriteAllLines(fileOutPath, line.ToArray());
        }

        /// <summary>
        /// 设置视图分辨率
        /// </summary>
        /// <param name="width">分辨率宽</param>
        /// <param name="height">分辨率高</param>
        /// <param name="baseText">文本</param>
        public void SetGameViewFixedResolution(int width, int height, string baseText)
        {
            if (m_Resolution == null)  m_Resolution = GetCustomResolution();
            if (m_Resolution.Find(p=>p.baseText== baseText) !=null) return;

            this.CreateGameViewSizesMenuItemProvider();
            var enumGammeViewSizeType = this.GetUnityEditorClass("GameViewSizeType");
            if (enumGammeViewSizeType != null)
            {
                var item = GetEnumValue(enumGammeViewSizeType, "FixedResolution");
                if (item.isExitValue)
                {
                    var gameViewSizeClass = this.GetUnityEditorClass("GameViewSize");
                    var gameViewSize = this.CreateClassInstance(gameViewSizeClass, item.enumValue, width, height, baseText);
                    this.InvokeMethodInstance(GameViewSizesMenuItemProvider, "Add", true, gameViewSize);
                }
            }
        }

    
        public int Count()
        {
            return GetCustomResolution().Count;
        }

        public void Remove(int index)
        {
           
            if (BuiltinCount == 0)
            {
                BuiltinCount = GetBuiltinResolution().Count;
            }

            int newIndex = index + BuiltinCount;
            if (newIndex >= BuiltinCount)
            {
                this.CreateGameViewSizesMenuItemProvider();
                HukiryUtilEditor.InvokeInstance(GameViewSizesMenuItemProvider, "Remove", newIndex);
            }
            else {
                Debug.LogError("Only custom game view sizes can be changed:"+ BuiltinCount);
            }
        }

        /// <summary>
        /// 获取自定义分辨率
        /// </summary>
        /// <returns></returns>
        public List<GameResolutionSize> GetCustomResolution()
        {
            return GetGameViewBuiltinAndCustom(true).Custom;
        }

        /// <summary>
        /// 获取内置Builtin分辨率
        /// </summary>
        /// <returns></returns>
        public List<GameResolutionSize> GetBuiltinResolution()
        {
            if (m_Builtin != null && m_Builtin.Count > 0)
            {
                return m_Builtin;
            }
            return GetGameViewBuiltinAndCustom(false).Builtin;
        }

        private void CreateGameViewSizesMenuItemProvider()
        {
            if (GameViewSizesMenuItemProvider == null)
            {
                var menuItemProviderclass = this.GetUnityEditorClass("GameViewSizesMenuItemProvider");
                GameViewSizesMenuItemProvider = this.CreateClassInstance(menuItemProviderclass, this.gameViewSizeGroup);
            }
        }


        /// <summary>
        /// 获取枚举值
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumName">枚举名</param>
        /// <returns>bool, object</returns>
        private (bool isExitValue, object enumValue) GetEnumValue(Type enumType, string enumName)
        {
            if (Enum.GetNames(enumType).Contains(enumName))
            {
                return (true, enumType.GetField(enumName).GetValue(enumType));
            }
            return (isExitValue: false, enumValue: null);
        }

        /// <summary>
        /// 创建动态类实例
        /// </summary>
        /// <param name="type">动态类型</param>
        /// <param name="objs">构造函数参数</param>
        /// <returns>new System.Type()</returns>
        private object CreateClassInstance(Type type, params object[] objs)
        {
            return Activator.CreateInstance(type, objs);
        }

        /// <summary>
        /// 获取Unity的编辑类
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns>System.Type</returns>
        private Type GetUnityEditorClass(string className)
        {
            string classPath = $"UnityEditor.{className},UnityEditor";
            return Type.GetType(classPath, false);
        }

        /// <summary>
        /// 获取游戏视图中的分辨率尺寸集合
        /// </summary>
        /// <returns></returns>
        private (List<GameResolutionSize> Builtin, List<GameResolutionSize> Custom) GetGameViewBuiltinAndCustom(bool isReadCustom = false)
        {
            List<GameResolutionSize> tempCustom = new List<GameResolutionSize>();
            this.CreateGameViewSizesMenuItemProvider();
            if (GameViewSizesMenuItemProvider != null)
            {
                BindingFlags notbindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
                if (m_GameViewSizeGroup == null)
                {
                    m_GameViewSizeGroup = GameViewSizesMenuItemProvider.GetType().GetField("m_GameViewSizeGroup", notbindingFlags)?.GetValue(GameViewSizesMenuItemProvider);
                }
                if (m_Builtin == null)
                {
                    if (!isReadCustom)
                        m_Builtin = GenericMakeElement(m_GameViewSizeGroup.GetType().GetField("m_Builtin", notbindingFlags)?.GetValue(m_GameViewSizeGroup));
                }
                tempCustom = GenericMakeElement(m_GameViewSizeGroup.GetType().GetField("m_Custom", notbindingFlags)?.GetValue(m_GameViewSizeGroup));
            }
            return (m_Builtin, tempCustom);
        }

        /// <summary>
        /// 解析集合元素
        /// </summary>
        /// <param name="objGeneric"></param>
        /// <returns></returns>
        private List<GameResolutionSize> GenericMakeElement(object objGeneric)
        {
            List<GameResolutionSize> tempCustom = new List<GameResolutionSize>();
            if (objGeneric == null) return tempCustom;
            BindingFlags notbindingFlags = BindingFlags.Instance | BindingFlags.NonPublic;
            if (objGeneric.GetType().IsGenericType)
            {
                var length = (int)objGeneric.GetType().GetField("_size", notbindingFlags)?.GetValue(objGeneric);
                var _items = objGeneric.GetType().GetField("_items", notbindingFlags)?.GetValue(objGeneric) as object[];
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Public;
                for (int i = 0; i < length; i++)
                {
                    var obj = _items[i];
                    GameResolutionSize sizeItem = new GameResolutionSize();
                    sizeItem.aspectRatio = (float)obj.GetType().GetProperty("aspectRatio", bindingFlags)?.GetValue(obj);
                    sizeItem.width = (int)obj.GetType().GetProperty("width", bindingFlags)?.GetValue(obj);
                    sizeItem.height = (int)obj.GetType().GetProperty("height", bindingFlags)?.GetValue(obj);
                    sizeItem.sizeType = obj.GetType().GetProperty("sizeType", bindingFlags)?.GetValue(obj).ToString();
                    sizeItem.baseText = (string)obj.GetType().GetProperty("baseText", bindingFlags)?.GetValue(obj);
                    sizeItem.displayText = (string)obj.GetType().GetProperty("displayText", bindingFlags)?.GetValue(obj);
                    sizeItem.index = i;
                    tempCustom.Add(sizeItem);
                }
            }
            else if (objGeneric.GetType().IsArray)
            {
                var length = (int)objGeneric.GetType().GetProperty("Length", BindingFlags.Instance | BindingFlags.Public)?.GetValue(objGeneric);
                var _items = objGeneric as object[];
            }
            return tempCustom;
        }

        /// <summary>
        /// 调用实例方法
        /// </summary>
        /// <param name="objType">实例类</param>
        /// <param name="methodName">方法名</param>
        /// <param name="isPublic">是公共方法</param>
        /// <param name="parameters">参数列表</param>
        private void InvokeMethodInstance(object objType, string methodName, bool isPublic = true, params object[] parameters)
        {
            var type = objType?.GetType();
            var methodInfo = type?.GetMethod(methodName, isPublic ? BindingFlags.Instance | BindingFlags.Public : BindingFlags.Instance | BindingFlags.NonPublic);
            methodInfo?.Invoke(objType, parameters);
        }
    }

    [Serializable]
    public class GameResolutionSize
    {
        public float aspectRatio;
        public int width;
        public int height;
        public string baseText;
        public string sizeType;
        public string displayText;
        public int index;
        public override string ToString()
        {
            return $"baseText:{baseText},sizeType:{sizeType},width:{width},height:{height},displayText:{displayText}";
        }
        public string ToLine()
        {
            return $"{baseText}, {sizeType}, {width}, {height}, {displayText}";
        }
    }
}
