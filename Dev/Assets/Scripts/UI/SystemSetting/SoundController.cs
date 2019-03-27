/***************************************************************


 *
 *
 * Filename:  	ExchangeController.cs	
 * Summary: 	主界面
 *
 * Version:   	1.0.0
 * Author: 		YanBin
 * Date:   		2015/03/24 17:46
 ***************************************************************/
using UI.Controller;
using UnityEngine;

using EventManager;
using network;
using network.protobuf;
using Msg;
public class SoundController : ControllerBase
{
    private SoundMono _mono;
    public SoundController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.NORMAL;
        prefabsPath = new string[]{UIPrefabPath.SOUND_WIN};
    }


    /// <summary>
    /// 销毁前处理
    /// </summary>
    protected override void UIDestroyCallback()
    {
        if (_mono != null)
        {
            UnityEngine.Object.DestroyImmediate(_mono);
            _mono = null;
        }
    }

    /// <summary>
    /// 界面加载完成后调用
    /// </summary>
    protected override void UICreateCallback()
    {
        _mono = winObject.AddComponent<SoundMono>();
    }
    private void OnExchangeResponse(object proto)
    {
        if (proto == null)
            return;
        /*SC_CRedeemcodeUseResponse msg = (SC_CRedeemcodeUseResponse)proto;
        if (msg == null)
            return;*/
        
    }

    public void GoBack(GameObject go)
    {
        UIManager.DestroyWinByAction(sName);
    }
}
