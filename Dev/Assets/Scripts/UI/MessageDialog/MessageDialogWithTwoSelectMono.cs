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

public class MessageDialogWithTwoSelectMono : MonoBehaviour
{
    private MessageDialogWithTwoSelectController controller;
    private UILabel labelTipText;
    //private UILabel labelTitleText;
    private Transform oKButton;
    private Transform cancleButton;
    private Transform winBox;
    private Transform backgroundMask;
    void Awake()
    {
        controller = (MessageDialogWithTwoSelectController)UIManager.GetControler(UIName.MESSAGE_DIALOG_WITH_TWO_SELECT);

        winBox = transform.Find("WinBox");
        labelTipText = transform.Find("WinBox/TipText").gameObject.GetComponent<UILabel>();
        //labelTitleText = transform.Find("WinBox/TitleBox/TitleText").gameObject.GetComponent<UILabel>();
        oKButton = transform.Find("WinBox/SubmitButton");
        cancleButton = transform.Find("WinBox/CancleButton");
        backgroundMask = transform.Find("Texture");
        UIEventListener.Get(oKButton.gameObject).onClick = OnClickOKButton;
        UIEventListener.Get(backgroundMask.gameObject).onClick = OnClickCancelButton;
        UIEventListener.Get(cancleButton.gameObject).onClick = OnClickCancelButton;
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
        UIManager.DestroyWin(UIName.MESSAGE_DIALOG_WITH_TWO_SELECT);
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
    }

    void OnClickCancelButton(GameObject go)
    {
        controller.OnClickCancel();
    }


    void OnEventSetText(EventMultiArgs args)
    {
        string text = args.GetArg<string>("text");
        string color = args.GetArg<string>("color");
        string alignment = args.GetArg<string>("alignment");
        int callbackCount = args.GetArg<int>("callbackCount");
        string toShowText = "[" + color + "]" + text + "[-]";
        UIWidget uiWidget = labelTipText.GetComponent<UIWidget>();
        if (alignment == "Center")
            uiWidget.pivot = UIWidget.Pivot.Top;
        else if (alignment == "Left")
            uiWidget.pivot = UIWidget.Pivot.TopLeft;

        labelTipText.text = toShowText;
        labelTipText.alignment = (labelTipText.printedSize.y > labelTipText.defaultFontSize + labelTipText.spacingY) ? NGUIText.Alignment.Left : NGUIText.Alignment.Automatic;
    }

}
