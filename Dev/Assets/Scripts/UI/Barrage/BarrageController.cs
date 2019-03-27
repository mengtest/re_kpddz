using Msg;
using network.protobuf;
/***************************************************************


 *
 *
 * Filename:  	ShopController.cs	
 * Summary: 	商店界面
 *
 * Version:   	1.0.0
 * Author: 		LiuYi
 * Date:   		2015/03/24 17:46
 ***************************************************************/
using System.Text.RegularExpressions;
using UI.Controller;
using UnityEngine;
public class SC_CBarrageResponse
{
    public SC_CBarrageResponse() { }

    private string _name;
    public string name
    {
        get { return _name; }
        set { _name = value; }
    }
    private int _vipLv;
    public int vipLv
    {
        get { return _vipLv; }
        set { _vipLv = value; }
    }
    private string _content;
    public string content
    {
        get { return _content; }
        set { _content = value; }
    }
    private int _itemId;
    public int itemId
    {
        get { return _itemId; }
        set { _itemId = value; }
    }
}
  
public class BarrageController : ControllerBase
{
    private BarrageMono _mono;
    public int nShownType = 1;
    public BetterList<SC_CBarrageResponse> _richCarBarrageList = new BetterList<SC_CBarrageResponse>();
    public BarrageController(string uiName)
    {
        sName = uiName;
        ELevel = UILevel.HIGHT;
        prefabsPath = new string[] { UIPrefabPath.BARRAGE_WIN };
        //_bAddToScene = true;
    }
    public void OnRichCarMessageAdd(string content,string name,int vip,int itemId)
    {
        SC_CBarrageResponse msg= new SC_CBarrageResponse();
        msg.content = content;
        msg.name = name;
        msg.vipLv = vip;
        msg.itemId = itemId;
        _richCarBarrageList.Add(msg);
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
        _mono = winObject.AddComponent<BarrageMono>();
    }

}
