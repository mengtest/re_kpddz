
#region Using
using asset;
using battleBaseDefine;
using effect;
using EventManager;
using HeroData;
using network;
using network.protobuf;
using player;
using Scene;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UI.Controller;
using UnityEngine;
using Utils;
using Object = UnityEngine.Object;
using sound;
using sdk;
//using dataEyeStatistics;
using System.Text;
using Msg;
using UnityStandardAssets.ImageEffects;

#endregion

//游戏中不同的帧率
public enum FPSLevel
{
    Normal,
    OnlyUI,//只有UI时,帧率调低
}

public enum EObjectType
{
    ITEM = 1,
    HERO = 2,
    GOLD,
    DIAMOND,
    HONOR,
    PLAYER_EXP,
}
public enum UseType
{
    DefaultType = 1,
    Resignation,
    ArenaClearCD,
    ArenaChange,
    TributeRefresh,
    TributeSpeedUp,
    BanditResearch,
    BanditKill,
    BanditSearch,
    ShopRefresh,
    TongQueTai,
    ChangeSkill,
}
public class EffectDelegate
{
    public delegate void OnEffectEvent(EffectObject pEffectObject);
    public static event OnEffectEvent OnModelCreateEvent;
}

[SLua.CustomLuaClass]
public static class UtilTools
{
    private static readonly string[] ItemQuality = { "equip_bg_green", "equip_bg_blue", "equip_bg_purple", "equip_bg_gold", "equip_bg_red" };
    private static readonly string[] HeroQuality = { "img_bg_green", "img_bg_blue", "img_bg_purple", "img_bg_gold", "img_bg_red" };
    public static bool CanTouchButton = true;

    // 视频播放回调
    public delegate void onPlayVideoComplete();


    /// <summary>
    /// 显示提示窗口
    /// </summary>
    /// <param name="text">提示文字</param>
    /// <param name="title">标题</param>
    /// <param name="color">颜色</param>
    /// <param name="alignment">对齐："Left" "Center"</param>
    /// <param name="okCallbackFunc">OK回调</param>
    /// <param name="cancelCallbackFunc">Cancel回调</param>
    public static void MessageDialog(string text, string color = "786964", string alignment = "Center", DelegateType.MessageDialogCallback okCallbackFunc = null, DelegateType.MessageDialogCallback cancelCallbackFunc = null, bool isShowToggle = false, string okBtnName = "", int closeSecond = 0,bool isShowClose = false)
    {
        UIManager.CreateWinByAction(UIName.MESSAGE_DIALOG);
        MessageDialogController controller = (MessageDialogController)UIManager.GetControler(UIName.MESSAGE_DIALOG);
        //         controller.ELevel = UILevel.HIGHT;
        controller.ShowMessageDialog(text, color, alignment, okCallbackFunc, cancelCallbackFunc, isShowToggle, okBtnName, closeSecond,isShowClose);
    }
    public static void MessageDialogWithTwoSelect(string text, string color = "786964", string alignment = "Center", DelegateType.MessageDialogCallback okCallbackFunc = null, DelegateType.MessageDialogCallback cancleCallbackFunc = null)
    {
        UIManager.CreateWinByAction(UIName.MESSAGE_DIALOG_WITH_TWO_SELECT);
        MessageDialogWithTwoSelectController controller = (MessageDialogWithTwoSelectController)UIManager.GetControler(UIName.MESSAGE_DIALOG_WITH_TWO_SELECT);
        //         controller.ELevel = UILevel.HIGHT;
        controller.ShowMessageDialog(text, color, alignment, okCallbackFunc, cancleCallbackFunc);
    }

    /// <summary>
    /// 系统级错误信息提示窗口（比如掉线，被踢等将显示在所有窗口之上）
    /// </summary>
    /// <param name="text">提示文字</param>
    /// <param name="title">标题</param>
    /// <param name="color">颜色</param>
    /// <param name="alignment">对齐："Left" "Center"</param>
    /// <param name="okCallbackFunc">OK回调</param>
    /// <param name="cancelCallbackFunc">Cancel回调</param>
    public static void ErrorMessageDialog(string text, string color = "614d46", string alignment = "Center", DelegateType.MessageDialogCallback okCallbackFunc = null, DelegateType.MessageDialogCallback cancelCallbackFunc = null, bool isShowToggle = false)
    {
        MessageDialogController controller = (MessageDialogController)UIManager.GetControler(UIName.MESSAGE_DIALOG);
        controller.ELevel = UILevel.TOP_HIGHT;
        controller.needCloseButton = false;
        UIManager.CreateWinByAction(UIName.MESSAGE_DIALOG);
        controller.ShowMessageDialog(text, color, alignment, okCallbackFunc, cancelCallbackFunc, isShowToggle);
    }

    public static void applicationExitDialog(string text, string color = "614d46", string alignment = "Center", DelegateType.MessageDialogCallback okCallbackFunc = null, DelegateType.MessageDialogCallback cancelCallbackFunc = null, bool isShowToggle = false)
    {
        MessageDialogController controller = (MessageDialogController)UIManager.GetControler(UIName.MESSAGE_DIALOG);
        controller.ELevel = UILevel.TOP_HIGHT;
        controller.needCloseButton = true;
        UIManager.CreateWinByAction(UIName.MESSAGE_DIALOG);
        controller.ShowMessageDialog(text, color, alignment, okCallbackFunc, cancelCallbackFunc, isShowToggle);
    }


    /// <summary>
    /// 消息提示窗口（需消耗游戏币）
    /// </summary>
    /// <param name="text">提示文字</param>
    /// <param name="_type">游戏币类型</param>
    /// <param name="iconNum">所需游戏币数量</param>
    /// <param name="alignment">对齐："Left" "Center"</param>
    /// <param name="okCallbackFunc">OK回调</param>
    /// <param name="cancelCallbackFunc">Cancel回调</param>
    public static void MessageDialogUseMoney(string text, MoneyType _type, string iconNum, string alignment = "Center",UseType useType=UseType.DefaultType, DelegateType.MessageDialogUseMoneyCallBack okCallbackFunc = null, DelegateType.MessageDialogUseMoneyCallBack cancelCallbackFunc = null)
    {
        /*if (okCallbackFunc != null)
        {
            okCallbackFunc();
            return;
        }*/
        MessageDialogUseMoneyController controller = UIManager.GetControler(UIName.MESSAGE_DIALOG_USE_MONEY) as MessageDialogUseMoneyController;
        controller.ShowMessageDialog(text, _type, iconNum, alignment, okCallbackFunc, cancelCallbackFunc,useType);
    }

    public static void CancelMessageDialog()
    {
        UIManager.DestroyWinByAction(UIName.MESSAGE_DIALOG);
    }


    public static void ShowMessage(byte[] text, string color = TextColor.RED)
    {
        ShowMessage(TextUtils.GetString(text), color);
    }

    public static void ShowMessage(string text, string color = TextColor.RED)
    {
        //LogSys.LogWarning("==========ShowMessage");
        UIManager.CreateWin(UIName.MESSAGE_WIN);
        MessageWinController controller = (MessageWinController)UIManager.GetControler(UIName.MESSAGE_WIN);
        controller.SetMessage(text, color);
    }

    public static void ShowMessageByCode(string errCode, string color = TextColor.RED)
    {
        ShowMessage(GameText.GetStr(errCode), color);
        HideWaitWin(WaitFlag.ClearAllWait);
    }

    public static void ShowMessageByCode(byte[] errCode, string color = TextColor.RED)
    {
        ShowMessageByCode(TextUtils.GetString(errCode), color);
        HideWaitWin(WaitFlag.ClearAllWait);
    }

    /// <summary>
    /// 设置灰态
    /// </summary>
    /// <param name="tex"></param>
    public static void SetGray(UITexture tex)
    {
        tex.color = new Color(0f, tex.color.g, tex.color.b, tex.color.a);
    }
    /// <summary>
    /// 设置灰态
    /// </summary>
    /// <param name="sprite"></param>
    public static void SetGray(UISprite sprite)
    {
        sprite.color = new Color(0f, sprite.color.g, sprite.color.b, sprite.color.a);
    }

    /// <summary>
    /// 设置灰态
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="bRecursion">是否递归</param>
    public static void SetGray(Transform tf, bool bRecursion = false)
    {
        SetGray(tf.gameObject, bRecursion);
    }

