using EventManager;
using UI.Controller;
using network;
using UnityEngine;
using System.Collections.Generic;

public enum MessageWinType
{
    Item=1,
    Icon,
}

class MessageWithIconController : ControllerBase
{
    private MessageWithIconMono monoComponent;
    private bool isReset = false;

    public MessageWithIconController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.TOP;
        prefabsPath = new string[] { UIPrefabPath.MESSAGE_WITH_ICON_WIN };
    }
    protected override void UICreateCallback()
    {
        monoComponent = winObject.AddComponent<MessageWithIconMono>();
    }

    protected override void UIDestroyCallback()
    {
        if (monoComponent != null)
        {
            UnityEngine.Object.DestroyImmediate(monoComponent);
            monoComponent = null;
        }
    }
    public void toBack()
    {
        UIManager.DestroyWin(sName);
    }
    public void SetMessage(string iconId, string numStr,MessageWinType type,Vector3 position)
    {
        EventMultiArgs args = new EventMultiArgs();
        args.AddArg("id", iconId);
        args.AddArg("num", numStr);
        args.AddArg("type", (int)type);
        args.AddArg("position", position);
        CallUIEvent(UIEventID.SET_MESSAGE_WITH_ICON, args);
    }
}
