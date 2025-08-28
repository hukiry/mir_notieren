using Hukiry.Project;

namespace Hukiry.Android
{
    /// <summary>
    /// 模块层打包 apk or aab
    /// </summary>
    class AndroidModuleBuild : AndroidBuildXGradleBase
    {
        public override void StartBuildAndroidX(string rootProjectPath, string unitylibraryPath)
        {
            base.StartBuildAndroidX(rootProjectPath, unitylibraryPath);

            //配置google服务
            if (HukiryProjectSettingsProvider.ins.enableGoogleServer)
                this.CopyFile($"{unitySdkLibraryDir}/google-services.json", unitylibraryPath, "google-services.json");
            this.CopyFile($"{unitySdkLibraryDir}/LauncherManifest.xml", rootProjectPath, "launcher/src/main/AndroidManifest.xml");
        }
    }
}
