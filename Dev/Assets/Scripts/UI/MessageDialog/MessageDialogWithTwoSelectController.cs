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

public class MessageDialogWithTwoSelectController : ControllerBase {

    private MessageDialogWithTwoSelectMono monoComponent;
    private DelegateType.MessageDialogCallback okCallback;
    private DelegateType.MessageDialogCallback cancleCallback;
    private bool isShowTip = false;
    public bool needCloseButton = true;
    public bool CloseAfterClick = true;

    public MessageDialogWithTwoSelectController(string uiName)
	{
		sName = uiName;
		ELevel = UILevel.HIGHT;
        prefabsPath = new string[] { UIPrefabPath.MESSAGE_DIALOG_WITH_TWO_SELECT };
	}

    /// <summary>
    /// 界面加载完成后的回调
    /// </summary>
    protected override void UICreateCallback()
    {
        monoComponent = winObject.AddComponent<MessageDialogWithTwoSelectMono>();
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
    public void ShowMessageDialog(string text, string color, string alignment, DelegateType.MessageDialogCallback okCallbackFunc, DelegateType.MessageDialogCallback cancleCallbackFunc)
    { 
        EventMultiArgs args = new EventMultiArgs();
        args.AddArg("text", text);
        args.AddArg("color", color);
        args.AddArg("alignment", alignment);
        okCallback = okCallbackFunc;
        cancleCallback = cancleCallbackFunc;

        int callbackCount = 0;
        if (okCallback != null)
            callbackCount += 1;
        if (cancleCallback != null)
            callbackCount += 1;
        args.AddArg("callbackCount", callbackCount);
        CallUIEvent(UIEventID.MESSAGE_DIALOG_SET_TEXT, args);
    }

    void ClearData()
    {
        cancleCallback = null;
        okCallback = null;
        ELevel = UILevel.HIGHT;
        needCloseButton = true;
        CloseAfterClick = true;
    }

    public void OnClickCancel()
    {
        if (cancleCallback != null)
            cancleCallback();

        if (CloseAfterClick)
        {
            ClearData();
            UIManager.DestroyWinByAction(UIName.MESSAGE_DIALOG_WITH_TWO_SELECT);
        }
    }
    public void OnClickOK()
    {
        if (okCallback != null)
            okCallback();

        if (CloseAfterClick)
        {
            ClearData();
            UIManager.DestroyWinByAction(UIName.MESSAGE_DIALOG_WITH_TWO_SELECT);
        }
    }
}
