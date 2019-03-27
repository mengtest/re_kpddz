/***************************************************************


 *
 *
 * Filename:  	CreateAssetBundle.cs	
 * Summary: 	创建模型各个部件的assetbundle<SkinMeshRender>,主要是蒙皮
 *              和骨骼信息
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/04/10 5:21
 ***************************************************************/

#region define
//是否打包材质
#define COLLECT_ELEMENTS_MATERIALS  
#undef COLLECT_ELEMENTS_MATERIALS

//材质assetbundle
#define CREATE_MATERIALS_ASSETBUNDLE
#undef CREATE_MATERIALS_ASSETBUNDLE
#endregion


#region Using
using customerPath;
using ModelConfig;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
#endregion


public class CreateAssetBundle
{

    public static string _strCurrentModelName = "";


    // This method creates an assetbundle of each SkinnedMeshRenderer
    // found in any selected character fbx, and adds any materials that
    // are intended to be used by the specific SkinnedMeshRenderer.
    [MenuItem("Model/模型生成/生成模型Assetbundle")]
    public static void excute_models()
    {
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            excute_single_model(o);
        }
    }

    public static void excute_single_model(Object o) {
        if (!(o is GameObject)) return;
        if (o.name.Contains("@")) return; //animations is processed seperated later
        if (!AssetDatabase.GetAssetPath(o).Contains("/Models/")) return;

        GameObject characterFBX = (GameObject)o;
        string name = characterFBX.name;
        _strCurrentModelName = name;

        Debug.Log("******* Creating assetbundles for: " + name + " *******");

        // Create a directory to store the generated assetbundles.
        if (!Directory.Exists(GameModelAssetbundlePath))
            Directory.CreateDirectory(GameModelAssetbundlePath);

        // Delete existing assetbundles for current character.
        string[] existingAssetbundles = Directory.GetFiles(GameModelAssetbundlePath);
        foreach (string bundle in existingAssetbundles) {
            if (bundle.EndsWith(".assetbundle") && !bundle.Contains("@"))
                File.Delete(bundle);
        }

        // Save bones and animations to a seperate assetbundle. Any 
        // possible combination of CharacterElements will use these
        // assets as a base. As we can not edit assets we instantiate
        // the fbx and remove what we dont need. As only assets can be
        // added to assetbundles we save the result as a prefab and delete
        // it as soon as the assetbundle is created.
        GameObject characterClone = (GameObject)Object.Instantiate(characterFBX);

        // postprocess animations: we need them animating even offscreen
        foreach (Animation anim in characterClone.GetComponentsInChildren<Animation>())
            anim.cullingType = AnimationCullingType.BasedOnRenderers;

        foreach (SkinnedMeshRenderer smr in characterClone.GetComponentsInChildren<SkinnedMeshRenderer>())
            Object.DestroyImmediate(smr.gameObject);

        characterClone.AddComponent<SkinnedMeshRenderer>();
        Object characterBasePrefab = GetPrefab(characterClone, "base");
        string path = GameModelAssetbundlePath + "base.assetbundle";

		AssetImporter aim = AssetImporter.GetAtPath ("Assets/base.prefab");
		aim.assetBundleName = _strCurrentModelName + "base";

		AssetBundleBuild abb = new AssetBundleBuild ();
		abb.assetBundleName = aim.assetBundleName;

		BuildPipeline.BuildAssetBundles (path, new AssetBundleBuild[]{ abb }, EditorVariables.eBuildABOpt, EditorVariables.eBuildTarget);

        BuildPipeline.BuildAssetBundle(characterBasePrefab, null, path, EditorVariables.eBuildABOpt, EditorVariables.eBuildTarget);
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(characterBasePrefab));

        // Collect materials.
        //List<Material> materials = EditorHelpers.CollectAll<Material>(GenerateMaterials.MaterialsPath(characterFBX));

        // Create assetbundles for each SkinnedMeshRenderer.
        foreach (SkinnedMeshRenderer smr in characterFBX.GetComponentsInChildren<SkinnedMeshRenderer>(true)) {
            List<Object> toinclude = new List<Object>();

            // Save the current SkinnedMeshRenderer as a prefab so it can be included
            // in the assetbundle. As instantiating part of an fbx results in the
            // entire fbx being instantiated, we have to dispose of the entire instance
            // after we detach the SkinnedMeshRenderer in question.
            GameObject rendererClone = (GameObject)PrefabUtility.InstantiatePrefab(smr.gameObject);
            GameObject rendererParent = rendererClone.transform.parent.gameObject;
            rendererClone.transform.parent = null;
            Object.DestroyImmediate(rendererParent);
            Object rendererPrefab = GetPrefab(rendererClone, "rendererobject");
            toinclude.Add(rendererPrefab);

            // Don't Collect applicable materials, as the elements materials is constantly 
            // changing in different level. 
#if COLLECT_ELEMENTS_MATERIALS
                foreach (Material m in materials)
                {
                    if (m.name.Contains(smr.name)) toinclude.Add(m);
                }
#endif

            // When assembling a character, we load SkinnedMeshRenderers from assetbundles,
            // and as such they have lost the references to their bones. To be able to
            // remap the SkinnedMeshRenderers to use the bones from the characterbase assetbundles,
            // we save the names of the bones used.
            List<string> boneNames = new List<string>();
            foreach (Transform t in smr.bones)
                boneNames.Add(t.name);
            string stringholderpath = "Assets/bonenames.asset";

            StringHolder holder = ScriptableObject.CreateInstance<StringHolder>();
            holder.content = boneNames.ToArray();
            AssetDatabase.CreateAsset(holder, stringholderpath);
            toinclude.Add(AssetDatabase.LoadAssetAtPath(stringholderpath, typeof(StringHolder)));

            // Save the assetbundle.
            string bundleName = smr.name;
            path = GameModelAssetbundlePath + bundleName + ".assetbundle";
            BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, EditorVariables.eBuildABOpt, EditorVariables.eBuildTarget);
            Debug.Log("Saved " + bundleName + " with " + (toinclude.Count - 2) + " materials");

            // Delete temp assets.
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rendererPrefab));
            AssetDatabase.DeleteAsset(stringholderpath);
        }
    }


    [MenuItem("Model/模型生成/生成动作Assetbundle(不用)")]
    public static void excute_animations()
    {
        foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            if (!(obj is GameObject)) continue;
            if ((obj is Texture2D) || (obj is Material)) continue;

            //object path
            string strAssetPath = AssetDatabase.GetAssetPath(obj);
            if (!strAssetPath.Contains("/Models/")) continue;

            //set current model name
            string[] arrStr = obj.name.Split(new char[] { '@' });
            _strCurrentModelName = arrStr[0];

            // Create a directory to store the generated assetbundles.
            if (!Directory.Exists(GameModelAssetbundlePath))
                Directory.CreateDirectory(GameModelAssetbundlePath);

            // Delete existing animation assetbundles for current character.
            string[] existingAssetbundles = Directory.GetFiles(GameModelAssetbundlePath);
            foreach (string bundle in existingAssetbundles)
            {
                if (bundle.EndsWith(".assetbundle") && bundle.Contains("@"))
                    File.Delete(bundle);
            }

            if (obj.name.Contains("@"))
            {
                string strTargetAssetBundlePath = GameModelAssetbundlePath + obj.name + ".assetbundle";
                BuildPipeline.BuildAssetBundle(obj, null, strTargetAssetBundlePath, EditorVariables.eBuildABOpt, EditorVariables.eBuildTarget);
            }
        }
    }

