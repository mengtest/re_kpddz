/***************************************************************


 *
 *
 * Filename:  	ModelPreProcess.cs	
 * Summary: 	模型导入时预处理：
 *               1、动作FBX文件删除多余的模型和蒙皮，只保留animation并裁剪
 *               2、修改模型文件的animationType为legency
 *              
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/04/09 22:28
 ***************************************************************/

#region define
#define ANIMATION_COMPRESS
#endregion


#region Using
using UnityEngine;
using UnityEditor;
using System.Collections;
using ModelConfig;
using System.Collections.Generic;
using System;
#endregion


public class ModelPreProcess : AssetPostprocessor
{
    public static List<string> _listModels = new List<string>();
    public void OnPreprocessModel()
    {
        ModelImporter mi = (ModelImporter)assetImporter;

        //修正缩放比
       // mi.globalScale = 0.01f;
        //动作类型
        mi.animationType = ModelImporterAnimationType.Legacy;
        mi.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;

        if (assetPath.Contains("/Models/")) {
            //不自动生成材质，有其他工具生成，统一生成管理
            mi.importMaterials = false;
            mi.globalScale = 1.0f;


        }
        else if (assetPath.Contains("/Effects/zhuanshu"))
        {
            mi.globalScale = 1.0f;
        } 
        else if (assetPath.Contains("/UIModel/"))
        {

        } 
        else
        {
            return;
        }
        //关闭动作压缩，会导致动作占用空间增大，但是可以避免产生额外的压缩损伤
#if ANIMATION_COMPRESS
        mi.animationCompression = ModelImporterAnimationCompression.KeyframeReduction;
#else
            mi.animationCompression = ModelImporterAnimationCompression.off;
#endif

        #region modify (Author: @XB.Wu)
        if (ModelDataProcess._dicModelData.Count == 0) {
            ModelDataProcess.excute();
        }
        #endregion

        //如果是纯动作文件则进行裁剪
        //动作文件命名格式:modelName@anim
        if (assetPath.Contains("@")) {
            string[] tempAssetList = assetPath.Split(new char[] { '@' });
            string strAssetModelName = tempAssetList[0].Substring(tempAssetList[0].LastIndexOf('/') + 1);
            if (assetPath.Contains("/Models/") && !_listModels.Contains(strAssetModelName)) {
                _listModels.Add(strAssetModelName);
            }

            ModelData pModelData = ModelDataProcess.getModelDataByIdx(strAssetModelName);
            if (pModelData == null) {
                //EditorUtility.DisplayDialog("Animation Clip", "No Clip config is found in the AnimationClipConfig.ini, and Check the config and make sure its available", "Ok");
                Debug.LogWarning(strAssetModelName + " Clip config can't be found in the AnimationClipConfig.ini, and Check the config and make sure its available");
                return;
            }

            #region modify (Author: @XB.Wu)
            //bool bInit = pModelData.InitFlag;
            //if (bInit)
            //{
            //    //EditorUtility.DisplayDialog("Animation Clip", "This animation's FBX has been imported already. Please delete the previous asset and try again.", "Ok");
            //    Debug.LogWarning("This animation's FBX has been imported already. Please delete the previous asset and try again.");
            //    return;
            //}
            #endregion

            AnimClipInfo info = null;
            ModelImporterClipAnimation[] animations = new ModelImporterClipAnimation[pModelData._listAnimClips.ToArray().Length];
            for (int i = 0; i < animations.Length; i++) {
                info = pModelData._listAnimClips[i];
                animations[i] = SetClipAnimation(info.name, info.firstFrame, info.lastFrame, info.isloop);
            }

            mi.clipAnimations = animations;
            //pModelData.InitFlag = true;
        } else {
            string[] tempAssetList = assetPath.Split(new char[] { '.', 'F', 'B', 'X' });
            int startIdx = assetPath.LastIndexOf('/');
            int endIdx = assetPath.LastIndexOf(".FBX");
            string strAssetModelName = assetPath.Substring(startIdx + 1, endIdx - startIdx - 1);
            if (assetPath.Contains("/Models/") && !_listModels.Contains(strAssetModelName)) {
                _listModels.Add(strAssetModelName);
            }
        }
    }

    ModelImporterClipAnimation SetClipAnimation(string _name, int _first, int _last, bool _isLoop)
    {
        ModelImporterClipAnimation tempClip = new ModelImporterClipAnimation();
        tempClip.name = _name;
        tempClip.firstFrame = _first;
        tempClip.lastFrame = _last;
        tempClip.loop = _isLoop;
        if (_isLoop)
            tempClip.wrapMode = WrapMode.Loop;
        else
            tempClip.wrapMode = WrapMode.Default;

        return tempClip;
    }


    // This method is called immediately after importing an FBX.  
    void OnPostprocessModel(GameObject go)
    {
       ModelImporter mi = (ModelImporter)assetImporter;
       if (assetPath.Contains("/Levels/") && mi.fileScale * mi.globalScale != 0.01f)
        {
            mi.globalScale = 0.01f / mi.fileScale;
        }

        if (!assetPath.Contains("/Models/") && !assetPath.Contains("/UIModel/")) return;

        // Assume an animation FBX has an @ in its name,
        // to determine if an fbx is a character or an animation.
        if (assetPath.Contains("@")) {
            // For animation FBX's all unnecessary Objects are removed.
            // This is not required but improves clarity when browsing assets.

            // Remove SkinnedMeshRenderers and their meshes.
            foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>()) {
                UnityEngine.Object.DestroyImmediate(smr.sharedMesh, true);
                UnityEngine.Object.DestroyImmediate(smr.gameObject);
            }

            // Remove the bones.
            //foreach (Transform o in go.transform)
            //    Object.DestroyImmediate(o);
        }
    }

    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (Environment.UserName == "USER") { return; }

        bool importIni = false;
        bool importModel = false;
        //打包配置
        for (int i = 0; i < importedAssets.Length; i++) {
            if (importedAssets[i].Contains("AnimationClipConfig.ini")) {
                importIni = true;
            }
            if (importedAssets[i].Contains("/Models/")) {
                importModel = true;
            }
        }

        //if (importIni || (_listModels.Count > 0 && importModel)) {
        //    CreateModelAssertBundleWin window = (CreateModelAssertBundleWin)EditorWindow.GetWindow(typeof(CreateModelAssertBundleWin));
        //    window.bImportIni = importIni;
        //    window.initialize();
        //}
    }
}
