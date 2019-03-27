using System;
using UnityEngine;
using System.Collections;
using UI.Controller;

public class WaitingMono : MonoBehaviour
{
    private WaitingController controller;
    private Transform container;
    private Transform juHua;
    private float rotate;
    void Awake()
    {
        controller = (WaitingController)UIManager.GetControler(UIName.WAITING);

        juHua = transform.Find("Container/JuHua");
        container = transform.Find("Container");
    }

    private void OnBackBtnClick(GameObject go)
    {
        if (controller.BackAction == null) return;
        controller.BackAction();
        controller.BackAction = null;
    }

    // 初始化
    void Start()
    {
        StartCoroutine("setRotateJuHua");
    }

    // Update 每帧调用一次
    //void Update()
    //{

    //}

    IEnumerator setRotateJuHua()
    {
        while (true)
        {
            rotate = rotate - 30f;
            if (rotate <= -360f)
                rotate = 0f;
            juHua.Rotate(new Vector3(0.0f, 0.0f, -30f));
            //Utils.LogSys.Log("wait for begin:" + rotate.ToString());
            yield return new WaitForSeconds(0.2f);
            //Utils.LogSys.Log("wait for end:" + rotate.ToString());
        }
    }

    public void ShowWin()
    {
        container.gameObject.SetActive(true);
        UIEventListener.Get(transform.Find("Container/Back").gameObject).onClick = OnBackBtnClick;
    }

    public void HideWin()
    {
        container.gameObject.SetActive(false);
        controller.BackAction = null;
        UIEventListener.Get(transform.Find("Container/Back").gameObject).onClick = null;
    }

    public void ShowJuHua()
    {
        juHua.gameObject.SetActive(true);
    }

    public void HideJuHua()
    {
        juHua.gameObject.SetActive(false);
    }
}
