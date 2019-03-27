/***************************************************************

 * @Filename:  MessageDialogUseMoneyMono.cs 
 * @Summary:  消息界面（需消耗游戏币）
 *
 * @version:  1.0.0
 * @Author:  Lvguizhen
 * @Date:  2015/11/02 21:05 
 ***************************************************************/
using UnityEngine;
using System.Collections;
using UI.Controller;
using EventManager;

public class MessageDialogUseMoneyMono : MonoBehaviour {
    private MessageDialogUseMoneyController controller;
    private UILabel labelTipText;
    private UILabel labelTitleText;
    private Transform closeButton;
    private Transform oKButton;
    private Transform cancelButton;
    private Transform winBox;
    private bool isSelect = false;
	// 初始化
	void Awake () {
        controller = UIManager.GetControler(UIName.MESSAGE_DIALOG_USE_MONEY) as MessageDialogUseMoneyController;

        winBox = transform.Find("Texture/WinBox");
        labelTipText = winBox.Find("TipText").GetComponent<UILabel>();
        labelTitleText = winBox.Find("TipText1").GetComponent<UILabel>();
        closeButton = winBox.Find("CloseButton");
        oKButton = winBox.Find("Button_OK");
        cancelButton = winBox.Find("Button_Cancel");
        UIEventListener.Get(closeButton.gameObject).onClick = OnClickCancelButton;
        UIEventListener.Get(oKButton.gameObject).onClick = OnClickOKButton;
        UIEventListener.Get(cancelButton.gameObject).onClick = OnClickCancelButton;
        UIEventListener.Get(transform.Find("Texture").gameObject).onClick = OnClickCancelButton;
        controller.RegisterUIEvent(UIEventID.MESSAGE_DIALOG_SET_TEXT, OnEventSetText);

        controller.RegisterUIEvent(UIEventID.CREATE_WIN_ACTION, UICreateAction);
        controller.RegisterUIEvent(UIEventID.DESTROY_WIN_ACTION, UIDestroyAction);
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
        UIManager.DestroyWin(UIName.MESSAGE_DIALOG_USE_MONEY);
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


    private void OnEventSetText(EventMultiArgs args)
    {
        string text = args.GetArg<string>("text");
        string icon = args.GetArg<string>("icon");
        string iconNum = args.GetArg<string>("iconNum");
        string alignment = args.GetArg<string>("alignment");
        //string type = args.GetArg<string>("moneyKey");

        labelTipText.text = text;
        if (icon.Equals("C104"))
        {
            labelTitleText.text = GameText.Format("message_desc2", iconNum);
        }
        else if (icon.Equals("C101"))
        {
            labelTitleText.text = GameText.Format("message_desc3", iconNum);
        }
        else if (icon.Equals("C102"))
        {
            labelTitleText.text = GameText.Format("message_desc5", iconNum);
        }
        else
        {
            labelTitleText.text = GameText.Format("message_desc3", iconNum);
        }
        UIWidget uiWidget = labelTipText.GetComponent<UIWidget>();
        if (alignment == "Center")
            uiWidget.pivot = UIWidget.Pivot.Top;
        else if (alignment == "Left")
            uiWidget.pivot = UIWidget.Pivot.TopLeft;
    }

    private void OnClickCancelButton(GameObject go)
    {
        UIManager.DestroyWinByAction(UIName.MESSAGE_DIALOG_USE_MONEY);
    }

    private void OnClickOKButton(GameObject go)
    {
        if (isSelect) {
            PlayerPrefs.SetString("CheckIsShow" + (int)controller.useflag, "1");
        }
        controller.OnClickOK();
        UIManager.DestroyWinByAction(UIName.MESSAGE_DIALOG_USE_MONEY);
    }
}
