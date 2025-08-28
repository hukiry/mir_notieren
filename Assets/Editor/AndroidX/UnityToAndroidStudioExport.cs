
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Hukiry.Editor
{
    /// <summary>
    /// Unity 导出工程 输出包
    /// </summary>
    public class UnityToAndroidStudioExport
    {
        /// <summary>
        /// 拷贝导出的Unity 工程
        /// </summary>
        /// <param name="dirPath">出包的工程路径</param>
        /// <param name="bundleVersion">版本名</param>
        /// <param name="versionCode">版本号</param>
        /// <param name="unityProjectName">Unity工程名</param>
        public static void CopyExportUnityProject(string dirPath, string bundleVersion, int versionCode)
        {
            List<string> arrayDirName = OutPackageConfigAssets.Instance.GetDirNames();
            //删除目录开始
            string androidResourcePath = OutPackageConfigAssets.Instance.androidResourcePath;
            if (!Directory.Exists(androidResourcePath)) { Directory.CreateDirectory(androidResourcePath); }

            EditorUtility.DisplayProgressBar("Unity Moving Start... ", "", 0.1f);
            foreach (var dirName in arrayDirName)
            {
                string dirshortPath = Path.Combine(androidResourcePath, dirName);
                if (Directory.Exists(dirshortPath))
                {
                    Directory.Delete(dirshortPath, true);
                    Debug.Log($"删除成功：{dirshortPath}");
                    EditorUtility.DisplayProgressBar("Unity delete ... ", "", 0.5f);
                }
            }
            AssetDatabase.Refresh();

            //拷贝目录开始
            string targetProject = Path.Combine(dirPath, $"unityLibrary/src/main");
            foreach (var dirName in arrayDirName)
            {
                string sourceDirName = Path.Combine(targetProject, dirName).Replace('/', '\\');
                string destDirName = Path.Combine(androidResourcePath, dirName).Replace('/', '\\');
                Debug.Log($"移动目录：{sourceDirName} 到 {destDirName}");
                FileHelper.CopyFolder(sourceDirName, destDirName, "*.o");
                EditorUtility.DisplayProgressBar("Unity Copying ... ", "", 0.7f);
            }
            EditorUtility.DisplayProgressBar("Unity Copying ... ", "", 0.8f);
            AssetDatabase.Refresh();

            //出包开始
            GradlewBatBuild(bundleVersion, versionCode);
            EditorUtility.DisplayProgressBar("Unity export arr ... ", "", 0.9f);
            EditorUtility.ClearProgressBar();
        }

        private static string GetUnityProjectName()
        {
            string unityProjectName = Path.GetDirectoryName(Application.dataPath);
            unityProjectName = unityProjectName.Substring(unityProjectName.LastIndexOf('\\') + 1);
            return unityProjectName;
        }

        private static void GradlewBatBuild(string bundleVersion, int versionCode)
        {
            string unityProjectName = GetUnityProjectName();
            var workMode = PackingWinConfigAssets.Instance.WorkMode;
            string dirNamePackage = workMode == "Release"?"aab":"apk";
           

            string WorkingDirectory = OutPackageConfigAssets.Instance.androidProjectPath;
            string packagePath = Path.Combine(WorkingDirectory, $"launcher/build/{dirNamePackage}");
            string buildPath = Path.Combine( WorkingDirectory, "launcher/build.gradle");
            (string outputVersionCode, string outputVersionName, string outputSetProperty) = (string.Empty,string.Empty, string.Empty);

            string gradleversionCode = $"        versionCode {versionCode}";
            string gradleversionName = $"        versionName '{bundleVersion}'";
            string gradleSetProperty = "        setProperty(\"archivesBaseName\", \""+$"../../../{dirNamePackage}/{unityProjectName}"+"-v${versionName}-${versionCode}\")";
            if (File.Exists(buildPath))
            {
                string[] lines = File.ReadAllLines(buildPath);
               
                foreach (var item in lines)
                {
                    string itemLine = item.Trim();
                    if (!string.IsNullOrEmpty(itemLine) && itemLine.StartsWith("versionCode"))
                        outputVersionCode = item;
                    if (!string.IsNullOrEmpty(itemLine) && itemLine.StartsWith("versionName"))
                        outputVersionName = item;
                    if (!string.IsNullOrEmpty(itemLine) && itemLine.StartsWith("setProperty"))
                        outputSetProperty = item;
                }

                string gradletext = File.ReadAllText(buildPath).Replace(outputVersionCode, gradleversionCode);
                gradletext = gradletext.Replace(outputVersionName, gradleversionName);
                gradletext = gradletext.Replace(outputSetProperty, gradleSetProperty);
                File.WriteAllText(buildPath, gradletext);
            }
            else
            {
                LogManager.LogColor("red", "buildPath  路径不存在" + buildPath);
            }

            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo.WorkingDirectory = WorkingDirectory;
            proc.StartInfo.FileName = "gradlew.bat";
            proc.StartInfo.Arguments = OutPackageConfigAssets.Instance.GetCmdParams(workMode == "Release");
            proc.Start();
            proc.WaitForExit();
            proc.Close();
            proc.Dispose();

            AssetDatabase.Refresh();
            if (Directory.Exists(packagePath))
            {
                Application.OpenURL(packagePath);
            }
        }
    }
}
