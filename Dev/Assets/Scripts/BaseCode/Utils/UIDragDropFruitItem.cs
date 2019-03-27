using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class UIDragDropFruitItem : UIDragDropItem
{
    public List<EventDelegate> onDragOver = new List<EventDelegate>();
        
    Vector3 startPos;
    public GameObject followItem;
    Vector3 followItemStartPosition;
    float minY = 0f;
    float maxY = 0f;
    bool _isAutoMode = false;
    BoxCollider _colloder;
    float _playTime = 0f;
    bool _isPlay = false;
    private float _distance = 0f;
    bool _isReset = false;
    void Awake()
    {
        startPos = transform.localPosition;
        maxY = startPos.y;
        minY = maxY - 180;
        _distance = maxY - minY;
        _colloder = gameObject.GetComponent<BoxCollider>();
        if (followItem != null)
        {
            followItemStartPosition = followItem.transform.localPosition;
        }
    }
    protected override void OnDragDropStart()
    {
        UtilTools.PlaySoundEffect("Sounds/Laba/spin", taget: gameObject);
        base.OnDragDropStart();
    }
    public void SetMode(bool isAuto)
    {
        _isAutoMode = isAuto;
        _colloder.enabled = !isAuto;
    }
    public void PlayAction()
    {
        _isPlay = true;
        _colloder.enabled = false;
    }
    void Update()
    {
        if (_isAutoMode && !_isPlay)
        {
            PlayAction();
        }
        if (_isPlay)
        {
            float deltaTime=Time.deltaTime;
            _playTime += deltaTime;
            OnDragDropMove(new Vector2(0, -_distance * _playTime));
            if (mTrans.localPosition.y <= minY && _playTime>=0.4)
            {
                _isPlay = false;
                EventDelegate.Execute(onDragOver);
                //_isReset = true;
                Reset();
                _playTime = 0f;
                if (_isAutoMode == false)
                {
                    _colloder.enabled = true;
                }
            }
        }
        if (_isReset)
        {
            float deltaTime = Time.deltaTime;
            _playTime += deltaTime;
            OnDragDropMove(new Vector2(0, _distance * _playTime));
            if (mTrans.localPosition.y >= maxY && _playTime >= 0.4)
            {
                _playTime = 0f;
                _isReset = false;
            }
        }
    }
    public void SetFollowItem(GameObject go)
    {
        followItemStartPosition = go.transform.localPosition;
    }
    protected override void OnDragDropMove(Vector2 delta)
    {
        if (mTrans.localPosition.y >= maxY && delta.y > 0)
            return;
        if (mTrans.localPosition.y <= minY && delta.y < 0)
            return;
        float moveToY = mTrans.localPosition.y + delta.y;
        float deltaY = delta.y;
        if (moveToY > maxY)
        {
            deltaY = maxY - mTrans.localPosition.y;
            moveToY = maxY;

        }
        else if (moveToY < minY)
        {
            deltaY = minY - mTrans.localPosition.y;
            moveToY = minY;
        }
        mTrans.localPosition = new Vector3(mTrans.localPosition.x, moveToY, mTrans.localPosition.z);
        if (followItem != null)
        {
            if (moveToY >= 44)
            {
                followItem.transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                followItem.transform.localScale = new Vector3(1, -1, 1);
            }
            followItem.transform.localPosition = new Vector3(followItem.transform.localPosition.x, followItem.transform.localPosition.y + deltaY / 1.85f, followItem.transform.localPosition.z);
            
            /*if (mTrans.position.y <= followItem.transform.position.y && deltaY<0)
            {
                followItem.transform.localPosition = new Vector3(followItem.transform.localPosition.x, followItem.transform.localPosition.y + deltaY, followItem.transform.localPosition.z);
            }
            else if (mTrans.position.y >= followItem.transform.position.y && deltaY > 0 && followItem.transform.position.y<followItemStartPosition.y)
            {
                if (followItem.transform.localPosition.y + deltaY > followItemStartPosition.y)
                {
                    followItem.transform.localPosition = followItemStartPosition;
                }
                else
                {
                    followItem.transform.localPosition = new Vector3(followItem.transform.localPosition.x, followItem.transform.localPosition.y + deltaY, followItem.transform.localPosition.z);
                }
            }*/
        }
    }
    protected override void OnDragDropEnd()
    {
        EventDelegate.Execute(onDragOver);
        Reset();
    }
    public void Reset()
    {
        transform.localPosition = startPos;
        if (followItem != null)
        {
            followItem.transform.localPosition = followItemStartPosition;
            followItem.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}