/***************************************************************

 *
 *
 * Filename:  	CameraAjustor.cs	
 * Summary: 	适配UI镜头和场景镜头
 *              前提：UNITY已做好高度满屏适配。
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/11/27 16:21
 ***************************************************************/
using UnityEngine;
using System.Collections;
using System;
 
 /// <summary>
 /// 根据设备的宽高比，调整camera.orthographicSize. 以保证UI在不同分辨率(宽高比)下的自适应
 /// 须与UIAnchor配合使用
 /// 将该脚本添加到UICamera同一节点上
 /// </summary>
 
 [RequireComponent(typeof(Camera))]
public class CameraAjustor : MonoBehaviour
{
    public const int manualWidth = 1136;
    public const int manualHeight = 640;
    public const float maxAspect = 1136f / 640f;
    public const float minAspect_UI = 1024f / 768f;//1136f / 730f;
    public const float minAspect_Scene = 1024f / 730f;//1024f / 730f;
    public float curWidth = 0f;//最终用来显示的区域宽度
    public float curHeight = 0f;//最终用来显示的区域高度
    public float curAspect = 0f;//最终显示区域的宽高比
    public static bool _isHeightFull = true;
    void Awake()
    {
        Vector2 screen;
        if (Screen.width > Screen.height)
            screen = new Vector2(Screen.width, Screen.height);
        else
            screen = new Vector2(Screen.height, Screen.width);
        float screenAspect = screen.x / screen.y;//CalculateAspect(screen.x, screen.y);//设备宽高比值
        float initialAspect = (float)manualWidth / manualHeight;//理想的宽高比值
        curWidth = initialAspect * screen.y;
        curHeight = screen.y;
        curAspect = screen.x / screen.y;
//         if (Math.Abs(screenAspect - initialAspect) < 0.01f)
//          {
//             //基本一样时，不用调整
//              return;
//          }

        //调整镜头内容投影到屏幕上的指定区域.
        Camera curCamera = GetComponent<Camera>();
        UICamera uiCamera = GetComponent<UICamera>();
        if (curCamera == null)
        {
            return;
        }
        if (screenAspect > initialAspect)//显示区域宽度要缩小
         {
             curAspect = initialAspect;
             curWidth = curAspect * screen.y;
             curHeight = screen.y;
             float scaleX = screen.y * curAspect / screen.x;
             curCamera.rect = new Rect((1f - scaleX) / 2f, 0f, scaleX, 1f);
         }
        else if (screenAspect < minAspect_UI)//显示区域高度要缩小(比如ipad)
        {
            curAspect = minAspect_UI;
            curWidth = screen.x;
            curHeight = screen.x / curAspect;
            float scaleY = screen.x / (curAspect * screen.y);
            curCamera.rect = new Rect(0f, (1f - scaleY) / 2f, 1f, scaleY);
            Utils.LogSys.Log("=========screenSize===========" + screen.x.ToString() + ", " + screen.y.ToString());
        }

        //******************************以上，实现了在新的指定区域内，高度满区域适配*********************************************
        //******************************下面，再调整镜头的视野大小, 实现高度或宽度满区域适配*********************************************

         //UI镜头是正交模式
        if (curCamera.orthographic && uiCamera != null)//
         {
             //在高度已满屏的情况下，如果左右未被裁, 不用扩大
             if (curAspect > initialAspect)
             {
                 _isHeightFull = true;
             }
             //在高度已满屏的情况下，如果左右被裁，就扩大视野，直到宽度满屏。（比如IPAD）
             else if (curAspect < initialAspect)
             {
                 float scaleX =  initialAspect / curAspect;
                 curCamera.orthographicSize = scaleX;// initialAspect;
                 Utils.LogSys.Log("=========orthographicSize===========" + scaleX.ToString());
                 _isHeightFull = false;
             }
         }
        else if (curCamera.orthographic && gameObject.transform.name == "SceneCamera")
        {
            //在高度已满屏的情况下，如果左右未被裁, 不用扩大
            if (curAspect > initialAspect)
            {
                _isHeightFull = true;
            }
            //在高度已满屏的情况下，如果左右被裁，就扩大视野，直到宽度满屏。（比如IPAD）
            else if (curAspect < initialAspect)
            {
                float scaleX = initialAspect / curAspect;
                float new_size = curCamera.orthographicSize * scaleX;
                curCamera.orthographicSize = new_size;// initialAspect;
                Utils.LogSys.Log("=========orthographicSize===========" + scaleX.ToString());
                _isHeightFull = false;
            }
        }
         //场景镜头是透视模式
         else
         {
             //在高度已满屏的情况下，如果左右未被裁, 不用扩大
             if (screenAspect > curAspect)
             {
                 _isHeightFull = true;
             }
             //在高度已满屏的情况下，如果左右被裁，就扩大视野，直到宽度视觉跟1136X640看到的一样大为止。（比如IPAD）
             else if (screenAspect < minAspect_Scene)
             {
                curAspect = minAspect_Scene;
                float scaleX = 0.92f * initialAspect / curAspect;
                curCamera.fieldOfView *= scaleX;
                _isHeightFull = false;
            }
         }
     }
 
 }
