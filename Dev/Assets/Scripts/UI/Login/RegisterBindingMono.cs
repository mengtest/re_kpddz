/***************************************************************
 * Copyright (c) 2013 福建沃动计算机技术有限公司
 *         All rights reserved.
 *
 *
 * Filename:  	RegisterBindingMono.cs
 * Summary: 	注册及绑定界面
 *
 * Version:   	1.0.0
 * Author: 		YQ.Qu
 * Date:   		2017/3/6 0006 下午 6:11
 ***************************************************************/

using UnityEngine;
using System.Collections;
using UI.Controller;

public class RegisterBindingMono : BaseMono
{
    private RegisterBindingController _ctrl;
    private GameObject _btnClose;
    private UIInput _phoneInput;
    private UIInput _passwordInput;
    private UIInput _verifyInput;
    private GameObject _btnGetverifyCode;
    private GameObject _btnSure;
    private UILabel _btnGetverifyCodeLb;

    private bool _bUpdateCooldwon = false;
    private GameObject _btnGetverifyCodeGray;

    protected override void ReplaceAwake()
    {
        _ctrl = UIManager.GetControler<RegisterBindingController>();
        _btnClose = FindGameObject("Container/bg/btnClose");
        _phoneInput = FindComponent<UIInput>("Container/phone/Sprite");
        _passwordInput = FindComponent<UIInput>("Container/password/Sprite");
        _verifyInput = FindComponent<UIInput>("Container/verify/Sprite");
        _btnGetverifyCode = FindGameObject("Container/verify/btnVerify");
        _btnGetverifyCodeGray = FindGameObject("Container/verify/btnVerifyGray");
        _btnGetverifyCodeLb = FindComponent<UILabel>("Container/verify/btnVerifyGray/Label");
        _btnSure = FindGameObject("Container/btnSure");
        //_verifyBtnSpr = _btnGetverifyCode.GetComponent<UISprite>();

//        _phoneInput.activeTextColor = new Color(dc661f);
        _phoneInput.defaultText = GameText.GetStr("login_name_default");
        _passwordInput.defaultText = GameText.GetStr("reset_passWord_default");
        _verifyInput.defaultText = GameText.GetStr("verify_default");
        if (_ctrl.cooldownEndTime - UtilTools.GetClientTime() > 0){
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
        UIEventListener.Get(_btnSure).onClick = OnSureRegisterOrBinder;
        UIEventListener.Get(_btnGetverifyCodeGray).onClick = OnGetVerifyCodeHandler;
    }

    private void OnSureRegisterOrBinder(GameObject go)
    {
        //TODO 确定注册或者绑定
        string phoneNum = _phoneInput.value;
        string password = _passwordInput.value;

        if (!IsPhoneNum(phoneNum)){
            UtilTools.MessageDialog(GameText.GetStr("please_input_right_phone_num"));
            return;
        }
        if (!IsPassword(password))
        {
            UtilTools.MessageDialog(GameText.GetStr("reset_passWord_default"));
            return;
        }
        string verifyCode = _verifyInput.value;
        if (_ctrl.isBinding){
            LoginInputController.startUpMono.PhoneBind(phoneNum, password, verifyCode);
//            UtilTools.ShowWaitWin(WaitFlag.BindPhone, 5f);
        }
        else{
            LoginInputController.startUpMono.PhoneRegister(phoneNum, password, verifyCode);
//            UtilTools.ShowWaitWin(WaitFlag.RegisterAccount, 5f);
        }
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
        if (_ctrl.isBinding){
            if (sdk.SDKManager.isAppStoreVersion())
            {
                LoginInputController.startUpMono.PhoneVerificationCode(phoneNum, "1003", "IOS");
            }
            else
            {
                UtilTools.GetAvmpSign(phoneNum, 1003);
            }
        }
        else{
            //
            if (sdk.SDKManager.isAppStoreVersion())
            {
                LoginInputController.startUpMono.PhoneVerificationCode(phoneNum, "1001", "IOS");
            }
            else
            {
                UtilTools.GetAvmpSign(phoneNum, 1001);
            }
        }
    }

    private void OnCloseHandler(GameObject go)
    {
        UIManager.DestroyWin(UIName.REGISTER_BINDING_WIN);
    }

    void Update()
    {
        if (_bUpdateCooldwon){
            int cur_time = UtilTools.GetClientTime();
            if (_ctrl.cooldownEndTime == 0) //未赋值
            {
                SetVerifyShow(false);
            }
            else if (_ctrl.cooldownEndTime - cur_time <= 0) //倒计时结束
            {
                SetVerifyShow(false);
                _ctrl.cooldownEndTime = 0;
                _bUpdateCooldwon = false;
            }
            else //倒计时
            {
                SetVerifyShow(true);
                _btnGetverifyCodeLb.text = GameText.Format("verify_wait", _ctrl.cooldownEndTime - cur_time);
            }
        }
    }

    void OnDestroy()
    {
    }

    private bool IsPhoneNum(string num)
    {
        if (string.IsNullOrEmpty(num)) return false;
        if (num.Length != 11) return false;
        return System.Text.RegularExpressions.Regex.IsMatch(num, @"^[1]+[3,4,5,6,7,8,9]+\d{9}");
    }
    private bool IsPassword(string str)
    {
        if (string.IsNullOrEmpty(str))
            return false;
        if (str.Length < 6 || str.Length > 15)
            return false;
        return true;
    }


    private void SetVerifyShow(bool isWait)
    {
        if (_btnGetverifyCode.activeSelf == !isWait) return;
        _btnGetverifyCode.SetActive(!isWait);
        _btnGetverifyCodeGray.SetActive(isWait);
    }
}