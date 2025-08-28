using Hukiry;
using Hukiry.UI;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;


[CustomEditor(typeof(PackageSpriteAtlasManger), true)]
public class PackageSpriteAtlasMangerEditor : Editor
{
    PackageSpriteAtlasManger atlasManger;
    SerializedProperty objList;
    SerializedProperty inputSpriteAtlas;
    Object lastObj = null;
    public void OnEnable()
    {
        objList = serializedObject.FindProperty("objList");
        inputSpriteAtlas = serializedObject.FindProperty("inputSpriteAtlas");
        atlasManger = target as PackageSpriteAtlasManger;
    }

    public override void OnInspectorGUI()
    {
        string labName = atlasManger.packageAtlasType == PackageAtlasType.DirectoryToSpriteAtlas ? "每个目录打一个图集" : "多个目录打包到图集";
        GUILayout.Label(labName);
        atlasManger.packageAtlasType = (PackageAtlasType)EditorGUILayout.EnumPopup("PackageAtlasType", atlasManger.packageAtlasType);

        switch (atlasManger.packageAtlasType)
        {
            case PackageAtlasType.DirectoryToSpriteAtlas:
                atlasManger.dirPath = EditorGUILayout.TextField("打包的父目录路径：", atlasManger.dirPath);
                atlasManger.creatSpriteAtlasDirPath = EditorGUILayout.TextField("保存图集的目录路径：", atlasManger.creatSpriteAtlasDirPath);
                break;
            case PackageAtlasType.MulDirToSpriteAtlas:
                GUILayout.BeginVertical(GUI.skin.box);
                serializedObject.Update();
                AtlasImageEditor.DrawAtlasPopupLayout(new GUIContent("Sprite Atlas"), new GUIContent("--"), inputSpriteAtlas);
                if (lastObj != inputSpriteAtlas.objectReferenceValue)
                {
                    lastObj = inputSpriteAtlas.objectReferenceValue;
                    atlasManger.objList.Clear();
                    atlasManger.objList.AddRange(atlasManger.inputSpriteAtlas.GetPackables());
                }
                EditorGUILayout.PropertyField(objList);
                serializedObject.ApplyModifiedProperties();
                GUILayout.EndVertical();
                break;
        }

        if (GUILayout.Button("打包图集"))
        {
            if (atlasManger.packageAtlasType == PackageAtlasType.DirectoryToSpriteAtlas)
            {
                atlasManger.PackageSpriteAtlasByDirectory();
            }
            else
            {
                atlasManger.PackageSpriteAtlas();
            }
            this.Repaint();
        }
    }
}

