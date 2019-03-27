/***************************************************************


 *
 *
 * Filename:  	fadeinwincontroller.cs	
 * Summary: 	场景切换控制 - 黑色渐显
 *
 * Version:   	1.0.0
 * Author: 		XB.Wu
 * Date:   		2015/06/24 21:08
 ***************************************************************/
using UnityEngine;
using System.Collections;
using UI.Controller;
using Utils;
using EventManager;
using network;
using network.protobuf;
using Scene;

public class FadeInWinController : ControllerBase {
    private FadeInWinMono monoComponent;        /// 协程终止订阅者代理。当且仅当显示调用stop函数终止协程时
    /// 终止事件，当全黑时触发
    public DelegateType.FadeInBlackCallback FadeInCallback = null;

    public FadeInWinController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.NORMAL;
        prefabsPath = new string[] { UIPrefabPath.FADE_IN_WIN };
    }

    protected override void UICreateCallback() {
        monoComponent = winObject.AddComponent<FadeInWinMono>();
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

    public void FadeInBlack(DelegateType.FadeInBlackCallback callback)
    {
        FadeInCallback = callback;
        UIManager.CreateWinByAction(UIName.FADE_IN_WIN);
    }

    public void FadeOutBlack()
    {
        UIManager.DestroyWinByAction(UIName.FADE_IN_WIN);
    }
}