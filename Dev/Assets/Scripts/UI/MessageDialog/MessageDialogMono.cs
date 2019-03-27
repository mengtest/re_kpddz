/***************************************************************


 *
 *
 * Filename:  	MessageDialogMono.cs	
 * Summary: 	文本提示框
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/04/10 11:38
 ***************************************************************/

using UnityEngine;
using System.Collections;
using UI.Controller;
using EventManager;

public class MessageDialogMono : MonoBehaviour
{
    private MessageDialogController controller;
    private UILabel labelTipText;
    //private UILabel labelTitleText;
    private Transform closeButton;
    private Transform oKButton;
    private Transform cancelButton;
    private Transform winBox;
    private Transform backgroundMask;
    //private Transform _toggle;
    UILabel coolText;
    private float rotate;
    int _coolSec = 0;
    int endTime = 0;
    void Awake()
    {
        controller = (MessageDialogController)UIManager.GetControler(UIName.MESSAGE_DIALOG);

        winBox = transform.Find("WinBox");
        labelTipText = transform.Find("WinBox/TipText").gameObject.GetComponent<UILabel>();
        //labelTitleText = transform.Find("WinBox/TitleBox/TitleText").gameObject.GetComponent<UILabel>();
        closeButton = transform.Find("WinBox/CloseButton");
        oKButton = transform.Find("WinBox/Grid/OKButton");
        cancelButton = transform.Find("WinBox/Grid/CancelButton");
        backgroundMask = transform.Find("Texture");
        coolText = transform.Find("WinBox/cool").GetComponent<UILabel>();
        //_toggle = transform.Find("WinBox/Checkbox");
        //UIEventListener.Get(closeButton.gameObject).onClick = OnClickCancelButton;
      
        UIEventListener.Get(oKButton.gameObject).onClick = OnClickOKButton;
        UIEventListener.Get(cancelButton.gameObject).onClick = OnClickCancelButton;
        UIEventListener.Get(backgroundMask.gameObject).onClick = OnClickCancelButton;
        UIEventListener.Get(closeButton.gameObject).onClick = OnClickCancelButton;
        controller.RegisterUIEvent(UIEventID.MESSAGE_DIALOG_SET_TEXT, OnEventSetText);
        controller.RegisterUIEvent(UIEventID.CREATE_WIN_ACTION, UICreateAction);
        controller.RegisterUIEvent(UIEventID.DESTROY_WIN_ACTION, UIDestroyAction);
    }
    void Update()
    {
        if (_coolSec>0)
        {
            coolText.text = GameText.Format("auto_back", endTime - UtilTools.GetServerTime());
            _coolSec = endTime - UtilTools.GetServerTime();
            if (_coolSec <= 0)
            {
                OnClickOKButton(null);
            }
        }

    }
    /// <summary>
    /// 销毁界面
    /// </summary>
    /// <param name="args"></param>
    private void UIDestroyAction(EventMultiArgs args)
    {
        winBox.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Hashtable closeArg = new Hashtable();
        closeArg.Add("time", 0.3f);
        closeArg.Add("scale", new Vector3(0.1f, 0.1f, 1.0f));
        closeArg.Add("easetype", iTween.EaseType.easeInBack);
        closeArg.Add("oncomplete", "OnDestroyActoinComplete");
        closeArg.Add("oncompletetarget", gameObject);

        iTween.ScaleTo(winBox.gameObject, closeArg);
    }

    public void OnDestroyActoinComplete()
    {
        UIManager.DestroyWin(UIName.MESSAGE_DIALOG);
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="args"></param>
    private void UICreateAction(EventMultiArgs args)
    {
        winBox.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Hashtable openArg = new Hashtable();
        openArg.Add("time", 0.3f);
        openArg.Add("scale", new Vector3(0.1f, 0.1f, 1.0f));
        openArg.Add("easetype", iTween.EaseType.easeOutBack);
        iTween.ScaleFrom(winBox.gameObject, openArg);
        
    }

    void OnClickOKButton(GameObject go)
    {
        controller.OnClickOK();
        /*bool isShow = _toggle.gameObject.GetComponent<UIToggle>().value;
        if (isShow == true)
        {
            PlayerPrefs.SetInt("isFormationTipShow", 1);
        }*/
        if (controller.CloseAfterClick)
            UIManager.DestroyWin(UIName.MESSAGE_DIALOG);
//            UIManager.DestroyWinByAction(UIName.MESSAGE_DIALOG);
    }

    void OnClickCancelButton(GameObject go)
    {

        if (!controller.needCloseButton)
        {
            return;
        }
        controller.OnClickCancel();
        if (controller.CloseAfterClick)
            UIManager.DestroyWin(UIName.MESSAGE_DIALOG);
//            UIManager.DestroyWinByAction(UIName.MESSAGE_DIALOG);
    }


    void OnEventSetText(EventMultiArgs args)
    {
        string text = args.GetArg<string>("text");
        string color = args.GetArg<string>("color");
        string alignment = args.GetArg<string>("alignment");
        int callbackCount = args.GetArg<int>("callbackCount");
        string okBtnName = args.GetArg<string>("okBtnName");
        _coolSec = args.GetArg<int>("closeSecond");
        bool isShowCloseBtn = args.GetArg<bool>("showClose");
        
        string toShowText = "[" + color + "]" + text + "[-]";

        UIWidget uiWidget = labelTipText.GetComponent<UIWidget>();
        if (alignment == "Center")
            uiWidget.pivot = UIWidget.Pivot.Top;
        else if (alignment == "Left")
            uiWidget.pivot = UIWidget.Pivot.TopLeft;
        if (_coolSec > 0)
        {
            coolText.text = GameText.Format("auto_back", _coolSec);
            endTime = UtilTools.GetServerTime() + _coolSec;
        }
        labelTipText.text = toShowText;
        labelTipText.alignment = (labelTipText.printedSize.y > labelTipText.defaultFontSize + labelTipText.spacingY) ? NGUIText.Alignment.Left : NGUIText.Alignment.Automatic;
        if (alignment == "CenterAll")
        {
            uiWidget.pivot = UIWidget.Pivot.Top;
            labelTipText.alignment = NGUIText.Alignment.Automatic;
        }
        if (okBtnName != "")
        {
            oKButton.Find("Label").GetComponent<UILabel>().text = okBtnName;
        }
        if (callbackCount == 0)
        {
            cancelButton.gameObject.SetActive(false);
        }
        else
        {
            cancelButton.gameObject.SetActive(true);
        }
        closeButton.gameObject.SetActive(isShowCloseBtn);
        //if (title == "default")
        //    title = GameText.GetStr("messageDialog_tip");

        //labelTitleText.text = title;
        /*
        if (controller.needCloseButton)
        {
            closeButton.gameObject.SetActive(true);
        }
        else
        {
            closeButton.gameObject.SetActive(false);
        }*/
    }

    public void SetToggleActive(bool isShow)
    {
        //_toggle.gameObject.SetActive(isShow);
    }
}
