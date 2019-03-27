#region

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

[CustomEditor(typeof(UITemplate))]
public class UITemplateInspector : Editor
{
    //------------------------------------------------------------//


    //模板存放的路径
    private const string UITemplatePath = "Assets/Resources/UI/UITemplate";


    //Prefab存放的路径
    private const string Forder = "Assets/Resources/UI";


    private UITemplate _uiTemplate;


    //------------------------------------------------------------//


    [MenuItem("GameObject/UITemplate/Creat To Prefab", false, 11)]
    private static void CreatToPrefab(MenuCommand menuCommand)
    {
        if (menuCommand.context != null)
        {
            CreatDirectory();
            var selectGameObject = menuCommand.context as GameObject;

            if (IsTemplatePrefabInHierarchy(selectGameObject))
            {
                CreatPrefab(selectGameObject);
            }
            else
            {
                CreatPrefab(selectGameObject);
                DestroyImmediate(selectGameObject);
            }
        }
        else
        {
            EditorUtility.DisplayDialog("错误！", "请选择一个GameObject", "OK");
        }
    }


    private void OnEnable()
    {
        _uiTemplate = (UITemplate)target;
        if (IsTemplatePrefabInInProjectView(_uiTemplate.gameObject))
        {
            ShowHierarchy();
        }
        CreatDirectory();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var isPrefabInProjectView = IsTemplatePrefabInInProjectView(_uiTemplate.gameObject);
        EditorGUILayout.LabelField("GUID:" + _uiTemplate.GUID);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Select"))
        {
            var directiory = CreatDirectory();
            var infos = directiory.GetFiles("*.prefab", SearchOption.AllDirectories);
            foreach (var file in infos)
            {
                var prefab = AssetDatabase.LoadAssetAtPath(file.FullName.Substring(file.FullName.IndexOf("Assets", StringComparison.Ordinal)), typeof(GameObject)) as GameObject;
                if (!prefab) return;
                if (prefab.GetComponent<UITemplate>().GUID != _uiTemplate.GUID) continue;
                EditorGUIUtility.PingObject(prefab);
                return;
            }
        }

        if (!isPrefabInProjectView) {
            if (GUILayout.Button("AdjustScale")) {
                var directiory = CreatDirectory();
                var infos = directiory.GetFiles("*.prefab", SearchOption.AllDirectories);
                foreach (var file in infos) {
                    var prefab = AssetDatabase.LoadAssetAtPath(file.FullName.Substring(file.FullName.IndexOf("Assets", StringComparison.Ordinal)), typeof(GameObject)) as GameObject;
                    if (!prefab) return;
                    if (prefab.GetComponent<UITemplate>().GUID != _uiTemplate.GUID) continue;
                    //重设scale
                    AdjustPrefab(prefab, _uiTemplate.gameObject);
                    break;
                }
            }
        } 
        

        if (isPrefabInProjectView)
        {
            if (GUILayout.Button("Search"))
            {
                TrySearchPrefab(_uiTemplate.GUID, out _uiTemplate.searPrefabs);
                return;
            }

            if (GUILayout.Button("Apply"))
            {
                if (IsTemplatePrefabInHierarchy(_uiTemplate.gameObject))
                {
                    ApplyPrefab(_uiTemplate.gameObject, PrefabUtility.GetPrefabParent(_uiTemplate.gameObject), true);
                }
                else
                {
                    ApplyPrefab(_uiTemplate.gameObject, _uiTemplate.gameObject, false);
                }
                return;
            }

            if (GUILayout.Button("Delete"))
            {
                DeletePrefab(IsTemplatePrefabInHierarchy(_uiTemplate.gameObject)
                    ? GetPrefabPath(_uiTemplate.gameObject)
                    : AssetDatabase.GetAssetPath(_uiTemplate.gameObject));
                return;
            }
        }

        GUILayout.EndHorizontal();
        if (!isPrefabInProjectView || (_uiTemplate == null || _uiTemplate.searPrefabs.Count <= 0)) return;
        EditorGUILayout.LabelField("Prefab :" + _uiTemplate.name);

        foreach (var p in _uiTemplate.searPrefabs)
        {
            EditorGUILayout.Space();
            if (!GUILayout.Button(AssetDatabase.GetAssetPath(p))) continue;
            EditorGUIUtility.PingObject(p);
        }
    }

