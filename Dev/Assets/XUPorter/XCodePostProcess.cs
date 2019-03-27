using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.XCodeEditor;
#endif
using System.IO;
using sdk;

//参考http://www.xuanyusong.com/archives/2734
public static class XCodePostProcess
{

#if UNITY_EDITOR
    static char speChar = Path.AltDirectorySeparatorChar;
    static string project_path;
	[PostProcessBuild(999)]
	public static void OnPostProcessBuild( BuildTarget target, string pathToBuiltProject )
	{
		if (target != BuildTarget.iOS) {
			Debug.LogWarning("Target is not iPhone. XCodePostProcess will not run");
			return;
		}
        //Debug.LogWarning("pathToBuiltProject " + pathToBuiltProject);
        string path = Path.GetFullPath(pathToBuiltProject);

        //复制icon
        //CopyIOSIcon.MoveIcon(path);

        // Create a new project object from build target
        XCProject project = new XCProject( pathToBuiltProject );
        project_path = pathToBuiltProject;
        // Find and run through all projmods files to patch the project.
        // Please pay attention that ALL projmods files in your project folder will be excuted!
        //通用projmods文件，\Assets\XUPorter\Mods
        var modePath = Path.Combine(Application.dataPath, "XUPorter" + speChar + "Mods");
        string[] files = Directory.GetFiles(modePath, "*.projmods", SearchOption.TopDirectoryOnly);
		foreach( string file in files ) {
            UnityEngine.Debug.Log("ProjMod File: " + file);
			project.ApplyMod( file );
		}

        var projectModPath = Path.Combine(modePath, projectName);
        //特有projmods文件，\Assets\XUPorter\Mods\projectName
        if (Directory.Exists(projectModPath)) {
            string[] proModels = Directory.GetFiles(projectModPath, "*.projmods", SearchOption.AllDirectories);
            foreach (string file in proModels) {
                UnityEngine.Debug.Log(projectName + " ProjMod File: " + file);
                project.ApplyMod(file);
            }
        }

		//TODO implement generic settings as a module option
		//project.overwriteBuildSetting("CODE_SIGN_IDENTITY[sdk=iphoneos*]", "iPhone Distribution", "Release");
        //设置签名的证书， 第二个参数 你可以设置成你的证书
        //project.overwriteBuildSetting("CODE_SIGN_IDENTITY", "iPhone Developer: xuming gao (2SNH65FUPV)", "Release");
        //project.overwriteBuildSetting("CODE_SIGN_IDENTITY", "iPhone Developer: xuming gao (2SNH65FUPV)", "Debug");
        project.overwriteBuildSetting("ENABLE_BITCODE", "NO");


		// Finally save the xcode project
		project.Save();

        //这里输出的projectName 就是 91  
        Debug.Log(projectName);
        EditorCode(projectName);
    }

    public static string projectName {
        get {
            foreach (string arg in System.Environment.GetCommandLineArgs()) {
                if (arg.StartsWith("project")) {
                    return arg.Split("-"[0])[1];
                }
            }
            return "test";
        }
    }


    public static void EditorPlist(string filePath)
    {
     
        XCPlist list =new XCPlist(filePath);
        //string bundle = "com.yusong.momo";

//         string PlistAdd = @"
//         <key>NSAppTransportSecurity</key>
//         <dict>
//             <key>NSAllowsArbitraryLoads</key>
//             <true/>
//         </dict>";
//        list.AddKey(PlistAdd);
        //     
        //list.ReplaceKey("<string>com.yusong.${PRODUCT_NAME}</string>","<string>"+bundle+"</string>");
        //更改应用名
        //list.ReplaceKey("<string>war3country</string>","<string>孔明没想到</string>");
        //更改区域语言
        //list.ReplaceKey("<string>en</string>", "<string>china</string>");
        //list.Save();

    }

