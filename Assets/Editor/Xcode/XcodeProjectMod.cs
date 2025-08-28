#if UNITY_IPHONE||UNITY_IOS
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
/*
 * 一路径部分
Framework Search Paths 管理导入的*.framework的路径
Library Search Paths 管理导入的*.a的路径
Header Search Paths 管理导入的头文件的路径

*二Other linker flags设置
－ObjC：加了这个参数后，链接器就会把静态库中所有的Objective-C类和分类都加载到最后的可执行文件中
－all_load：会让链接器把所有找到的目标文件都加载到可执行文件中，但是千万不要随便使用这个参数！假如你使用了不止一个静态库文件，然后又使用了这个参数，那么你很有可能会遇到ld: duplicate symbol错误，因为不同的库文件里面可能会有相同的目标文件，所以建议在遇到-ObjC失效的情况下使用-force_load参数。
-force_load：所做的事情跟-all_load其实是一样的，但是-force_load需要指定要进行全部加载的库文件的路径，这样的话，你就只是完全加载了一个库文件，不影响其余库文件的按需加载
 */
public class XcodeProjectMod {
    [PostProcessBuild(100)]
    public static void OnPostprocessBuild(BuildTarget BuildTarget, string _pathToBuiltProject) {
		if (BuildTarget != BuildTarget.iOS)
		{
			Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
			return;
		}

        return;

        string pathToBuiltProject1 = Path.GetFullPath(_pathToBuiltProject);

        /// 获取工程 projPath=E:/Unity_sdk_Test/Unity_Package/TestXOProcssTxt/Unity-iPhone.xcodeproj/project.pbxproj
        string projPath = PBXProject.GetPBXProjectPath(_pathToBuiltProject);
        PBXProject proj = new PBXProject();

        proj.ReadFromString(File.ReadAllText(projPath));
#if UNITY_2019_3_OR_NEWER
        string target = proj.GetUnityMainTargetGuid();
        string str_Framework_Guid = proj.GetUnityFrameworkTargetGuid();
#else
        string target = proj.TargetGuidByName(PBXProject.GetUnityTargetName());
#endif
        var projRoot = Path.GetDirectoryName(Application.dataPath);


        var unityLinkPhaseGuid = proj.GetFrameworksBuildPhaseByTarget(target);
        var unityFrameworkLinkPhaseGuid = proj.GetFrameworksBuildPhaseByTarget(str_Framework_Guid);

        //启动字节码：lua库需要
        proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
        // 链接器:将o文件与c运行库链接起来
        proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
        //iphone 6s以上全部支持64位指令库  ${ARCHS_STANDARD}为armv7, arm64
        proj.SetBuildProperty(target, "ARCHS", "arm64");
        //设置开发平台
        proj.SetBuildProperty(target, "IPHONEOS_DEPLOYMENT_TARGET", "12.2");
        //设置swift版本
        proj.SetBuildProperty(target, "SWIFT_VERSION", "5.0");

        //2.设置系统库拷贝并添加自定义的Framework
        // 添加通用框架
        SetSystemFrameworks(proj, target);

        //自定义库
        Debug.Log($"target={target}\r\nprojRoot={projRoot}\r\n_pathToBuiltProject={_pathToBuiltProject}\r\nprojPath={projPath}");

        //添加引用库
        //proj.AddFileToBuild(target, proj.AddFile("Libraries/libiPhone-lib.a", "Frameworks/libiPhone-lib.a", PBXSourceTree.Source));
        proj.AddFileToBuild(target, proj.AddFile("Libraries/libil2cpp.a", "Frameworks/libil2cpp.a", PBXSourceTree.Source));
        //proj.AddFileToBuild(target, proj.AddFile("Libraries/Plugins/iOS/jpush-extension-ios-1.1.2.a", "Frameworks/jpush-extension-ios-1.1.2.a", PBXSourceTree.Source));
        //proj.AddFileToBuild(target, proj.AddFile("Libraries/Plugins/iOS/jpush-ios-4.4.0.a", "Frameworks/jpush-ios-4.4.0.a", PBXSourceTree.Source));
        //proj.AddFileToBuild(target, proj.AddFile("Libraries/Plugins/iOS/jcore-noidfa-ios-3.1.2.a", "Frameworks/jcore-noidfa-ios-3.1.2.a", PBXSourceTree.Source));
        proj.AddFileToBuild(target, proj.AddFile("Libraries/Plugins/iOS/libtolua.a", "Frameworks/libtolua.a", PBXSourceTree.Source));

        //添加库 
        proj.AddFileToBuild(target, proj.AddFile("usr/lib/libsqlite3.tbd", "Frameworks/libsqlite3.tbd", PBXSourceTree.Sdk));//支持本地Sqlite
        proj.AddFileToBuild(target, proj.AddFile("usr/lib/swift/libswiftFoundation.tbd", "Frameworks/libswiftFoundation.tbd", PBXSourceTree.Sdk));//支持swift库
        proj.AddFileToBuild(target, proj.AddFile("usr/lib/libc++abi.tbd", "Frameworks/libc++abi.tbd", PBXSourceTree.Sdk));
        proj.AddFileToBuild(target, proj.AddFile("usr/lib/libbz2.tbd", "Frameworks/libbz2.tbd", PBXSourceTree.Sdk));
        proj.AddFileToBuild(target, proj.AddFile("usr/lib/libc++.tbd", "libc++.tbd", PBXSourceTree.Sdk));   //支持最新的c++11标准
        //proj.AddFileToBuild(target, proj.AddFile("usr/lib/libz.tbd", "Frameworks/libz.tbd", PBXSourceTree.Sdk));   //进行数据压缩
        //proj.AddFileToBuild(target, proj.AddFile("usr/lib/libresolv.tbd", "Frameworks/libresolv.tbd", PBXSourceTree.Sdk));

        //资源签名
        proj.SetBuildProperty(target, "CODE_SIGN_RESOURCE_RULES_PATH", "");
        //证书详情
        //proj.SetBuildProperty(target, "PROVISIONING_PROFILE", "");

        //设置框架搜索路径・追加
        proj.SetBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(inherited)");
        proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)");
        proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");