    /// <summary>
    /// 设置灰态
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="bRecursion">是否递归</param>
    ///
    /// <param name="bIgnoreUnActive">忽略unactive</param>
    public static void SetGray(GameObject obj, bool bRecursion = false, bool bIgnoreUnActive = true)
    {
        if (bRecursion) {
            var spriteList = obj.GetComponentsInChildren<UISprite>();
            for (int i = 0; i < spriteList.Length; i++) {
                if (!bIgnoreUnActive || spriteList[i].gameObject.activeSelf) {
                    SetGray(spriteList[i]);
                }
            }
            var labelList = obj.GetComponentsInChildren<UILabel>();
            for (int i = 0; i < labelList.Length; i++) {
                SetGray(labelList[i]);
            }

            var textureList = obj.GetComponentsInChildren<UITexture>();
            for (int i = 0; i < textureList.Length; i++) {
                if (!bIgnoreUnActive || textureList[i].gameObject.activeSelf)
                {
                    SetGray(textureList[i]);
                }
            }
        } else {
            var sprite = obj.GetComponent<UISprite>();
            if (sprite != null && (!bIgnoreUnActive || sprite.gameObject.activeSelf)) {
                SetGray(sprite);
                return;
            }

            var label = obj.GetComponent<UILabel>();
            if (label != null) {
                SetGray(label);
            }

            var texture = obj.GetComponent<UITexture>();
            if (texture != null && (!bIgnoreUnActive || texture.gameObject.activeSelf))
            {
                SetGray(texture);
            }
        }
    }

    private static void ToGrayColor(UILabel label)
    {
        ComponentData.Get(label.gameObject).color = label.color;
        ComponentData.Get(label.gameObject).effColor = label.effectStyle;

        if (label.effectStyle == UILabel.Effect.None)
        {
            label.color = new Color(0.47f, 0.47f, 0.47f);
        }
        else
        {
            ComponentData.Get(label.gameObject).eColor = label.effectColor;
            label.color = new Color(0.717f, 0.717f, 0.717f);
            label.effectColor = new Color(0.274f, 0.274f, 0.274f);
        }


    }

