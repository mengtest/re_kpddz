/***************************************************************


 *
 *
 * Filename:  	AnimationClipConfig.cs	
 * Summary: 	生成模型数据的asset和assetbundle
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/04/09 23:16
 ***************************************************************/

#region Using
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using ModelConfig;
using BasicDataScructs;
#endregion


#region typedef
using AnimClipsList = System.Collections.Generic.List<ModelConfig.AnimClipInfo>;
using AnimEventsList = System.Collections.Generic.List<BasicDataScructs.AnimEventInfo>;
using ModelBpsList = System.Collections.Generic.List<string>;
using ModelDic = System.Collections.Generic.Dictionary<string, ModelConfig.ModelData>;
using AnimEventsDic = System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<BasicDataScructs.AnimEventInfo>>;
using customerPath;
#endregion


namespace ModelDataExport
{   
   
    public class ModelDataAssetAndCreateAssetBundle
    {
        [MenuItem("Model/模型数据/第2步-导出(Asset和Assetbundle)")]
        public static void excute()
        {   
            //创建实例
            GameModelDataHolder go = ScriptableObject.CreateInstance<GameModelDataHolder>();

            ModelDataProcess.excute();

            ModelDic dictModelData = ModelDataProcess._dicModelData;
            foreach (KeyValuePair<string, ModelData> kvp in dictModelData)
            {
                GameModelData pData = new GameModelData();
                pData._strAssetName = kvp.Value._strAssetName;
                pData._strAnimationAssetName = kvp.Value._strAnimationAssetName;
                pData._strModelName = kvp.Value._strModelName;
                pData._listAnimEvents = kvp.Value._listAnimEvents;
                pData._listModelBps = kvp.Value._listModelBps;
                pData._listAnimations = kvp.Value._listAnimations;
                pData.nColliderType = kvp.Value._nColliderType;
                pData.nColliderParams = kvp.Value._nColliderParams;
                pData.colliderCenter = kvp.Value._nColliderCenter;

                Debug.Log(kvp.Key);

                go._listGameModel.Add(pData);
            }

            //生成asset
            string p = "Assets/Resources/GameModelData/GameModelData.asset";
            AssetDatabase.CreateAsset(go, p);

            string assetbundle_path = GameDataAssetbundlePath + IPath.getPlatformName() + "/modelsassetbundles";
            //创建路径
            if (!Directory.Exists(assetbundle_path))
                Directory.CreateDirectory(assetbundle_path);

            //打包
            UnityEngine.Object o = AssetDatabase.LoadAssetAtPath(p, typeof(GameModelDataHolder));
            BuildPipeline.BuildAssetBundle(o, null, assetbundle_path + "/gamemodeldata.assetbundle", EditorVariables.eBuildABOpt, EditorVariables.eBuildTarget);

            //删除临时的asset
            //AssetDatabase.DeleteAsset(p);
        }

        //路径
        public static string GameDataAssetbundlePath
        {
            get { return Application.dataPath + "/StreamingAssets/"; }
        }

    }

}


