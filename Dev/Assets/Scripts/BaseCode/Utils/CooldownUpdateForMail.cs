using System;
using UnityEngine;

public class CooldownUpdateForMail : MonoBehaviour
{
    public delegate void TimeOverCallback(GameObject obj);

    public TimeOverCallback OnComplete;
    public Action OnUpdate = null;
    private UILabel textLabel;
    private int left_time = 0; //结束时间点（isCooldown为true时生效）
    private int startTime = 0;
    private int type = 0;
    private void Awake()
    {
        textLabel = gameObject.GetComponent<UILabel>();
    }

    private void Update()
    {
        if (textLabel == null)
            return;
        //倒计时表现
        if (left_time == 0)
        {
            textLabel.text = "";
            if (OnComplete != null)
                OnComplete(gameObject);
            return;
        }
        if (type == 0)
        {
            int nowTime = (int)UtilTools.GetServerTime();
            left_time -= (nowTime - startTime);
            startTime = nowTime;
            textLabel.text = timeStr(left_time);
            if (left_time == 0)
            {
                textLabel.text = "";
                if (OnComplete != null)
                    OnComplete(gameObject);

            }
        }
        else if(type==1)
        {
            int left = left_time - (int)UtilTools.GetServerTime();
            textLabel.text = timeStr(left);
            if (left == 0)
            {
                textLabel.text = "";
                if (OnComplete != null)
                    OnComplete(gameObject);

            }
        }
        if (OnUpdate != null)
        {
            OnUpdate();
        }
    }

    private string timeStr(int ts)
    {
        
        string timeS = "";
        if (type == 0)
        {
            int leftday = ts / 86400;
            int lefthour = (ts - 86400 * leftday) / 3600;
            timeS = GameText.Format("mail_desc1", leftday, lefthour);
        }
        else if(type == 1)
        {
            TimeSpan timeSpan = new TimeSpan(0, 0, ts);
            timeS = string.Format("{0:T}", timeSpan);
        }
        return timeS;
    }

    /// <summary>
    /// 设好结束时间点后，开始自动倒计时
    /// </summary>
    /// <param name="endtime"></param>
    public void SetTime(int leftTime,int type=0)
    {
        left_time = leftTime;
        startTime = (int)UtilTools.GetServerTime();
        this.type = type;
    }
}