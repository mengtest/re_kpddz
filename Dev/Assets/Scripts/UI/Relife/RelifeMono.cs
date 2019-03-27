using UnityEngine;
using System.Collections;
using EventManager;
using MyExtensionMethod;
using UI.Controller;

public class RelifeMono : MonoBehaviour
{
    private RelifeController controller;
    private GameObject _btnClose;
    private GameObject _btnConfirm;
    private UILabel _desc;
    Transform WinBg;
    // Use this for initialization
	void Start ()
	{
        _desc.text = GameText.Format("relife_desc1", controller._getCount);
	}
    void Awake()
    {
        controller = UIManager.GetControler(UIName.RELIFE_WIN) as RelifeController;
        WinBg = transform.Find("Container");
        _btnClose = transform.Find("Container/closeBtn").gameObject;
        _btnConfirm = transform.Find("Container/confirmBtn").gameObject;
        _desc = transform.Find("Container/Label").GetComponent<UILabel>();
        UIEventListener.Get(_btnClose).onClick = OnClickBack;
        UIEventListener.Get(_btnConfirm).onClick = OnClickConfirm;
        controller.RegisterUIEvent(UIEventID.CREATE_WIN_ACTION, UICreateAction);
        controller.RegisterUIEvent(UIEventID.DESTROY_WIN_ACTION, UIDestroyAction);

    }
    private void OnClickConfirm(GameObject go)
    {
        OnClickBack(null);
    }
    private void OnClickBack(GameObject go)
    {
        UIManager.DestroyWinByAction(UIName.RELIFE_WIN);
    }
    private void UIDestroyAction(EventMultiArgs args)
    {
        WinBg.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Hashtable closeArg = new Hashtable();
        closeArg.Add("time", 0.3f);
        closeArg.Add("scale", new Vector3(0.1f, 0.1f, 1.0f));
        closeArg.Add("easetype", iTween.EaseType.easeInBack);
        closeArg.Add("oncomplete", "OnDestroyActoinComplete");
        closeArg.Add("oncompletetarget", gameObject);

        iTween.ScaleTo(WinBg.gameObject, closeArg);
    }

    public void OnDestroyActoinComplete()
    {
        UIManager.DestroyWin(UIName.RELIFE_WIN);
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="args"></param>
    private void UICreateAction(EventMultiArgs args)
    {
        WinBg.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Hashtable openArg = new Hashtable();
        openArg.Add("time", 0.3f);
        openArg.Add("scale", new Vector3(0.1f, 0.1f, 1.0f));
        openArg.Add("easetype", iTween.EaseType.easeOutBack);
        iTween.ScaleFrom(WinBg.gameObject, openArg);
    }
}
