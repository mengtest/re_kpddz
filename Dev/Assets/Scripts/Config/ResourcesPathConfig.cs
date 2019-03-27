/***************************************************************


 *
 *
 * Filename:  	ResourcesPathConfig.cs	
 * Summary: 	游戏中的资源路径配置
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/04/17 4:10
 ***************************************************************/



using UnityEngine;
using System.Collections;

//资源路径配置
public class ResourcesPathConfig{
    public static string MODELS     = "Models";
    public static string TEXTURES   = "Textures";
    public static string MATERIALS  = "Materials";
    public static string CONFIG     = "Config";
    public static string EFFECTS    = "Effects";
    public static string PREFABS    = "Prefabs";
    public static string FONTS      = "Fonts";
    public static string UITEXTURES = "UITextures";

    //只读路径
    //即SreamingAssets路径
    public static string READ_ONLY_PATH = Application.streamingAssetsPath;
}
