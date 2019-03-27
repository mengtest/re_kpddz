using UnityEngine;
using System.Collections;
using EventManager;

public class ProgressRunMono : MonoBehaviour {
    float _startValue = 0f;
    float _addValue = 0f;
    float _nTime = 0f;
    UIProgressBar _bar;
    float _runningTime = 0f;
    string _onUpdateCallback;
    GameObject _onUpdateTarget;
    System.Object _onUpdateParams;
    string _onCompleteCallback;
    GameObject _onCompleteTarget;
    System.Object _onCompleteParams;
    string _onFullCallback;
    GameObject _onFullTarget;
    System.Object _onFullParams;

    float lastValue = 200f;//上一帧进度条的值
	// 初始化
	void Awake () {
        _bar = gameObject.GetComponent<UIProgressBar>();
	}
	
	// Update 每帧调用一次
    void Update()
    {
        if (Mathf.Abs(_nTime) <= 0.01f)//若时间太短，不做表现，直接设值。
        {
            float to_value = _startValue + _addValue;
            to_value -= Mathf.Floor(to_value);
            _bar.value = to_value;
            CompleteCallback();
            Destroy(this);
            return;
        }

        _runningTime += Time.deltaTime;
        float percentage = Mathf.Clamp01(_runningTime / _nTime);
        float tempValue = _startValue + _addValue * percentage;
        float toValue = tempValue - Mathf.Floor(tempValue);
        if (tempValue > 0f)
        {
            _bar.value = toValue;
        }
        else
        {
            _bar.value = 1+toValue;
        }

        UpdateCallback();
        if (lastValue > 100f )
        {
        }
        else if ((_addValue > 0f && (lastValue - toValue) > 0f) || (_addValue < 0f && (lastValue - toValue) < 0f))
        {
            if (_onFullParams == null)
                _onFullParams = new EventMultiArgs();
            EventMultiArgs args = (EventMultiArgs)_onFullParams;
            args.AddArg("fullTimes", Mathf.FloorToInt(tempValue));
            FullCallback();
        }
        lastValue = toValue;
        if (Mathf.Abs(_runningTime) > Mathf.Abs(_nTime))
        {
            //[[结束]]
            CompleteCallback();
            Destroy(this);
        }
    }
    /// <summary>
    /// 跑进度条表现
    /// </summary>
    /// <param name="startValue">0~1初始值</param>
    /// <param name="addValue">增加值, >0表示向前跑, <0表示往回跑。</param>
    /// <param name="nTime"></param>
//     public void ProgressRunTo(float startValue, float addValue, float nTime)
//     {
//         if (_bar == null || Mathf.Abs(startValue) < 0f || Mathf.Abs(startValue) > 1f || addValue == 0f)
//             return;
// 
//         _startValue = startValue;
//         _addValue = addValue;
//         _nTime = nTime;
//     }

    /// <summary>
    /// 跑进度条表现
    /// </summary>
    /// <param name="startvalue">
    /// A <see cref="float"/> 0~1初始值
    /// </param>
    /// <param name="addValue">
    /// A <see cref="float"/> 增加值, >0表示向前跑, <0表示往回跑。
    /// </param>
    /// <param name="nTime">
    /// A <see cref="float"/> 时间
    /// </param>
    /// <param name="onupdate"> 
    /// A <see cref="System.String"/> for the name of a function to launch on every step of the animation.
    /// </param>
    /// <param name="onupdatetarget">
    /// A <see cref="GameObject"/> for a reference to the GameObject that holds the "onupdate" method.
    /// </param>
    /// <param name="onupdateparams">
    /// A <see cref="System.Object"/> for arguments to be sent to the "onupdate" method.
    /// </param> 
    /// <param name="oncomplete">
    /// A <see cref="System.String"/> for the name of a function to launch at the end of the animation.
    /// </param>
    /// <param name="oncompletetarget">
    /// A <see cref="GameObject"/> for a reference to the GameObject that holds the "oncomplete" method.
    /// </param>
    /// <param name="oncompleteparams">
    /// A <see cref="System.Object"/> for arguments to be sent to the "oncomplete" method.
    /// </param>
    public void ProgressRunTo(Hashtable args)
    {
        if (args.Contains("startvalue"))
        {
            _startValue = (float)args["startvalue"];
        }
        else
        {
            Utils.LogSys.LogError("ProgressRunTo Error: startvalue is lost!");
            return;
        }

        if (args.Contains("addValue"))
        {
            _addValue = (float)args["addValue"];
        }
        else
        {
            Utils.LogSys.LogError("ProgressRunTo Error: addValue is lost!");
            return;
        }

        if (args.Contains("nTime"))
        {
            _nTime = (float)args["nTime"];
        }
        else
        {
            Utils.LogSys.LogError("ProgressRunTo Error: nTime is lost!");
            return;
        }


        if (args.Contains("onupdate"))
        {
            _onUpdateCallback = (string)args["onupdate"];
            if (args.Contains("onupdatetarget"))
            {
                _onUpdateTarget = (GameObject)args["onupdatetarget"];
            }
            else
            {
                Utils.LogSys.LogError("ProgressRunTo Error: onupdatetarget is lost!");
                return;
            }
            if (args.Contains("onupdateparams"))
            {
                _onUpdateParams = (System.Object)args["onupdateparams"];
            }
        }
        if (args.Contains("onfull"))
        {
            _onFullCallback = (string)args["onfull"];
            if (args.Contains("onfulltarget"))
            {
                _onFullTarget = (GameObject)args["onfulltarget"];
            }
            else
            {
                Utils.LogSys.LogError("ProgressRunTo Error: onfulltarget is lost!");
                return;
            }
            if (args.Contains("onfullparams"))
            {
                _onFullParams = (System.Object)args["onfullparams"];
            }
        }

        if (args.Contains("oncomplete"))
        {
            _onCompleteCallback = (string)args["oncomplete"];
            if (args.Contains("oncompletetarget"))
            {
                _onCompleteTarget = (GameObject)args["oncompletetarget"];
            }
            else
            {
                Utils.LogSys.LogError("ProgressRunTo Error: oncompletetarget is lost!");
                return;
            }
            if (args.Contains("oncompleteparams"))
            {
                _onCompleteParams = (System.Object)args["oncompleteparams"];
            }
        }

    }

    void UpdateCallback()
    {
        if (_onUpdateCallback == null || _onUpdateCallback == "")
            return;

        _onUpdateTarget.SendMessage((string)_onUpdateCallback, (object)_onUpdateParams, SendMessageOptions.DontRequireReceiver);
    }
    void CompleteCallback()
    {
        if (_onCompleteCallback == null || _onCompleteCallback == "")
            return;

        _onCompleteTarget.SendMessage((string)_onCompleteCallback, (object)_onCompleteParams, SendMessageOptions.DontRequireReceiver);
    }
    void FullCallback()
    {
        if (_onFullCallback == null || _onFullCallback == "")
            return;

        _onFullTarget.SendMessage((string)_onFullCallback, (object)_onFullParams, SendMessageOptions.DontRequireReceiver);
    }
}