    private static bool IsGrayColor(UILabel label)
    {
        if (ComponentData.Get(label.gameObject).color !=  Color.white)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void SetGray(UILabel label)
    {
        if (!IsGrayColor(label))
        {
            ToGrayColor(label);
        }
    }

    /// <summary>
    /// 从灰态复原
    /// </summary>
    /// <param name="sprite"></param>
    public static void RevertGray(UISprite sprite)
    {
        sprite.color = new Color(1f, sprite.color.g, sprite.color.b, sprite.color.a);
    }

    private static void RevertGray(UILabel label)
    {
        if (IsGrayColor(label))
        {
            label.color = ComponentData.Get(label.gameObject).color;
            label.effectStyle = ComponentData.Get(label.gameObject).effColor;
            if (label.effectStyle != UILabel.Effect.None)
            {
                label.effectColor = ComponentData.Get(label.gameObject).eColor;
            }
            ComponentData.Get(label.gameObject).color = Color.white;
            ComponentData.Get(label.gameObject).eColor = Color.white;
        }
    }

    public static void RevertGray(UITexture texture)
    {
        texture.color = new Color(1f, texture.color.g, texture.color.b, texture.color.a);
    }

    /// <summary>
    /// 从灰态复原
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="bRecursion">是否递归</param>
    /// <param name="bIgnoreUnActive">忽略unactive</param>
    public static void RevertGray(Transform tf, bool bRecursion = false, bool bIgnoreUnActive = true)
    {
        RevertGray(tf.gameObject, bRecursion);
    }

    /// <summary>
    /// 从灰态复原
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="bRecursion">是否递归</param>
    /// <param name="bIgnoreUnActive">忽略unactive</param>
    public static void RevertGray(GameObject obj, bool bRecursion = false, bool bIgnoreUnActive = true)
    {
        if (bRecursion) {
            var spriteList = obj.GetComponentsInChildren<UISprite>();
            for (int i = 0; i < spriteList.Length; i++) {
                if (!bIgnoreUnActive || spriteList[i].gameObject.activeSelf) {
                    RevertGray(spriteList[i]);
                }
            }
            var labelList = obj.GetComponentsInChildren<UILabel>();
            for (int i = 0; i < labelList.Length; i++) {
                if (!bIgnoreUnActive || labelList[i].gameObject.activeSelf)
                {
                    RevertGray(labelList[i]);
                }
            }

            var textureList = obj.GetComponentsInChildren<UITexture>();
            for (int i = 0; i < textureList.Length; i++) {
                if (!bIgnoreUnActive || textureList[i].gameObject.activeSelf) {
                    RevertGray(textureList[i]);
                }
            }
        } else {
            var sprite = obj.GetComponent<UISprite>();
            if (sprite != null && (!bIgnoreUnActive || sprite.gameObject.activeSelf)) {
                RevertGray(sprite);
            }

            //var label = obj.GetComponent<UILabel>();
            //if (label != null) {
            //    RevertGray(label);
            //}

            var texture = obj.GetComponent<UITexture>();
            if (texture != null && (!bIgnoreUnActive || sprite.gameObject.activeSelf)) {
                RevertGray(texture);
            }
        }
    }
    /*public static void SetIconAndNum(Transform img, int id, string strnum)
    {
        ItemBaseConfigItem item = ConfigDataMgr.getInstance().ItemBaseConfig.GetDataByKey(id);
        if (item == null)
            return;
        int num = 0;
        if (int.TryParse(strnum, out num))
        {
            img.Find("num").GetComponent<UILabel>().text = strnum;
        }
        SetIcon(img, item.icon);
    }
    public static void SetIconAndNum(Transform img, int id, int num)
    {
        ItemBaseConfigItem item = ConfigDataMgr.getInstance().ItemBaseConfig.GetDataByKey(id);
        if (item == null)
            return;
        if (num > 0)
        {
            img.Find("num").GetComponent<UILabel>().text = num.ToString();
        }
        SetIcon(img, item.icon);
    }
    public static void SetHead(Transform img, string headid, string maskid)
    {
        HeadBaseConfigItem head = ConfigDataMgr.getInstance().HeadBaseConfig.GetDataByKey(headid);
        MaskBaseConfigItem mask = ConfigDataMgr.getInstance().MaskBaseConfig.GetDataByKey(maskid);
        if (head == null || mask==null)
            return;
        Transform maskImg = img.Find("mask");
        if (maskImg != null)
        {
            maskImg.GetComponent<UISprite>().spriteName = mask.frame_id;
        }
        SetIcon(img, head.image_id);
    }*/

    /// <summary>
    /// 根据Sprite的图集来设置其他图片
    /// </summary>
    /// <param name="img"></param>
    /// <param name="icon"></param>
    /// <param name="atlasPath"></param>
    public static void SetIcon(Transform img, string icon, int iType = -1, int iLev = 0, string atlasPath = "", int iDepth = 0,uint star=0,bool isShowRank=true)
    {
        Transform icon_img = img.Find("img");
        if (icon_img == null)
        {
            return;
        }
        icon_img.localScale = Vector3.one;

        if (string.IsNullOrEmpty(icon)) {
            icon = "C100001";
            LogSys.LogWarning("SetIcon：Icon不能为空");
            return;
        }
        int depth = 0;
        if (icon.Length <= 0) return;


        //string atlasPath1 = "";
        List<string> atlasPathList = new List<string>();
        if (string.IsNullOrEmpty(atlasPath)) {
            switch (icon[0]) {
                case 'A'://天赋
                    //atlasPathList.Add(UIPrefabPath.ATLAS_ICON_TALENT);
                    //depth = 301;//blank ? 245 : 246;
                    break;
                case 'C'://天赋
                    atlasPathList.Add(UIPrefabPath.ATLAS_ICON_CONSUME);
                    depth = 319;
                    break;
                case 'H'://天赋
                    atlasPathList.Add(UIPrefabPath.ATLAS_ICON_HEAD);
                    depth = 320;
                    break;
                default:

                    //atlasPathList.Add(UIPrefabPath.ATLAS_ICON_CONSUME);
                    //depth = 319;
                    break;
            }
        }
        //Object ob = MyResourceManager.GetInstance().GetResource(atlasPath);

        UIAtlas atlas = null;

        //string[] atlasPathArr = new[] { atlasPath, atlasPath1 };
        for (int i = 0; i < atlasPathList.Count; i++) {
            string atPath = atlasPathList[i];
            Object ob = AssetManager.getInstance().loadAsset(atPath, true);
            if (ob == null) {
                LogSys.LogError("atlas not found: " + atPath);
                return;
            }

            GameObject go = ob as GameObject;
            if (go == null) {
                LogSys.LogError("target not gameobject: " + atPath);
                return;
            }
            atlas = go.GetComponent<UIAtlas>();
            if (atlas == null) {
                LogSys.LogError("target not UIAtlas: " + atPath);
                return;
            }

            if (atlas.GetSprite(icon) != null) {
                break;
            } else {
                if (i == atlasPathList.Count - 1) {
                    LogSys.LogWarning("SetIcon： 没有图标 " + icon);
                    return;
                }
            }
        }

        //显示图标
        UISprite sprite = icon_img.GetComponent<UISprite>();
        if (sprite == null) return;
        sprite.atlas = atlas;
        sprite.spriteName = icon;
        if (iDepth != 0) {
            sprite.depth = iDepth;
        } else {
            sprite.depth = depth;
        }

    }

    /// <summary>
    /// 设置品质底图
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="eType"></param>
    /// <param name="quality"></param>
    public static void SetQualityIcon(UISprite sprite, EObjectType eType, int quality)
    {
        UIAtlas _atlas = null;
        string icon = "";
        //int depth = 100;
        int index = quality <= 0 ? 0 : quality - 1;
        switch (eType) {
            case EObjectType.ITEM://物品品质底
                //_atlas = ((GameObject)AssetManager.getInstance().getAsset(UIPrefabPath.ATLAS_COMMON_BACK, true)).GetComponent<UIAtlas>();
                icon = ItemQuality[index];
                break;
            case EObjectType.HERO://英雄品质底
                //_atlas = ((GameObject)AssetManager.getInstance().getAsset(UIPrefabPath.ATLAS_COMMON_BACK, true)).GetComponent<UIAtlas>();
                icon = HeroQuality[index];
                break;
        }

        if (_atlas == null || icon.Length <= 0)
            return;

        sprite.atlas = _atlas;
        sprite.spriteName = icon;
        //sprite.depth = depth;
    }



    /// <summary>
    /// 根据品质设置取颜色
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="icon"></param>
    public static string GetQualityColor(EObjectType eType, int quality)
    {
        switch (eType) {
            case EObjectType.ITEM://物品
                return "[FFFFFF]";
//                 if (quality <= 1)
//                     return TextColor.WHITE;
//                 else if (quality == 2)
//                     return TextColor.GREEN;
//                 else if (quality == 3)
//                     return TextColor.BLUE;
//                 else if (quality == 4)
//                     return TextColor.PURPLE;
//                 else if (quality == 5)
//                     return TextColor.YELLOW;
//                 else if (quality == 6)
//                     return TextColor.RED;
//                 break;
            case EObjectType.HERO://英雄
                return "[FFFFFF]";
//                 if (quality <= 1)
//                     return TextColor.GREEN;
//                 else if (2 <= quality && quality <= 3)
//                     return TextColor.BLUE;
//                 else if (4 <= quality && quality <= 6)
//                     return TextColor.PURPLE;
//                 else if (7 <= quality && quality <= 9)
//                     return TextColor.YELLOW;
//                 else if (10 <= quality && quality <= 12)
//                     return TextColor.RED;
//                 break;
            default:
                break;
        }
        return "[FFFFFF]";
    }

    /// <summary>
    /// 获取颜色字体和描边颜色，根据品阶
    /// </summary>
    /// <param name="eType"></param>
    /// <param name="quality"></param>
    public static Color GetQualityEffectColor(EObjectType eType, int quality)
    {
        Color color = new Color(0.2156f, 0.1137f, 0.055f);
        switch (eType) {
            case EObjectType.ITEM://物品
                //if (quality == 1)
                //    color = new Color(245, 236, 227);
                //else if (quality == 2)
                //    color = new Color(24, 219, 56);
                //else if (quality == 3)
                //    color = new Color(30, 221, 255);
                //else if (quality == 4)
                //    color = new Color(202, 23, 226);
                //else if (quality == 5)
                //    color = new Color(234, 192, 81);
                //break;
                if (quality <= 1)
                    color = new Color(0.2156f, 0.1137f, 0.055f);
                else if (quality == 2)
                    color = new Color(0.062f, 0.1098f, 0.043f);
                else if (quality == 3)
                    color = new Color(0.0549f, 0.149f, 0.2159f);
                else if (quality == 4)
                    color = new Color(0.1098f, 0.043f, 0.1019f);
                else if (quality == 5)
                    color = new Color(0.2039f, 0.1098f, 0.47f);
                break;
            case EObjectType.HERO://英雄
                if (quality <= 1)
                    color = new Color(0.2156f, 0.1137f, 0.055f);
                else if (2 <= quality && quality <= 3)
                    color = new Color(0.062f, 0.1098f, 0.043f);
                else if (4 <= quality && quality <= 6)
                    color = new Color(0.0549f, 0.149f, 0.2159f);
                else if (7 <= quality && quality <= 9)
                    color = new Color(0.1098f, 0.043f, 0.1019f);
                else if (10 <= quality && quality <= 13)
                    color = new Color(0.2039f, 0.1098f, 0.47f);

                break;
            default:
                break;
        }

        return color;
    }

    /// <summary>
    /// 设置颜色和描边颜色
    /// </summary>
    /// <param name="label"></param>
    /// <param name="value"></param>
    /// <param name="eType"></param>
    /// <param name="quality"></param>
    public static void SetLabelQuilityColor(UILabel label, string value, EObjectType eType, int quality)
    {
        string textColor = GetQualityColor(eType, quality);
        switch (eType) {
            case EObjectType.ITEM://物品
                label.text = textColor + value;
                break;
            case EObjectType.HERO://英雄
                //label.text = textColor + value + GetHeroNameSuffix(quality);
                //英雄名字始终绿色+官职数字
                label.text = TextColor.GREEN + value + (quality > 0 ? "+" + quality : "");
                break;
            default:
                break;
        }
        label.effectColor = GetQualityEffectColor(eType, quality);
    }

    /// <summary>
    /// 显示 Tips 窗口
    /// args 参数内容：
    /// Name:名称；
    /// Icon:图标；
    /// Des:描述；
    /// 附加属性 key 以 L 开头，key[1] = 'U' 带下划线，不需要下划线时填非 'U' 字符；
    /// key[2] = 'L'、'R'、'C' 对齐方式；
    /// AutoHide:int 存在为自动隐藏; Position:Vector3 位置信息；
    /// </summary>
    /// <param name="args">参数</param>
    /// <param name="go">添加 tip 的对象</param>
    public static void ShowTips(EventMultiArgs args, GameObject go)
    {
        UIManager.DestroyWin(UIName.TIPS);
        ShowTips(args, go, true);
    }

    /// <summary>
    /// 长按类型
    /// </summary>
    /// <param name="args"></param>
    /// <param name="go"></param>
    /// <param name="state"></param>
    public static void ShowTips(EventMultiArgs args, GameObject go, bool state)
    {
        // 详细信息提示界面
        /*
        if (args.GetDictionary().ContainsKey("ID"))
        {
            //showDetailTips(args, go, state);
            return;
        }
         * */
        if (state) {
            UIManager.CreateWin(UIName.TIPS);
            var controller = (TipsController)UIManager.GetControler(UIName.TIPS);
            args.AddArg("TransObj", go.transform);
            controller.ShowTips(args);
        } else {
            UIManager.DestroyWin(UIName.TIPS);
        }
    }


    public static void AddChildNodeLayer(Transform trans)
    {
        Transform[] transforms = trans.GetComponentsInChildren<Transform>(true);
        foreach (Transform node in transforms) {
            node.gameObject.layer = LayerMask.NameToLayer("UI");
        }
    }

    public static GameObject AddChild(GameObject parent, GameObject prefab, Vector3 initPos)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
        if (go != null && parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = initPos;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }

    public static GameObject AddChild(GameObject parent, GameObject prefab)
    {
        return AddChild(parent, prefab, Vector3.zero);
    }


    /// <summary>
    /// 跑进度条表现
    /// </summary>
    /// <param name="startValue">0~1初始值</param>
    /// <param name="addValue">增加值, >0表示向前跑, <0表示往回跑。</param>
    /// <param name="nTime"></param>
    public static void ProgressRunTo(GameObject target, float startValue, float addValue, float nTime)
    {
        Hashtable args = new Hashtable();
        args.Add("startvalue", startValue);
        args.Add("addValue", addValue);
        args.Add("nTime", nTime);
        ProgressRunTo(target, args);
    }
    /// <summary>
    /// 跑进度条表现
    /// </summary>
    /// <param name="startvalue">
    /// A <see cref="float"/> 0~1初始值
    /// </param>
    /// <param name="addValue">
    /// A <see cref="float"/> 增加值, >0表示向前跑, <0表示往回跑。
    /// </param>
    /// <param name="nTime">
    /// A <see cref="float"/> 时间
    /// </param>
    /// <param name="onupdate">
    /// A <see cref="System.String"/> for the name of a function to launch on every step of the animation.
    /// </param>
    /// <param name="onupdatetarget">
    /// A <see cref="GameObject"/> for a reference to the GameObject that holds the "onupdate" method.
    /// </param>
    /// <param name="onupdateparams">
    /// A <see cref="System.Object"/> for arguments to be sent to the "onupdate" method.
    /// </param>
    /// <param name="onfull">
    /// A <see cref="System.String"/> for the name of a function to launch on every step of the animation.
    /// </param>
    /// <param name="onfulltarget">
    /// A <see cref="GameObject"/> for a reference to the GameObject that holds the "onupdate" method.
    /// </param>
    /// <param name="onfullparams">
    /// A <see cref="System.Object"/> for arguments to be sent to the "onupdate" method.
    /// </param>
    /// <param name="oncomplete">
    /// A <see cref="System.String"/> for the name of a function to launch at the end of the animation.
    /// </param>
    /// <param name="oncompletetarget">
    /// A <see cref="GameObject"/> for a reference to the GameObject that holds the "oncomplete" method.
    /// </param>
    /// <param name="oncompleteparams">
    /// A <see cref="System.Object"/> for arguments to be sent to the "oncomplete" method.
    /// </param>
    public static void ProgressRunTo(GameObject target, Hashtable args)
    {
        UIProgressBar bar = target.GetComponent<UIProgressBar>();
        if (bar == null) {
            LogSys.LogError("ProgressRunTo Error: target is not UIProgressBar");
            return;
        }

        ProgressRunMono mono = target.GetComponent<ProgressRunMono>();
        if (mono == null) {
            mono = target.AddComponent<ProgressRunMono>();
        }
        mono.ProgressRunTo(args);
    }

    public static float GetPlayerExpAddPercent(int curLev, int curExp, int curMaxExp, int getExp)
    {
        //int playerTempAddExp = 0;//（已经跑过多少经验）
        int playerTempLev = curLev;//(当前Lev)
        int playerTempExp = curExp;//(当前经验值)
        //int playerTempMaxExp = curMaxExp;//(满条的经验值)

        float reslut = 0f;
        /*
        MyPlayer _currData = GameDataMgr.PLAYER_DATA;
        int lastExp = getExp;//剩余未计算的经验值
        while (lastExp > 0) {
            if (playerTempExp + lastExp < curMaxExp) {
                //不会满时
                reslut += (float)lastExp / (float)curMaxExp;
                return reslut;
            } else if (playerTempExp + lastExp == curMaxExp) {
                //刚好满时(多加一点点，让进度条变空)
                reslut += (float)lastExp / (float)curMaxExp + 0.001f;
                return reslut;
            } else {
                //满了还有剩余时
                reslut += (float)(curMaxExp - playerTempExp) / (float)curMaxExp;
                lastExp = lastExp - (curMaxExp - playerTempExp);
                playerTempLev += 1;
                playerTempExp = 0;
                LeadHeroExpConfigItem role = _currData.GetRoleLevelData(playerTempLev.ToString());
                if (role != null) {
                    curMaxExp = role.exp;
                } else {
                    //满级
                    return reslut;
                }
            }
        }
         */
        return reslut;
    }

    public static float GetHeroExpAddPercent(int curLev, int curExp, int curMaxExp, int getExp)
    {
        //int tempAddExp = 0;//（已经跑过多少经验）
        int tempLev = curLev;//(当前Lev)
        int tempExp = curExp;//(当前经验值)
        int tempMaxExp = curMaxExp;//(满条的经验值)
        float reslut = 0f;
        /*
        MyPlayer _currData = GameDataMgr.PLAYER_DATA;
        int lastExp = getExp;//剩余未计算的经验值
        while (lastExp > 0) {
            if (tempExp + lastExp < curMaxExp) {
                //不会满时
                reslut += (float)lastExp / (float)curMaxExp;
                return reslut;
            } else if (tempExp + lastExp == curMaxExp) {
                //刚好满时(多加一点点，让进度条变空)
                reslut += (float)lastExp / (float)curMaxExp + 0.001f;
                return reslut;
            } else {
                //满了还有剩余时
                reslut += (float)(curMaxExp - tempExp) / (float)curMaxExp;
                lastExp = lastExp - (curMaxExp - tempExp);
                tempLev += 1;
                tempExp = 0;
                LeadHeroExpConfigItem role = _currData.GetRoleLevelData(tempLev.ToString());
                if (role != null) {
                    curMaxExp = role.hero_exp;
                } else {
                    //满级
                    return reslut;
                }
            }
        }
         */
        return reslut;
    }

    public static void FadeIn(GameObject target, float time, EventDelegate complete = null)
    {
        UIRect mRect = target.GetComponent<UIRect>();
        mRect.alpha = 0f;
        var comp = TweenAlpha.Begin(target, time, 1f);
        if (complete == null) return;
        comp.onFinished.Add(complete);
    }

    /// <summary>
    /// 创建模型
    /// </summary>
    /// <param name="shaping">外形</param>
    /// <param name="callBack">回调</param>
    public static void CreateModel(string shaping, PlayerDelegate.onPlayerEvent callBack)
    {
        /*
        HeroWeaponConfigItem item = ConfigDataMgr.getInstance().HeroWeaponCfg.GetDataByKey(shaping);
        string modelId = "";
        int equipLv = 1;
        if (item != null) {
            modelId = item.hero_id.ToString();
            equipLv = item.weapon1_id;
        } else {
            modelId = shaping.Substring(0, shaping.IndexOf(",", StringComparison.Ordinal));
        }
        PlayerManager.getInstance().createModel(modelId, callBack, equipLv);
         */
    }

    public static string GetFileMD5(byte[] data)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] result = md5.ComputeHash(data);
        string fileMD5 = "";
        foreach (byte b in result) {
            fileMD5 += Convert.ToString(b, 16);
        }
        return fileMD5;
    }

    public static string GetStringMD5(string input, Encoding encode)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] t = md5.ComputeHash(encode.GetBytes(input));
        StringBuilder sb = new StringBuilder(32);
        for (int i = 0; i < t.Length; i++)
            sb.Append(t[i].ToString("x").PadLeft(2, '0'));
        return sb.ToString();
    }



    /// <summary>
    /// 获取时间戳
    /// </summary>
    /// <returns></returns>
    public static int GetTimeStamp()
    {
        //        var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //        return (long)ts.TotalSeconds;
        return ClientNetwork.Instance.heartBeatCheck.NowTime;
    }

    /// <summary>
    /// 获取客户端当前时间（时间戳）
    /// </summary>
    /// <returns></returns>
    public static int GetClientTime()
    {
        double timeStamp = GetCurrentTime();
        return (int)timeStamp;//时间戳表达
    }

    public static double GetTimeStampWithHMS(int year, int month, int day, int hour, int min, int sec)
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
        double timeStamp = (new System.DateTime(year, month, day, hour, min, sec) - startTime).TotalSeconds;
        return timeStamp;
    }

    public static double GetCurrentTime()
    {
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0));
        double timeStamp = (DateTime.Now - startTime).TotalSeconds;
        return timeStamp;
    }

    /// <summary>
    /// 获取服务端当前时间（时间戳）
    /// </summary>
    /// <returns></returns>
    public static int GetServerTime()
    {
        int clientTime = GetClientTime();
        if (ClientNetwork.Instance.heartBeatCheck == null)
        {
            return clientTime;
        }
        return clientTime - ClientNetwork.Instance.heartBeatCheck.OffsetTime;//时间戳表达
    }

    /// <summary>
    /// 时间戳转换成日期
    /// </summary>
    /// <param name="timestamp">时间戳（秒）</param>
    /// <returns></returns>
    public static DateTime TimeStampToDateTime(uint timestamp)
    {
        DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timestamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);

        return dateTimeStart.Add(toNow);
    }

    /// <summary>
    /// 时间戳转换成字串(HH:MM:SS)
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static string TimeStampToString(uint timestamp)
    {
        return TimeStampToString(timestamp, "HH:mm:ss");
    }

    public static string TimeStampToString(uint timestamp, string format)
    {
        DateTime date = TimeStampToDateTime(timestamp);
        return date.ToString(format);//data
    }

    public static string GetTimeSpan(uint timestamp)
    {
        var a = GetServerTime() - timestamp;
        var ts = TimeSpan.FromSeconds(a);
        if (ts.Days > 31) {
            return string.Format("{0}月", (int) ts.Days/31);
        }
        if (ts.Days > 0) {
            return string.Format("{0}天", ts.Days);
        }

        if (ts.Hours > 0) {
            return string.Format("{0}小时", ts.Hours);
        }

        if (ts.Minutes > 0) {
            return string.Format("{0}分钟", ts.Minutes);
        }

        if (ts.Minutes > 0) {
            return string.Format("{0}分钟", ts.Minutes);
        }

        return string.Format("{0}秒", ts.Minutes);
    }

    /// <summary>
    /// DateTime时间格式转换为Unix时间戳格式
    /// </summary>
    /// <param name="time"> DateTime时间格式</param>
    /// <param name="isUtc"></param>
    /// <returns>Unix时间戳格式</returns>
    public static uint ConvertDateTimeInt(DateTime time, bool isUtc = false)
    {
        var startTime = isUtc ? new DateTime(1970, 1, 1, 0, 0, 0, 0) : TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
        return (uint)(time - startTime).TotalSeconds;
    }

    public static void ShowWaitWin(WaitFlag eFlag = WaitFlag.Unkown, float fWaitTime = 10f, Action action = null)
    {
        WaitingController controller = (WaitingController)UIManager.GetControler(UIName.WAITING);
        controller.BackAction = action;
        controller.ShowWaitingWin(eFlag, fWaitTime);
    }
    public static void HideWaitWin(WaitFlag eFlag = WaitFlag.Unkown)
    {
        WaitingController controller = (WaitingController)UIManager.GetControler(UIName.WAITING);
        controller.HideWaitingWin(eFlag);
    }

    public static bool IsWaitShowing(WaitFlag eFlag)
    {
        WaitingController controller = (WaitingController)UIManager.GetControler(UIName.WAITING);
        return controller.IsWaitShowing(eFlag);
    }
    /// <summary>
    /// 时间戳转为C#格式时间
    /// </summary>
    /// <param name="timeStamp"></param>
    /// <returns></returns>
    public static DateTime ConvertIntDateTime(uint timeStamp)
    {
        var dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));
        return dateTimeStart.AddSeconds(timeStamp);
    }


    /// <summary>
    /// 仅用于两个类名不同字段完全相同的对象转换
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T CopySameFieldsObject<T>(object source)
    {
        var srcT = source.GetType();
        var destT = typeof(T);
        var instance = destT.InvokeMember("", BindingFlags.CreateInstance, null, null, null);                        // 构造一个要转换对象实例
        var srcFields = srcT.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);        // 这里指定搜索所有公开和非公开的字段

        for (int i = 0; i < srcFields.Length; i++)                                                                   // 将 source 的每个字段的值分别赋值到目标对象里
        {
            var field = srcFields[i];
            var fieldInfo = destT.GetField(field.Name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo != null) {
                fieldInfo.SetValue(instance, field.GetValue(source));
            }
        }
        return (T)instance;
    }


    /// <summary>
    /// 处理换行
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string Wrap(string str)
    {
        return str.Replace("\\n", "\n");
    }

    //递归设置layer
    public static void SetLayerRecursive(GameObject obj, string sLayer)
    {
        int layer = LayerMask.NameToLayer(sLayer);
        SetLayerRecursive(obj, layer);
    }
    //递归设置layer
    public static void SetLayerRecursive(GameObject obj, int iLayer)
    {
        obj.layer = iLayer;
        for (int i = 0; i < obj.transform.childCount; i++) {
            GameObject childObj = obj.transform.GetChild(i).gameObject;
            SetLayerRecursive(childObj, iLayer);
        }
    }

    //递归设置active
    public static void SetActiveRecursive(GameObject obj, bool bActive)
    {
        obj.SetActive(bActive);
        for (int i = 0; i < obj.transform.childCount; i++) {
            GameObject childObj = obj.transform.GetChild(i).gameObject;
            SetActiveRecursive(childObj, bActive);
        }

    }

    /// <summary>
    /// 获取最后 n (n > 0) 个字符
    /// </summary>
    /// <param name="str"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static string ReverseSubstring(string str, int n = 1)
    {
        var m = string.Format(@"\S{{1,{0}}}\b", n);
        var reg = new Regex(m);
        return reg.Match(str).ToString();
    }

    public static void CallUIEvent(string sUIName, short uiEventID, EventMultiArgs args)
    {
        ControllerBase cont = UIManager.GetControler(sUIName);
        if (cont != null) {
            cont.CallUIEvent(uiEventID, args);
        }
    }


    /// <summary>
    /// 设置模型描边
    /// </summary>
    /// <param name="pModel"></param>
    /// <param name="width"></param>
    public static void SetModelOutLine(Model pModel, float width)
    {
        if (Math.Abs(width) < 0.000000000001f) return;

        if (pModel == null || pModel.ModelRootObj == null) return;
        SkinnedMeshRenderer skmr = pModel.ModelRootObj.GetComponent<SkinnedMeshRenderer>();
        for (int i = 0; i < skmr.materials.Length; i++) {
            Material mat = skmr.materials[i];
            Shader sd = Resources.Load("Shaders/Outline") as Shader;
            mat.shader = sd;
            mat.SetFloat("_Outline", width);
        }
    }

    public static void AddShadowObject(Transform tfTarget, float fShadowSize = 41.03f)
    {
        if (tfTarget.Find("shadow") == null) {
            GameObject objShadow = new GameObject("shadow");
            //if (objShadow != null)//  这个总为 ture
            //{
            objShadow.name = "shadow";
            objShadow.transform.parent = tfTarget;
            objShadow.transform.localPosition = new Vector3(0.0f, 3.0f, 0.0f);
            objShadow.transform.localEulerAngles = new Vector3(90.0f, 0.0f, 0.0f);

            Projector componentProjShadow = objShadow.AddComponent<Projector>();
            if (componentProjShadow != null) {
                componentProjShadow.nearClipPlane = 1.22f;
                componentProjShadow.farClipPlane = 9.08f;
                componentProjShadow.fieldOfView = fShadowSize;
                componentProjShadow.material = Resources.Load("Materials/PlayerShadow") as Material;
                componentProjShadow.ignoreLayers = ~(1 << LayerMask.NameToLayer("Ground"));
            }
            //}
        }
    }

    public static void ReturnToLoginScene()
    {
        //DataEye登出统计
        //DataEyeUtils.logout();

        //MyPlayer._bLoginOut = true;

        //-------  模糊。。。。。清理了
        var CamObj = UnityEngine.GameObject.Find("Scene/Cameras/SceneCamera");
        BlurOptimized blurEff = CamObj.GetComponent<BlurOptimized>();
        if (blurEff!=null && blurEff.enabled){
            blurEff.enabled = false;
        }

        ClientNetwork.ResetAllData();//网络管理初始化
        GameDataMgr.ClearAllData();//游戏数据清空
        LoginInputController.ClearLuaData();
        GameDataMgr.LOGIN_DATA.IsConnectGamerServer = false;
        GameDataMgr.LOGIN_DATA.IsLoginGameServer = false;
        HideWaitWin(WaitFlag.ClearAllWait);
        ClientNetwork.Instance.CloseSocket();
        UtilTools.RemoveAllWinExpect();
        if (!UIManager.IsWinShow(UIName.LOGIN_INPUT_WIN)){
            UIManager.CreateWin(UIName.LOGIN_INPUT_WIN);
        }
        GameObject sceneObj = GameObject.Find("Scene");
        if (sceneObj)
        {
            StartUpScene startUpMono = sceneObj.GetComponent<StartUpScene>();
            if (startUpMono != null)
                startUpMono.ReturnToLogin();
        }
    }

    public static void ChangeLogin()
    {
        var ctrl = UIManager.GetControler<LoginInputController>();
        if (ctrl != null){
            
//            ctrl.LoginPhone();
            ctrl.ChangeAccount = true;
        }
        PlayerPrefs.SetString("accountServerLoginContent", "");
        ReturnToLoginScene();

    }

    //设置RenderQueue
    public static void SetModelRenderQueueByUIParent(Transform parent, Transform target, int offset = 0)
    {
        int renderQueue = 0;
        if (parent != null && target != null) {
            // 改变renderqueue
            UIPanel[] panels = parent.GetComponentsInParent<UIPanel>();
            foreach (var p in panels) {
                if (p.startingRenderQueue > renderQueue) {
                    renderQueue = p.startingRenderQueue;
                }
            }
            if (renderQueue == 0)
                renderQueue = 3000;
            renderQueue += offset;
            SetModelRenderQueue(target, renderQueue);
        }
    }

    public static void SetModelRenderQueue(Transform target, int renderQueue)
    {

        MeshRenderer[] arr = target.GetComponentsInChildren<MeshRenderer>(true);
        foreach (var ps in arr)
        {
            ps.material.renderQueue = renderQueue;
        }

        SkinnedMeshRenderer[] arr1 = target.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (var ps in arr1)
        {
            for (int i = 0; i < ps.materials.Length; i++)
            {
                ps.materials[i].renderQueue = renderQueue;
            }
            //ps.material.renderQueue = renderQueue;
        }
    }


    //设置RenderQueue
    public static void SetEffectRenderQueueByUIParent(Transform parent, Transform targetEffect, int offset = 0)
    {
        int renderQueue = 0;
        if (parent != null && targetEffect != null)
        {
            // 改变renderqueue
            UIPanel[] panels = parent.GetComponentsInParent<UIPanel>();
            foreach (var p in panels)
            {
                if (p.startingRenderQueue > renderQueue)
                {
                    renderQueue = p.startingRenderQueue;
                }
            }
            if (renderQueue == 0)
                renderQueue = 3000;
            renderQueue += offset;


            ParticleSystem[] arr = targetEffect.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in arr)
            {
                ps.GetComponent<Renderer>().material.renderQueue = renderQueue;
            }

            MeshRenderer[] arr_mesh = targetEffect.GetComponentsInChildren<MeshRenderer>(true);
            foreach (var mesh in arr_mesh)
            {
                mesh.GetComponent<MeshRenderer>().material.renderQueue = renderQueue;
            }
        }
    }

    //设置RenderQueue
    public static void SetEffectRenderQueue(Transform targetEffect, int renderQueue)
    {
        if (targetEffect == null)
            return;
        ParticleSystem[] arr = targetEffect.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in arr)
        {
            ps.GetComponent<Renderer>().material.renderQueue = renderQueue;
        }

        MeshRenderer[] mesh_arr = targetEffect.GetComponentsInChildren<MeshRenderer>(true);
        foreach (var mesh in mesh_arr)
        {
            mesh.material.renderQueue = renderQueue;
        }
    }

    //重新播放光效
    public static void ReplayEffect(Transform tfEffect)
    {
        tfEffect.gameObject.SetActive(true);
        Animation men_ani = tfEffect.GetComponent<Animation>();
        if (men_ani != null) {
            men_ani.enabled = true;
            men_ani.Stop();
            men_ani.Play();
        }

        ParticleSystem[] particles = tfEffect.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < particles.Length; i++) {
            particles[i].Stop();
            particles[i].Play();
        }
    }

    public static void SetScale(GameObject _effectGameObj, float fScale)
    {
        if (_effectGameObj == null) return;
        Vector3 oldScale = _effectGameObj.transform.localScale;
        _effectGameObj.transform.localScale = oldScale * fScale;

        ParticleSystem[] arr = _effectGameObj.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in arr)
        {
            ps.startSize = ps.startSize * fScale;
            //ps.velocityOverLifetime =
            if (ps.shape.enabled == true && ps.shape.shapeType == ParticleSystemShapeType.Box)
            {
                var sh = ps.shape;
                sh.box = sh.box * fScale;
            }
            else if (ps.shape.enabled == true && ps.shape.shapeType == ParticleSystemShapeType.Mesh)
            {
                ps.transform.localScale *= fScale;
            }
            else if (ps.shape.enabled == true && ps.shape.shapeType == ParticleSystemShapeType.Sphere)
            {
                var sh = ps.shape;
                sh.radius = sh.radius * fScale;
            }
        }

    }

    //播放音效(默认2秒后删除)
    public static SoundEffect PlaySoundEffect(string strAudioClipName, float autoDestroyTime = 2f, bool bLoop = false, float fPerTime = 1f, GameObject taget = null, int soundType = 1, float delay = 0)//soundType 1
    {
        if (!ApplicationMgr._bInitOK)
        {
            return null;
        }
        GameObject pTarget = GameSceneManager.uiCameraObj;
        if (taget != null) {
            pTarget = taget;
        }
        SoundEffect sound = pTarget.AddComponent<SoundEffect>();
        float volume = 1f;
        if (soundType == 1)
        {
            volume = PlayerPrefs.GetFloat("gameValue", 50f) / 100f;
        }
        else if(soundType==2)
        {
            volume = PlayerPrefs.GetFloat("gunValue", 50f) / 100f;
        }
        sound.Delay = delay;
        sound.Volume = volume;
        sound.Play(strAudioClipName, autoDestroyTime, bLoop, fPerTime);
        return sound;
    }
    public static void StopBGM()
    {
        BGM csBGM = GameSceneManager.getInstance().CurSceneObject.GetComponent<BGM>();
        if (csBGM != null){
            csBGM.Stop();
        }
    }

    public static void SetServerListTip(string sTip,bool isShowProgress=false,float process=0f)
    {
        Transform tfVersionUpdate = GameSceneManager.getInstance().CurSceneObject.transform.Find("VersionUpdate");
        if (tfVersionUpdate != null) {
            tfVersionUpdate.GetComponent<VersionUpdate>().SetServerListTip(sTip,isShowProgress,process);
        }

    }


    /// <summary>
    /// 银币和元宝不足提示，跳转至充值界面
    /// </summary>
    /// <param name="type"></param>
    public static bool IsMoneyEnough(int type, ulong needMoney)
    {
        return true;
        /*
        if (type == 100 && needMoney > GameDataMgr.PLAYER_DATA.Gold)//银币
        {
            GameDataMgr.FASTBUY_DATA.flag = 1;
            MessageDialog(GameText.GetStr("gold_lmt"), "614d46", "Center", CancelMessageDialog, CancelMessageDialog);
            return false;
        } else if (type == 101 && needMoney > GameDataMgr.PLAYER_DATA.Diamond)//元宝
          {
              MessageDialog(GameText.GetStr("diamond_lmt"), "614d46", "Center", CancelMessageDialog, CancelMessageDialog);
            return false;
        } else
            return true;
         */
    }


    //点击的是否是屏幕有效区
    public static bool ClickInValidArea()
    {
        // #if UNITY_EDITOR || UNITY_EDITOR_OSX
        //         return true;
        // #endif
        Camera uiCamera = GameSceneManager.uiCameraObj.GetComponent<Camera>();
        if (uiCamera == null)
            return true;

        Ray uiRay = uiCamera.ScreenPointToRay(Input.mousePosition);//点到UI
        RaycastHit uiHit = new RaycastHit();
        if (Physics.Raycast(uiRay, out uiHit, 1000, 1 << LayerMask.NameToLayer("ValidViewArea")))
            //if (nguiCamera.isOrthoGraphic)
            return true;
        else
            return false;
    }

    //点击的是否是UI
    public static bool ClickUI()
    {
        Camera uiCamera = GameSceneManager.uiCameraObj.GetComponent<Camera>();
        if (uiCamera == null)
            return false;

        Ray uiRay = uiCamera.ScreenPointToRay(Input.mousePosition);//点到UI
        RaycastHit uiHit = new RaycastHit();
        if (Physics.Raycast(uiRay, out uiHit, 1000, 1 << LayerMask.NameToLayer("UI")))
            //if (nguiCamera.isOrthoGraphic)
            return true;

        return false;
    }

    public static void SetTag(GameObject target, int nTag)
    {
        ComponentData data = ComponentData.Get(target);
        data.Tag = nTag;
    }

    public static int GetTag(GameObject target)
    {
        int nTag = 0;
        ComponentData data = target.GetComponent<ComponentData>();
        if (data != null) {
            nTag = data.Tag;
        }
        return nTag;
    }
    public static void SetId(GameObject target, int nID)
    {
        ComponentData data = target.GetComponent<ComponentData>();
        if (data == null) {
            data = target.AddComponent<ComponentData>();
        }
        data.Id = nID;
    }

    public static int GetId(GameObject target)
    {
        int nID = 0;
        ComponentData data = target.GetComponent<ComponentData>();
        if (data != null) {
            nID = data.Id;
        }
        return nID;
    }

    public static void ShowScreenshot()
    {
        ScreenshotController cont = (ScreenshotController)UIManager.GetControler(UIName.SCREENSHOT_WIN);
        cont.ShowScreenshot();
    }

    public static void HideScreenshot()
    {
        ScreenshotController cont = (ScreenshotController)UIManager.GetControler(UIName.SCREENSHOT_WIN);
        cont.HideScreenshot();
    }

    public static void RemoveAllWinExpect()
    {
        UIManager.RemoveAllWinExpect(new string[] { UIName.SCREENSHOT_WIN, UIName.WAITING, UIName.LOADING_WIN, UIName.MESSAGE_DIALOG,UIName.LOGIN_INPUT_WIN});
        HideScreenshot();
    }

    public static void SaveScreenshot()
    {
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null)
            tools.SaveScreenshot();
    }

    /// <summary>
    /// 复制字符串到粘贴板
    /// </summary>
    /// <param name="strValue"></param>
    public static void CopyString2Clipboard(string strValue)
    {
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null)
            tools.copyString2Clipboard(strValue);
    }

    /// <summary>
    /// 从粘贴板获取字符串
    /// </summary>
    /// <returns></returns>
    public static string GetStringFromClipboard()
    {
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null)
            return tools.getStringFromClipboard();

        return "";
    }

    /// <summary>
    /// 播放视频
    /// </summary>
    /// <param name="videoName"></param>
    /// <param name="videoWidth"></param>
    /// <param name="videoHeight"></param>
    /// <param name="mode"></param>
    public static void PlayeVideo(string videoName, int videoWidth=1136, int videoHeight=640, int clipCount = 1, int mode=1, onPlayVideoComplete callback = null)
    {
#if UNITY_ANDROID
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null)
            tools.playVideo(videoName, videoWidth, videoHeight, clipCount, mode, callback);
