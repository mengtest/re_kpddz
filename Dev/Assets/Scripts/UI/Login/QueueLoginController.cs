using UnityEngine;
using System.Collections;
using EventManager;
using UI.Controller ;


public class QueueLoginController : ControllerBase{

    public QueueLoginMono _mono;
    public static short UPDATE_SHOW = 1;

    public QueueLoginController(string uiName){
        sName = uiName;
        ELevel = UILevel.HIGHT;
        prefabsPath =new string[]{UIPrefabPath.QUEUE_LOGIN_WIN};

    }

    protected override void  UICreateCallback()
    {
        _mono = winObject.AddComponent<QueueLoginMono>();
    }

    protected override void UIDestroyCallback()
    {

        if (_mono != null)
        {
            UnityEngine.Object.DestroyImmediate(_mono);
            _mono = null;
        }
    }

    /// <summary>
    /// 打开
    /// </summary>
    public void OpenQueueLogin(uint queue,uint enterSpeed)
    {
        if (winObject)
        {
            _mono.UpdateQueue(queue,enterSpeed);
            return;
        }
        var msg = new EventMultiArgs();
        msg.AddArg("queue",queue);
        msg.AddArg("enterSpeed",enterSpeed);
        
        UIManager.CreateWin(UIName.QUEUE_LOGIN_WIN);
        CallUIEvent(UPDATE_SHOW, msg);
    }

    public void CloseQueueLogin()
    {
        UIManager.DestroyWin(UIName.QUEUE_LOGIN_WIN);  
    }
    public static void Close()
    {
        var ctrl = UIManager.GetControler<QueueLoginController>();
        if(ctrl!=null)ctrl.CloseQueueLogin();
    }
    

}
