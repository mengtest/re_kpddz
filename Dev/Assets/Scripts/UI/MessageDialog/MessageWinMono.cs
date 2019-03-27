/***************************************************************


 *
 *
 * Filename:  	MessageWinMono.cs	
 * Summary: 	信息提示 出后几秒内自动消失
 *
 * Version:   	1.0.0
 * Author: 		XB.Wu
 * Date:   		2015/07/02 14:32
 ***************************************************************/
using UnityEngine;
using System.Collections;
using UI.Controller;
using Utils;
using EventManager;
using network;
using network.protobuf;
using System.Collections.Generic;

public class MessageWinMono : MonoBehaviour {
    struct MessageInfo {
        public GameObject go;
        public float y { get; set; }
        public MessageInfo(GameObject _go, float _y) : this()
        {
            go = _go;
            y = _y;
        }

        public void SetY(float _y){
            y = _y;
        }
    }

    MessageWinController _controller;

    int _showNum = 0;//显示条数
    List<MessageInfo> _messageList = new List<MessageInfo>();

    GameObject _messageBg;

    void Awake() {
        _controller = (MessageWinController)UIManager.GetControler(UIName.MESSAGE_WIN);
        _controller.RegisterUIEvent(UIEventID.MESSAGE_WIN_SET_TEXT, OnEventSetText);
        _messageBg = transform.Find("message_bg").gameObject;
    }

    private void OnEventSetText(EventMultiArgs args) {
        _showNum++;
        GameObject go = NGUITools.AddChild(gameObject, _messageBg);
        string text = args.GetArg<string>("text");
        string color = args.GetArg<string>("color");
        UILabel label = go.transform.Find("Label").GetComponent<UILabel>();
        label.text = color + text;
        go.GetComponent<UIWidget>().width = (int)label.printedSize.x + 100;
        go.GetComponent<UIWidget>().height = (int)label.printedSize.y + 21;
        //以显示的提示向上移动
        if (_messageList.Count > 0){
            oncomplete();
            //GameObject lastGo = _messageList[_messageList.Count - 1].go;
            //float y = (float)(go.transform.GetComponent<UIWidget>().height + (float)lastGo.GetComponent<UIWidget>().height) / 2 + 4f;
            //for (int i = 0; i < _messageList.Count; i++) {
            //    MessageInfo item = _messageList[i];
            //    GameObject oldGo = _messageList[i].go;
            //    float newY = item.y + y;
            //    Hashtable hash = new Hashtable();
            //    hash.Add("time", 0.3f);
            //    hash.Add("y", newY);
            //    hash.Add("islocal", true);
            //    iTween.MoveTo(oldGo, hash);
            //    item.y = newY;
            //    _messageList[i] = item;
            //}

            //_messageList[0].y += 45;
        }
        MessageInfo messageInfo = new MessageInfo(go, 0);
        _messageList.Add(messageInfo);
        action(go);
    }
    float scaleTime = 0.3f;
    float stayTime = 2f;
    float fadeTime = 1f;

    void action(GameObject go) {

        ///出现效果
        Hashtable args = new Hashtable();
        args.Clear();
        args.Add("easeType", iTween.EaseType.easeOutBack);
        args.Add("scale", new Vector3(0.6f, 0.6f, 1f));
        args.Add("time", scaleTime);
        args.Add("oncomplete", "onScaleComplete");
        args.Add("oncompletetarget", gameObject);
        args.Add("oncompleteparams", go);
        iTween.ScaleFrom(go, args);

       
        //渐隐
        TweenAlpha tweenAlpha = TweenAlpha.Begin(go, fadeTime, 0);
        tweenAlpha.delay = scaleTime + stayTime;
        tweenAlpha.from = 1f;
        tweenAlpha.SetOnFinished(oncomplete);

        //StartCoroutine(destroyMessageBg(messageInfo));
    }

    //IEnumerator destroyMessageBg(MessageInfo messageInfo) {
    //yield return new WaitForSeconds(scaleTime + stayTime + fadeTime + 0.05f);
    void oncomplete() {
        if (_messageList.Count > 0) {
            GameObject.DestroyObject(_messageList[0].go);
            _messageList.RemoveAt(0);
            //messageInfo.go = null;
        } 
        _showNum--;
        ShowEnd();
    }

    void ShowEnd() {
        if (_showNum <= 0) {
            _controller.toBack();
        } 
    }
}
