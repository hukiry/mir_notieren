using Hukiry.Project;
using System;
using System.IO;
using UnityEditor.Android;
using UnityEngine;

namespace Hukiry.Android
{
    /// <summary>
    /// 应用层打包 apk or aab
    /// </summary>
    class AndroidApplicationBuild : AndroidBuildXGradleBase
    {

        public override void StartBuildAndroidX(string rootProjectPath, string unitylibraryPath)
        {
            base.StartBuildAndroidX(rootProjectPath, unitylibraryPath);

            //配置google服务
            if (HukiryProjectSettingsProvider.ins.enableGoogleServer)
                this.CopyFile($"{unitySdkLibraryDir}/google-services.json", $"{rootProjectPath}/{launcher}", "google-services.json");
            this.CopyFile($"../libraryAndroid/xml/network_security_config.xml", $"{rootProjectPath}/{launcher}/src/main/res/xml", "network_security_config.xml");

            if (!string.IsNullOrEmpty(unitySdkPackageName))
            {
                //配置Java库服务
                this.CopyJavaPackageDir(unitySdkPackageName, rootProjectPath);
                //this.CopyJavaPackageDir("com.five.pay", rootProjectPath);
                this.CopyJavaPackageDir("com.yiliu.util", rootProjectPath);
            }

            this.AppLayerConfiguration($"{rootProjectPath}/{launcher}/build.gradle");
        }

        private void CopyJavaPackageDir(string packageName, string rootProjectPath)
        {
            string targetDir = this.GetDirPath(launcher, packageName, rootProjectPath);
            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

            string librayDir = this.GetDirPath(unityLibrary, packageName, rootProjectPath);
            string[] files = Directory.GetFiles(librayDir, "*.java");
            foreach (var filePath in files)
            {
                string FileName = Path.GetFileName(filePath);
                File.Copy(filePath, Path.Combine(targetDir, FileName), true);
            }

            Directory.Delete(librayDir, true);
        }
    }
}