    private static bool TrySearchPrefab(int guid, out List<GameObject> searchList)
    {
        var prefabs = new List<GameObject>();
        var trySearch = false;

        var directiory = new DirectoryInfo(Application.dataPath + "/" + Forder.Replace("Assets/", ""));
        var infos = directiory.GetFiles("*.prefab", SearchOption.AllDirectories);
        foreach (var file in infos)
        {
            if (file.FullName.Contains("Template")||file.FullName.Contains("dropItem")) continue;
            var prefab = (GameObject)AssetDatabase.LoadAssetAtPath(file.FullName.Substring(file.FullName.IndexOf("Assets", StringComparison.Ordinal)), typeof(GameObject));
            var list = prefab.transform.GetComponentsInChildren<UITemplate>(true);
            if (list.Length <= 0) continue;
            var go = (GameObject)Instantiate(prefab);
            var templates = go.transform.GetComponentsInChildren<UITemplate>(true);
            foreach (var template in templates)
            {
                if (template.GetComponentsInChildren<UITemplate>(true).Length > 1)
                {
                    Debug.LogError(file.FullName + " 模板 " + template.name + " 进行了嵌套的错误操作~请删除重试");
                    if (!trySearch)
                        trySearch = true;
                }
                else
                {
                    if (template.GUID != guid || prefabs.Contains(prefab)) continue;
                    prefabs.Add(prefab);
                }
            }
            DestroyImmediate(go);
        }

        searchList = prefabs;
        return !trySearch;
    }

