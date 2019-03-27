/***************************************************************
 * Copyright (c) 2013 福建沃动计算机技术有限公司
 *         All rights reserved.
 *
 *
 * Filename:  	PIChangePasswordController.cs
 * Summary: 	修改密码
 *
 * Version:   	1.0.0
 * Author: 		YQ.Qu
 * Date:   		2017/3/7 0007 下午 3:19
 ***************************************************************/

using UnityEngine;
using System.Collections;
using UI.Controller;

public class PIChangePasswordController : ControllerBase
{
    private PIChangePasswordMono _mono;

    public PIChangePasswordController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.HIGHT;
        prefabsPath = new string[] {UIPrefabPath.PI_CHANGE_PASSWORD};
    }

    protected override void UICreateCallback()
    {
        _mono = winObject.AddComponent<PIChangePasswordMono>();
    }

    protected override void UIDestroyCallback()
    {
    }

    public void ToBack(GameObject go)
    {
        UIManager.DestroyWin(sName);
    }

    public void ResetPasswordSuccess()
    {
        UIManager.DestroyWin(sName);
//        UtilTools.ShowMessage(GameText.GetStr("change_password_succ"), TextColor.WHITE);
    }
}