using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Editor常用变量 
/// </summary>
public class EditorVariables
{
	/// <summary>
    /// 创建Assetbundle选项
	/// </summary>
	public static BuildAssetBundleOptions eBuildABOpt;

	/// <summary>
    /// 创建Assetbundle目标
	/// </summary>
	public static BuildTarget eBuildTarget;


    //静态构造函数
	static EditorVariables()
	{
#if UNITY_IPHONE
        eBuildTarget = BuildTarget.iOS;
#elif UNITY_ANDROID
        eBuildTarget = BuildTarget.Android;
#elif UNITY_STANDALONE_WIN
        eBuildTarget = BuildTarget.StandaloneWindows;
#endif

        eBuildABOpt = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.CompleteAssets;
	}

}