#if CREATE_MATERIALS_ASSETBUNDLE
    [MenuItem("Model/模型生成/生成材质Assetbundle(不用)")]
    public static void excute_materials()
    {
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            if (!(o is GameObject)) continue;
            if (o.name.Contains("@"))
            {
               
            }
        }
    }
#endif

    static Object GetPrefab(GameObject go, string name)
    {
        Object tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/" + name + ".prefab");
        tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);
        Object.DestroyImmediate(go);
        return tempPrefab;
    }


    public static string GameModelAssetbundlePath
    {
        get
        {
            //string platformName = IPath.getPlatformName();
            //return Application.streamingAssetsPath + "/" + platformName + "/modelsassetbundles/" + _strCurrentModelName + Path.DirectorySeparatorChar;
            return IPath.streamingAssetsPathPlatform() + "/modelsassetbundles/" + _strCurrentModelName + Path.DirectorySeparatorChar;
        }
    }

    public static string GameModelAssetPath
    {
        get { return Application.dataPath + "/Resources/ModelAsset/" + _strCurrentModelName + Path.DirectorySeparatorChar; }
    }
}

public class CreateModelAssertBundleWin : EditorWindow {
    public bool bImportIni = false;

    /// <summary>
    /// 初始化
    /// </summary>
    public void initialize() {

    }

    void OnGUI() {
        

        //分隔线
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        if (ModelPreProcess._listModels.Count > 0) {
            EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.Width(350), GUILayout.Height(140));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("模型");
            EditorGUILayout.EndHorizontal();
            //分隔线
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();

            EditorGUILayout.BeginVertical();
            for (int i = 0; i < ModelPreProcess._listModels.Count; i++) {
                string strAssetModelName = ModelPreProcess._listModels[i];
                ModelData pModelData = ModelDataProcess.getModelDataByIdx(strAssetModelName);
                if (pModelData != null) {
                    
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(strAssetModelName);
                    EditorGUILayout.LabelField(pModelData._strModelName);
                    EditorGUILayout.EndHorizontal();
                } 
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            //分隔线
            EditorGUILayout.Separator();
            EditorGUILayout.Separator();
        } 
        
        

        if (bImportIni) {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("ini配置");
            EditorGUILayout.EndHorizontal();
        } 
        

        //分隔线
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        if (GUILayout.Button("打包", GUILayout.Width(100), GUILayout.ExpandWidth(true))) {
            //打包模型
            for (int i = 0; i < ModelPreProcess._listModels.Count; i++) {
                Utils.LogSys.LogWarning(ModelPreProcess._listModels[i]);
                string name = ModelPreProcess._listModels[i];
                UnityEngine.Object o = Resources.Load("Models/" + name);
                //CreateAssetBundle.excute_single_model(o);
				customer.collectModel (o);
            }

            if (bImportIni) {
                //ModelDataExport.ModelDataAssetAndCreateAssetBundle.excute();
				customer.collectAllAssetbundles ();
            }
            ModelPreProcess._listModels.Clear();
            ModelDataProcess._dicModelData.Clear();
            Close();
            AssetDatabase.Refresh();
        }
        //分隔线
        EditorGUILayout.Separator();
        EditorGUILayout.Separator();
        if (GUILayout.Button("取消", GUILayout.Width(100), GUILayout.ExpandWidth(true))) {
            //打包模型
            if (EditorUtility.DisplayDialogComplex("警告", "确定不打包该资源?若要重新打包，请先删除模型再导入。", "确定", "取消", "") == 0) {
                ModelPreProcess._listModels.Clear();
                ModelDataProcess._dicModelData.Clear();
                Close();
            }
        }

        //if (GUILayout.Button("取消", GUILayout.Width(100), GUILayout.ExpandWidth(true))) {
        //    ModelPreProcess._listModels.Clear();
        //    ModelDataProcess._dicModelData.Clear();
        //}
    }
}
