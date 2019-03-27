using UnityEngine;
using System.Collections;

public class CardColorAlphaUpdate : MonoBehaviour {

    Color _color = new Color(0.5f, 0.5f, 0.5f, 1.0f);

    /// <summary>
    /// 卡牌更新事件
    /// </summary>
    /// <param name="go"></param>
    /// <param name="delta"></param>
    public void onCardUpdateEventHandle(GameObject go, float delta)
    {
        UISprite[] spts = go.GetComponentsInChildren<UISprite>();
        for (int i=0; i<spts.Length; i++)
        {
            spts[i].color = _color + (1.0f-delta) * _color;
        }
    }

    // Use this for initialization
    void Start () {
        var cardAnim = GetComponent<CardAnimation>();
        cardAnim.onCardUpdateEvent = onCardUpdateEventHandle;
    }
}
