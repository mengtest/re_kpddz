using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ArtistSceneTools
{

    static Dictionary<GameObject, bool> _dic_is_active = new Dictionary<GameObject, bool>();
    static Dictionary<GameObject, bool> _dic_is_static = new Dictionary<GameObject, bool>();
    /**
     *  场景结点父子关系
     *  World: 所有场景组件的父节点
     *   |- BasicComponents：组成场景的基础部件，所有设备都会显示(美术)
     *   |- OptionalDetailsComponents:可选显示部件，为优化考虑（美术）。
     *   |- WalkableArea：可行走区域，用来设置寻路（美术）。
     *   |- BornPoints：出生点
     *   |- Triggers：触发器点（暗雷），分为刷怪触发器，场景事件触发器，剧情触发器等等
     *   |- MonsterPoints：刷怪点
     *   |- Heros：英雄对象父节点
     *   |- Monsters：怪物对象父节点
     *   |- CanPickUpGoods： 可拾取物品父节点（美术）
     *   |- CustomObjects：其他自定义节点，用来存放自定义的一些组件，比如大魔王。
     */

    [MenuItem("SceneDirectory/生成场景目录")]
    public static void createSceneDirectory()
    {
        //父节点
        GameObject objWorld = new GameObject("World");

        //生成字节点
        // BasicComponents：组成场景的基础部件，所有设备都会显示(美术)
        // OptionalDetailsComponents:可选显示部件，为优化考虑（美术）。
        // WalkableArea：可行走区域，用来设置寻路（美术）。
        // BornPoints：出生点
        // Triggers：触发器点（暗雷），分为刷怪触发器，场景事件触发器，剧情触发器等等
        // MonsterPoints：刷怪点
        // Heros：英雄对象父节点
        // Monsters：怪物对象父节点
        // CanPickUpGoods： 可拾取物品父节点（美术）
        // CustomObjects：其他自定义节点，用来存放自定义的一些组件，比如大魔王。

        GameObject obj = new GameObject("BasicComponents");
        obj.transform.parent = objWorld.transform;

        obj = new GameObject("OptionalDetailsComponents");
        obj.transform.parent = objWorld.transform;

        obj = new GameObject("WalkableArea");
        obj.transform.parent = objWorld.transform;

        obj = new GameObject("BornPoints");
        obj.transform.parent = objWorld.transform;

        obj = new GameObject("Triggers");
        obj.transform.parent = objWorld.transform;

        obj = new GameObject("MonsterPoints");
        obj.transform.parent = objWorld.transform;

        obj = new GameObject("Heros");
        obj.transform.parent = objWorld.transform;

        obj = new GameObject("Monsters");
        obj.transform.parent = objWorld.transform;

        obj = new GameObject("CanPickUpGoods");
        obj.transform.parent = objWorld.transform;

        obj = new GameObject("CustomObjects");
        obj.transform.parent = objWorld.transform;
    }


    [MenuItem("SceneDirectory/网格合并")]
    public static void meshCombine()
    {
        foreach (GameObject go in Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel))
        {
            //静态不合并
            if (go.isStatic)
            {
                Debug.LogError("[" + go.name + "] is static, and mesh combine will not be executed!");
                continue;
            }

            //未激活不合并
            if (go.activeInHierarchy == false)
            {
                Debug.LogWarning("[" + go.name + "] is disabled, and mesh combine will not be executed!");
                continue;
            }

            //检测MeshFilter组件
            MeshFilter comptMeshFilter = go.GetComponent<MeshFilter>();
            if (comptMeshFilter == null)
            {
                go.AddComponent<MeshFilter>();
            }

            //检测MeshRenderer组件
            MeshRenderer comptMeshRender = go.GetComponent<MeshRenderer>();
            if (comptMeshRender == null)
            {
                go.AddComponent<MeshRenderer>();
            }

            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            MeshRenderer[] meshRenderer = go.GetComponentsInChildren<MeshRenderer>();  //获取自身和所有子物体中所有MeshRenderer组件
            Material[] mats = new Material[1];                    //新建材质球数组，材质相同，只声明大小为1个

            for (int i = 0; i < meshFilters.Length; i++)
            {
                mats[0] = meshRenderer[i].sharedMaterial;                           //获取材质球列表,因为要求材质相同的才能合并，所以只存储一个材质

                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = go.transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;  //注意变换矩阵
                meshFilters[i].gameObject.SetActive(false);
            }

            go.transform.GetComponent<MeshFilter>().mesh = new Mesh();
            try
            {
                go.transform.GetComponent<MeshFilter>().sharedMesh.CombineMeshes(combine, true);//为mesh.CombineMeshes添加一个 false 参数，表示并不是合并为一个网格，而是一个子网格列表
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.Message);
            }
            go.transform.GetComponent<MeshRenderer>().sharedMaterials = mats;          //为合并后的GameObject指定材质

            go.transform.gameObject.SetActive(true);
            //go.transform.gameObject.isStatic = true;
        }
    }

    [MenuItem("SceneDirectory/删除多余Animation组件")]
    public static void removeUnusedAnimationComponents()
    {
        foreach (GameObject go in Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel))
        {
            Animation[] anims = go.GetComponentsInChildren<Animation>();
            for (int i=0; i< anims.Length; i++)
            {
                if (anims[i].GetClipCount() <= 0)
                    GameObject.DestroyImmediate(anims[i]);
            }
        }
    }

    [MenuItem("SceneDirectory/删除多余MeshCollider组件")]
    public static void removeUnusedMeshColliderComponents()
    {
        foreach (GameObject go in Selection.GetFiltered(typeof(GameObject), SelectionMode.TopLevel))
        {
            MeshCollider[] colliders = go.GetComponentsInChildren<MeshCollider>();
            for (int i = 0; i < colliders.Length; i++)
            {
                 GameObject.DestroyImmediate(colliders[i]);
            }
        }
    }


    [MenuItem("SceneDirectory/材质修改Lightmap->Diffuse")]
    public static void shaderToDiffuse()
    {
        _dic_is_static.Clear();
        UnityEngine.SceneManagement.Scene s = EditorSceneManager.GetActiveScene();
        GameObject[] objs = s.GetRootGameObjects();
        for (int k = 0; k < objs.Length; k++)
        {
            GameObject go = objs[k];
            if (go.name == "Scene")
            {
                SetAllChildrenStatic(go, false);
                MeshRenderer[] meshRds = go.GetComponentsInChildren<MeshRenderer>();
                for (int i = 0; i < meshRds.Length; i++)
                {
                    //检测MeshRenderer组件
                    changeToDiffuseShader(meshRds[i]);
                }
                SkinnedMeshRenderer[] skinedMeshRds = go.GetComponentsInChildren<SkinnedMeshRenderer>();
                for (int i = 0; i < skinedMeshRds.Length; i++)
                {
                    //检测MeshRenderer组件
                    changeToDiffuseShader(skinedMeshRds[i]);
                }
            }
        }
        EditorSceneManager.SaveScene(s);
    }

    public static void changeToDiffuseShader(Renderer comptMeshRender)
    {
        if ("uint_zhiwu03" == comptMeshRender.gameObject.name)
        {
            int a = 0;
        }
        if (comptMeshRender != null && comptMeshRender.sharedMaterial != null && comptMeshRender.sharedMaterial.shader != null)
        {
            string nameMat = comptMeshRender.sharedMaterial.shader.name;
            if (nameMat == "Custom/Lightmap" || nameMat == "Hidden/InternalErrorShader")
            {
                Shader shaderLg = Shader.Find("Legacy Shaders/Diffuse") as Shader;
                comptMeshRender.sharedMaterial.shader = shaderLg;
                SetAllChildrenStatic(comptMeshRender.gameObject, true);
            }
            else if (nameMat == "Custom/FuckOutline")
            {
                Shader shaderLg = Shader.Find("Legacy Shaders/Transparent/Cutout/Diffuse") as Shader;
                comptMeshRender.sharedMaterial.shader = shaderLg;
                SetAllChildrenStatic(comptMeshRender.gameObject, true);
            }
            else if (nameMat == "Legacy Shaders/Transparent/Cutout/Diffuse" || nameMat == "Legacy Shaders/Diffuse")
            {
                SetAllChildrenStatic(comptMeshRender.gameObject, true);
            }
        }
    }

    [MenuItem("SceneDirectory/材质还原Diffuse->Lightmap")]
    public static void shaderToLightmap()
    {
        UnityEngine.SceneManagement.Scene s = EditorSceneManager.GetActiveScene();
        GameObject[] objs = s.GetRootGameObjects();
        for (int k = 0; k < objs.Length; k++)
        {
            GameObject go = objs[k];
            MeshRenderer[] meshRds = go.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < meshRds.Length; i++)
            {
                //检测MeshRenderer组件
                changeToLightmapShader(meshRds[i]);

            }
            SkinnedMeshRenderer[] skinedMeshRds = go.GetComponentsInChildren<SkinnedMeshRenderer>();
            for (int i = 0; i < skinedMeshRds.Length; i++)
            {
                //检测MeshRenderer组件
                changeToLightmapShader(skinedMeshRds[i]);
            }
        }
        EditorSceneManager.SaveScene(s);
        foreach (KeyValuePair<GameObject, bool> item in _dic_is_static)
        {
            item.Key.isStatic = item.Value;
        }
        _dic_is_static.Clear();
    }
    public static void changeToLightmapShader(Renderer comptMeshRender)
    {
        if (comptMeshRender != null && comptMeshRender.sharedMaterial != null && comptMeshRender.sharedMaterial.shader != null)
        {
            string nameMat = comptMeshRender.sharedMaterial.shader.name;
            if (nameMat == "Legacy Shaders/Diffuse" || nameMat == "Hidden/InternalErrorShader")
            {
                Shader shaderLg = Shader.Find("Custom/Lightmap") as Shader;
                comptMeshRender.sharedMaterial.shader = shaderLg;
            }
            else if (nameMat == "Legacy Shaders/Transparent/Cutout/Diffuse")
            {
                Shader shaderLg = Shader.Find("Custom/FuckOutline") as Shader;
                comptMeshRender.sharedMaterial.shader = shaderLg;
            }
        }
    }

    [MenuItem("SceneDirectory/创建Navmesh->当前场景")]
    public static void bakeCurNavMesh()
    {
        BakeNavMesh("");
    }
    [MenuItem("SceneDirectory/创建Navmesh->所有场景")]
    public static void bakeAllNavMesh()
    {
        List<string> sceneNames = SearchFiles("Assets/Resources/Levels/", "*.unity");
        foreach (string f in sceneNames)
        {
            BakeNavMesh(f);
        }
    }

    static void BakeNavMesh(string sceneName)
    {
        GameObject[] _list = null;
        UnityEngine.SceneManagement.Scene s = EditorSceneManager.GetActiveScene();
        if (sceneName.IndexOf("zhandoufuben_") >= 0 || (sceneName == ""))
        {
            if (sceneName != "")
            {
                s = EditorSceneManager.OpenScene(sceneName);
            }
            GameObject[] objs = s.GetRootGameObjects();
            bool bFindObj = false;
            for (int i = 0; i < objs.Length; i++ )
            {
                if (objs[i].name == "Scene")
                {
                    Transform area = objs[i].transform.Find("World/WalkableArea");//显示寻路层，并设为static
                    if (area != null)
                    {
                        bFindObj = true;
                        SetAllChildrenStatic(objs[i], false);
                        SetAllChildrenStatic(area.gameObject, true);
                        SetAllChildrenVisible(area.gameObject, true);
                    }
                    else
                    {
                        Utils.LogSys.LogError("Not Found WalkableArea: " + sceneName);
                    }
                    _list  = GameObject.FindGameObjectsWithTag("WalkObstacle");
                    if (_list != null && _list.Length > 0)
                    {
                        for (int k=0; k < _list.Length; k++ )
                        {
                            if (!_dic_is_active.ContainsKey(_list[i]))
                                _dic_is_active.Add(_list[i], _list[i].activeSelf);
                            if (!_dic_is_static.ContainsKey(_list[i]))
                                _dic_is_static.Add(_list[i], _list[i].isStatic);
                            _list[i].SetActive(true);
                            _list[i].isStatic = true;
                        }
                    }
                    break;
                }
            }
            if (!bFindObj)
            {
                if (sceneName == "")
                {
                    Utils.LogSys.LogError("Not Found Scene Node: " + s.name);
                }
                else
                {
                    Utils.LogSys.LogError("Not Found Scene Node: " + sceneName);
                }
                
            }
            else
            {
                // Rebake navmesh data
                NavMeshBuilder.BuildNavMesh();
            }


            foreach (KeyValuePair<GameObject, bool> item in _dic_is_active)
            {
                item.Key.SetActive(item.Value);
            }
            foreach (KeyValuePair<GameObject, bool> item in _dic_is_static)
            {
                item.Key.isStatic = item.Value;
            }
            EditorSceneManager.SaveScene(s);
            _dic_is_active.Clear();
            _dic_is_static.Clear();
        }
    }

    static List<string> SearchFiles(string dir, string pattern)
    {
        List<string> sceneNames = new List<string>();
        foreach (string f in Directory.GetFiles(dir, pattern, SearchOption.AllDirectories))
        {
            sceneNames.Add(f);
        }
        return sceneNames;
    }

    static void SetAllChildrenStatic(GameObject obj, bool isStatic)
    {
        if (!_dic_is_static.ContainsKey(obj))
            _dic_is_static.Add(obj, obj.isStatic);
        obj.isStatic = isStatic;
        Transform tf = obj.transform;
        for (int i = 0; i < tf.childCount; i++)
        {
            SetAllChildrenStatic(tf.GetChild(i).gameObject, isStatic);
        }
    }
    static void SetAllChildrenVisible(GameObject obj, bool isActive)
    {
        if (!_dic_is_active.ContainsKey(obj))
            _dic_is_active.Add(obj, obj.activeSelf);
        obj.SetActive(isActive);
        Transform tf = obj.transform;
        for (int i = 0; i < tf.childCount; i++)
        {
            SetAllChildrenVisible(tf.GetChild(i).gameObject, isActive);
        }
    }
}
