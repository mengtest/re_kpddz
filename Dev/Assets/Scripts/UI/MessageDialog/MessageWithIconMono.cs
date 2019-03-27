

using UnityEngine;
using System.Collections;
using UI.Controller;
using Utils;
using EventManager;
using network;
using network.protobuf;
using System.Collections.Generic;
using HeroData;
class MessageWithIconMono : MonoBehaviour
{
    struct MessageInfo
    {
        
        public GameObject go;
        public float y { get; set; }
        public MessageInfo(GameObject _go, float _y)
            : this()
        {
            go = _go;
            y = _y;

        }

        public void SetY(float _y)
        {
            y = _y;
        }
    }
    MessageWithIconController _controller;
    int _showNum = 0;//显示条数
    List<MessageInfo> _messageList = new List<MessageInfo>();
    GameObject _messageBg;
    GameObject _attrBg;
    //ItemBaseConfig itemCfg;
    void Awake()
    {
    }
    private int _leftCount = 0;
    int  _callCount = 0;
    List<EventMultiArgs> _argsList=new List<EventMultiArgs>();
    private void OnEventSetText(EventMultiArgs args)
    {
    }
    private void ShowAct()
    {
    }
    float scaleTime = 0.3f;
    float stayTime = 1f;
    float fadeTime = 0.7f;
    float appearTime = 0.3f;
    float stoptime = 1f;
    float moveTime = 1f;
    void action(GameObject go)
    {
       
    }
    void OnAppearComplete(MessageInfo info)
    {
      
    }
    void oncomplete()
    {
       
    }
    void onLeftComplete()
    {
      
    }
    void ShowEnd()
    {
    }

}