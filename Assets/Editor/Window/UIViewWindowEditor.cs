using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;

namespace Hukiry.Editor
{
    public class UIViewWindowEditor : CreateWindowEditor<UIViewWindowEditor>
    {
        /// <summary>
        /// 创建类型
        /// </summary>
        public enum CreateType {
            /// <summary>
            /// 窗口
            /// </summary>
            [FieldName("面板")]
            Window,
            //控件：水平或垂直循环滚动控件，进度条，按钮（图标，普通），文本框
            [FieldName("控件")]
            Component
        }

        public enum ComponentPrefab
        {
            /// <summary>
            /// 滚动视图：水平和垂直
            /// </summary>
            ScrollView,
            /// <summary>
            /// 进度条：水平和垂直
            /// </summary>
            Slider,
            /// <summary>
            /// 普通按钮
            /// </summary>
            Button,
            /// <summary>
            /// 超级文本：居中，字体40
            /// </summary>
            Text,
            /// <summary>
            /// 物品显示，奖励显示
            /// </summary>
            Item,
        }

        /// <summary>
        /// 面板系统类型，type=1 单击 ，2=联网+单击
        /// </summary>
        public enum PanelPrefab
        {
            /// <summary>
            /// 通用
            /// </summary>
            [FieldName("New View Prefab",1)]
            Default,
            /// <summary>
            /// 商城：充值，通行证，道具，兑换，礼包
            /// </summary>
            [FieldName("商城",1)]
            Shop,
            /// <summary>
            /// 抽卡
            /// </summary>
            [FieldName("抽卡",1)]
            Drawcard,
            /// <summary>
            /// 设置：音乐，音效，振动, 评分，注销和绑定
            /// </summary>
            [FieldName("设置",1)]
            Setting,
            /// <summary>
            /// 主视图：场景主视图，货币栏，挑战
            /// </summary>
            [FieldName("主视图",1)]
            Home,
            /// <summary>
            /// 任务
            /// </summary>
            [FieldName("任务",1)]
            Task,

            /// <summary>
            /// 排行榜
            /// </summary>
            [FieldName("排行榜",2)]
            Rank,
            /// <summary>
            /// 邮件
            /// </summary>
            [FieldName("邮件",2)]
            Mail,
            /// <summary>
            /// 好友
            /// </summary>
            [FieldName("好友",2)]
            Friend,
            /// <summary>
            /// 签到
            /// </summary>
            [FieldName("签到",2)]
            Sign,
           
            /// <summary>
            /// 修改昵称：在设置里面
            /// </summary>
            [FieldName("修改昵称",2)]
            FixNick,
            /// <summary>
            /// 修改头像：在设置里面
            /// </summary>
            [FieldName("修改头像",2)]
            RepairHead,
            /// <summary>
            /// 语言切换：在设置里面
            /// </summary>
            [FieldName("更改语言", 1)]
            Language,
            /// <summary>
            /// 隐私政策：在设置里面
            /// </summary>
            [FieldName("隐私政策", 1)]
            Private,
            /// <summary>
            /// 提示框
            /// </summary>
            [FieldName("提示框",1)]
            Tip,
        }

        [System.Serializable]
        public class ViewData
        {
            [FieldName("分辨率")]
            public Vector2Int defaultResolution;
            [FieldName("UI根目录路径")]
            public string saveDirPath = "Assets";
            [FieldName("面板名称")]
            public string viewName = "New View Prefab";
            [FieldName("创建类型")]
            public CreateType createType;
            [FieldName("是联网")][Space]
            public bool isOnline;
        }

        [SerializeField]
        ViewData m_viewData = new ViewData();

        private void OnEnable()
        {

        }

        public override void DrawGUI()
        {
            m_viewData = HukiryGUIEditor.DrawType(m_viewData);
            //GameObject meshPrefab = PrefabUtility.LoadPrefabContents(savePath);
            //PrefabUtility.SaveAsPrefabAsset(meshPrefab, savePath);
            //AssetDatabase.SaveAssets();
            //AssetDatabase.Refresh();

            if (GUILayout.Button("Create And Apply"))
            {
                this.CreateDefault();
            }
        }

        public override Color TitleColor()
        {
            return Color.cyan*0.5f;
        }

        private GameObject CreateStart()
        {
            var root =new GameObject(m_viewData.viewName,typeof(CanvasRenderer), typeof(RectTransform));
            root.layer = LayerMask.NameToLayer("UI");
            RectTransform rectTransform = root.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            return root;
        }

        private void CreateEnd(GameObject rootGo, string viewPath)
        {
            string savePath = Path.Combine(m_viewData.saveDirPath, viewPath + ".prefab");
            PrefabUtility.SaveAsPrefabAsset(rootGo, savePath, out bool success);
            AssetDatabase.SaveAssets();
            GameObject.DestroyImmediate(rootGo);
        }

        private void CreateDefault()
        {
            GameObject rootGo = this.CreateStart();

            this.CreateEnd(rootGo, m_viewData.viewName);
        }
    }
}
