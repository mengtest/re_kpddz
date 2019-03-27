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

public class FadeOutWinMono : MonoBehaviour {
    //FadeOutWinController _controller;
    float _fadeOutTime = 2f;
    UIWidget _bg;

    void Awake() {
        //_controller = (FadeOutWinController)UIManager.GetControler(UIName.FADE_OUT_WIN);
        _bg = transform.Find("bg").GetComponent<UIWidget>();
        _bg.alpha = 1f;
    }

    void Start() {
        FadeOut();
    }

    internal void FadeOut() {
        TweenAlpha tAlpha = TweenAlpha.Begin(_bg.gameObject, _fadeOutTime, 0f);
        tAlpha.AddOnFinished(FadeOutFinish);
    }

    private void FadeOutFinish() {
        GameObject.Destroy(gameObject);
    }
}
