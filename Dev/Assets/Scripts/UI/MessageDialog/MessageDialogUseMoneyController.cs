/***************************************************************

 *
 * @Filename:  MessageDialogUseMoneyController.cs 
 * @Summary:  消息界面（需消耗游戏币）
 *
 * @version:  1.0.0
 * @Author:  Lvguizhen
 * @Date:  2015/11/02 21:05 
 ***************************************************************/
using UnityEngine;
using System.Collections;
using UI.Controller;
using EventManager;

public enum MoneyType
{
    /// <summary>
    /// 金币
    /// </summary>
    SILVER,
    /// <summary>
    /// 元宝
    /// </summary>
    GLOD,
    /// <summary>
    /// 钻石
    /// </summary>
    DIAMOND,
}

public class MessageDialogUseMoneyController : ControllerBase {
    private MessageDialogUseMoneyMono monoComponent;
    private DelegateType.MessageDialogUseMoneyCallBack okCallback;
    private DelegateType.MessageDialogUseMoneyCallBack cancelCallback;
    private bool isEnough;
    private uint moneyKey;
    public UseType useflag=UseType.DefaultType;
	public MessageDialogUseMoneyController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.HIGHT;
        prefabsPath = new string[] { UIPrefabPath.MESSAGE_DIALOG_USE_MONEY };
    }

    /// <summary>
    /// 界面加载完成后的回调
    /// </summary>
    protected override void UICreateCallback()
    {
        monoComponent = winObject.AddComponent<MessageDialogUseMoneyMono>();
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
    /// 显示提示窗口
    /// </summary>
    /// <param name="text">提示内容</param>
    /// <param name="icon">游戏币图标名称</param>
    /// <param name="iconNum">游戏币数量</param>
    /// <param name="alignment">"Left" "Center"</param>
    /// <param name="okCallbackFunc"></param>
    /// <param name="cancelCallbackFunc"></param>
    public void ShowMessageDialog(string text, MoneyType _type, string iconNum, string alignment, DelegateType.MessageDialogUseMoneyCallBack okCallbackFunc, DelegateType.MessageDialogUseMoneyCallBack cancelCallbackFunc,UseType useType)
    {
        EventMultiArgs args = new EventMultiArgs();
        useflag=useType;
        args.AddArg("text", text);        
        args.AddArg("iconNum", iconNum);
        args.AddArg("alignment", alignment);
        string icon = "C100";
        switch(_type)
        {
            case MoneyType.GLOD:
                icon = "C102";
                moneyKey = 102;
                if (GameDataMgr.PLAYER_DATA.Gold >= ulong.Parse(iconNum))
                {
                    isEnough = true;
                }
                else
                {
                    isEnough = false;
                }
                break;
            case MoneyType.DIAMOND:
                icon = "C104";
                moneyKey = 104;
                if (GameDataMgr.PLAYER_DATA.Diamond >= int.Parse(iconNum))
                {
                    isEnough = true;
                }
                else
                {
                    isEnough = false;
                }
                break;
            case MoneyType.SILVER:
                icon = "C101";
                moneyKey = 101;
                if (GameDataMgr.PLAYER_DATA.Gold >= ulong.Parse(iconNum))
                {
                    isEnough = true;
                }
                else
                {
                    isEnough = false;
                }
                break;
        }
        if (!isEnough)
        {
            ItemBaseConfigItem item = ConfigDataMgr.getInstance().ItemBaseConfig.GetDataByKey(moneyKey);
            if(item!=null)
            {
                UtilTools.MessageDialog(GameText.Format("message_desc4", item.name));
            }
            return;
        }
        okCallback = okCallbackFunc;
        cancelCallback = cancelCallbackFunc;
        if (useType != UseType.DefaultType) {
            string flag=PlayerPrefs.GetString("CheckIsShow" + (int)useType);
            if (!string.IsNullOrEmpty(flag)) {
                if (flag == "1") {
                    OnClickOK();
                    return;
                }
            }
        }
        UIManager.CreateWinByAction(UIName.MESSAGE_DIALOG_USE_MONEY);
        args.AddArg("icon", icon);

        //int callbackCount = 0;
        //if (okCallback != null)
        //    callbackCount += 1;
        //if (cancelCallback != null)
        //    callbackCount += 1;
        //args.AddArg("callbackCount", callbackCount);

        CallUIEvent(UIEventID.MESSAGE_DIALOG_SET_TEXT, args);
    }

    void ClearData()
    {
        cancelCallback = null;
        okCallback = null;
    }

    public void OnClickCancel()
    {
        if (cancelCallback != null)
            cancelCallback();

        ClearData();
    }
    public void OnClickOK()
    {
        if (okCallback != null)
        {
            if (!isEnough)
            {
                //ItemBaseConfigItem item = GameDataMgr.ITEM_DATA.GetItemBaseConfig(moneyKey);
                if(moneyKey == 100 )
                {
                    //UtilTools.MessageDialog(GameText.Format("notEnoughTip", item.name), "614d46", "Center", UtilTools.GoToFastBuyWin, UtilTools.CancelMessageDialog);
                }
                else if( moneyKey == 101 )
                {
                    //UtilTools.MessageDialog(GameText.Format("notEnoughTip", item.name), "614d46", "Center", UtilTools.GoToRecharge, UtilTools.CancelMessageDialog);
                }

            }
            else
            {
                okCallback();
            }
        }

        ClearData();
    }
}
