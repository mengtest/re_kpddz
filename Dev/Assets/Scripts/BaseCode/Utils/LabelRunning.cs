/***************************************************************


 *
 *
 * Filename:  	LabelRunning.cs	
 * Summary: 	数字跑动动画
 *
 * Version:    	1.0.0
 * Author: 	    HR.Chen
 * Date:   	    2016-02-29 16:55:35
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
#endregion

public class LabelRunning : MonoBehaviour
{

    #region Variable
    /// <summary>
    /// 数字跑动时间
    /// </summary>
    public float RunTime = 1.0f;

    /// <summary>
    /// 数字跑动曲线
    /// </summary>
    public iTween.EaseType EaseType = iTween.EaseType.easeInSine;

    /// <summary>
    /// 跑动放大
    /// </summary>
    public float RunningScale = 1.3f;

    /// <summary>
    /// 放大触发时间
    /// </summary>
    public float ScaleTime = 0.25f;

    /// <summary>
    /// 目标文本
    /// </summary>
    public UILabel TargetLabel = null;

    private int _currentValue = 0;
    private int _willToValue = 0;

    #endregion

    #region Override And Constructor
    /// Use this for initialization
    void Awake()
    {
        parseAndJudgeLabelText();
	}

    /// Update is called once per frame
 //   void Update() {
 //       /* 测试的代码
	//    if (Input.GetMouseButtonDown(0)) {
 //           print("On Mouse Left Click!!!!!!!!!");
 //           AddValue(100);
 //       }
 //       else if (Input.GetMouseButtonDown(1))
 //       {
 //           AddValue(-50, true);
 //       }
 //        */
	//}
    #endregion

    /// parse label text to int
    private void parseAndJudgeLabelText()
    {
        if (!TargetLabel)
        {
            TargetLabel = gameObject.GetComponent<UILabel>();
        }
        
        if (!TargetLabel || !int.TryParse(TargetLabel.text, out _currentValue))
        {
            _currentValue = 0;
            TargetLabel.text = "0";
        }
        _willToValue = _currentValue;
    }

    public void OnValueUpdate(int value) {
        _currentValue = value;
        TargetLabel.text = _currentValue.ToString();
        if (_currentValue == _willToValue)
        {
            iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(1f, 1f, 1f), "time", ScaleTime)); 
        }
    }

    public void Reset()
    {
        iTween.Stop(gameObject);
        OnValueUpdate(_willToValue);
        if (ScaleTime < 0)
        {
            ScaleTime = 0;
        }
    }

    /// <summary>
    /// 动画设置数值,immediately表示是否立即增加，false表示动画
    /// <summary>
    public void SetValue(int value, bool immediately = false)
    {
        _willToValue = 0;
        AddValue(value, immediately);
    }

    /// <summary>
    /// 增加数值,immediately表示是否立即增加，false表示动画
    /// <summary>
    public void AddValue(int value, bool immediately = false)
    {
        Reset();
        _willToValue = _willToValue + value;
        if (!immediately)
        {
            var itweenArgs = new Hashtable { { "easeType", EaseType } };
            itweenArgs.Clear();
            itweenArgs.Add("from", _currentValue);
            itweenArgs.Add("to", _willToValue);
            itweenArgs.Add("onupdate", "OnValueUpdate");
            itweenArgs.Add("time", RunTime);
            iTween.ValueTo(gameObject, itweenArgs);
            iTween.ScaleTo(gameObject, iTween.Hash("scale", new Vector3(RunningScale, RunningScale, 1f), "time", ScaleTime)); 
            
        }
        else
        {
            OnValueUpdate(_willToValue);
        }
    }

    /// <summary>
    /// 返回最终值
    /// </summary>
    public int GetValue()
    {
        return _willToValue;
    }

    /// <summary>
    /// 返回当前显示的数值
    /// </summary>
    public int GetRunningValue()
    {
        return _currentValue;
    }

    public static LabelRunning GetRunningScript(UILabel label)
    {
        LabelRunning runningScript = label.GetComponent<LabelRunning>();
        runningScript.TargetLabel = label;
        return runningScript;
    }

    public static void AddValue(UILabel label, int value, bool immediately = false)
    {
        GetRunningScript(label).AddValue(value, immediately);
    }

    public static void SetValue(UILabel label, int value, bool immediately = false)
    {
        GetRunningScript(label).SetValue(value, immediately);
    }
}