    private static void ApplyPrefab(GameObject prefab, Object targetPrefab, bool replace)
    {
        if (EditorUtility.DisplayDialog("注意！", "是否进行递归查找批量替换模板？", "ok", "cancel"))
        {
            Debug.Log("ApplyPrefab : " + prefab.name);
            GameObject replacePrefab;
            var count = 0;
            if (replace)
            {
                
                
                PrefabUtility.ReplacePrefab(prefab, targetPrefab, ReplacePrefabOptions.ConnectToPrefab);
                Refresh();
                replacePrefab = targetPrefab as GameObject;
                count = prefab.GetComponentsInChildren<UITemplate>(true).Length;
            }
            else
            {
                replacePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(targetPrefab), typeof(GameObject));
                var checkPrefab = (GameObject)PrefabUtility.InstantiatePrefab(replacePrefab);
                count = checkPrefab.GetComponentsInChildren<UITemplate>(true).Length;
                DestroyImmediate(checkPrefab);
            }


            if (count != 1)
            {
                EditorUtility.DisplayDialog("注意！", "无法批量替换，因为模板不支持嵌套。", "ok");
                return;
            }

            var template = replacePrefab.GetComponent<UITemplate>();
            if (template != null)
            {
                List<GameObject> references;
                if (TrySearchPrefab(template.GUID, out references))
                {
                    foreach (var reference in references)
                    {
                        var go = PrefabUtility.InstantiatePrefab(reference) as GameObject;
                        var instanceTemplates = go.GetComponentsInChildren<UITemplate>(true);
                        foreach (var instance in instanceTemplates)
                        {
                            if (instance.GUID != template.GUID) continue;
                            AdjustPrefab(replacePrefab, instance.gameObject);
                            
                        }

                        PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go), ReplacePrefabOptions.ConnectToPrefab);
                        DestroyImmediate(go);
                    }
                }
            }
            ClearHierarchy();
            Refresh();
        }
    }

    private static void AdjustPrefab(GameObject replacePrefab, GameObject instance) {
        var newInstance = (GameObject)Instantiate(replacePrefab);
        newInstance.name = instance.name;
        newInstance.transform.SetParent(instance.transform.parent);
        newInstance.transform.localPosition = instance.transform.localPosition;
        newInstance.transform.localRotation = instance.transform.localRotation;

        try {
            //GameObject targetGo = targetPrefab as GameObject;
            //GameObject prefabGo = prefab as GameObject;
            float sourceWidth = instance.GetComponent<UIWidget>().width;
            float prefabWidth = newInstance.GetComponent<UIWidget>().width;
            float fScale = sourceWidth / prefabWidth;
            //float fScaleY = newWidth / replaceWidth;
            newInstance.transform.localScale = instance.transform.localScale * fScale;
        } catch (System.Exception ex) {
            //int a = 0;
            newInstance.transform.localScale = replacePrefab.transform.localScale;
        }
        //newInstance.transform.localScale = replacePrefab.transform.localScale;

        DestroyImmediate(instance.gameObject);
    }

    private static void DeletePrefab(string path)
    {
        if (!EditorUtility.DisplayDialog("注意！", "是否进行递归查找批量删除模板？", "ok", "cancel")) return;
        Debug.Log("DeletePrefab : " + path);
        var deletePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
        var template = deletePrefab.GetComponent<UITemplate>();
        if (template != null)
        {
            List<GameObject> references;
            if (TrySearchPrefab(template.GUID, out references))
            {
                foreach (var reference in references)
                {
                    var go = PrefabUtility.InstantiatePrefab(reference) as GameObject;
                    var instanceTemplates = go.GetComponentsInChildren<UITemplate>(true);
                    foreach (var instance in instanceTemplates)
                    {
                        if (instance.GUID != template.GUID) continue;
                        DestroyImmediate(instance.gameObject);
                    }
                    PrefabUtility.ReplacePrefab(go, PrefabUtility.GetPrefabParent(go), ReplacePrefabOptions.ConnectToPrefab);
                    DestroyImmediate(go);
                }
            }
        }
        AssetDatabase.DeleteAsset(path);
        ClearHierarchy();
        Refresh();
    }


    private static void CreatPrefab(GameObject prefab)
    {
        var creatPath = UITemplatePath + "/" + prefab.name + ".prefab";
        Debug.Log("CreatPrefab : " + creatPath);

        if (AssetDatabase.LoadAssetAtPath(creatPath, typeof(GameObject)) == null)
        {
            var temps = prefab.GetComponentsInChildren<UITemplate>(true);

            for (var i = 0; i < temps.Length; i++)
            {
                DestroyImmediate(temps[i]);
            }

            prefab.AddComponent<UITemplate>().InitGUID();
            PrefabUtility.CreatePrefab(UITemplatePath + "/" + prefab.name + ".prefab", prefab);
            Refresh();
        }
        else
        {
            EditorUtility.DisplayDialog("错误！", "Prefab名字重复，请重命名！", "OK");
        }
    }


    private static void Refresh()
    {
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorApplication.SaveScene();
    }


    private static void ClearHierarchy()
    {
        var canvas = FindObjectOfType<Canvas>();

        if (canvas == null) return;
        for (var i = 0; i < canvas.transform.childCount; i++)
        {
            var t = canvas.transform.GetChild(i);
            if (t.GetComponent<UITemplate>() != null)
            {
                DestroyImmediate(t.gameObject);
            }
        }
    }

    private void ShowHierarchy()
    {
        if (IsTemplatePrefabInHierarchy(_uiTemplate.gameObject)) return;
        var canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;
        if ((canvas.transform.childCount != 0) && (canvas.transform.childCount != 1 || canvas.transform.GetChild((0)).GetComponent<UITemplate>() == null)) return;
        ClearHierarchy();
        var go = PrefabUtility.InstantiatePrefab(_uiTemplate.gameObject) as GameObject;
        go.name = _uiTemplate.gameObject.name;
        GameObjectUtility.SetParentAndAlign(go, canvas.gameObject);
        EditorGUIUtility.PingObject(go);
    }

    private static bool IsTemplatePrefabInHierarchy(GameObject go)
    {
        return (PrefabUtility.GetPrefabParent(go) != null);
    }

    private static bool IsTemplatePrefabInInProjectView(GameObject go)
    {
        var path = AssetDatabase.GetAssetPath(go);
        if (!string.IsNullOrEmpty(path))
            return (path.Contains(UITemplatePath));
        return false;
    }

    private static DirectoryInfo CreatDirectory()
    {
        var directiory = new DirectoryInfo(Application.dataPath + "/" + UITemplatePath.Replace("Assets/", ""));
        if (directiory.Exists) return directiory;
        directiory.Create();
        Refresh();
        return directiory;
    }

    private static string GetPrefabPath(GameObject prefab)
    {
        var prefabObj = PrefabUtility.GetPrefabParent(prefab);
        return prefabObj != null ? AssetDatabase.GetAssetPath(prefabObj) : null;
    }
}