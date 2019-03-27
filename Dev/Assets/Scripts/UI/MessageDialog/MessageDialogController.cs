/***************************************************************


 *
 *
 * Filename:  	MessageDialogController.cs	
 * Summary: 	文本提示框
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/04/10 11:39
 ***************************************************************/

using UnityEngine;
using System.Collections;
using UI.Controller;
using EventManager;

public class MessageDialogController : ControllerBase {

    private MessageDialogMono monoComponent;
    private DelegateType.MessageDialogCallback okCallback;
    private DelegateType.MessageDialogCallback cancelCallback;
    private bool isShowTip = false;
    public bool needCloseButton = true;
    public bool CloseAfterClick = true;

    public MessageDialogController(string uiName)
	{
		sName = uiName;
		ELevel = UILevel.HIGHT;
		prefabsPath = new string[] { UIPrefabPath.MESSAGE_DIALOG };
	}

    /// <summary>
    /// 界面加载完成后的回调
    /// </summary>
    protected override void UICreateCallback()
    {
        monoComponent = winObject.AddComponent<MessageDialogMono>();
        monoComponent.SetToggleActive(isShowTip);
    }

    /// <summary>
    /// 销毁后的处理
    /// </summary>
    protected override void UIDestroyCallback()
    {
        if (monoComponent != null)
        {
            UnityEngine.Object.DestroyImmediate(monoComponent);
            monoComponent = null;
        }

    }

    /// <summary>
    /// 显示提示窗口
    /// </summary>
    /// <param name="text"></param>
    /// <param name="title"></param>
    /// <param name="color"></param>
    /// <param name="alignment">"Left" "Center"</param>
    /// <param name="okCallbackFunc"></param>
    /// <param name="cancelCallbackFunc"></param>
    public void ShowMessageDialog(string text, string color, string alignment, DelegateType.MessageDialogCallback okCallbackFunc, DelegateType.MessageDialogCallback cancelCallbackFunc, bool isShowToggle = false, string okBtnName = "", int closeSecond = 0,bool isShowClose = false)
    {
        EventMultiArgs args = new EventMultiArgs();
        args.AddArg("text", text);
        args.AddArg("color", color);
        args.AddArg("alignment", alignment);
        args.AddArg("okBtnName", okBtnName);
        args.AddArg("closeSecond", closeSecond);
        args.AddArg("showClose", isShowClose);
        okCallback = okCallbackFunc;
        cancelCallback = cancelCallbackFunc;

        int callbackCount = 0;
        if (okCallback != null)
            callbackCount += 1;
        if (cancelCallback != null)
            callbackCount += 1;
        args.AddArg("callbackCount", callbackCount);

        isShowTip = isShowToggle;
        CallUIEvent(UIEventID.MESSAGE_DIALOG_SET_TEXT, args);
    }

    void ClearData()
    {
        cancelCallback = null;
        okCallback = null;
        ELevel = UILevel.HIGHT;
        needCloseButton = true;
        CloseAfterClick = true;
    }
    public bool isShowClose()
    {
        if (okCallback == null)
            return true;
        return false;
    }
    public void OnClickCancel()
    { 
        if (cancelCallback != null)
            cancelCallback();

        if (CloseAfterClick)
            ClearData();
    }
    public void OnClickOK()
    {
        if (okCallback != null)
            okCallback();

        if (CloseAfterClick)
            ClearData();
    }
}
