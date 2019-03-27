using System;
using System.Text;
using UnityEngine;

[SLua.CustomLuaClass]
public class CooldownUpdate : MonoBehaviour
{
    public delegate void TimeOverCallback(GameObject obj);

    public TimeOverCallback OnComplete;
    public Action OnUpdate = null;
    public bool isCooldown = true;//true为倒计时，false为正计时
    public bool isServerTime = true; //(服务端时间,或客户端时间)
    public bool isNoHour = false;//false时(HH:MM:SS)  true时(MM:SS)
    public bool isTicket = false;//false时(HH:MM:SS)  true时(MM:SS)
    private UILabel textLabel;
    private uint end_time = 0; //结束时间点（isCooldown为true时生效）
    private uint start_time = 0; //结束时间点（isCooldown为false时生效）
    public uint cooldownSound = 0;//从几秒以后开始播放声音，若不需要设置0
    private long preTime = 0;
    private StringBuilder strBulder = null;


    private void Awake()
    {
        textLabel = gameObject.GetComponent<UILabel>();
        strBulder = new StringBuilder(64);
    }

    private void Update()
    {
		if (textLabel == null)
			return;
        if (isTicket)
        {
            if (end_time == 0)
                return;
            int nowTime = isServerTime ? UtilTools.GetServerTime() : UtilTools.GetClientTime();
            long lastTime = (long)end_time - (long)nowTime;
            if (lastTime <= 0)
            {
                textLabel.text = "0";
                if (OnComplete != null)
                {
                    OnComplete(gameObject);
                }
            }
            else
            {
                if (cooldownSound > 0 && lastTime <= cooldownSound && preTime != lastTime)
                {
                    UtilTools.PlaySoundEffect("Sounds/RichCar/start", 1f);
                    preTime = lastTime;
                }
                textLabel.text = lastTime.ToString();//isNoHour ? string.Format("{0:T}", ts).Substring(3) : string.Format("{0:T}", ts);
                if (OnUpdate != null)
                {
                    OnUpdate();
                }
            }
            return;
        }

        if (isCooldown) {
            //倒计时表现
            if (end_time == 0)
                return;

            int nowTime = isServerTime ? UtilTools.GetServerTime() : UtilTools.GetClientTime();
            long lastTime = (long)end_time - (long)nowTime;
            if (lastTime <= 0) {
                textLabel.text = isNoHour ? "00:00" : "00:00:00";
                end_time = 0;
                if (OnComplete != null) {
                    OnComplete(gameObject);
                }
            } else {
                if (cooldownSound > 0 && lastTime <= cooldownSound && preTime != lastTime)
                {
                    UtilTools.PlaySoundEffect("Sounds/clock", 1f);
                    preTime = lastTime;
                }
                TimeSpan ts = new TimeSpan(0, 0, (int)lastTime);
                textLabel.text = timeStr(ts);//isNoHour ? string.Format("{0:T}", ts).Substring(3) : string.Format("{0:T}", ts);
                if (OnUpdate != null) {
                    OnUpdate();
                }
            }
        } else {
            //正计时表现
            if (start_time == 0)
                return;

            int nowTime = isServerTime ? UtilTools.GetServerTime() : UtilTools.GetClientTime();
            long countTime = (long)nowTime - (long)start_time;
            if (countTime <= 0) {
                textLabel.text = isNoHour ? "00:00" : "00:00:00";
            } else {
                TimeSpan ts = new TimeSpan(0, 0, (int)countTime);
                textLabel.text =timeStr(ts);//isNoHour ? string.Format("{0:T}", ts).Substring(3) : string.Format("{0:T}", ts);
            }
        }
    }

    private string timeStr(TimeSpan ts)
    {
        if (strBulder == null)
            return "";

        strBulder.Length = 0;
        if (isNoHour)
        {
            strBulder.Append(ts.Minutes).Append(":").Append(ts.Seconds);
        }
        else
        {
            strBulder.Append(ts.Hours).Append(":").Append(ts.Minutes).Append(":").Append(ts.Seconds);
        }

        if (ts.Days > 0)
        {
            strBulder.Append(ts.Days).Append("天").Append(ts.Hours).Append(":").Append(ts.Minutes).Append(":").Append(ts.Seconds);
        }

        return strBulder.ToString();


        //string timeS = "";
        //if (isNoHour) {
        //    timeS = string.Format("{0:T}", ts).Substring(3);
        //}else {
        //    timeS = string.Format("{0:T}", ts);
        //}
        //if (timeS.Length > 8) {
        //    timeS = GameText.Format("time_cool_down", ts.Days, timeS.Substring(timeS.Length - 8));
        //}
        //return timeS;
    }

    /// <summary>
    /// 设好结束时间点后，开始自动倒计时
    /// </summary>
    /// <param name="endtime"></param>
    public void SetEndTime(uint endtime)
    {
        isCooldown = true;
        end_time = endtime;
        if (end_time == 0 && textLabel != null)
        {
            textLabel.text = isNoHour ? "00:00" : "00:00:00";
        }
    }

    /// <summary>
    /// 设好开始时间点后，开始自动计时
    /// </summary>
    /// <param name="starttime"></param>
    public void SetStartTime(uint starttime)
    {
        isCooldown = false;
        start_time = starttime;
        if (starttime == 0 && textLabel != null)
        {
            textLabel.text = isNoHour ? "00:00" : "00:00:00";
        }
    }
}