    //修改指定代码文件
    private static void EditorCode(string projectName)
    {
        string sdk_name = SDKManager.CurSDK;
		string unity_full_screen = project_path + "/Classes/Unity/FullScreenVideoPlayer.mm";
		//string unity_full_screen_new = project_path + "/Libraries/Plugins/iOS/FullScreenVideoPlayer.mm";
        string path = Application.dataPath;
        string unity_full_screen_new = path.Replace("/Assets", "/FullScreenVideoPlayer.mm");
		if (System.IO.File.Exists(unity_full_screen) && System.IO.File.Exists(unity_full_screen_new))
		{
			System.IO.File.SetAttributes(unity_full_screen, FileAttributes.Normal);
			System.IO.File.Delete(unity_full_screen);
			System.IO.File.Copy(unity_full_screen_new, unity_full_screen);
		}
		CopyToDir("IOS_BAKE/Xib", project_path);
		CopyToDir("IOS_BAKE/Unity-iPhone", project_path + "/Unity-iPhone");
		CopyToDir("IOS_BAKE/ShareSDK", project_path + "/ShareSDK");

        if (sdk_name == "version_war3g_ios_quick")
        {
            //读取UnityAppController.mm文件
            XClass UnityAppController = new XClass(project_path + "/Classes/UnityAppController.mm");

            //在指定代码后面增加一行代码
            UnityAppController.WriteBelow("#include \"PluginBase/AppDelegateListener.h\"", "#import <SMPCQuickSDK/SMPCQuickSDK.h>");
            UnityAppController.WriteBelow("#import <SMPCQuickSDK/SMPCQuickSDK.h>", "#import \"QuickSDK_ios.h\"");

            //在指定代码中替换一行
            //UnityAppController.Replace("return YES;", "return [ShareSDK handleOpenURL:url sourceApplication:sourceApplication annotation:annotation wxDelegate:nil];");

            //在指定代码后面增加一行
            UnityAppController.WriteBelow("[KeyboardDelegate Initialize];", "    //注册事件监听\n [[QuickSDK_ios shareInstance] addNotifications];\n  //初始化\n     SMPCQuickSDKInitConfigure *cfg = [[SMPCQuickSDKInitConfigure alloc] init];\n    cfg.productKey = @\"18725305\";\n   cfg.productCode = @\"05351757016130194037017341621541\";\n  int error = [[SMPCQuickSDK defaultInstance] initWithConfig:cfg application:application didFinishLaunchingWithOptions:launchOptions];\n  if (error != 0) {\n     NSLog(@\"不能启动初始化： %d\", error);\n   }\n float sysVersion=[[UIDevice currentDevice]systemVersion].floatValue;\n  if (sysVersion>=8.0) {\n    UIUserNotificationType type=UIUserNotificationTypeBadge | UIUserNotificationTypeAlert | UIUserNotificationTypeSound;\n  UIUserNotificationSettings *setting=[UIUserNotificationSettings settingsForTypes:type categories:nil];\n    [[UIApplication sharedApplication]registerUserNotificationSettings:setting];    }");
            UnityAppController.WriteBelow("::printf(\"-> applicationWillEnterForeground()\\n\");", "[[UIApplication sharedApplication]setApplicationIconBadgeNumber:0];//进入前台取消应用消息图标");



            //读取UnityAppController.mm文件
            XClass PlistController = new XClass(project_path + "/Info.plist");
            PlistController.Replace("<key>System.Collections.Hashtable</key>", "<key>NSAppTransportSecurity</key>");
        }

		EditInfoPlist (project_path);
    }

	private static void EditInfoPlist(string projPath)
	{
		//读取UnityAppController.mm文件

		XCPlist plist = new XCPlist (projPath + "/Info.plist");

		//info 添加
		string PlistAdd1 = @"	<key>NSCameraUsageDescription</key> <string>${PRODUCT_NAME}自定义头像需要</string>";
		string PlistAdd2 = @"  	<key>NSPhotoLibraryUsageDescription</key><string>${PRODUCT_NAME}自定义头像需要</string>";
		string PlistAdd3 = @"	<key>NSMicrophoneUsageDescription</key><string>${PRODUCT_NAME}语音聊天需要</string>";
        
        //URL Scheme 添加
		string PlistAdd = @"  
            <key>CFBundleURLTypes</key>
			<array>
				<dict>
					<key>CFBundleURLName</key>
					<string>WX</string>
					<key>CFBundleURLSchemes</key>
					<array>
						<string>wxca7116033db16bdf</string>
					</array>
				</dict>
				<dict>
					<key>CFBundleURLSchemes</key>
					<array>
						<string>alipayruiqugamesniuniu</string>
					</array>
					<key>CFBundleURLName</key>
					<string>Alipay</string>
				</dict>
			</array>";
		//在plist里面增加一行
		plist.AddKey(PlistAdd1);
		plist.AddKey(PlistAdd2);
		plist.AddKey(PlistAdd3);
		plist.AddKey(PlistAdd);
		plist.Save();

		//XClass PlistController = new XClass(projPath + "/Info.plist");
		//PlistController.Replace("<key>System.Collections.Hashtable</key>", "<key>NSAppTransportSecurity</key>");
	}

	public static void CopyToDir(string sFromPath, string sSaveToPath)
	{
		string[] files = Directory.GetFiles(sFromPath, "*.*", SearchOption.AllDirectories);
		//遍历资源文件
		foreach (string file in files)
		{
			if (file.Contains(".meta")) continue;
			if (file.Contains(".svn")) continue;
			if (file.Contains(".manifest")) continue;

			int index = file.IndexOf(sFromPath);
			string toPath = sSaveToPath + file.Substring(index + sFromPath.Length);
			if (File.Exists(toPath))
			{
				System.IO.File.SetAttributes(toPath, FileAttributes.Normal);
				System.IO.File.Delete(toPath);
			}
			string dirPath = Path.GetDirectoryName(toPath);
			if (!Directory.Exists(dirPath))
			{
				Directory.CreateDirectory(dirPath);
			}
			System.IO.File.Copy(file, toPath);
		}
	}

#endif

    public static void Log(string message)
	{
		UnityEngine.Debug.Log("PostProcess: "+message);
	}
}