#else
        if (callback != null)
            callback();
#endif
    }



    /// <summary>
    /// 设置物品模板
    /// </summary>
    /// <param name="tr">模板对象</param>
    /// <param name="icon">图标</param>
    /// <param name="num">数量</param>
    /// <param name="align">默认右对齐</param>
    public static void SetItemTemplate(Transform tr, string icon, string num, NGUIText.Alignment align = NGUIText.Alignment.Right)
    {
        SetIcon(tr.Find("img"), icon);
        var label = tr.Find("num").GetComponent<UILabel>();
        label.alignment = align;
        label.text = num;
    }

    /// <summary>
    /// 读取assetbundle下资源时，要加"resources/"
    /// </summary>
    /// <param name="strAssetPath"></param>
    /// <returns></returns>
    public static string PathCheck(string strAssetPath)
    {
        //不裁则会去assetbundle下找
        if (AssetManager.getInstance().IsFirstUseStreamingAssets)
        {
            string pre_str = "";
            if (strAssetPath.Length > 10)
                pre_str = strAssetPath.Substring(0, 10);
            if (pre_str != "Resources/" && pre_str != "resources/")
            {
                return "resources/" + strAssetPath;
            }
        }
        return strAssetPath;
    }


    /// <summary>
    /// 读取assetbundle下资源时，要加后缀".prefab"
    /// </summary>
    /// <param name="strAssetPath"></param>
    /// <returns></returns>
    public static string PrefabPathCheck(string strAssetPath)
    {
        if (strAssetPath.Length <= 7 || ".prefab" != strAssetPath.Substring(strAssetPath.Length - 7))
        {
            strAssetPath = strAssetPath + ".prefab";
        }
        return strAssetPath;
    }
    /// <summary>
    /// 读取assetbundle下texture时，要加后缀".png"
    /// </summary>
    /// <param name="strAssetPath"></param>
    /// <returns></returns>
    public static string PngPathCheck(string strAssetPath)
    {
        if (strAssetPath.Length <= 4 || ".png" != strAssetPath.Substring(strAssetPath.Length - 4))
        {
            strAssetPath = strAssetPath + ".png";
        }
        return strAssetPath;
    }

    public static void UpdateShaders(GameObject target)
    {
        Renderer[] rends = target.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer rnd in rends)
        {
            string shader_name = rnd.material.shader.name;
            //rnd.material.shader = Shader.Find(shader_name) as Shader;
            if (rnd.materials.Length > 0)//如果用多个material
            {
                for (int k = 0; k < rnd.materials.Length; k++)
                {
                    shader_name = rnd.materials[k].shader.name;
                    rnd.materials[k].shader = Shader.Find(shader_name) as Shader;
                }

            }
        }
    }

    public static void MultiTouchSwitch(bool bFlag)
    {
        if (GameSceneManager.uiCameraObj != null)
        {
            UICamera uicam = GameSceneManager.uiCameraObj.GetComponent<UICamera>();
            uicam.allowMultiTouch = bFlag;
        }
    }
    //返回0:没网络  1:wifi网络   2:移动网络  3:未知
    public static int GetCurrentNetworkType()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return 0;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            return 1;
        }
        //当用户使用移动网络时  
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            return 2;
        }
        return 3;
    }

    public static int GetCurViewHeight()
    {
        int nHeight = 640;
        if (GameSceneManager.uiCameraObj != null)
        {
            CameraAjustor adj = GameSceneManager.uiCameraObj.GetComponent<CameraAjustor>();
            nHeight = (int)adj.curHeight;
        }
        return nHeight;
    }
    public static void SetBgm(string path)
    {
        if (GameSceneManager.getInstance() == null || GameSceneManager.getInstance().CurSceneObject == null) return;
        BGM bgm = GameSceneManager.getInstance().CurSceneObject.GetComponent<BGM>();
        float volume = 50;

        if (PlayerPrefs.HasKey("bgmValue"))
        {
            volume = PlayerPrefs.GetFloat("bgmValue") / 100f;
        }
        if (bgm == null)
        {
            bgm = GameSceneManager.getInstance().CurSceneObject.AddComponent<BGM>();
            bgm.strAudioClipName = path;
            bgm._fVolume = volume;

        }
        else
        {
            bgm.Stop();
            bgm.strAudioClipName = path;
            bgm._fVolume = volume;
            bgm.StartPlay();
        }
    }
    public static void PlayeMovie(string mp4Path, string videoName = "")
    {

#if UNITY_ANDROID
        UtilTools.PlayeVideo(videoName.ToLower());
#elif UNITY_IOS
		Handheld.PlayFullScreenMovie(mp4Path, Color.black, FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFit);
#endif
    }

    public static void ResetMessageWithIconCount()
    {
        MessageWithIconController controller = (MessageWithIconController)UIManager.GetControler(UIName.MESSAGE_WITH_ICON_WIN);
        controller.toBack();
    }
    public static void ShowMessageWithIcon(string id, string num, Vector3 position, MessageWinType type)
    {
        if (!UIManager.IsWinShow(UIName.MESSAGE_WITH_ICON_WIN))
            UIManager.CreateWin(UIName.MESSAGE_WITH_ICON_WIN);
        MessageWithIconController controller = (MessageWithIconController)UIManager.GetControler(UIName.MESSAGE_WITH_ICON_WIN);
        controller.SetMessage(id, num, type, position);
    }

    public static bool ArrayHeadIsWoDong(byte[] array)
    {
        //"WoDong"
        if (array.Length < 6*2)
        {
            return false;
        }
        byte[] wodong = System.Text.Encoding.Default.GetBytes("WoDong");
        for (int i=0; i< 6; i++)
        {
            if (wodong[i] != array[i])
            {
                return false;
            }
        }
        return true;
    }



    public static void SetFPS(FPSLevel level)
    {
        switch (level)
        {
            case FPSLevel.Normal:
#if UNITY_IOS
                Application.targetFrameRate = 40;
#elif UNITY_ANDROID
                Application.targetFrameRate = 40;
#else
                Application.targetFrameRate = 60;
#endif
                break;
            case FPSLevel.OnlyUI:
#if UNITY_IOS
                Application.targetFrameRate = 40;
#elif UNITY_ANDROID
                Application.targetFrameRate = 40;
#else
                Application.targetFrameRate = 60;
#endif
                break;
            default:
                break;
        }
    }

    public static void LoginFailedAndShowLoginWin()
    {
        GameObject sceneObj = GameObject.Find("Scene");
        if (sceneObj)
        {
            StartUpScene startUpMono = sceneObj.GetComponent<StartUpScene>();
            if (startUpMono != null)
                startUpMono.LoginFailedAndShowLoginWin();
        }
    }

    public static int GetFishRenderQueueByDepth(int depth)
    {
        return 3000 + depth;
    }

    public static void LoadHead(string path, UITexture head,bool isPlayer)
    {
        GameHeadLoader.Instance.LoadHead(path,head,isPlayer);
    }

    /// <summary>
    /// 从相册捡取图片
    /// </summary>
    /// <param name="compressFormat">压缩格式 (png, jpeg/jpg)</param>
    /// <param name="crop">是否裁剪</param>
    /// <param name="outputX">输出X（宽度）</param>
    /// <param name="outputY">输出Y（高度）</param>
    /// <param name="gameObjectName">unity回调对象名称</param>
    public static void PickPhotoFormAlbum(string compressFormat = "png", bool crop = false, int outputX = 120, int outputY = 120, string gameObjectName = "Scene")
    {
        gameObjectName = "Scene";
        //#if UNITY_ANDROID
            JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
            if (tools != null)
                tools.pickPhotoFromAlbum(compressFormat,crop,outputX,outputY,gameObjectName);
        //#endif
    }
    /// <summary>
    /// 从相册捡取图片
    /// </summary>
    /// <param name="compressFormat">压缩格式 (png, jpeg/jpg)</param>
    /// <param name="crop">是否裁剪</param>
    /// <param name="outputX">输出X（宽度）</param>
    /// <param name="outputY">输出Y（高度）</param>
    /// <param name="gameObjectName">unity回调对象名称</param>
    public static void pickPhotoFromCamera(string compressFormat, bool crop, int outputX, int outputY,
        string gameObjectName)
    {
//#if UNITY_ANDROID
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null)
            tools.pickPhotoFromCamera(compressFormat,crop,outputX,outputY,gameObjectName);
