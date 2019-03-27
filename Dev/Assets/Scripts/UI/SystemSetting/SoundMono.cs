/***************************************************************


 *
 *
 * Filename:  	ExchangeMono.cs	
 * Summary: 	主界面
 *
 * Version:   	1.0.0
 * Author: 		YanBin
 * Date:   		2015/03/24 17:46
 ***************************************************************/
using UnityEngine;
using System.Collections;
using EventManager;
using MyExtensionMethod;
using UI.Controller;
using Msg;
using network.protobuf;
using network;
public class SoundMono : MonoBehaviour
{
    private SoundController _ctrl;
    private UISlider _bgm;
    private UISlider _game;
    private UISlider _gun;

    private Transform _bg;
    float _bgmValue = 0;
    float _gameValue = 0;
    float _gunValue = 0;
    float _defaultValue = 50f;
    // Use this for initialization
	void Start ()
	{
        _bgmValue = PlayerPrefs.GetFloat("bgmValue", _defaultValue);
        _gameValue = PlayerPrefs.GetFloat("gameValue", _defaultValue);
        _gunValue = PlayerPrefs.GetFloat("gunValue", _defaultValue);
        _bgm.value = _bgmValue / 100f;
        _game.value = _gameValue / 100f;
        _gun.value = _gunValue / 100f;
	}
	

    void Awake()
    {
        _ctrl = (SoundController)UIManager.GetControler(UIName.SOUND_WIN);
        initUI();
        _ctrl.RegisterUIEvent(UIEventID.CREATE_WIN_ACTION, UICreateAction);
        _ctrl.RegisterUIEvent(UIEventID.DESTROY_WIN_ACTION, UIDestroyAction);
        
    }
    private void initUI()
    {
        _bg = transform.Find("Container");
        GameObject backGo = transform.Find("Container/closeBtn").gameObject;
        UIEventListener.Get(backGo).onClick = _ctrl.GoBack;
        GameObject btnConfirm = transform.Find("Container/defaultBtn").gameObject;
        UIEventListener.Get(btnConfirm).onClick = OnClickDefault;
        
        _bgm = _bg.Find("bgm/slider").GetComponent<UISlider>();
        _game = _bg.Find("game/slider").GetComponent<UISlider>();
        _gun = _bg.Find("gun/slider").GetComponent<UISlider>();
        ComponentData.Get(_bgm.gameObject).Id = 1;
        ComponentData.Get(_game.gameObject).Id = 2;
        ComponentData.Get(_gun.gameObject).Id = 3;
        _bgm.onDragFinished = OnDragBgm;
        _game.onDragFinished = OnDragGame;
        _gun.onDragFinished = OnDragGun;
        
    }
    private void OnDragBgm()
    {
        PlayerPrefs.SetFloat("bgmValue", (_bgm.value * 100f));
        AudioSource asc = Scene.GameSceneManager.getInstance().CurSceneObject.GetComponent<AudioSource>();
        if (asc != null)
        {
            asc.volume = _bgm.value;
        }
    }
    private void OnDragGame()
    {
        PlayerPrefs.SetFloat("gameValue", _game.value * 100f);
    }
    private void OnDragGun()
    {
        PlayerPrefs.SetFloat("gunValue", _gun.value * 100f);
    }
   
    private void OnClickDefault(GameObject go)
    {
        _bgmValue = _defaultValue;
        _gameValue = _defaultValue;
        _gunValue = _defaultValue;
        PlayerPrefs.SetFloat("bgmValue", _defaultValue);
        PlayerPrefs.SetFloat("gameValue", _defaultValue);
        PlayerPrefs.SetFloat("gunValue", _defaultValue);
        _bgm.value = _bgmValue / 100f;
        _game.value = _gameValue / 100f;
        _gun.value = _gunValue / 100f;
        AudioSource asc = Scene.GameSceneManager.getInstance().CurSceneObject.GetComponent<AudioSource>();
        if (asc != null)
        {
            asc.volume = _bgm.value;
        }
    }

    private void UIDestroyAction(EventMultiArgs args)
    {
        _bg.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Hashtable closeArg = new Hashtable();
        closeArg.Add("time", 0.3f);
        closeArg.Add("scale", new Vector3(0.1f, 0.1f, 1.0f));
        closeArg.Add("easetype", iTween.EaseType.easeInBack);
        closeArg.Add("oncomplete", "OnDestroyActoinComplete");
        closeArg.Add("oncompletetarget", gameObject);

        iTween.ScaleTo(_bg.gameObject, closeArg);
    }

    public void OnDestroyActoinComplete()
    {
        UIManager.DestroyWin(UIName.SOUND_WIN);
    }

    /// <summary>
    /// 打开界面
    /// </summary>
    /// <param name="args"></param>
    private void UICreateAction(EventMultiArgs args)
    {
        _bg.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        Hashtable openArg = new Hashtable();
        openArg.Add("time", 0.3f);
        openArg.Add("scale", new Vector3(0.1f, 0.1f, 1.0f));
        openArg.Add("easetype", iTween.EaseType.easeOutBack);
        iTween.ScaleFrom(_bg.gameObject, openArg);
    }
}
