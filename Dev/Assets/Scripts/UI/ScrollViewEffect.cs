/***************************************************************
 * Copyright (c) 2013 福建沃动计算机技术有限公司
 *         All rights reserved.
 *
 *
 * Filename:  	ScrollViewEffect.cs
 * Summary: 	ScrollView 上的特效资源
 *
 * Version:   	1.0.0
 * Author: 		YQ.Qu
 * Date:   		2017/3/27 0027 上午 11:03
 ***************************************************************/

using UnityEngine;
using System.Collections;
using Utils;

public class ScrollViewEffect : MonoBehaviour
{
    public UIScrollView _scrollView;

    public string effectName = "effect";

    private UIGrid _grid;

    public GameObject _Cell;

    // Use this for initialization
    void Start()
    {
        if (_scrollView != null){
            _grid = _scrollView.transform.GetComponentInChildren<UIGrid>();
            if (_Cell != null && _grid != null){
                UIPanel mScrollViewPanel = NGUITools.FindInParents<UIPanel>(_scrollView.gameObject);
                mScrollViewPanel.onClipMove += onItemOnDrag;
            }
        }
    }


    void onItemOnDrag(UIPanel panel)
    {
        UIPanel mPanel = NGUITools.FindInParents<UIPanel>(_grid.gameObject);
        UIPanel mScrollViewPanel = NGUITools.FindInParents<UIPanel>(_scrollView.gameObject);

        UIScrollView mScroll = _scrollView.GetComponent<UIScrollView>();
        UIWidget _CellWidget = _Cell.GetComponent<UIWidget>();
        int count = _grid.transform.childCount;
        if (count == 0) return;

        Vector3[] corners = mPanel.worldCorners;
        for (int i = 0; i < 4; ++i){
            Vector3 v = corners[i];
            v = transform.InverseTransformPoint(v);
            corners[i] = v;
        }
        if (mScroll.movement == UIScrollView.Movement.Horizontal){
            float cellPos = 0f;
            float cellDis = -mScrollViewPanel.clipOffset.x;
            float minPos = 0;
            float maxPos = corners[2].x - corners[0].x;

            for (int i = 0; i < count; ++i){
                Transform t = _grid.transform.GetChild(i);
                if (t.name.Substring(0, 4) != "Cell") continue;
                cellPos = t.localPosition.x;

                Transform cell = t.transform;
                Transform effect_paotaiParnet = cell.Find("Cell/" + effectName);
                if (effect_paotaiParnet == null) continue;
                if ((cellPos + _CellWidget.width + cellDis) > maxPos || cellPos + cellDis < minPos) //显示区之外的,移除cell
                {
                    SetEffect(effect_paotaiParnet, false);
                }
                else{
                    SetEffect(effect_paotaiParnet, true);
                }
            }
        }
        else if (mScroll.movement == UIScrollView.Movement.Vertical){
            float cellPos = 0f;
            float cellDis = mScrollViewPanel.clipOffset.y;
            float minPos = 0;
            float maxPos = corners[0].y - corners[2].y + _CellWidget.height / 2;
            for (int i = 0; i < count; ++i){
                Transform t = _grid.transform.GetChild(i);
                if (t.name.Substring(0, 4) != "Cell") continue;
                cellPos = t.localPosition.y;

                Transform cell = t.transform;
                Transform effect_paotaiParnet = cell.Find("Cell/" + effectName);
                if (effect_paotaiParnet == null) continue;
                float currentPos = cellPos - cellDis;
                SetEffect(effect_paotaiParnet, currentPos >= maxPos && currentPos <= minPos);
            }
        }
    }


    private void SetEffect(Transform tr, bool isShow)
    {
        ParticleSystem[] arr = tr.GetComponentsInChildren<ParticleSystem>(true);
        foreach (var ps in arr){
            ps.gameObject.SetActive(isShow);
        }
    }
}