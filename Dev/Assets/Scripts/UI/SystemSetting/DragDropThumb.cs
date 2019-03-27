using UnityEngine;
using System.Collections;
using UI.Controller;
using HeroData;
using EventManager;
using System.Collections.Generic;
using sdk;
using network;
using network.protobuf;
public class DragDropThumb : UIDragDropItem
{
    public ControllerBase cont;
    public delegate void delegateThumb(GameObject go);
    public delegateThumb OnDragRelease = null;
    public UISlider slider;
    public UISprite sliderSp;
    void Awake()
    {
    }
    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
    }
    protected override void OnDragDropMove(Vector2 delta)
    {
        if (slider == null || sliderSp==null)
            return;
        float x = transform.localPosition.x + delta.x;
        if (x > sliderSp.width / 2)
        {
            x = sliderSp.width / 2;
        } else if (x < -sliderSp.width / 2)
        {
            x = -sliderSp.width / 2;
        }
        x += sliderSp.width / 2;
        slider.value = x / (float)sliderSp.width;
    }
    protected override void OnDragDropRelease(GameObject go)
    {
        base.OnDragDropRelease(go);
        if (OnDragRelease != null)
            OnDragRelease(go);
    }

}

