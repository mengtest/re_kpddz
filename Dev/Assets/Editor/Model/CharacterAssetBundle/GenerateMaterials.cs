using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

class GenerateMaterials
{
    // As each SkinnedMeshRenderer that make up a character can be
    // textured with several textures, we cant use the materials
    // Unity generates. This method creates a material for each texture
    // which name contains the name of any SkinnedMeshRenderer present
    // in a any selected character fbx's.
    [MenuItem("Model/模型生成/生成材质")]
    static void Execute()
    {
        bool validMaterial = false;
        //Texture2D causticTex = EditorHelpers.Collect<Texture2D>(TexturesPath(characterFbx));
        Texture2D causticTex = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Resources/Textures/caustic_01.png", typeof(Texture2D));
        foreach (Object o in Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets))
        {
            if (!(o is GameObject)) continue;
            if (o.name.Contains("@")) continue;
            if (!AssetDatabase.GetAssetPath(o).Contains("/Models/")) continue;

            GameObject characterFbx = (GameObject)o;

            // Create directory to store generated materials.
            if (!Directory.Exists(MaterialsPath(characterFbx)))
                Directory.CreateDirectory(MaterialsPath(characterFbx));

            // Collect all textures.
            List<Texture2D> textures = EditorHelpers.CollectAll<Texture2D>(TexturesPath(characterFbx));

            // Create materials for each SkinnedMeshRenderer.
            foreach (SkinnedMeshRenderer smr in characterFbx.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                // Check if this SkinnedMeshRenderer has a normalmap.
                Texture2D normalmap = null;
                foreach (Texture2D t in textures)
                {
                    if (!t.name.ToLower().Contains("normal")) continue;
                    if (!t.name.ToLower().Contains(smr.name.ToLower())) continue;
                    normalmap = t;
                    break;
                }

                // Create a Material for each texture which name contains
                // the SkinnedMeshRenderer name.
                foreach (Texture2D t in textures)
                {
                    if (t.name.ToLower().Contains("normal")) continue;
                    if (!t.name.ToLower().Contains(smr.name.ToLower())) continue;
                
                    validMaterial = true;
                    string materialPath = MaterialsPath(characterFbx) + "/" + t.name + ".mat";

                    // Dont overwrite existing materials, as we would
                    // lose existing material settings.
                    if (File.Exists(materialPath)) continue;

                    // Use a default shader according to artist preferences.
                    string shader = "Custom/CommonFish";//"Render Depth";// "Mobile/Unlit (Supports Lightmap)";// "Custom/NLitTextureColorAlpha";
                    if (normalmap != null) shader = "Custom/CommonFish";//"Render Depth";//"Mobile/Unlit (Supports Lightmap)";//"Custom/NLitTextureColorAlpha";
                    if (smr.name.Equals("11023") || smr.name.Equals("12003") || smr.name.Equals("13009") || smr.name.Equals("13017"))
                    {
                        //水母用特殊材质
                        shader = "Custom/NLitTextureColorAlpha";
                        // Create the Material
                        Material m = new Material(Shader.Find(shader));
                        m.SetTexture("_MainTex", t);
                        if (normalmap != null) m.SetTexture("_BumpMap", normalmap);
                        AssetDatabase.CreateAsset(m, materialPath);
                    }
                    else
                    {
                        //鱼用变通材质
                        // Create the Material
                        Material m = new Material(Shader.Find(shader));
                        m.SetTexture("_mainTex", t);
                        m.SetTexture("_causticTex", causticTex);
                        m.SetFloat("_intencity", 0.7f);
                        m.SetFloat("_scrollSpeedY", -0.2f);
                        //m.DisableKeyword("USING_CAUSTIC_OFF");
                        m.EnableKeyword("USING_CAUSTIC");
                        if (normalmap != null) m.SetTexture("_BumpMap", normalmap);
                        AssetDatabase.CreateAsset(m, materialPath);

                    }


                }
            }
        }
        AssetDatabase.Refresh();
        
        if (!validMaterial) 
            EditorUtility.DisplayDialog("Character Generator", "No Materials created. Select the characters folder in the Project pane to process all characters. Select subfolders to process specific characters.", "Ok");
    }

    // Returns the path to the directory that holds the specified FBX.
    static string CharacterRoot(GameObject character)
    {
        string root = AssetDatabase.GetAssetPath(character);
        return root.Substring(0, root.LastIndexOf('/') + 1);
    }

    // Returns the path to the directory that holds materials generated
    // for the specified FBX.
    public static string MaterialsPath(GameObject character)
    {
		// we will use it only for file enumeration, and separator will be appended for us
		// if we do append here, AssetDatabase will be confused
        string strFBXName = character.name;
        return "Assets/Resources/Materials/" + strFBXName;
    }

    public static string TexturesPath(GameObject character)
    {
        string strFBXName = character.name;
        return "Assets/Resources/Textures/" + strFBXName;
    }
}