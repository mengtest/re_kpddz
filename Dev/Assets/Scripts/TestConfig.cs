/***************************************************************
 * Copyright (c) 2013 福建沃动计算机技术有限公司
 *         All rights reserved.
 *
 *
 * Filename:  	TestConfig.cs
 * Summary: 	GUI方便测试（新手引导开启等）
 *
 * Version:   	1.0.0
 * Author: 		YQ.Qu
 * Date:   		2017/4/10 0010 下午 5:51
 ***************************************************************/

using UnityEngine;
using System.Collections;
using sluaAux;
using UI.Controller;
using System.IO;
using UnityEngine.UI;
using System;
using System.Diagnostics;

public class TestConfig : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        if (luaSvrManager.getInstance().IsLoaded){
            GUI.color = Color.white;
            GUI.backgroundColor = Color.green;
            if (GUI.Button(new Rect(50, 0, 200, 50), "开启新手引导")){
                UIManager.CallLuaFuncCall("event_open_guide", GameObject.Find("UIRoot"));
                UtilTools.ShowMessage("开启新手引导成功","[FFFFFF]");
            }
        }
#endif
    }
}