using Hukiry.Android;
using System.IO;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEngine;


#if !UNITY_2022_1_OR_NEWER
/// <summary>
/// 导出临时工程后执行此方法，后续然后再build
/// </summary>
public class AndroidXGradleBuildProcessor : IPostGenerateGradleAndroidProject
{
    IAndroidBuildXGradle androidBuildXGradle;
    int IOrderedCallback.callbackOrder => 99;

    void IPostGenerateGradleAndroidProject.OnPostGenerateGradleAndroidProject(string unitylibraryPath)
    {
        string rootProjectPath = unitylibraryPath.Substring(0, unitylibraryPath.LastIndexOf('\\'));
#if MODEL_LAYER
        androidBuildXGradle = new AndroidModuleBuild();
#else
        androidBuildXGradle = new AndroidApplicationBuild();
#endif

#if ENABLE_SDK
        androidBuildXGradle.BuildAndroidXGradle(rootProjectPath, unitylibraryPath);
#endif
    }
}
#endif