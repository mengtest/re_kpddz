using UnityEngine;
using System.Collections;

public class BaseActionMono : MonoBehaviour {

    /// <summary>
    /// 激活
    /// </summary>
    /// <param name="go"></param>
	public void Active()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 不激活
    /// </summary>
    /// <param name="go"></param>
    public void Deactive()
    {
        gameObject .SetActive(false);
    }

    /// <summary>
    /// 看向
    /// </summary>
    /// <param name="target"></param>
    public void Lookat(Transform target)
    {
        gameObject.transform.LookAt(target);
    }

    /// <summary>
    /// 发送给信息
    /// </summary>
    /// <param name="strMsg"></param>
    public new void SendMessage(string strMsg)
    {
        gameObject.SendMessage(strMsg);
    }

    /// <summary>
    /// 设置父节点
    /// </summary>
    /// <param name="parent"></param>
    public void SetParent(Transform parent)
    {
        gameObject.transform.SetParent(parent);
    }

    /// <summary>
    /// 移动
    /// </summary>
    /// <param name="target"></param>
    public void MoveTo(Transform target)
    {
        gameObject.transform.Translate(target.position);
    }

    /// <summary>
    /// 设置文字颜色
    /// </summary>
    /// <param name="color"></param>
    public void SetTextColor(Color color)
    {
        var lbl = GetComponent<UILabel>();
        if (lbl != null)
            lbl.color = color;

    }
}
