/***************************************************************


 *
 *
 * Filename:  	PListMono.cs	
 * Summary: 	序列帧动画组件
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/08/26 11:49
 ***************************************************************/
using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

public class PListMono : MonoBehaviour
{
    public GameObject pListPrefab = null;
    public float frameSpeed = 0.2f;
    public bool bStartOnAwake = true;
    public bool bLoop = true;
    public float fLoopInterval = 0f;
    public bool bLoopShowLastFrame = true;
    public float delay = 0f;
    public bool bPlayOverHide = true;//循环播放的间隔
    [HideInInspector]
    float fStartTime = 0f;
    float nextFrameTime;
    BetterList<string> framSprites;
    public int frameCount = 0;
    public int nCurFrame = 0;
    int _playTimes = 1;
    bool bStop = false;

    public delegate void onFinishDelegate(GameObject go);
    public onFinishDelegate onFinish;
//    public int setCurrent { set { nCurFrame = value; } }

    /// <summary>
    /// 是否正在动画
    /// </summary>
    public bool isPlaying {
        get { return !bStop; }
    }

    void Awake()
    {
        if (pListPrefab == null) return;

        UIAtlas atlas = ((GameObject)pListPrefab).GetComponent<UIAtlas>();
        framSprites = atlas.GetListOfSprites();
        framSprites.Sort(sortFrame);
        frameCount = framSprites.size;
        bStop = !bStartOnAwake;
        fStartTime = Time.time;
    }

    private int sortFrame(string left, string right)
    {
        //兼容 1~10 的情况
        int leftInt = 0;
        int rightInt = 0;
        //if (int.TryParse(left, out leftInt) && int.TryParse(right, out rightInt)) {
        //    return leftInt.CompareTo(rightInt);
        //}
        string sLeftNum = Regex.Replace(left, @"[^\d.\d]", "");
        string sRightNum = Regex.Replace(right, @"[^\d.\d]", "");
        if (int.TryParse(sLeftNum, out leftInt) && int.TryParse(sRightNum, out rightInt))
        {
            return leftInt.CompareTo(rightInt);
        }
        return left.CompareTo(right);
    }

    // Update is called once per frame
    void Update()
    {
        if (framSprites == null) return;
        if (framSprites.size <= 0) return;

        if (bStop)
            return;

        if (Time.time < fStartTime + delay) return; 

        if (nCurFrame < frameCount)
        {
            if (!(Time.time > nextFrameTime)) return;
            nextFrameTime = Time.time + frameSpeed;
            this.GetComponent<UISprite>().spriteName = framSprites[nCurFrame];
            nCurFrame++;
        }
        else
        {
            if (nCurFrame == frameCount) {
                if (bPlayOverHide)
                {
                    fStartTime = Time.time;
                }
                if (!bLoop) {
                    _playTimes--;

                    if (_playTimes <= 0) {
                        gameObject.SetActive(!bPlayOverHide);
                        bStop = true;
                        if (onFinish != null) onFinish(gameObject);
                    }
                } else {
                    nCurFrame = 0;
                    if (fLoopInterval == 0) {
                        if (!(Time.time > nextFrameTime)) return;
                        nextFrameTime = Time.time + frameSpeed;
                        this.GetComponent<UISprite>().spriteName = framSprites[nCurFrame];
                        nCurFrame++;
                    } else {
                        nextFrameTime = Time.time + fLoopInterval;
                        if (!bLoopShowLastFrame) {
                            this.GetComponent<UISprite>().spriteName = "werwerwe";
                        }
                    }
                }
                //nCurFrame++;
            }

            //if (bLoop) {
            //    if (fLoopInterval == 0f || Time.time >= fNextLoopTime) {
            //        this.GetComponent<UISprite>().spriteName = framSprites[nCurFrame];
            //        nCurFrame++;
            //    }
            //} else {
            //    nCurFrame = 0;
            //}
        }
    }

    public void Play(bool isPlayOverHide = true, int playTimes = 1)
    {
        GetComponent<UISprite>().spriteName = "xxx";
        nCurFrame = 0;
        fStartTime = Time.time;
        gameObject.SetActive(true);

        bStop = false;
        bPlayOverHide = isPlayOverHide;
        _playTimes = playTimes;
    }

    public void Pause()
    {
        bStop = true;
    }

    /// <summary>
    /// 暂停在指定帧(1~n)或（-1~-n）
    /// </summary>
    /// <param name="nFrame"></param>
    public void Pause(int nFrame)
    {
        bStop = true;
        if (nFrame == 0 || nFrame < -frameCount || nFrame > frameCount)//所指定的帧不存在时
            return;

        if (nFrame < 0)
        {
            nCurFrame = frameCount + nFrame;
        }
        if (nFrame > 0)
        {
            nCurFrame = nFrame - 1;
        }

        this.GetComponent<UISprite>().spriteName = framSprites[nCurFrame];
    }
}
