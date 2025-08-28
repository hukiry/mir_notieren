
using UnityEditor;
using UnityEngine;

namespace Hukiry.Editor.ToolBar
{
    [InitializeOnLoad]
    public static class ToolbarMenu
    {
      
        static ToolbarMenu()
        {
            ToolbarExtender.LeftToolbarGUI.Add(OnToolbarLeftGUI);
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarRightGUI);
        }

        private static void OnToolbarLeftGUI()
        {
            GUILayout.Space(10);
            if (GUILayout.Button("场景工具"))
            {
                Debug.Log("场景工具");
            }
            GUILayout.FlexibleSpace();
        }

        private static void OnToolbarRightGUI()
        {
            GUILayout.Space(10);
            if (GUILayout.Button("自定义工具条"))
            {
                Debug.Log("测试成功");
            }
            GUILayout.FlexibleSpace();
        }

    }
}
