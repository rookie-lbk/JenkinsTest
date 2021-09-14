using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

public partial class BuildWindows : EditorWindow
{
    public static void ExportAppByGit()
    {
        Debug.Log("------------------ExportAppByGit--------------------");
        string[] args = System.Environment.GetCommandLineArgs();
        int count = args.Length;

        string env = "";
        for (int i = 0; i < count; i++)
        {
            Debug.Log(args[i]);
            if (args[i] == "-env")
            {
                env = args[i + 1];
            }
        }
        //设置平台
        BuildTarget buildTarget = BuildTarget.Android;
        AssetDatabase.Refresh();

        string SavePath = Application.dataPath + "/../../../../SaveData/" + env;
        try
        {
            if (!Directory.Exists(SavePath))
            {
                Directory.CreateDirectory(SavePath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        PlayerSettingsConfig();

        string[] BUILD_SCENES = new string[] { "Assets/Scenes/LuaMainScene.unity" };
        string BaseDefinition = "ENABLE_IL2CPP;THREAD_SAFE;BESTHTTP_DISABLE_SERVERSENT_EVENTS;BESTHTTP_DISABLE_SIGNALR;"
           + "BESTHTTP_DISABLE_UNITY_FORM;BESTHTTP_DISABLE_ALTERNATE_SSL;BESTHTTP_DISABLE_SOCKETIO;BESTHTTP_DISABLE_CACHING";//开发所用宏

        string ResFromAB = "RES_FROM_AB";
        string LuaFromAB = "LUA_FROM_AB";
        string DownLoadAB = "DOWNLOAD_AB";

        BuildOptions _BuildOptions = BuildOptions.None;
        string def = BaseDefinition + ";" + ResFromAB + ";" + LuaFromAB + ";" + DownLoadAB;

        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, def);

        string exportPath = Application.dataPath + "/../OutBuild" + "/" + PlayerSettings.productName + ".apk";

        _BuildOptions = BuildOptions.Development | BuildOptions.ConnectWithProfiler | BuildOptions.AllowDebugging;

        AssetDatabase.Refresh();
        Debug.Log("on start build player " + PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android));
        bool isSuccess = false;

        try
        {
            if (File.Exists(exportPath))
            {
                File.Delete(exportPath);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        try
        {
            //如果存在目录文件，就将其目录文件删除 
            if (Directory.Exists(exportPath))
            {
                Directory.Delete(exportPath, true);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        try
        {
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;

            EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.Generic;
            EditorUserBuildSettings.androidETC2Fallback = AndroidETC2Fallback.Quality16Bit;
            BuildReport result = BuildPipeline.BuildPlayer(BUILD_SCENES, exportPath, buildTarget, _BuildOptions);
            if (result.summary.result != BuildResult.Succeeded)
            {
                Debug.Log("Build___失败" + result);
                EditorUtility.DisplayDialog("Error", "Build___失败" + result, "ok");
                isSuccess = false;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log("Build___失败" + e);
            EditorUtility.DisplayDialog("Error", "Build___失败" + e, "ok");
            isSuccess = false;
        }

        Debug.Log("isSuccess " + isSuccess);

        Debug.Log("------------------ExportAppByGit-----over---------------");
    }

    public static void PlayerSettingsConfig()
    {

        string bundleVersion = string.Format("{0}.{1}.0", 1, 1);
        string companyName = "dd";
        int bundleVersionCode = 1;
        BuildTargetGroup target = BuildTargetGroup.Android;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        PlayerSettings.productName = "UnityProjectDebug";
        PlayerSettings.SetScriptingBackend(target, ScriptingImplementation.Mono2x);
        PlayerSettings.SetApplicationIdentifier(target, "com.dd.UnityProject");

        PlayerSettings.SplashScreen.show = false;
        //PlayerSettings.useSecurityBuild = true;
        PlayerSettings.protectGraphicsMemory = false;//启用后，如果设备和平台支持，则图形内存将免受外部读取影响。 这将确保用户无法截图。 在Android上，这需要一个支持EGL_PROTECTED_CONTENT_EXT扩展的设备
        PlayerSettings.accelerometerFrequency = 60;//ios  //加速度计更新频率。注意：构建时选项。 如果在应用程序已经运行时更改，则不起作用。
        //- PlayerSettings.actionOnDotNetUnhandledException //设置.NET未处理异常的崩溃行为。选项是ActionOnDotNetUnhandledException.Crash（应用程序几乎崩溃并强制iOS生成可由应用程序用户提交给iTunes并由开发人员检查的崩溃报告）和ActionOnDotNetUnhandledException.Silent Exit（应用程序正常退出）。
        //-PlayerSettings.advancedLicense //是否使用了高级版本？Unity可以个人或专业版的形式提供。 在Professional版本上运行时，advancedLicense将返回true。
        PlayerSettings.allowFullscreenSwitch = false;//如果启用，则允许用户使用操作系统特定的键盘快捷方式在全屏和窗口模式之间切换
                                                     //PlayerSettings.aotOptions -//其他AOT编译选项。 由AOT平台共享。
                                                     //PlayerSettings.applicationIdentifier = applicationIdentifier;   //包名
        PlayerSettings.bakeCollisionMeshes = true; //预烘烤碰撞网格在玩家身上。
        PlayerSettings.bundleVersion = bundleVersion; //iOS和Android平台之间共享的应用程序包版本。
        //--PlayerSettings.captureSingleScreen = false; //定义全屏游戏是否应使辅助显示屏变暗。
        PlayerSettings.colorSpace = ColorSpace.Gamma; //设置当前项目的渲染颜色空间。用线性空间(Color Space)的一个显着优点是，
                                                      //随着光强度的增加，提供给场景中的着色器的颜色会线性地变亮。而“伽玛(Camma)”色彩空间，随着数值上升时，亮度将迅速增强直至变成白色，这对图像质量是不利的。线性颜色空间支持PC和一些最新的移动设备上。
        PlayerSettings.companyName = companyName;
        //PlayerSettings.productName = productName;
        //PlayerSettings.d3d11FullscreenMode = D3D11FullscreenMode.ExclusiveMode;// win FullscreenWindow
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.AutoRotation;
        PlayerSettings.useAnimatedAutorotation = false;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.fullScreenMode = FullScreenMode.FullScreenWindow;//全屏
        //启用CrashReport API。启用自定义崩溃记录器来捕获崩溃。 通过CrashReport API可以为脚本提供崩溃日志。

        PlayerSettings.enableInternalProfiler = false;//启用内部分析器。启用内部分析器，收集应用程序的性能数据并将报告输出到控制台。 该报告包含每个Unity子系统在每个帧上执行的毫秒数。 数据平均跨30帧。
        // PlayerSettings.forceSingleInstance = false; //win将独立播放器限制为单个并发运行实例。这会在启动时检测同一个播放器的另一个实例是否已在运行，如果是，则会以错误消息中止。

        PlayerSettings.gpuSkinning = true; //在有能力的平台上启用GPU皮肤。以下支持GPU上的网格蒙皮：DX11，DX12，OpenGL ES 3.0，Xbox One，PS4，Nintendo Switch和Vulkan（Windows，Mac和Linux）
        //PlayerSettings.graphicsJobMode = GraphicsJobMode.Native; //不敢用 选择图形作业模式以在支持Native和Legacy图形作业的平台上使用。
        PlayerSettings.graphicsJobs = true; //启用图形作业（多线程渲染）。这使得渲染代码可以在多核心机器上的多个核心上并行分割和运行。
        //PlayerSettings.keyaliasPass
        PlayerSettings.logObjCUncaughtExceptions = false;//ObjC未捕获的异常是否被记录？启用自定义的Objective - C未捕获异常处理程序，该处理程序将向控制台打印异常信息。
        PlayerSettings.MTRendering = true; //是否启用了多线程渲染？
        PlayerSettings.muteOtherAudioSources = false;//true停止其他应用程序的音频在Unity应用程序运行时在后台播放。
        //PlayerSettings.preserveFramebufferAlpha = true;//不敢用 启用后，在帧缓冲区中保留alpha值以支持在Android上通过本机UI进行渲染。

        PlayerSettings.runInBackground = true; //默认是false 如果启用，您的游戏将在失去焦点后继续运行。
        PlayerSettings.statusBarHidden = true;//如果状态栏应该隐藏，则返回。 仅在iOS上支持; 在Android上，状态栏始终隐藏。
        PlayerSettings.stripEngineCode = false; //从您的版本中删除未使用的引擎代码（仅限IL2CPP）。如果启用此功能，则在IL2CPP版本中将删除未使用的Unity Engine 代码库的模块和类。 这将导致更小的二进制大小。 建议使用此设置，但是，如果您怀疑这会导致项目出现问题，则可能需要禁用该设置。请注意，托管程序集的字节码剥离始终对IL2CPP脚本后端启用。PlayerSettings.strippingLevel = StrippingLevel.Disabled; //托管代码剥离级别。
        PlayerSettings.SetManagedStrippingLevel(target, ManagedStrippingLevel.Disabled);
        //keep loaded shaders alive = true
        PlayerSettings.stripUnusedMeshComponents = false;//是否应该从游戏构建中排除未使用的Mesh组件？当此设置打开时，未使用的网格组件（例如切线矢量，顶点颜色等）将被删除。 这有利于游戏数据大小和运行时性能。
        PlayerSettings.use32BitDisplayBuffer = true;//使用32位显示缓冲区。
        // PlayerSettings.useAnimatedAutorotation = false;//随着设备方向改变，让操作系统自动旋转屏幕。
        PlayerSettings.useHDRDisplay = false;//将显示切换到HDR模式（如果可用）。
        //PlayerSettings.useMacAppStoreValidation = true; //不敢设置
        PlayerSettings.usePlayerLog = false; //用调试信息写一个日志文件。如果您的游戏存在问题，这对了解发生了什么很有用。 在为Apple的Mac App Store发布游戏时，建议关闭它，因为Apple可能会拒绝您的提交。
                                             //PlayerSettings.virtualRealitySplashScreen //虚拟现实特定的启动画面。

        PlayerSettings.allowUnsafeCode = true;
        #region vr
        PlayerSettings.virtualRealitySupported = false; //在当前构建目标上启用虚拟现实支持。
        #endregion
        PlayerSettings.visibleInBackground = false; //Windows上，如果使用全屏窗口模式，则在后台显示应用程序。

        PlayerSettings.Android.androidTVCompatibility = false;
        PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.Auto;



        PlayerSettings.Android.startInFullscreen = true;

        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel22;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel30;
        PlayerSettings.Android.forceSDCardPermission = false;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, true);
        PlayerSettings.Android.forceInternetPermission = true;
        PlayerSettings.Android.disableDepthAndStencilBuffers = false;
        PlayerSettings.Android.androidIsGame = true;
        PlayerSettings.Android.useAPKExpansionFiles = false;
        PlayerSettings.Android.bundleVersionCode = bundleVersionCode;
        PlayerSettings.Android.ARCoreEnabled = false;
        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.iOS, true);
        PlayerSettings.iOS.requiresFullScreen = true;
        //PlayerSettings.iOS.appleEnableAutomaticSigning =  isDebug;
        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.iOS, ApiCompatibilityLevel.NET_4_6);
        PlayerSettings.iOS.disableDepthAndStencilBuffers = false;
        PlayerSettings.iOS.hideHomeButton = false;
        PlayerSettings.iOS.deferSystemGesturesMode = UnityEngine.iOS.SystemGestureDeferMode.All;
        PlayerSettings.iOS.allowHTTPDownload = true;
        PlayerSettings.iOS.targetDevice = iOSTargetDevice.iPhoneAndiPad;
        PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
        PlayerSettings.iOS.targetOSVersionString = "10.0";
        PlayerSettings.iOS.buildNumber = bundleVersionCode.ToString();
        EditorUserBuildSettings.buildAppBundle = false;
    }
}
