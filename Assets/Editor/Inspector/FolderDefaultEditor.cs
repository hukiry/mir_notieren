using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

[CustomEditor(typeof(DefaultAsset))]
public class FolderDefaultEditor : Editor
{
    private bool isEnableButton;
    private readonly string[] abPaths =
    {
        "Assets/luascript","Assets/ResourcesEx"
    };
    private void OnEnable()
    {
        isEnableButton = target.name.Equals("ResourcesEx");
    }

    protected override void OnHeaderGUI()
    {
        base.OnHeaderGUI();
        if (isEnableButton)
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("清除所有ab名"))
                {
                    if (EditorUtility.DisplayDialog("ab文件修改", "确定清空所有ab文件", "确认"))
                    {
                        foreach (var item in abPaths)
                        {
                            var files = Directory.GetFiles(item, "*.*", SearchOption.AllDirectories);
                            AutoSetAssetbundleName.ClearAllAb(files);
                        }

                        AssetDatabase.RemoveUnusedAssetBundleNames();
                    }
                }

                if (GUILayout.Button("标记所有ab名"))
                {
                    foreach (var item in abPaths)
                    {
                        var files = Directory.GetFiles(item, "*.*", SearchOption.AllDirectories);
                        AutoSetAssetbundleName.MarkALLAbFile(files);
                    }
                }
            }
        }
    }

}