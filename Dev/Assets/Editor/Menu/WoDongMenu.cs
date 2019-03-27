using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UI.Controller;
using Scene;

public class WoDongMenu : MonoBehaviour
{

	[MenuItem("WoDong/PM")]
    static void ShowPMWin ()
    {
        UIManager.CreateWin(UIName.PM_WIN);
    }

    [MenuItem("WoDong/保存镜头轨迹")]
    static void SaveCameraPathToXML()
    {
        if (GameSceneManager.sCurSenceName == "")
            return;

//         GameObject curScene = SceneManager.getInstance().CurSceneObject;
//         if (curScene == null)
//             return;
// 
//         MainScene sceneMono = curScene.GetComponent<MainScene>();
//         if (sceneMono == null || sceneMono.movePath == null)
//             return;
// 
//         sceneMono.movePath.SavePathToXML();
//        GameObject cam = SceneManager.sceneCameraObj;
        GameObject cam = GameObject.Find("Scene/Cameras/SceneCamera");
        if (cam == null)
            return;
        GameObject world = GameObject.Find("Scene/World");
        MovePathMono[] paths = world.GetComponentsInChildren<MovePathMono>();
        for (int i = 0; i < paths.Length; i++ )
        {
            if (paths[i].enabled)
            {
                paths[i].SavePathToXML();
            }
            
        }
        UnityEditor.AssetDatabase.Refresh();
    }
    /*
    [MenuItem("WoDong/保存部件位置到XML")]
    static void SaveSceneUnitPos()
    {
        if (GameSceneManager.sCurSenceName == "")
            return;

        GameObject curScene = GameSceneManager.getInstance().CurSceneObject;
        if (curScene == null)
            return;

        SceneUnitXMLMgr xmgMgr = curScene.AddComponent<SceneUnitXMLMgr>();
        if (xmgMgr == null || xmgMgr.dicUnitCofig == null)
            return;

        //建筑
        GameObject[] builders = GameObject.FindGameObjectsWithTag("builder");
        if (builders == null)
            return;

        int count = builders.Length;
        for (int i = 0; i < count; i++ )
        {
            SceneUnitConfig config = new SceneUnitConfig();
            config.localPosition = builders[i].transform.localPosition;
            config.localRotation = builders[i].transform.localRotation;
            config.localScale = builders[i].transform.localScale;
            xmgMgr.updateUnit(builders[i].transform.name, config);
        }

        //部件
        GameObject[] units = GameObject.FindGameObjectsWithTag("unit");
        if (units == null)
            return;

        count = units.Length;
        for (int i = 0; i < count; i++)
        {
            SceneUnitConfig config = new SceneUnitConfig();
            config.localPosition = units[i].transform.localPosition;
            config.localRotation = units[i].transform.localRotation;
            config.localScale = units[i].transform.localScale;
            xmgMgr.updateUnit(units[i].transform.name, config);
        }

        xmgMgr.WriteXML();
        Destroy(xmgMgr);

        UnityEditor.AssetDatabase.Refresh();
    }
    [MenuItem("WoDong/读取XML中的部件位置")]
    static void ReadSceneUnitPos()
    {
//         if (SceneManager.sCurSenceName != "")
//             return;

        SceneUnitXMLMgr xmlMgr = new SceneUnitXMLMgr();
        xmlMgr.init("Config/SceneUnit_temp");
        foreach (KeyValuePair<string, SceneUnitConfig> keyPair in xmlMgr.dicUnitCofig)
        {
            GameObject world = GameObject.Find("Scene/World");
            Transform transformObj = world.transform.Find(keyPair.Key);
            if (transformObj == null)
            {
                int count = world.transform.childCount;
                for (int i = 0; i < count; i++)
                {
                    transformObj = world.transform.GetChild(i).Find(keyPair.Key);
                    if (transformObj != null)
                        break;
                }
            }
            if (transformObj != null)
            {
                transformObj.localPosition = keyPair.Value.localPosition;
                transformObj.localRotation = keyPair.Value.localRotation;
                transformObj.localScale = keyPair.Value.localScale;
            }
        }

    }
    
    */
    [MenuItem("WoDong/修改anchor为OnStart")]
    static void setAnchorOnstart() {
        Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.OnlyUserModifiable);
        Utils.LogSys.Log(SelectedAsset.Length);
        if (SelectedAsset[0] == null) return;
        foreach (var item in SelectedAsset) {
            GameObject go = item as GameObject;
            Transform tf = go.transform;
            ResetAnchorExcute(tf);
        }

        //刷新编辑器
        AssetDatabase.Refresh();
        Utils.LogSys.Log("setAnchorOnstart success!!!");
    }

    private static void ResetAnchorExcute(Transform tf) {
        var rects = tf.GetComponentsInChildren<UIRect>();
        foreach (var item in rects) {
            if (item.canBeAnchored) {
                item.updateAnchors = UIRect.AnchorUpdate.OnStart;
            }
        }
    }
}
