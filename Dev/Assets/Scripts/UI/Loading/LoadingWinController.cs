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

using EventManager;
using Scene;
using UI.Controller;
using sound;
using UnityEngine;
//public enum LoadingType {
//    Show,
//    Hide,
//}

public class LoadingWinController : ControllerBase {
    private LoadingWinMono monoComponent;
    public DelegateType.ChangeSceneCallback _callbackLoadStart;
    public DelegateType.ChangeSceneCallback _callbackLoadOver;
    public DelegateType.FadeInLoadingCallback _callbackFadeIn = null;
    //public LoadingType eType;
    //public string _loadingSceneName;
    //public SceneConfig _loadingSceneConfig;
    //public delegate void FadeDelegate();
    //public FadeDelegate fadeInCallBack;
    //public FadeDelegate fadeOutCallBack;

    public LoadingWinController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.TOP_HIGHT;
        prefabsPath = new string[] { UIPrefabPath.LOADING_WIN };

        //MsgCallManager.AddCallback(ProtoID.SC_USE_ITEM_REPLY, OnUseItemReply);
        //EventSystem.RegisterEvent(EventID.LOADING_FINISH, OnLoadingFinish);
    }

    protected override void UICreateCallback() {
        monoComponent = winObject.AddComponent<LoadingWinMono>();
    }

    protected override void UIDestroyCallback() {
        /*if (UIManager.IsWinShow(UIName.BATTLE_SCENE_2D_WIN))
        {
            BGM bgm = GameSceneManager.getInstance().CurSceneObject.GetComponent<BGM>();
            if (bgm != null)
            {
                if (!bgm.strAudioClipName.Contains("BOSSBGM"))
                {
                    UnityEngine.Object.Destroy(bgm);
                    int index = BattleManager.getInstance().cutLevel;
                    index = Mathf.Min(index, 5);
                    UtilTools.SetBgm("Sounds/BGM/zhandou" + index + "BGM");
                }
            }
            else
            {
                int index = BattleManager.getInstance().cutLevel;
                index = Mathf.Min(index, 5);
                UtilTools.SetBgm("Sounds/BGM/zhandou" + index + "BGM");
            }
        }
        else */if (UIManager.IsWinShow(UIName.MAIN_CENTER_WIN))
        {
            BGM bgm = GameSceneManager.getInstance().CurSceneObject.GetComponent<BGM>();
            if (bgm != null)
            {
                UnityEngine.Object.Destroy(bgm);
            }
            UtilTools.SetBgm("Sounds/BGM/zhuchengBGM");
        }
        if (monoComponent != null)
        {
            UnityEngine.Object.DestroyImmediate(monoComponent);
            monoComponent = null;
        }
    }

    public void toBack() {
        UIManager.DestroyWin(sName);
    }

    public void RegisterLoadStartDelegate(DelegateType.ChangeSceneCallback callback)
    {
        _callbackLoadStart = callback;
    }
    public void RegisterLoadOverDelegate(DelegateType.ChangeSceneCallback callback)
    {
        _callbackLoadOver = callback;
    }

    public void FadeInWin(string name, SceneConfig config, DelegateType.FadeInBlackCallback callback)
    {
//         _loadingSceneName = name;
//         _loadingSceneConfig = config;
//        _callbackFadeIn = callback;
        UIManager.CreateWinByAction(UIName.LOADING_WIN);
    }

    public void FadeInWin( DelegateType.FadeInLoadingCallback callback)
    {
        _callbackFadeIn = callback;
        UIManager.CreateWinByAction(UIName.LOADING_WIN);
    }
    public void FadeOutWin()
    {
        if (monoComponent == null)
            return;

        UIManager.DestroyWinByAction(UIName.LOADING_WIN);
    }
}