        //library的查找路径
        proj.SetBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(inherited)");
        proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(PROJECT_DIR)");
        proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(PROJECT_DIR)/Frameworks");
        proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)");
        proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)/Libraries");
        proj.AddBuildProperty(target, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)/Libraries/Plugins/iOS");

        //拷贝google清单文件
        //File.Copy("iostool/GoogleService-Info.plist", _pathToBuiltProject + "/GoogleService-Info.plist", true);
        //proj.AddFileToBuild(target, proj.AddFile("GoogleService-Info.plist", "GoogleService-Info.plist", PBXSourceTree.Source));

        //添加 File.swift
        //File.Copy("iostool/File.swift", _pathToBuiltProject + "/File.swift", true);
        //proj.AddFileToBuild(target, proj.AddFile("File.swift", "File.swift", PBXSourceTree.Source));

        //多语言添加
        //CopyAndReplaceDirectory("iostool/en.lproj", _pathToBuiltProject + "/en.lproj");
        //CopyAndReplaceDirectory("iostool/zh-Hans.lproj", _pathToBuiltProject + "/zh-Hans.lproj");
        //CopyAndReplaceDirectory("iostool/zh-Hans.lproj", _pathToBuiltProject + "/zh-Hans.lproj");
        //CopyAndReplaceDirectory("iostool/zh-HK.lproj", _pathToBuiltProject + "/zh-HK.lproj");
        //proj.AddFileToBuild(target, proj.AddFile("en.lproj/InfoPlist.strings", "InfoPlist.strings", PBXSourceTree.Source));
        //proj.AddFileToBuild(target, proj.AddFile("zh-Hans.lproj/InfoPlist.strings", "InfoPlist.strings", PBXSourceTree.Source));
        //proj.AddFileToBuild(target, proj.AddFile("zh-Hans.lproj/InfoPlist.strings", "InfoPlist.strings", PBXSourceTree.Source));
        //proj.AddFileToBuild(target, proj.AddFile("zh-HK.lproj/InfoPlist.strings", "InfoPlist.strings", PBXSourceTree.Source));
        //权限添加 
        proj.AddCapability(target, PBXCapabilityType.InAppPurchase);//苹果内购
        proj.AddCapability(target, PBXCapabilityType.SignInWithApple);//苹果登录
        proj.AddCapability(target, PBXCapabilityType.PushNotifications);//苹果推送
        proj.AddCapability(target, PBXCapabilityType.BackgroundModes);//后台接受推送服务,选择远程
        proj.AddCapability(target, PBXCapabilityType.KeychainSharing);//key分享
        //proj.AddCapability(target, PBXCapabilityType.GameCenter);//游戏中心
        //proj.AddCapability(target, PBXCapabilityType.HealthKit);//健康
        //proj.AddCapability(target, PBXCapabilityType.AccessWiFiInformation);//wifi访问
        //proj.AddCapability(target, PBXCapabilityType.WirelessAccessoryConfiguration);//wifi配置
        //proj.AddCapability(target, PBXCapabilityType.Maps);//地图
        //写入依赖库文件 Unity-iPhone.xcodeproj/project.pbxproj
        File.WriteAllText(projPath, proj.WriteToString());

        //文件清单配置
        //File.Copy("iostool/Info.plist", _pathToBuiltProject + "/Info.plist", true);

        //创建设置Capability类：权限添加 （TARGETS --->Capabilities 里边打开了一些功能 ）  
        //#if UNITY_2019_3_OR_NEWER
        //        var capManager = new ProjectCapabilityManager(projPath, "mergegame.entitlements", targetGuid: proj.GetUnityMainTargetGuid());
        //#else
        //       var capManager = new ProjectCapabilityManager(projPath, "mergegame.entitlements", PBXProject.GetUnityTargetName());
        //#endif
        //        //capManager.AddGameCenter();//游戏中心
        //        capManager.AddInAppPurchase();//支付
        //        capManager.AddSignInWithApple();//苹果登录
        //        capManager.AddPushNotifications(false);//推送
        //        capManager.AddBackgroundModes(BackgroundModesOptions.RemoteNotifications);//后台接受推送服务,选择远程
        //        capManager.AddHealthKit();//添加健康
        //        capManager.WriteToFile();//写入文件保存

        //远程通知启动
        XClass xClass = new XClass(_pathToBuiltProject + "/Classes/Preprocessor.h");
        xClass.Replace("#define UNITY_USES_REMOTE_NOTIFICATIONS 0", "#define UNITY_USES_REMOTE_NOTIFICATIONS 1");
        xClass.Save();
        Debug.Log("===============您使用的SDK打包完成了,呵呵哒==========================");
    }

    private static void SetSystemFrameworks(PBXProject pbxProject, string targetGuid)
    {
        //Unity需要的库
        pbxProject.AddFrameworkToProject(targetGuid, "AuthenticationServices.framework", false);//验证登录
        pbxProject.AddFrameworkToProject(targetGuid, "AVKit.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "Security.framework", false);//用于存储keychain
        pbxProject.AddFrameworkToProject(targetGuid, "MediaToolbox.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "CoreText.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "AudioToolbox.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "AVFoundation.framework", true);//音频处理
        pbxProject.AddFrameworkToProject(targetGuid, "CFNetwork.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "CoreGraphics.framework", false);//视图渲染
        pbxProject.AddFrameworkToProject(targetGuid, "CoreLocation.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "CoreMedia.framework", false);//核心媒体
        pbxProject.AddFrameworkToProject(targetGuid, "CoreMotion.framework", true);//陀螺仪，加速计
        pbxProject.AddFrameworkToProject(targetGuid, "CoreVideo.framework", false);//核心视频
        pbxProject.AddFrameworkToProject(targetGuid, "Foundation.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "OpenAL.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "OpenGLES.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "QuartzCore.framework", false);
        pbxProject.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", false);//系统配置
        pbxProject.AddFrameworkToProject(targetGuid, "UIKit.framework", false);//用户界面
        pbxProject.AddFrameworkToProject(targetGuid, "StoreKit.framework", false);//内购服务
        pbxProject.AddFrameworkToProject(targetGuid, "Metal.framework", true);
        pbxProject.AddFrameworkToProject(targetGuid, "GameController.framework", true);
        pbxProject.AddFrameworkToProject(targetGuid, "UserNotifications.framework", false);//通知

        //
        pbxProject.AddFrameworkToProject(targetGuid, "CoreData.framework", false);//文件存储框架
        pbxProject.AddFrameworkToProject(targetGuid, "JavaScriptCore.framework", true);//支持Google网页版登录
        pbxProject.AddFrameworkToProject(targetGuid, "SafariServices.framework", false);//Safari浏览器服务（如果没有安装fb，默认用safri）
        pbxProject.AddFrameworkToProject(targetGuid, "MapKit.framework", false);//地图服务
        pbxProject.AddFrameworkToProject(targetGuid, "WebKit.framework", false);//app内网页功能
        pbxProject.AddFrameworkToProject(targetGuid, "AssetsLibrary.framework", false);//获取图片和视频文件
        //pbxProject.AddFrameworkToProject(targetGuid, "AdServices.framework", true);//广告服务
        //pbxProject.AddFrameworkToProject(targetGuid, "iAd.framework", true);//广告标识符
        //pbxProject.AddFrameworkToProject(targetGuid, "AdSupport.framework", true);//获取advertisingIdentifier
        //pbxProject.AddFrameworkToProject(targetGuid, "AppTrackingTransparency.framework", true);///跟踪权限

        //pbxProject.AddFrameworkToProject(targetGuid, "CoreFoundation.framework", false);///核心模块
        //pbxProject.AddFrameworkToProject(targetGuid, "Accelerate.framework", false);
        //pbxProject.AddFrameworkToProject(targetGuid, "MediaPlayer.framework", false);//媒体播放
        //pbxProject.AddFrameworkToProject(targetGuid, "MessageUI.framework", false);//短信服务
        //pbxProject.AddFrameworkToProject(targetGuid, "CoreServices.framework", false);
        //pbxProject.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);//电话服务
        //pbxProject.AddFrameworkToProject(targetGuid, "CoreImage.framework", false);//人脸识别
        //pbxProject.AddFrameworkToProject(targetGuid, "LocalAuthentication.framework", false);//指纹识别
        //pbxProject.AddFrameworkToProject(targetGuid, "ImageIO.framework", false);//图片服务

    }

    static internal void CopyAndReplaceDirectory(string srcPath, string dstPath) {
		if (dstPath.EndsWith(".meta"))
			return;

		if (dstPath.EndsWith(".DS_Store"))
			return;
		if (Directory.Exists(dstPath))
			Directory.Delete(dstPath);
		if (File.Exists(dstPath))
			File.Delete(dstPath);

		Directory.CreateDirectory(dstPath);

		foreach (var file in Directory.GetFiles(srcPath)) {
			File.Copy(file, Path.Combine(dstPath, Path.GetFileName(file)));
		}

		foreach (var dir in Directory.GetDirectories(srcPath)) {
			CopyAndReplaceDirectory(dir, Path.Combine(dstPath, Path.GetFileName(dir)));
		}
	}

    public class XClass : System.IDisposable
    {

        private string filePath;
        private string text_all;
        public XClass(string fPath)
        {
            filePath = fPath;
            if (!System.IO.File.Exists(filePath))
            {
                Debug.LogError(filePath + "路径下文件不存在");
                return;
            }

            StreamReader streamReader = new StreamReader(filePath);
            text_all = streamReader.ReadToEnd();
            streamReader.Close();
        }

        public void Replace(string below, string newText)
        {
            int beginIndex = text_all.IndexOf(below);
            if (beginIndex == -1)
            {
                Debug.LogError(filePath + "中没有找到标致" + below);
                return;
            }
            text_all = text_all.Replace(below, newText);
        }

        public void Save()
        {
            StreamWriter streamWriter = new StreamWriter(filePath);
            streamWriter.Write(text_all);
            streamWriter.Close();
        }



        public void Dispose()
        {

        }
    }
}
#endif