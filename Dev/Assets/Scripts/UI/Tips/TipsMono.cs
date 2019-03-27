using System;
using EventManager;
using Scene;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MyExtensionMethod;
using UI.Controller;
using UnityEngine;

internal class TipsMono : MonoBehaviour
{
    private static Transform _root;
    private TipsController _controller;
    private Camera _camObj;

    private UILabel _name;
    private UILabel _des;
    private GameObject _btnClose;
    private Transform _itemBg;
    private UISprite _bg;
    private void Awake()
    {
        FindObject(transform);
        _controller = (TipsController)UIManager.GetControler(UIName.TIPS);
        UIEventListener.Get(_btnClose).onClick = TipsController.GoBack;
        _controller.RegisterUIEvent(UIEventID.TIPS, OnEventSetContent);
        _camObj = GameSceneManager.uiCameraObj.GetComponent<Camera>();
    }

    private void FindObject(Transform tr)
    {
        _itemBg = tr.Find("Container");
        _name = tr.Find<UILabel>("Container/tittle");
        _des = tr.Find<UILabel>("Container/content");
        _btnClose = tr.Find<GameObject>("BtnClose");
        _bg = tr.Find<UISprite>("Container/bg");
    }

    /// <summary>
    /// 显示 Tips 窗口
    /// args 参数内容：
    /// Name:名称；
    /// Icon:图标；
    /// Des:描述；
    /// 存在 Position，则按 Position 指定位置显示
    /// 附加属性 key 以 L 开头，key[1] = 'U' 带下划线，不需要下划线时填非 'U' 字符；
    /// key[2] = 'L'、'R'、'C' 对齐方式；
    /// AutoHide:int 存在为自动隐藏; Position:Vector3 位置信息；
    /// </summary>
    /// <param name="args"></param>
    private void OnEventSetContent(EventMultiArgs args)
    {
        var dic = args.GetDictionary();
        var labels = dic.Where(n => n.Key[0] == 'L' && n.Key != "Level").ToList();
        var transObj = dic["TransObj"] as Transform;

        // TODO: 存在 Position，则按 Position 指定位置显示
        object pos;
        SetContentWithoutIcon(ref dic, ref labels);
        if (dic.TryGetValue("Position", out pos)) _itemBg.position = (Vector3)pos;
        else GetPos(transObj);
        object offset;
        if (dic.TryGetValue("OffSet", out offset))
        {
            Vector2 vOffSet = (Vector2)offset;
            _itemBg.position = new Vector3(_itemBg.position.x + vOffSet.x, _itemBg.position.y + vOffSet.y, _itemBg.position.z);
        }
        //TODO: 设置是否自动隐藏自动隐藏
        if (dic.ContainsKey("AutoHide")) { _btnClose.SetActive(false); FadeWin(); }
        var hasIcon = dic.ContainsKey("Icon");

        /*if (hasIcon) {
            SetContentWithIcon(ref dic, ref labels);
        } else {
            SetContentWithoutIcon(ref dic, ref labels);
        }*/
        
        //StartCoroutine(SetContentAtEndOfFrame(labels, hasIcon));
    }

