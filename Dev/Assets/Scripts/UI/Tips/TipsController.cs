using EventManager;
using UI.Controller;
using UnityEngine;

internal class TipsController : ControllerBase
{
    private TipsMono monoComponent;

    /// <summary>
    /// 构造函数，初始化类
    /// </summary>
    /// <param name="uiName"></param>
    public TipsController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.TOP;
        prefabsPath = new[] {UIPrefabPath.TIPS};
    }

    internal void ShowTips(EventMultiArgs args)
    {
        CallUIEvent(UIEventID.TIPS, args);
    }

    /// <summary>
    /// 界面加载完成后的回调
    /// </summary>
    protected override void UICreateCallback()
    {
        monoComponent = winObject.AddComponent<TipsMono>();
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
    /// 返回按钮
    /// </summary>
    /// <param name="go"></param>
    internal static void GoBack(GameObject go=null)
    {
        UIManager.DestroyWin(UIName.TIPS);
    }
}