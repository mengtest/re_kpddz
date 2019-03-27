using System;
using System.Collections.Generic;
using UnityEngine;


public delegate void TimerEvent();

class TimerManager
{
    static private TimerManager _instance = null;
    static public TimerManager GetInstance()
    {
        if (_instance == null)
            _instance = new TimerManager();
        return _instance;
    }

    private PriorityQueue<Timer> _priorQueue;
    private PriorityQueue<Timer> _battleTimerQueue;

    private TimerManager()
    {
        _priorQueue = new PriorityQueue<Timer>();
        _battleTimerQueue = new PriorityQueue<Timer>();
    }

    public void Clean()
    {
        Utils.LogSys.Log("TimerClean");
        Clean(_priorQueue);
        Clean(_battleTimerQueue);
    }

    public void AllClear()
    {
        while (_priorQueue.Count > 0)
            _priorQueue.Pop();

        while (_battleTimerQueue.Count > 0)
            _battleTimerQueue.Pop();
    }

    public void Clean(PriorityQueue<Timer> queue)
    {
        Timer t;
        List<Timer> newQueue = new List<Timer>();
        while (queue.Count > 0)
        {
            t = queue.Pop();
            if (!t._inactiveWhenSceneChange)
                newQueue.Add(t);
        }

        foreach (var timer in newQueue)
            queue.Push(timer);
    }

    public void AddTimer(Timer timer, int type)
    {
        if (type == 1)
            _battleTimerQueue.Push(timer);
        else
            _priorQueue.Push(timer);
    }

    public void RemoveTimer(Timer timer)
    {
        if (timer != null)
            timer._active = false;
    }

    public void Trigger()
    {
		long realTimeMilli = System.DateTime.Now.ToFileTime() / 10000;
		Trigger(_priorQueue, realTimeMilli);
        Trigger(_battleTimerQueue, (long)(Time.time * 1000));
    }

    public void Trigger(PriorityQueue<Timer> queue, long time)
    {
        int count = 0;
        while (queue.Count > 0 && queue.Top()._triggerTime < time && count < 60)
        {
            Timer t = queue.Pop();
            if (t._active)
            {
                try
                {
                    t.Callback();
                }
                catch (Exception e)
                {
                    Utils.LogSys.LogError("TimerError: " + e.ToString());
                    Debug.LogException(e);
                }
            }
            ++count;
        }
    }
}


public class Timer : IComparable
{
    public long _triggerTime;
    public event TimerEvent _callback;
    public bool _active;
    public bool _inactiveWhenSceneChange;

    /// <summary>
    /// 添加一个计时器
    /// triggerTime：触发时间
    /// callback：回调
    /// type：1为战斗定时器，在加速时也会加速；0为普通计时器，战斗加速也不会加快
    /// inactiveWhenSceneChange：切换场景时是否被消除
    /// </summary>
    public Timer(float delayTime, TimerEvent callback, int type = 1, bool inactiveWhenSceneChange = true)
    {
        if (type == 1)
            _triggerTime = (long)((delayTime + Time.time) * 1000);
        else
        {
			long realTimeMilli = System.DateTime.Now.ToFileTime() / 10000;
            // 系统时间，第八位为秒
			_triggerTime = (long)(delayTime * 1000) + realTimeMilli;
        }
        _callback = callback;
        _active = true;
        _inactiveWhenSceneChange = inactiveWhenSceneChange;
        TimerManager.GetInstance().AddTimer(this, type);
    }

    virtual public void Callback()
    {
        if (_callback == null) return;

        try
        {
            _callback();
        }
        catch (Exception e)
        {
            Utils.LogSys.Log(e.ToString());
        }
    }

    public int CompareTo(object other)
    {
        return (other as Timer)._triggerTime.CompareTo(_triggerTime);
    }
}

public class PeriodicTimer : Timer
{
    public long _periodic;
    public int _runTime;
    public int _topLimit;
    public int _type;

    /// <summary>
    /// 添加一个周期定时器
    /// triggerTime：触发时间
    /// callback：回调
    /// periodic：周期，单位秒
    /// type：1为战斗定时器，在加速时也会加速；0为普通计时器，战斗加速也不会加快
    /// toplimit：执行次数，0为无限次
    /// inactiveWhenSceneChange：切换场景时是否被消除
    /// </summary>
    public PeriodicTimer(float delayTime, TimerEvent callback, float periodic, int type = 1, int topLimit = 0, bool inactiveWhenSceneChange = true): base(delayTime, callback, type, inactiveWhenSceneChange)
    {
        _periodic = (long)(periodic * 1000);
        _runTime = 0;
        _topLimit = topLimit;
        _type = type;
    }

    public override void Callback()
    {
        base.Callback();
        _triggerTime += _periodic;
        _runTime += 1;

        if (_topLimit == 0 || _runTime < _topLimit)
        {
            TimerManager.GetInstance().AddTimer(this, _type);
        }
    }
}