    /// <summary>
    /// 不带图标
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="labels"></param>
    private void SetContentWithoutIcon(ref Dictionary<string, object> dic, ref List<KeyValuePair<string, object>> labels)
    {
        var title = _name;
        title.text = dic["Title"].ToString();
        /*title.SetAnchor(_itemBg);
        title.leftAnchor.relative = 0f;
        title.topAnchor.relative = 1f;
        title.bottomAnchor.relative = 1f;
        title.leftAnchor.absolute = 20;
        title.topAnchor.absolute = -20;
        title.bottomAnchor.absolute = -42;
        */
        var des = _des;
        //des.width = 495;
        var aaa = des.processedText;

        des.text = dic["Des"].ToString().Replace("&#x000D;&#x000A;", "\n").Replace("[d6573e]", "").Replace("[685753]", "").Replace("[-]", "");//string.Format("        {0}", );
        _bg.height = 42 + des.height + 10;
    }
    /*
    /// <summary>
    /// 带图标
    /// </summary>
    /// <param name="dic"></param>
    /// <param name="labels"></param>
    private void SetContentWithIcon(ref Dictionary<string, object> dic, ref List<KeyValuePair<string, object>> labels)
    {
        int type = -1;
        int lev = 0;
        if (dic.ContainsKey("Type"))
        {
            type = (int)dic["Type"];
        }
        if (dic.ContainsKey("Level"))
        {
            lev = (int)dic["Level"];
        }
        var icon = _itemIcon;
        var iconBg = _itemIconBg;
        var title = _name;
        var hasIconBg = dic.ContainsKey("IconBg");
        var offsetIconBg = hasIconBg ? 27 : 10;
        var offsetText = hasIconBg ? 37 : 20;
        UtilTools.SetIcon(icon, dic["Icon"].ToString(), type, lev);
        var itemIcon = icon.GetComponent<UISprite>();
        itemIcon.MakePixelPerfect();
        itemIcon.topAnchor.absolute = -offsetIconBg;
        itemIcon.bottomAnchor.absolute = -itemIcon.height - offsetIconBg;
        itemIcon.leftAnchor.absolute = offsetIconBg;
        itemIcon.rightAnchor.absolute = itemIcon.width + offsetIconBg;

        var imgWidth = itemIcon.width + 40;
        if (hasIconBg) {
            iconBg.spriteName = iconBg.ToString();
            iconBg.SetAnchor(icon.gameObject, -17, -17, 17, 17);
            iconBg.gameObject.SetActive(true);
            imgWidth = iconBg.width + 40;
        } else {
            iconBg.gameObject.SetActive(false);
        }

        title.text = dic["Title"].ToString();
        title.topAnchor.target = _itemBg;
        title.topAnchor.relative = 1f;
        title.topAnchor.absolute = -20;

        title.bottomAnchor.target = _itemBg;
        title.bottomAnchor.relative = 1f;
        title.bottomAnchor.absolute = -42;

        title.leftAnchor.target = icon;
        title.leftAnchor.relative = 1f;
        title.leftAnchor.absolute = offsetText;

        title.rightAnchor.target = icon;
        title.rightAnchor.relative = 1f;
        title.rightAnchor.absolute = 200;

        _titleUnderLine.GetComponent<UISprite>().width = title.width + 15;
        _des.width = 535 - imgWidth;
        _des.text = dic["Des"].ToString();
        var iconHeight = hasIconBg ? itemIcon.height + 54 : itemIcon.height + 20;
        var textHeight = 78 + labels.Count * 30 + _des.height;
        _itemBg.GetComponent<UISprite>().height = Math.Max(iconHeight, textHeight);
    }
    /// <summary>
    /// 在当前帧结束时设置窗口内容
    /// </summary>
    /// <param name="labels"></param>
    /// <param name="hasIcon"></param>
    /// <returns></returns>
    private IEnumerator SetContentAtEndOfFrame(List<KeyValuePair<string, object>> labels, bool hasIcon)
    {
        yield return new WaitForEndOfFrame();
        var pos = hasIcon
            ? _name.transform.localPosition + new Vector3(-15, -22, 0)
            : _name.transform.localPosition + 22 * Vector3.down;
        _des.transform.localPosition = pos;
        for (int i = 0; i < labels.Count; i++) {
            var label = labels[i];
            var obj = NGUITools.AddChild(_labesObj, _labelCell);
            obj.transform.localPosition += Vector3.down * 30 * i;
            var addLabel = obj.GetComponent<UILabel>();
            addLabel.text = label.Value.ToString();

            var line = obj.transform.Find("Line").gameObject;
            if (label.Key[1] == 'U') {
                line.SetActive(true);
                line.GetComponent<UISprite>().width = addLabel.width + 10;
            } else {
                line.SetActive(false);
            }
            switch (label.Key[2]) {
                case 'L':
                    obj.GetComponent<UILabel>().alignment = NGUIText.Alignment.Left;
                    break;
                case 'R':
                    obj.GetComponent<UILabel>().alignment = NGUIText.Alignment.Right;
                    break;
                default:
                    obj.GetComponent<UILabel>().alignment = NGUIText.Alignment.Center;
                    break;
            }
        }
    }
    
    */
    /// <summary>
    /// 设置位置
    /// </summary>
    /// <param name="trans"></param>
    private void GetPos(Transform trans)
    {
        
        var itemBg = _itemBg;
        if (trans == null)
            TipsController.GoBack();
        UIWidget wid = trans.GetComponent<UIWidget>();
        if(wid==null)
            TipsController.GoBack();
        var parentCorners = wid.worldCorners;
        var tipBoxCorners = itemBg.GetComponent<UIWidget>().worldCorners;
        var viewPoint = new Vector3[4];
        for (var i = 0; i < 3; i++) {
            viewPoint[i] = _camObj.WorldToViewportPoint(parentCorners[i]);
            parentCorners[i].z = tipBoxCorners[i].z;
        }
        var x1 = viewPoint[0].x;
        var y1 = viewPoint[0].y;
        var x2 = viewPoint[3].x;
        var y2 = viewPoint[2].y;

        if (x1 < 0.5 && y1 < 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.BottomLeft;
            itemBg.position = parentCorners[2];
        } else if (x1 < 0.5 && y1 > 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.TopLeft;
            itemBg.position = parentCorners[0];
        } else if (x1 > 0.5 && y1 < 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.BottomRight;
            itemBg.position = parentCorners[0];
        } else if (x1 > 0.5 && y1 > 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.TopRight;
            itemBg.position = parentCorners[1];
        } else if (x1 < 0.5 && y2 < 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.BottomLeft;
            itemBg.position = parentCorners[1];
        } else if (x1 < 0.5 && y2 > 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.TopLeft;
            itemBg.position = parentCorners[0];
        } else if (x1 > 0.5 && y2 < 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.BottomRight;
            itemBg.position = parentCorners[0];
        } else if (x1 > 0.5 && y2 > 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.TopRight;
            itemBg.position = parentCorners[1];
        } else if (x2 < 0.5 && y1 < 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.BottomLeft;
            itemBg.position = parentCorners[3];
        } else if (x2 < 0.5 && y1 > 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.TopLeft;
            itemBg.position = parentCorners[2];
        } else if (x2 > 0.5 && y1 < 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.BottomRight;
            itemBg.position = parentCorners[0];
        } else if (x2 > 0.5 && y1 > 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.TopRight;
            itemBg.position = parentCorners[1];
        } else if (x2 < 0.5 && y2 < 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.BottomLeft;
            itemBg.position = parentCorners[3];
        } else if (x2 < 0.5 && y2 > 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.TopLeft;
            itemBg.position = parentCorners[2];
        } else if (x2 > 0.5 && y2 < 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.BottomRight;
            itemBg.position = parentCorners[0];
        } else if (x2 > 0.5 && y2 > 0.5) {
            itemBg.GetComponent<UIWidget>().rawPivot = UIWidget.Pivot.TopRight;
            itemBg.position = parentCorners[1];
        }
    }

    /// <summary>
    /// 渐隐
    /// </summary>
    private void FadeWin()
    {
        var fade = TweenAlpha.Begin(_itemBg.gameObject, 1f, 0f);
        fade.delay = 1f;
        fade.SetOnFinished(() => TipsController.GoBack());
    }
}