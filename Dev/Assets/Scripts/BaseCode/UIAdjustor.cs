/***************************************************************

 *
 *
 * Filename:  	UIAdjustor.cs	
 * Summary: 	UI适合组件
 *              思路：适配前可以确定的是宽已经拉伸全屏，高度可能不够，所以只要适配高度就行。
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2016/03/22 22:38
 ***************************************************************/
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 让UIWidget全屏
/// </summary>

//[RequireComponent(typeof(UIWidget))]
public class UIAdjustor : MonoBehaviour
{
    public bool _bAdjustSelfPos = false;
    public bool _bAdjustChildrenPos = false;
    public bool _bAdjustUIWidget = true;//调整widget的高度
    public bool _bAdjustUIBoxCollider = true;//调整BoxCollider的大小
    public bool _bAdjustSelfScale = false;//
    void Awake()
    {
        GameObject CamObj = GameObject.Find("UIRoot/UICamera");
        CameraAjustor adjustor = CamObj.GetComponent<CameraAjustor>();
        float fRate = CameraAjustor.maxAspect / adjustor.curAspect;
        if (_bAdjustUIBoxCollider)
        {
            BoxCollider colldier = transform.GetComponent<BoxCollider>();
            if (colldier != null)
            {
                float newY = (float)colldier.size.y * fRate;
                float newX = colldier.size.x;
                if (newX >= 1130)
                    newX = 1600;
                colldier.size = new Vector3(newX, newY, colldier.size.z);
            }
        }
        if (Math.Abs(adjustor.curAspect - CameraAjustor.maxAspect) < 0.01f)
        {
            return;
        }
        if (_bAdjustSelfPos)
        {
            Vector3 pos = transform.localPosition;
            pos.y *= fRate;
            transform.localPosition = pos;
        }
        if (_bAdjustChildrenPos)
        {
            float manualWidth = (float)CameraAjustor.manualWidth;
            float manualHeight = (float)CameraAjustor.manualHeight;
            //float viewWidth = adjustor.curWidth;
            //float viewHeight = adjustor.curHeight;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                Vector3 pos = child.transform.localPosition;
                pos.y *= fRate;
                child.localPosition = pos;
            }
         }

        if (_bAdjustUIWidget)
         {
             UIWidget widget = transform.GetComponent<UIWidget>();
             if (widget != null)
             {
                 float newHeight = (float)widget.height * fRate;
                 widget.width = widget.width + 2;
                 widget.height = (int)newHeight + 2;
             }
             else
             {
                 UISprite sprite = transform.GetComponent<UISprite>();
                 if (sprite != null)
                 {
                     float newHeight = (float)sprite.height * fRate;
                     sprite.width = sprite.width + 2;
                     sprite.height = (int)newHeight + 2;

                 }
                 else
                 {
                     UITexture texture = transform.GetComponent<UITexture>();
                     if (texture != null)
                     {
                         float newHeight = (float)texture.height * fRate;
                         texture.width = texture.width + 2;
                         texture.height = (int)newHeight + 2;

                     }
                 }
             }
         }
        if (_bAdjustSelfScale)
        {
            Vector3 localScale = transform.localScale;
            transform.localScale = transform.localScale * fRate;
        }
    }

}
