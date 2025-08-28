using Hukiry.Project;
using Hukiry.SDK;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Hukiry.Android
{

    public abstract class AndroidBuildXGradleBase : IAndroidBuildXGradle
    {
        protected const string launcher = nameof(launcher);
        protected const string unityLibrary = nameof(unityLibrary);
        /// <summary>
        /// 资源库目录
        /// </summary>
        protected string unitySdkLibraryDir => SdkConfiguration.AndroidLibraryDir;

        /// <summary>
        /// unity sdk主活动交互包名
        /// </summary>
        protected  string unitySdkPackageName => SdkConfiguration.AndroidPackageName;

        private string GetAndroidGradlePath => EditorPrefs.GetString("GradlePath");
        private string GetAndroidSdkPath => EditorPrefs.GetString("AndroidSdkRoot");
        private string GetAndroidJdkPath => EditorPrefs.GetString("JdkPath");
        private string GetAndroidNDKPath =>
#if UNITY_2022_1_OR_NEWER
            EditorPrefs.GetString("AndroidNdkRootR23b");
#elif UNITY_2021_1_OR_NEWER
            "D:/Android_sdk/android_SDK/ndk/21.3.6528147";
#elif UNITY_2019_1_OR_NEWER
            EditorPrefs.GetString("AndroidNdkRootR19");
#elif UNITY_5_3_OR_NEWER
            EditorPrefs.GetString("AndroidNdkRootR16b");
#else
            EditorPrefs.GetString("AndroidNdkRootR23");
#endif
        private const string ndkDirPath = "E\\:/Android_Sdk_Ndk/android-ndk-r19c";

        void IAndroidBuildXGradle.BuildAndroidXGradle(string rootProjectPath, string unitylibraryPath)=> this.StartBuildAndroidX(rootProjectPath, unitylibraryPath);
        public virtual void StartBuildAndroidX(string rootProjectPath, string unitylibraryPath) {
            this.CommonXGradle(rootProjectPath, unitylibraryPath);
        }

        protected void CopyFile(string libPath, string rootProjectPath, string targerPath)
        {
            if (!Directory.Exists(rootProjectPath)) Directory.CreateDirectory(rootProjectPath);
            string resPath = Path.Combine(Application.dataPath, libPath);
            if (File.Exists(resPath))
            {
                File.Copy(resPath, Path.Combine(rootProjectPath, targerPath), true);
            }
            else
            {
                Debug.LogError($"unity 资源库文件不存在：{resPath}");
            }
        }

        protected string GetDirPath(string moduleName, string packageName, string rootProjectPath)
        {
            const string shortDir = "src/main/java";
            return $"{rootProjectPath}/{moduleName}/{shortDir}/{packageName.Replace('.', '/')}/";
        }

        private void CommonXGradle(string rootProjectPath, string unitylibraryPath)
        {

#if UNITY_2021_1_OR_NEWER
            //根gradle属性配置——通配符
            WriteLineFile(rootProjectPath, "gradle.properties", writer =>
            {
                writer.WriteLine("org.gradle.jvmargs=-Xmx4096M");
                writer.WriteLine("org.gradle.parallel=true");
                writer.WriteLine("android.enableJetifier=false");
                writer.WriteLine("android.useAndroidX=true");
                writer.WriteLine("android.injected.testOnly=false");
                writer.WriteLine("unityStreamingAssets=.txt, .manifest, .byte, .json, .bytes, build_info, StreamingAssets");
                writer.WriteLine("aunityTemplateVersion=3");
            });
#else
            //根gradle属性配置——通配符
            WriteLineFile(rootProjectPath, "gradle.properties", writer =>
            {
                writer.WriteLine("org.gradle.jvmargs=-Xmx4096M");
                writer.WriteLine("android.useAndroidX=true");
                writer.WriteLine("android.enableJetifier=false");
                writer.WriteLine("android.org.gradle.parallel=true");
                writer.WriteLine("android.injected.testOnly=false");
            });
#endif
            //android studio 工程测试——通配符
            ASTest(rootProjectPath, HukiryProjectSettingsProvider.ins.gradleHukiryVersion);

            //根local路径属性配置
            WriteLineFile(rootProjectPath, "local.properties", writer =>
            {
                writer.WriteLine($"sdk.dir={HukiryProjectSettingsProvider.ins.sdkHukiryPath.Replace(":", "\\:")}");
                writer.WriteLine($"ndk.dir={HukiryProjectSettingsProvider.ins.ndkHukiryPath.Replace(":", "\\:")}");
            });

            //google服务版本4.3.15
            if (HukiryProjectSettingsProvider.ins.enableGoogleServer)
                this.WriteRootBuildGradle(rootProjectPath, $"\t\t\tclasspath 'com.google.gms:google-services:{HukiryProjectSettingsProvider.ins.googleClasspathVersion}'");
            else
                this.WriteRootBuildGradle(rootProjectPath);

            //不进行混淆的代码配置：先添加接口，后添加类
            this.MultidexProguard(unitylibraryPath, HukiryProjectSettingsProvider.ins.GetProguard);

            //implementation(name: 'billing-3.0.3', ext:'aar')
            this.ReplaceBilling(unitylibraryPath, "3.0.3");
        }

        private void ReplaceBilling(string unitylibraryPath, string version)
        {
            string buildPath = Path.Combine(unitylibraryPath, "build.gradle");
            string text = File.ReadAllText(buildPath);
            string implementTxt = $"implementation(name: 'billing-{version}', ext:'aar')";
            string common = "implementation(name: 'common', ext:'aar')";
            if (text.Contains(common)) text = text.Replace(common, $"//{common}");
            if (text.Contains(implementTxt))
            {
                text= text.Replace(implementTxt, $"//{implementTxt}");
                File.WriteAllText(buildPath, text);
            }
        }

        private void ASTest(string rootProjectPath, string version)
        {
            //创建gradle目录 Gradle版本gradle-7.4-bin.zip 对应 build:gradle:4.2.2
            string dirwrapper = Path.Combine(rootProjectPath, "gradle/wrapper");
            if (!Directory.Exists(dirwrapper)) Directory.CreateDirectory(dirwrapper);
            //this.CopyFile($"{unitySdkLibraryDir}/gradle-wrapper.jar", rootProjectPath, "gradle/wrapper/gradle-wrapper.jar");
            WriteLineFile(rootProjectPath, "gradle/wrapper/gradle-wrapper.properties", writer =>
            {
                writer.WriteLine("distributionBase=GRADLE_USER_HOME");
                writer.WriteLine("distributionPath=wrapper/dists");
                writer.WriteLine($"distributionUrl=https\\://services.gradle.org/distributions/gradle-{version}-bin.zip");
                writer.WriteLine("zipStoreBase=GRADLE_USER_HOME");
                writer.WriteLine("zipStorePath=wrapper/dists");
            });
        }

        private void MultidexProguard(string unitylibraryPath, params string[] lines)
        {
            WriteLineFile(unitylibraryPath, "proguard-unity.txt", writer =>
            {
                writer.WriteLine("-keep class bitter.jnibridge.* { *; }");
                writer.WriteLine("-keep class com.unity3d.player.* { *; }");
                writer.WriteLine("-keep interface com.unity3d.player.IUnityPlayerLifecycleEvents { *; }");
                writer.WriteLine("-keep class org.fmod.* { *; }");
                writer.WriteLine("-keep class com.google.androidgamesdk.ChoreographerCallback { *; }");
                writer.WriteLine("-keep class com.google.androidgamesdk.SwappyDisplayManager { *; }");

                //自定义部分：先定义接口，后定义类
                if (lines != null && lines.Length > 0)
                {
                    writer.WriteLine("#unity custom interface or class");
                    int len = lines.Length;
                    for (int i = 0; i < len; i++)
                    {
                        writer.WriteLine(lines[i]);
                    }
                }

                writer.WriteLine("-ignorewarnings");
            });
        }

        private void WriteLineFile(string path, string fileName, Action<StreamWriter> actionWriter)
        {
            string gradlePropertiesFile = Path.Combine(path, fileName);
            if (File.Exists(gradlePropertiesFile))
            {
                File.Delete(gradlePropertiesFile);
            }
            StreamWriter writer = File.CreateText(gradlePropertiesFile);
            actionWriter(writer);
            writer.Flush();
            writer.Close();
        }

        protected void WriteRootBuildGradle(string rootProjectPath, params string[] classPaths)
        {
            string BuildGradleTemplate = @"
// GENERATED BY UNITY. REMOVE THIS COMMENT TO PREVENT OVERWRITING WHEN EXPORTING AGAIN
allprojects {
    buildscript {
        repositories {
            google()
            jcenter()
        }

        dependencies {
            {CLASSPATH}
        }
    }

    repositories {
        google()
        jcenter()
        flatDir {
            dirs #DIRS
        }
    }
}
                
task clean(type: Delete) {
    delete rootProject.buildDir
}
";
            BuildGradleTemplate = BuildGradleTemplate.Replace("#DIRS", "\"${project(':unityLibrary').projectDir}/libs\"");
            BuildGradleTemplate = BuildGradleTemplate.Replace("{CLASSPATH}",
                $"classpath 'com.android.tools.build:gradle:4.2.2'\n{string.Join("\n", classPaths)}");
            string BuildGradleFile = Path.Combine(rootProjectPath, "build.gradle");
            File.WriteAllText(BuildGradleFile, BuildGradleTemplate);
        }

        protected void AppLayerConfiguration(string buildPath)
        {
            const string IMPLEMENTATION= "#IMPLEMENTATION#", NDKVERSION= "#NDKVERSION#", 
                GOOGLESERVICES= "#GOOGLESERVICES#", DEBUGSYMBOL= "#DEBUGSYMBOL#", MULTIDEXENABLED= "#MULTIDEXENABLED#",
                minifyEnabled= "minifyEnabled false", RESVALUE= "#RESVALUE#";
            if (!File.Exists(buildPath)) throw new Exception("文件不存在：" + buildPath);
            string textGradle = File.ReadAllText(buildPath);
            textGradle = textGradle.Replace(IMPLEMENTATION, HukiryProjectSettingsProvider.ins.GetImplementation);
            textGradle = textGradle.Replace(NDKVERSION, HukiryProjectSettingsProvider.ins.GetNdkVersion());
            textGradle = textGradle.Replace(RESVALUE, HukiryProjectSettingsProvider.ins.GetResValue);

            if (HukiryProjectSettingsProvider.ins.enableGoogleServer)
                textGradle = textGradle.Replace(GOOGLESERVICES, HukiryProjectSettingsProvider.ins.googleApplyPlugin);
            else
                textGradle = textGradle.Replace(GOOGLESERVICES, string.Empty);

            if (HukiryProjectSettingsProvider.ins.enableDebugSymbol)
                textGradle = textGradle.Replace(DEBUGSYMBOL, HukiryProjectSettingsProvider.ins.apkSymbol);
            else
                textGradle = textGradle.Replace(DEBUGSYMBOL, string.Empty);

            if (HukiryProjectSettingsProvider.ins.enableMinify)
                textGradle = textGradle.Replace(minifyEnabled, HukiryProjectSettingsProvider.ins.apkMinify);

            if (HukiryProjectSettingsProvider.ins.enableMultiDex)
                textGradle = textGradle.Replace(MULTIDEXENABLED, HukiryProjectSettingsProvider.ins.apkMultiDex);
            else
                textGradle = textGradle.Replace(MULTIDEXENABLED, string.Empty);

            File.WriteAllText(buildPath, textGradle);
        }
    }
}
