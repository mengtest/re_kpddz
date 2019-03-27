/***************************************************************


 *
 *
 * Filename:  	TabListBaseMono.cs	
 * Summary: 	含有列表及tab的mono基类
 *
 * Version:   	1.0.0
 * Author: 		HR.Chen
 * Date:   		2016年4月26日 14:17:42
 ***************************************************************/

#region Using
using EventManager;
using network;
using System.Collections;
using System.Collections.Generic;
using MyExtensionMethod;
using UI.Controller;
using UnityEngine;
using System;
#endregion


internal class TabListBaseMono : BaseMono
{
    #region Variable

    protected ControllerBase _controller = null;
    protected static string _rootPathName = "Container";
    protected static string _scrollPathName = _rootPathName + "/list_bg/scroll_view";

    protected GameObject _btnBack = null;

    // 标题
    protected GameObject _titleBg = null;
    protected UILabel _titleLb = null;

    // 列表
    protected GameObject _listBg = null;                             //列表容器
    protected GameObject _scrollView = null;                         //滚动层
    protected GameObject _cellPrefab = null;                         //列表Cell Prefab
    protected UIGridCellMgr _cellMgr = null;                         //Cell 管理器

    // tab
    protected GameObject _tabBg = null;                              //类型容器
    protected GameObject _tabCellPrefab = null;                      //类型Cell Prefab
    protected int _tabIndex = 0;
    protected List<string> _tabNameList = new List<string>(); //tab标签名
    protected Transform[] _tabButtons;                        //类型按钮

    #endregion

    #region Override And Constructor
    /// <summary>
    /// 初始化
    /// </summary>
    protected override void ReplaceAwake()
    {
        findObject();
        UIEventListener.Get(_btnBack).onClick = toBack;

    }

    protected virtual void Start()
    {
        //UpdateWin();
    }

    public virtual void OnDestoryWin()
    {

    }

    #endregion

    protected virtual void setTitleInfo(string titleStr)
    {
        if (titleStr != null)
        {
            _titleBg.SetActive(true);
            _titleLb.text = titleStr;
        }
        else
        {
            _titleBg.SetActive(false);
        }
    }

    protected virtual void findObject()
    {
        _btnBack = FindGameObject(_rootPathName + "/button_back");

        _titleBg = FindGameObject(_rootPathName + "/titleBg");
        _titleLb = FindComponent<UILabel>(_rootPathName + "/titleBg/Label");
        _titleBg.SetActive(false);

        _listBg = FindGameObject(_scrollPathName + "/content");
        _scrollView = FindGameObject(_scrollPathName);
        _cellMgr = FindGameObject(_scrollPathName + "/content").GetComponent<UIGridCellMgr>();
        _cellMgr.onShowItem = OnShowItem;

        _tabBg = FindGameObject(_rootPathName + "/tab_bg");
        _tabCellPrefab = FindGameObject(_rootPathName + "/tabCell");
        _tabButtons = new Transform[_tabNameList.Count];
        for (int i = 0; i < _tabNameList.Count; i++)
        {
            GameObject tabObj = NGUITools.AddChild(_tabBg, _tabCellPrefab);
            tabObj.SetActive(true);
            tabObj.transform.Find("Label").GetComponent<UILabel>().text = _tabNameList[i];
            ComponentData.Get(tabObj).Tag = i;
            _tabButtons[i] = tabObj.transform;
            UIEventListener.Get(tabObj).onClick = OnClickType;
        }
    }

    public virtual void UpdateWin()
    {
        UpdateTab();
        UpdateList();
    }

    public virtual void UpdateTab(int index = -1)
    {
        if (index == -1)
        {
            index = _tabIndex;
        }
        if (_tabButtons.Length < index) return;
        _tabButtons[_tabIndex].GetComponent<UIToggle>().value = false;
        _tabButtons[index].GetComponent<UIToggle>().value = true;
        _tabIndex = index;
    }

    public virtual void UpdateList(int tabIndex = -1)
    {
        if (tabIndex == -1) tabIndex = _tabIndex;
        int listDataCount = getListDataCount(tabIndex);
        if (listDataCount == 0) return;

        for (var i = 0; i < listDataCount; i++)
        {
            UIWidget cellBox = _cellMgr.NewCellsBox(_listBg);
            changeCellBoxInfo(cellBox);
        }

        repositionScroll();
    }

    public virtual void UpdateListCell(GameObject go, int index)
    {

    }

    protected virtual void changeCellBoxInfo(UIWidget cellBox)
    {

    }

    protected virtual void repositionScroll()
    {
        _scrollView.GetComponent<UIScrollView>().ResetPosition();
        _cellMgr.Grid.Reposition();
        _cellMgr.UpdateCells(); 
    }

    protected virtual int getListDataCount(int tabIndex)
    {
        return 0;
    }

    protected virtual void toBack(GameObject go)
    {

    }

    protected virtual void OnListCellClick(GameObject go)
    {

    }

    protected virtual void OnShowItem(GameObject itemBox, int nIndex, GameObject item)
    {
        UIEventListener.Get(item).onClick = OnListCellClick;
        UpdateListCell(item, nIndex);
    }

    public void OnClickType(GameObject sender)
    {
        UIButton button = sender.GetComponent<UIButton>();
        int index = ComponentData.Get(sender).Tag;

        if (_tabIndex == index)
        {
            _tabButtons[_tabIndex].GetComponent<UIToggle>().value = true;
            return;
        }

        UpdateTab(index);
    }

    public void AddTabObj(string str)
    {
        _tabNameList.Add(str);
    }

    public void AddTabsWithGameText(string text, int cnt)
    {
        for (var i = 0; i < cnt; i++)
        {
            AddTabObj(GameText.GetStr(text + i));
        }
    }
}