//#endif
    }


    /// <summary>
    /// 上传文件（流）到服务器
    /// </summary>
    /// <param name="url">URL地址</param>
    /// <param name="destFileName">目标文件名</param>
    /// <param name="fileAbsolutePath">文件绝对路径</param>
    public static void asyncHttpUploadFile(string url, string destFileName, string fileAbsolutePath)
    {
//#if UNITY_ANDROID
        JARUtilTools tools = GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (tools != null) tools.asyncHttpUploadFile(url, false, destFileName, fileAbsolutePath, "UIRoot");
//#endif
    }

    public static void BindingPhone()
    {
        var ctrl = UIManager.GetControler<RegisterBindingController>();
        if (ctrl != null){
            ctrl.isBinding = true;
            UIManager.CreateWin(UIName.REGISTER_BINDING_WIN);
        }
    }
    /// <summary>
    /// 复制文本到手机
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static bool CopyTextToPhone(string str)
    {
#if UNITY_ANDROID
        var jarTools =  GameSceneManager.uiCameraObj.GetComponent<JARUtilTools>();
        if (jarTools != null){
            jarTools.copyString2Clipboard(str);
            return true;
        }
        return false;
#endif
        ShowMessage(GameText.GetStr("onlySupportOnPhone"));
        return false;
    }
    public static void ShowWaitFlag()
    {
        ShowWaitWin(WaitFlag.ActivityRequest, 5f);
    }
    public static void HideWaitFlag()
    {
        HideWaitWin(WaitFlag.ActivityRequest);
    }

    public static void loadTexture(UITexture texture,string path,bool isMainTexture)
    {
//        return  asset.AssetManager.getInstance().loadTexture(path);
        asset.AssetManager.getInstance().loadAssetAsync(path, (manual, task) =>
            {
                
                if(texture==null)return;
                if (isMainTexture){
                    texture.mainTexture = (Texture) AssetManager.getInstance().getAsset(path);
                }
                else{
                    texture.alphaTexture = (Texture) AssetManager.getInstance().getAsset(path);
                }
                AssetManager.getInstance().minusAssetbundleRefCount(path);
            });
    }
    public static void RegisterCount(string Id)
    {
        SDKManager.getInstance().CommonSDKInterface.OnRegister(Id);
    }
    public static void OnUmSdkInit(string appkey, string channel)
    {
        SDKManager.getInstance().CommonSDKInterface.OnUmSdkInit(appkey, channel);
    }
    public static void GetAvmpSign(string input,int type)
    {
        SDKManager.getInstance().CommonSDKInterface.GetAvmpSign(input, type);
    }
    // 分享给微信好友或群
    public static void ShareWeChat(string code, string title, string desc)
    {
         #if UNITY_IOS
        // 获取分享URL
        string targetUrl = SDKManager.ShareWebURL + code;
        var www = new WWW(targetUrl);
        while (!www.isDone) ;
        if (www.error == null)
        {
            _object_c_interface_.WXSDK.requestWXWebShare(1, title, desc, "100x100", www.text);
        }
        #else
        CommonShareInfo shareInfo = new CommonShareInfo();
        string targetUrl = SDKManager.ShareWebURL + code;
        var www = new WWW(targetUrl);
        while (!www.isDone) ;
        if (www.error == null)
        {
            shareInfo.shareUrl = www.text;
            shareInfo.title = title;
            shareInfo.description = desc;
            shareInfo.isToFriend = false;
            shareInfo.iconUrl = "http://www.mob.com/static/app/icon/1494916929.png";
            SDKManager.getInstance().CommonSDKInterface.Share(shareInfo);
        }
        #endif
    }
    public static void InitSharePic(string userid)
    {
        LoginInputController.startUpMono.ShareMutilPic(userid);
    }
    public static void ShareWeChatPic(int flag, string code, string desc, string img1, string img2, string img3, string img4, string img5, string img6)
    {

        SDKManager.getInstance().CommonSDKInterface.ShareMutilPic(flag, desc, img1, img2, img3, img4, img5, img6);
    }
    // 分享到朋友圈
    public static void ShareWeChatMoments(string code, string title, string desc)
    {
        #if UNITY_IOS
        // 获取分享URL
        string targetUrl = SDKManager.ShareWebURL + code;
        var www = new WWW(targetUrl);
        while (!www.isDone) ;
        if (www.error == null)
        {
            _object_c_interface_.WXSDK.requestWXWebShare(2, title, desc, "100x100", www.text);
        }
        #else
        CommonShareInfo shareInfo = new CommonShareInfo();
        string targetUrl = SDKManager.ShareWebURL + code;
        var www = new WWW(targetUrl);
        while (!www.isDone) ;
        if (www.error == null)
        {
            shareInfo.shareUrl = www.text;
            shareInfo.title = title;
            shareInfo.description = desc;
            shareInfo.isToFriend = true;
            shareInfo.iconUrl = "http://www.mob.com/static/app/icon/1494916929.png";
            SDKManager.getInstance().CommonSDKInterface.Share(shareInfo);
        }
        #endif
    }

    // 分享图片给微信 
    public static void ShareImageToWeChat(bool isShareToWeChatMoments = false)
    {
        Application.CaptureScreenshot("Shot4Share.png");
        if (isShareToWeChatMoments)
        {
            _object_c_interface_.WXSDK.requestWXImageShare(2, "100x100", Application.persistentDataPath + "/Shot4Share.png");
        }
        else
        {
            _object_c_interface_.WXSDK.requestWXImageShare(1, "100x100", Application.persistentDataPath + "/Shot4Share.png");
        }
    }


    //上传聊天文件
    public static void UpdateAudioFile(string sAudioInfo, GameObject chatGo)
    {
        string[] sAudioArray = sAudioInfo.Split(',');
        if (string.IsNullOrEmpty(sAudioArray[1])) return;

        string fileAbsolutePath = Application.persistentDataPath;
        fileAbsolutePath = fileAbsolutePath.Replace("\\", "/");
        fileAbsolutePath = string.Format("{0}/Audio/{1}", fileAbsolutePath, sAudioArray[0]);

        byte[] byteFile = File.ReadAllBytes(fileAbsolutePath);
        //上传声音文件
        Utils.AsyncHttpUpDown.getInstance().PostUpload(SDKManager.AudioUpUrl, byteFile, sAudioArray[0], new Utils.AsyncHttpUpDown.delegateOnUpFinsh(
            delegate(int errorCode, string result)
            {
                if (chatGo != null)
                {
                    if (errorCode == 0)
                    {
                        ComponentData.Get(chatGo).Name = sAudioInfo;
                    }
                    UIManager.CallLuaFuncCall("ChatMainWin:AudioUpdateComplete", chatGo);
                }
            }
        ));
    }

    //上传聊天文件
    public static void UploadAudioFileAndToText(string sAudioInfo, GameObject chatGo)
    {
        string[] sAudioArray = sAudioInfo.Split(',');
        if (string.IsNullOrEmpty(sAudioArray[1])) return;

        string fileAbsolutePath = Application.persistentDataPath;
        fileAbsolutePath = fileAbsolutePath.Replace("\\", "/");
        fileAbsolutePath = string.Format("{0}/Audio/{1}", fileAbsolutePath, sAudioArray[0]);

        bool bUploadFinish = false;
        bool bToTextFinish = false;
        if (!File.Exists(fileAbsolutePath))
        {
            return;
        }
        byte[] byteFile = File.ReadAllBytes(fileAbsolutePath);
        //上传声音文件
        Utils.AsyncHttpUpDown.getInstance().PostUpload(SDKManager.AudioUpUrl, byteFile, sAudioArray[0], new Utils.AsyncHttpUpDown.delegateOnUpFinsh(
            delegate(int errorCode, string result)
            {
                if (chatGo != null)
                {
                    bUploadFinish = true;
                    if (errorCode == 0)
                    {
                        ComponentData.Get(chatGo).Name = sAudioInfo;
                    }
                    if (bToTextFinish == true)
                    {
                        UIManager.CallLuaFuncCall("ChatMainWin:AudioUpdateComplete", chatGo);
                    }
                }
            }
        ));

        //翻译成文字
        int len = byteFile.Length;
        string base64str = Convert.ToBase64String(byteFile);
        sdk.BaiDuSDK.getInstance().SoundToText(base64str, len, new sdk.BaiDuSDK.delegateTranslateComplete(
            delegate(int errorCode, string result)
            {
                if (chatGo != null)
                {
                    bToTextFinish = true;
                    if (errorCode == 0)
                    {
                        ComponentData.Get(chatGo).Text = result;
                    }
                    if (bUploadFinish == true)
                    {
                        UIManager.CallLuaFuncCall("ChatMainWin:AudioUpdateComplete", chatGo);
                    }
                }
            }
         ));
    }

    //下载聊天文件
    public static void LoadAudioFile(string sAudioInfo, GameObject chatGo)
    {
        if (string.IsNullOrEmpty(sAudioInfo))
        {
            return;
        }
        Utils.LogSys.LogError("sAudioInfo: " + sAudioInfo);
        string[] sAudioArray = sAudioInfo.Split(',');
        if (string.IsNullOrEmpty(sAudioArray[1])) return;
        Debug.LogError("sAudioArray[1] = " + sAudioArray[1]);

        string fileAbsolutePath = Application.persistentDataPath;
        fileAbsolutePath = fileAbsolutePath.Replace("\\", "/");
        fileAbsolutePath = string.Format("{0}/Audio/{1}", fileAbsolutePath, sAudioArray[0]);

        if (System.IO.File.Exists(fileAbsolutePath))
        {
            if (chatGo != null)
            {
                Debug.LogError("sAudioArray[0] = " + sAudioArray[0]);
                ComponentData.Get(chatGo).Text = sAudioInfo;
                UIManager.CallLuaFuncCall("ChatMainWin:AudioLoadComplete", chatGo);
            }
        }
        else
        {
            //下载
            string downloadUrl = string.Format("{0}/{1}", SDKManager.AudioSaveUrl, sAudioArray[0]);
            Utils.AsyncHttpUpDown.getInstance().DownloadFile(downloadUrl, fileAbsolutePath, new Utils.AsyncHttpUpDown.delegateOnDownFinsh(
                delegate(int errorCode, string result)
                {
                    if (chatGo != null)
                    {
                        ComponentData.Get(chatGo).Text = sAudioInfo;
                        UIManager.CallLuaFuncCall("ChatMainWin:AudioLoadComplete", chatGo);
                    }
                }
             ));
        }
    }

    public static void clearAllAudio()
    {
        string fileAbsolutePath = Application.persistentDataPath;
        fileAbsolutePath = fileAbsolutePath.Replace("\\", "/");
        fileAbsolutePath = string.Format("{0}/Audio", fileAbsolutePath);
        if (Directory.Exists(fileAbsolutePath))
        {
            string[] files = Directory.GetFiles(fileAbsolutePath, "*.*", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                File.SetAttributes(files[i], FileAttributes.Normal);
                File.Delete(files[i]);
            }
        }
    }
}