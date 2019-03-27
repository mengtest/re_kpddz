/***************************************************************


 *
 *
 * Filename:  	MessageWinController.cs	
 * Summary: 	信息提示 出后几秒内自动消失
 *
 * Version:   	1.0.0
 * Author: 		XB.Wu
 * Date:   		2015/07/02 14:33
 ***************************************************************/
using EventManager;
using UI.Controller;

public class MessageWinController : ControllerBase {

	private MessageWinMono monoComponent;

    public MessageWinController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.TOP;
        prefabsPath = new string[] { UIPrefabPath.MESSAGE_WIN };

        //MsgCallManager.AddCallback(ProtoID.SC_USE_ITEM_REPLY, OnUseItemReply);
    }

    protected override void UICreateCallback() {
        monoComponent = winObject.AddComponent<MessageWinMono>();
    }

    protected override void UIDestroyCallback() {

        if (monoComponent != null)
        {
            UnityEngine.Object.DestroyImmediate(monoComponent);
            monoComponent = null;
        }
    }

    public void toBack() {
        UIManager.DestroyWin(sName);
    }

    public void SetMessage(string msg, string color) {
        EventMultiArgs args = new EventMultiArgs();
        args.AddArg("text", msg);
        args.AddArg("color", color);
        CallUIEvent(UIEventID.MESSAGE_WIN_SET_TEXT, args);
    }
}
