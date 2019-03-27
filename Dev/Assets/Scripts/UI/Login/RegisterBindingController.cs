/***************************************************************
 * Copyright (c) 2013 福建沃动计算机技术有限公司
 *         All rights reserved.
 *
 *
 * Filename:  	RegisterBindingController.cs
 * Summary:
 *
 * Version:   	1.0.0
 * Author: 		YQ.Qu
 * Date:   		2017/3/6 0006 下午 6:12
 ***************************************************************/

using UnityEngine;
using System.Collections;
using Scene;
using UI.Controller;

public class RegisterBindingController : ControllerBase
{
    private RegisterBindingMono _mono;
    public bool isBinding = false;
    public int cooldownEndTime = 0;
    public static StartUpScene startUpMono;

    public RegisterBindingController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.HIGHT;
        prefabsPath = new string[] {UIPrefabPath.REGISTER_BINDING_WIN};
        GameObject sceneObj = GameObject.Find("Scene");
        if (sceneObj)
        {
            startUpMono = sceneObj.GetComponent<StartUpScene>();
        }
    }

    protected override void UICreateCallback()
    {
        _mono = winObject.AddComponent<RegisterBindingMono>();
    }

    protected override void UIDestroyCallback()
    {
    }

    public void ToBack(GameObject go)
    {
        UIManager.DestroyWin(sName);
    }
}