namespace Hukiry.Android
{
    /// <summary>
    /// 版本说明：
    ///    Gradle版本：7.4-bin.zip 
    ///    build:gradle 版本：4.2.2 
    ///    NDK版本：19.0.5232133
    ///    JDK版本：1.8
    ///    最小SDK版本：22
    ///    最大SDK版本：33
    ///    如果google服务在模块层，包名需要和应用层包名一致。否则模块层包名默认，应用层模块是自定义
    /// </summary>
    /// <param name="path"></param>
    public interface IAndroidBuildXGradle
    {
        /// <summary>
        /// android 打包
        /// </summary>
        /// <param name="rootProjectPath">工程根目录路径</param>
        /// <param name="unitylibraryPath">模块层目录路径</param>
        void BuildAndroidXGradle(string rootProjectPath, string unitylibraryPath);

    }
}

