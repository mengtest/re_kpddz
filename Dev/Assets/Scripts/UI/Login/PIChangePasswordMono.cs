/***************************************************************
 * Copyright (c) 2013 福建沃动计算机技术有限公司
 *         All rights reserved.
 *
 *
 * Filename:  	PIChangePasswordMono.cs
 * Summary: 	修改密码界面
 *
 * Version:   	1.0.0
 * Author: 		YQ.Qu
 * Date:   		2017/3/7 0007 下午 3:19
 ***************************************************************/

using UnityEngine;
using System.Collections;
using UI.Controller;

public class PIChangePasswordMono : BaseMono
{
    private PIChangePasswordController _ctrl;
    private GameObject _winBg;
    private UIInput _phoneInput;
    private UIInput _newPasswordInput;
    private UIInput _verifyInput;
    private GameObject _btnClose;
    private GameObject _btnGetverifyCode;
    private GameObject _btnGetverifyCodeGray;
    private UILabel _btnGetverifyCodeLb;
    private GameObject _btnSure;
    private bool _bUpdateCooldwon = false;
    private RegisterBindingController _registerBindingCtrl;

    protected override void ReplaceAwake()
    {
        _ctrl = UIManager.GetControler<PIChangePasswordController>();
        _winBg = FindGameObject("Container");
        _btnClose = FindGameObject("Container/bg/btnClose");
        _phoneInput = FindComponent<UIInput>("Container/phone/Label");
        _newPasswordInput = FindComponent<UIInput>("Container/newPassWord/Label");
        _verifyInput = FindComponent<UIInput>("Container/verify/Label");
        _btnGetverifyCode = FindGameObject("Container/verify/btnGetVerify");
        _btnGetverifyCodeGray = FindGameObject("Container/verify/btnGetVerifyGray");
        _btnGetverifyCodeLb = FindComponent<UILabel>("Container/verify/btnGetVerifyGray/Label");
        _btnSure = FindGameObject("Container/btnSure");

        _phoneInput.defaultText = GameText.GetStr("login_name_default");
        _newPasswordInput.defaultText = GameText.GetStr("reset_passWord_default");
        _verifyInput.defaultText = GameText.GetStr("verify_default");
        _registerBindingCtrl = UIManager.GetControler<RegisterBindingController>();
        if (_registerBindingCtrl.cooldownEndTime - UtilTools.GetClientTime() > 0){
            _bUpdateCooldwon = true;
            SetVerifyShow(true);
        }
        else{
            SetVerifyShow(false);
        }
    }

    void Start()
    {
        UIEventListener.Get(_btnClose).onClick = OnCloseHandler;
        UIEventListener.Get(_btnGetverifyCode).onClick = OnGetVerifyCodeHandler;
        UIEventListener.Get(_btnSure).onClick = OnSureResetPassWord;
        UIEventListener.Get(_btnGetverifyCodeGray).onClick = OnGetVerifyCodeHandler;
    }

    void Update()
    {
        if (_bUpdateCooldwon){
            int cur_time = UtilTools.GetClientTime();
            if (_registerBindingCtrl.cooldownEndTime == 0) //未赋值
            {
                SetVerifyShow(false);
            }
            else if (_registerBindingCtrl.cooldownEndTime - cur_time <= 0) //倒计时结束
            {
                SetVerifyShow(false);
                _registerBindingCtrl.cooldownEndTime = 0;
                _bUpdateCooldwon = false;
            }
            else //倒计时
            {
                SetVerifyShow(true);
                _btnGetverifyCodeLb.text = GameText.Format("verify_wait",
                    _registerBindingCtrl.cooldownEndTime - cur_time);
            }
        }
    }

    private void SetVerifyShow(bool isWait)
    {
        if (_btnGetverifyCode.activeSelf == !isWait) return;
        _btnGetverifyCode.SetActive(!isWait);
        _btnGetverifyCodeGray.SetActive(isWait);
    }


    private void OnCloseHandler(GameObject go)
    {
        UIManager.DestroyWin(UIName.PI_CHANGE_PASSWORD);
    }

    private void OnGetVerifyCodeHandler(GameObject go)
    {
        string phoneNum = _phoneInput.value;
        if (!IsPhoneNum(phoneNum)){
            UtilTools.ShowMessage(GameText.GetStr("login_name_empty"), TextColor.WHITE);
            return;
        }
        if (_bUpdateCooldwon){
            UtilTools.ShowMessage(GameText.GetStr("try_later"));
            return;
        }
        _bUpdateCooldwon = true;
        if (sdk.SDKManager.isAppStoreVersion())
        {
            LoginInputController.startUpMono.PhoneVerificationCode(phoneNum, "1002", "IOS");
        }
        else
        {
            UtilTools.GetAvmpSign(phoneNum, 1002);
        }
        //UtilTools.GetAvmpSign(phoneNum, 1002);
        //LoginInputController.startUpMono.PhoneVerificationCode(phoneNum, "1002");
    }


    private void OnSureResetPassWord(GameObject go)
    {
        string phoneNum = _phoneInput.value;
        string password = _newPasswordInput.value;


        if (!IsPhoneNum(phoneNum)){
            UtilTools.MessageDialog(GameText.GetStr("please_input_right_phone_num"));
            return;
        }
        if (!IsPassword(password)){
            UtilTools.MessageDialog(GameText.GetStr("reset_passWord_default"));
            return;
        }
        string code = _verifyInput.value;
        UtilTools.ShowWaitWin(WaitFlag.ChangePassword, 5f);
        LoginInputController.startUpMono.PhoneResetPassword(phoneNum, password, code);
    }


    private bool IsPhoneNum(string num)
    {
        if (string.IsNullOrEmpty(num)) return false;
        if (num.Length != 11) return false;
        return System.Text.RegularExpressions.Regex.IsMatch(num, @"^[1]+[3,4,5,6,7,8,9]+\d{9}");
    }

    private bool IsPassword(string str)
    {
        if (string.IsNullOrEmpty(str)) return false;
        if (str.Length < 6 || str.Length > 15) return false;
        return true;
    }

    void OnDestroy()
    {
    }
}