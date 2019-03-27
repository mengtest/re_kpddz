/***************************************************************


 *
 *
 * Filename:  	LoadingWinMono.cs	
 * Summary: 	场景切换界面
 *
 * Version:   	1.0.0
 * Author: 		XB.Wu
 * Date:   		2015/06/16 19:53
 ***************************************************************/

#region Using
using System.Collections;
using EventManager;
using UI.Controller;
using UnityEngine;
using Scene;
using sound;
#endregion 

public class LoadingWinMono : MonoBehaviour {
    LoadingWinController _controller;

    public bool _isFadeInFinish = false;
    public bool _loadFinish = false;

    float _fadeInTime = 0.2f;
    float _fadeOutTime = 0.4f;

    public static int _tip_index = -1;
    public static int _text_index = -1;
    UIWidget _bg;
    UILabel _tip;
    UILabel _slider_value;
    UIProgressBar _slider;
    GameObject cloud1;
    GameObject cloud2;
    GameObject cloud3;
    GameObject cloud4;
    GameObject cloud5;

    EventDelegate _fadeInDel;
    EventDelegate _fadeOutDel;
    TweenAlpha tAlpha;
    UITexture _uiTexture_model;
    UITexture _uiTexture_text;
    float _curValue;
    bool _bRunToComplete = false;

    void Awake()
    {
        _controller = (LoadingWinController)UIManager.GetControler(UIName.LOADING_WIN);
        BGM bgm = GameSceneManager.getInstance().CurSceneObject.GetComponent<BGM>();
        if (bgm != null)
        {
            Destroy(bgm);
        }
        _fadeInDel = new EventDelegate(OnCreateActoinComplete);
        _fadeOutDel = new EventDelegate(OnDestroyActoinComplete);

        _bg = transform.Find("Container").GetComponent<UIWidget>();

        _bg.alpha = 0f;

        _controller.RegisterUIEvent(UIEventID.CREATE_WIN_ACTION, UICreateAction);
        _controller.RegisterUIEvent(UIEventID.DESTROY_WIN_ACTION, UIDestroyAction);

        
    }


    public void UICreateAction(EventMultiArgs args)
    {
        ////渐显
        _bg.alpha = 0f;
        TweenAlpha tAlpha = TweenAlpha.Begin(_bg.gameObject, _fadeInTime + 0.2f, 1f);
        tAlpha.AddOnFinished(_fadeInDel);
        //Invoke("OnCreateActoinComplete", _fadeInTime);
    }



    public void OnCreateActoinComplete()
    {
        if (_controller._callbackFadeIn!=null)
            _controller._callbackFadeIn();
    }

    public void UIDestroyAction(EventMultiArgs args)
    {
        _bg.alpha = 1f;
        tAlpha = TweenAlpha.Begin(_bg.gameObject, _fadeOutTime, 0f);

        tAlpha.RemoveOnFinished(_fadeInDel);
        tAlpha.AddOnFinished(_fadeOutDel);

        Invoke("OnDestroyActoinComplete", _fadeOutTime + 0.2f);
    }

    public void OnDestroyActoinComplete()
    {
        UIManager.DestroyWin(UIName.LOADING_WIN);
        UIManager.DestroyWin(UIName.MESSAGE_WIN);
        
    }
}
