/***************************************************************


 *
 *
 * Filename:  	ModelBind.cs	
 * Summary: 	模型绑点测试 美术工具 将模型放在 "Assets/Resources/Models" 文件夹下进行检测
 *
 * Version:   	1.0.0
 * Author: 		XB.Wu
 * Date:   		2015/10/22 18:03
 ***************************************************************/
#region Using
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;
using ModelConfig;
using System.IO;
using Utils;
#endregion



public class ModelBindTest : Editor{
    public static bool bCheckModel = false;
    //static string modelPath = Application.dataPath + "/Resources/Models/";
    static string modelPath = Application.dataPath + "/ModelTest/";

    //不该有的节点名
    static string[] _listErrorName = new string[] {
        "element_primary_weapon_01"
        ,"element_primary_weapon_02"
        ,"element_primary_weapon_03"
        ,"element_primary_weapon_04"
        ,"element_primary_weapon_05"
        ,"element_primary_weapon_06"

        ,"element_primary_weapon_4"
        ,"element_primary_weapon_5"
        ,"element_primary_weapon_6"
    };

    //必须的节点名
    static string[] _listNeedName = new string[] { 
        "element_body" 
    };

    static List<string> _listElementError = new List<string>();
    private static string _curModelId;

    [MenuItem("CheckModel/模型测试")]
    static void CheckModel(){
        bCheckModel = true;
        //加载配置
        ModelDataProcess.excute();
        bCheckModel = false;
        //检测配置文件
        checkConfig();

        if (!Directory.Exists(modelPath)) {
            LogSys.LogError("无法检测模型，不存在文件夹 " + modelPath);
            return;
        }

        string[] files = Directory.GetFiles(modelPath);
         
        
        _listElementError.Clear();

        

        

        //文件的数量
        int fileNum = files.Length;
        if (fileNum == 0) {
            LogSys.LogError("文件夹下没有任何模型 " + modelPath);
            return;
        }

        foreach (var item in ModelDataProcess._dicModelData) {
            
            bool result = true;
            string modelId = item.Key;
            _curModelId = modelId;
            ModelData cfg = item.Value;

            //测试绑点
            List<string> listBindError = new List<string>();
            UnityEngine.Object obj = Resources.Load("Models/" + modelId);
            if (obj != null) {
                GameObject go = GameObject.Instantiate(obj) as GameObject;
                for (int i = 0; i < cfg._listModelBps.Count; i++) {
                    string bp = cfg._listModelBps[i];
                    if (!getBp(go.transform, bp)) {
                        listBindError.Add(bp);
                        result = false;
                    }
                }
                result = checkElement(go.transform);

                if (listBindError.Count > 0) {
                    string bindError = "";
                    for (int i = 0; i < listBindError.Count; i++) {
                        bindError += " [" + listBindError[i] + "] ";
                    }
                    Utils.LogSys.LogError(string.Format("绑点缺失 {0} {1}", modelId, bindError));
                }

                GameObject.DestroyImmediate(go);

                Utils.LogSys.Log("正确 " + modelId);
                
            } 
        }
        
    }

    private static void checkConfig() {
        foreach (var item in ModelDataProcess._dicModelData) {
            ModelData cfg = item.Value;
            for (int i = 0; i < cfg._listAnimations.Count; i++) {
                int animateID = Convert.ToInt32(cfg._listAnimations[i]);
                if (((animateID >= 300 && animateID <= 400) || animateID >= 1000 )
                    && !cfg._dicAnimLoop[animateID.ToString()]) {
                    int index = cfg._listAnimEvents.FindIndex(dd => dd._nAnimID == animateID);
                    if (index >= 0) {
                        if (cfg._listAnimEvents[index]._listEvents.Count > 0) {
                            continue;
                        }
                    } 
                    LogSys.LogError(string.Format("没有关键帧 {0} {1}", cfg._strAssetName, animateID));
                }
            }
            //cfg._listAnimEvents;
        }
    }

    private static bool getBp(Transform tfRoot, string strName) {
        foreach (Transform tf in tfRoot.GetComponentsInChildren<Transform>()) {
            if (tf.name == strName) {
                return true;
            }
        }

        return false;
    }

    private static bool checkElement(Transform tfRoot) {
        bool result = true;
        string needElement = "";
        for (int i = 0; i < _listNeedName.Length; i++) {
            if (tfRoot.Find(_listNeedName[i]) == null) {
                needElement += " [" + _listErrorName[i] + "] ";
                result = false;
            }
        }
        if (needElement != "") {
            Utils.LogSys.LogError(string.Format("部件缺失 {0} {1}", _curModelId, needElement));
        }

        string errorElement = "";
        for (int i = 0; i < _listErrorName.Length; i++) {
            if (tfRoot.Find(_listErrorName[i]) != null) {
                errorElement += " [" + _listErrorName[i] + "] ";
                result = false;
            }
        }
        if (errorElement != "") {
            Utils.LogSys.LogError(string.Format("这些部件可能错误 {0} {1}", _curModelId, errorElement));
        } 
        
        return result;
    }
}

