/***************************************************************
 * Copyright (c) 2013 福建沃动计算机技术有限公司
 *         All rights reserved.
 *
 *
 * Filename:  	LoginInputMono.cs
 * Summary: 	登陆界面
 *
 * Version:   	1.0.5
 * Author: 		YQ.Qu
 * Date:   		2017/2/10 0010 下午 4:38
 ***************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MyExtensionMethod;
using UI.Controller;
using Utils;

public class LoginInputMono : BaseMono
{
    private LoginInputController _ctrl;
    private UIInput _nameLb;
    private UIInput _passWordLb;
    private GameObject _phoneLoginContainer;

    private GameObject _resetPassWordContainer;
    private UIInput _resetNameLb;
    private UIInput _resetNewPassWordLb;
    private UIInput _resetVerificationLb;
    private GameObject _phonePopList;

    private List<string> _popList = new List<string>();
    private UIScrollView _phonePopListScrollView;
    private UIGridCellMgr _phonePopListMgr;
    private GameObject _btnPop;
    private bool _bUpdateCooldwon = false;
    private RegisterBindingController _registerBindingCtrl;
    private GameObject _getVerification;
    private GameObject _getVerificationGray;
    private UILabel _getVerificationGrayLb;
    public GameObject winBg;

    protected override void ReplaceAwake()
    {
        _ctrl = UIManager.GetControler<LoginInputController>();
        winBg = FindGameObject("Container");
        _phoneLoginContainer = FindGameObject("Container/phoneLoginContainer");
        _nameLb = FindComponent<UIInput>("Container/phoneLoginContainer/name/nameLb");
        _passWordLb = FindComponent<UIInput>("Container/phoneLoginContainer/passWord/passWordLb");
        _btnPop = FindGameObject("Container/phoneLoginContainer/name/btnPop");
        var registerBtn = FindGameObject("Container/phoneLoginContainer/registerBtn");
        var btnForget = FindGameObject("Container/phoneLoginContainer/passWord/btnForget");
        UIEventListener.Get(FindGameObject("Container/phoneLoginContainer/loginBtn")).onClick = OnLoginHandler;
        UIEventListener.Get(registerBtn).onClick = OnRegisterHandler;
        UIEventListener.Get(_btnPop).onClick = OnPopHandler;
        UIEventListener.Get(btnForget).onClick = OnForgetPassWardHandler;
        UIEventListener.Get(FindGameObject("Container/phoneLoginContainer/btnClose")).onClick = OnCloseHandler;
        
        _registerBindingCtrl = UIManager.GetControler<RegisterBindingController>();

        _nameLb.defaultText = GameText.GetStr("login_name_default");
//        _nameLb.onChdjipange = OnNameLbChangeHandler
        _nameLb.onChange.Add(new EventDelegate(OnNameLbChangeHandler));
        _passWordLb.defaultText = GameText.GetStr("login_passWord_default");


        _resetPassWordContainer = FindGameObject("Container/resetPassWord");

        UIEventListener.Get(FindGameObject("Container/resetPassWord/btnClose")).onClick = OnCloseResetPassWordHandler;
        UIEventListener.Get(FindGameObject("Container/resetPassWord/btnSure")).onClick = OnResetPassWordSureHandler;

        _resetNameLb = FindComponent<UIInput>("Container/resetPassWord/name/nameLb");
        _resetNewPassWordLb = FindComponent<UIInput>("Container/resetPassWord/newPassWord/nameLb");
        _resetVerificationLb = FindComponent<UIInput>("Container/resetPassWord/verification/nameLb");
        _getVerification = FindGameObject("Container/resetPassWord/verification/getVerification");
        _getVerificationGray = FindGameObject("Container/resetPassWord/verification/getVerificationGray");
        _getVerificationGrayLb =
            FindComponent<UILabel>("Container/resetPassWord/verification/getVerificationGray/Label");
        UIEventListener.Get(_getVerification).onClick = OnToGetVerificationHandler;
        UIEventListener.Get(_getVerificationGray).onClick = OnToGetVerificationHandler;
        _resetNameLb.defaultText = GameText.GetStr("login_name_default");
        _resetNewPassWordLb.defaultText = GameText.GetStr("reset_passWord_default");


        _phonePopList = FindGameObject("Container/phoneLoginContainer/name/popList");
        _phonePopListScrollView = FindComponent<UIScrollView>("Container/phoneLoginContainer/name/popList/ScrollView");
        _phonePopListMgr = FindComponent<UIGridCellMgr>("Container/phoneLoginContainer/name/popList/ScrollView/Grid");
        _phonePopListMgr.onShowItem = OnPopListShowViewItemShow;
    }

    void Start()
    {
        string allUserName = _ctrl.GetAllUserName();
        if (allUserName != ""){
            JSONObject userList = new JSONObject(allUserName);
//            _phonePopList.Clear();
            _popList.Clear();
            _phonePopListMgr.ClearCells();
            if (userList[0].Count >= 2) userList[0].list.Reverse();
            for (int i = 0; i < userList[0].Count; i++){
                if (i == 0){
                    _nameLb.value = userList[0].list[i].str;
                    if (GameDataMgr.LOGIN_DATA.lastLoginAccount.Equals(_nameLb.value)){
                        _passWordLb.value = GameDataMgr.LOGIN_DATA.lastLoginPassword;
                    }
                }
//                _phonePopList.AddItem(userList[0].list[i].str);
                _popList.Add(userList[0].list[i].str);
                _phonePopListMgr.NewCellsBox(_phonePopListMgr.Go);
            }
        }

        _phonePopListScrollView.ResetPosition();
        _phonePopListMgr.Grid.Reposition();
        _phonePopListMgr.UpdateCells();

        _ctrl.SetScrollViewRenderQueue(_phonePopListScrollView.gameObject);
        _phonePopList.gameObject.SetActive(false);

        _phoneLoginContainer.SetActive(true);
        _resetPassWordContainer.SetActive(false);
    }


    void Update()
    {
        if (_bUpdateCooldwon && _resetPassWordContainer.activeSelf){
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
                _getVerificationGrayLb.text =
                    GameText.Format("verify_wait", _registerBindingCtrl.cooldownEndTime - cur_time);
            }
        }
    }

    private void SetVerifyShow(bool isWait)
    {
//        if (_getVerificationGray.activeSelf == !isWait) return;
        _getVerification.SetActive(!isWait);
        _getVerificationGray.SetActive(isWait);
    }

    void OnDestroy()
    {
    }

    /// <summary>
    /// 登陆
    /// </summary>
    /// <param name="go"></param>
    private void OnLoginHandler(GameObject go)
    {
        if (!IsPhoneNum(_nameLb.text)){
            UtilTools.ShowMessage(GameText.GetStr("login_name_empty"));
            return;
        }

        if (!IsPassword(_passWordLb.text)){
            UtilTools.ShowMessage(GameText.GetStr("login_passWord_empty"));
            return;
        }

        LoginInputController.AccountServer_PhoneLogin(_nameLb.text, _passWordLb.text);
    }

    /// <summary>
    /// 重置密码成功
    /// </summary>
    public void ResetPasswordSucc()
    {
//        UtilTools.ShowWaitWin(WaitFlag.LoginWin);
        LoginInputController.AccountServer_PhoneLogin(GameDataMgr.LOGIN_DATA.nowLoginAccount,
            GameDataMgr.LOGIN_DATA.nowLoginPassword);
    }

    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="go"></param>
    private void OnRegisterHandler(GameObject go)
    {
//        UtilTools.ShowMessage("功能开发中。。。", "[FFFFFF]");
        UIManager.CreateWin(UIName.REGISTER_BINDING_WIN);
    }

    #region 帐号显示

    private void OnPopListShowViewItemShow(GameObject cellBox, int index, GameObject cell)
    {
        if (index >= _popList.Count) return;
        var lb = cell.transform.Find<UILabel>("Label");
        var spr = cell.transform.Find<GameObject>("Sprite");
        lb.text = _popList[index];
        UIEventListener.Get(lb.gameObject).onClick = n => SetName(index);
        UIEventListener.Get(spr).onClick = n => DelName(_popList[index]);
    }

    private void SetName(int index)
    {
        _nameLb.text = _popList[index];
        _phonePopList.SetActive(false);
        _btnPop.transform.localScale = new Vector3(1f, _phonePopList.activeSelf ? -1f : 1f);
    }

    private void DelName(string na)
    {
        _ctrl.RemoveUser(na);
        string allUserName = _ctrl.GetAllUserName();
        if (_nameLb.value.Equals(na)) _nameLb.value = "";
        if (allUserName != ""){
            JSONObject userList = new JSONObject(allUserName);
//            _phonePopList.Clear();
            _popList.Clear();
            _phonePopListMgr.ClearCells();
            if (userList[0].Count >= 2) userList[0].list.Reverse();
            for (int i = 0; i < userList[0].Count; i++){
                if (i == 0 && _nameLb.value.Equals(na)){
                    _nameLb.value = userList[0].list[i].str;
                }
                _popList.Add(userList[0].list[i].str);
                _phonePopListMgr.NewCellsBox(_phonePopListMgr.Go);
            }
        }

        _phonePopListScrollView.ResetPosition();
        _phonePopListMgr.Grid.Reposition();
        _phonePopListMgr.UpdateCells();
    }

    private void OnNameLbChangeHandler()
    {
        if (_phonePopList.activeSelf){
            _phonePopList.SetActive(!_phonePopList.activeSelf);
            _btnPop.transform.localScale = new Vector3(1f, _phonePopList.activeSelf ? -1f : 1f);
        }
    }

    /// <summary>
    /// 切换帐号
    /// </summary>
    /// <param name="go"></param>
    private void OnPopHandler(GameObject go)
    {
        _phonePopList.SetActive(!_phonePopList.activeSelf);
        _btnPop.transform.localScale = new Vector3(1f, _phonePopList.activeSelf ? -1f : 1f);
        _phonePopListMgr.Grid.Reposition();
    }


    /// <summary>
    /// 选择并关闭
    /// </summary>
    private void ShowUserList()
    {
//        _nameLb.value = _phonePopList.value;
//        _phonePopList.Close();
        //ShowOrHideImg();
    }

    #endregion


    /// <summary>
    /// 忘记密码
    /// </summary>
    /// <param name="go"></param>
    private void OnForgetPassWardHandler(GameObject go)
    {
        _phoneLoginContainer.SetActive(false);
        _resetPassWordContainer.SetActive(true);
    }

    private void OnCloseHandler(GameObject go)
    {
        gameObject.SetActive(false);
    }


    public void ShowPhoneLoginWin()
    {
        if (!_phoneLoginContainer.activeSelf) _phoneLoginContainer.SetActive(true);
    }

    /// <summary>
    ///重置密码
    /// </summary>
    /// <param name="go"></param>
    private void OnCloseResetPassWordHandler(GameObject go)
    {
        _resetPassWordContainer.SetActive(false);
        _phoneLoginContainer.SetActive(true);
    }

    /// <summary>
    /// 确认重置密码
    /// </summary>
    /// <param name="go"></param>
    private void OnResetPassWordSureHandler(GameObject go)
    {
        string phoneNum = _resetNameLb.value;
        string password = _resetNewPassWordLb.value;


        if (!IsPhoneNum(phoneNum)){
            UtilTools.MessageDialog(GameText.GetStr("please_input_right_phone_num"));
            return;
        }
        if (!IsPassword(password)){
            UtilTools.MessageDialog(GameText.GetStr("reset_passWord_default"));
            return;
        }
        string code = _resetVerificationLb.value;
        UtilTools.ShowWaitWin(WaitFlag.ChangePassword, 5f);
        LoginInputController.startUpMono.PhoneResetPassword(phoneNum, password, code);
    }

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <param name="go"></param>
    private void OnToGetVerificationHandler(GameObject go)
    {
        string phoneNum = _resetNameLb.value;
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
            LoginInputController.startUpMono.PhoneVerificationCode(phoneNum, "1002","IOS");
        }
        else
        {
            UtilTools.GetAvmpSign(phoneNum, 1002);
        }
        //
    }


    #region 手机及密码验证

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

    #endregion
}