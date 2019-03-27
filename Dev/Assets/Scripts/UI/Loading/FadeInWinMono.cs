/***************************************************************


 *
 *
 * Filename:  	ChangeSceneWinMono.cs	
 * Summary: 	场景切换界面
 *
 * Version:   	1.0.0
 * Author: 		XB.Wu
 * Date:   		2015/06/16 19:53
 ***************************************************************/
using UnityEngine;
using System.Collections;
using UI.Controller;
using Utils;
using EventManager;
using network.protobuf;
using network;
using Scene;

public class FadeInWinMono : MonoBehaviour {
    FadeInWinController _controller;
    float _fadeInTime = 0.2f;
    float _fadeOutTime = 0.2f;
    UIWidget _bg;

    void Awake() {
        _controller = (FadeInWinController)UIManager.GetControler(UIName.FADE_IN_WIN);
        _bg = transform.Find("bg").GetComponent<UIWidget>();
        _bg.alpha = 0f;

        _controller.RegisterUIEvent(UIEventID.CREATE_WIN_ACTION, UICreateAction);
        _controller.RegisterUIEvent(UIEventID.DESTROY_WIN_ACTION, UIDestroyAction);
    }

    public void UICreateAction(EventMultiArgs args)
    {
        //渐显
        TweenAlpha tAlpha = TweenAlpha.Begin(_bg.gameObject, _fadeInTime, 1f);
        tAlpha.AddOnFinished(OnCreateActoinComplete);
    }
    public void OnCreateActoinComplete()
    {
        StartLoadScene();
        if (_controller.FadeInCallback != null)
        {
            _controller.FadeInCallback("", new SceneConfig());
            _controller.FadeInCallback = null;
        }
    }
    public void UIDestroyAction(EventMultiArgs args)
    {
        _bg.alpha = 1f;
        TweenAlpha tAlpha = TweenAlpha.Begin(_bg.gameObject, _fadeOutTime, 0f);
        tAlpha.AddOnFinished(OnDestroyActoinComplete);
    }
    public void OnDestroyActoinComplete()
    {
        UIManager.DestroyWin(UIName.FADE_IN_WIN);
    }

    private void StartLoadScene() {
        //SceneManager.getInstance().ChangeToSceneFromFadeInWin(SceneManager.changeToSence);
    }
}
