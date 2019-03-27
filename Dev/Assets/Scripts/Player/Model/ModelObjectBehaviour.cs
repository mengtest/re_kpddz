/***************************************************************


 *
 *
 * Filename:  	ModelObjectBehaviour.cs	
 * Summary: 	模型对象行为控制
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/08/12 17:10
 ***************************************************************/

#region Using
using System;
using System.Collections.Generic;
using BasicDataScructs;
using UnityEngine;
using EventManager;
#endregion

namespace player
{
    /// <summary>
    /// 模型事件
    /// </summary>
    public struct ModelAnimationEventData
    {
        public int idAnimation;
        public float fTime;
        public string strEventName;
    }

    /*************************************************************************************************/

    /// <summary>
    /// 模型行为控制，控制动作，动作事件等等
    /// </summary>
    public class ModelObjectBehaviour : MonoBehaviour
    {
        public delegate void animationEventDelegate(int idAnim, string strEventName);
        public  animationEventDelegate onAnimationEvent = null;

        [HideInInspector]
        public Model _objModel = null;

        //动作事件集合
        List<ModelAnimationEventData> _listAnimEvents = new List<ModelAnimationEventData>(); 

        //////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 动作事件
        /// </summary>
        public ModelAnimationEventData[] AnimEvents
        {
            get { return _listAnimEvents.ToArray(); }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 动作事件
        /// </summary>
        /// <param name="strValue">动作参数(格式：idAnim,strEvent,例：601,start)</param>
        /// <param name="intValue">动作参数(格式：strEvent + idAnim,例：idEvent:10000, idAnim:301---->10000301)</param>
        public void onAnimationEventHandle(int intValue)
        {
            //             string[] arrStrInfo = strValue.Split(',');
            //             int idAnim = int.Parse(arrStrInfo[0].Trim());
            //             string strEvent = arrStrInfo[1];

            int idEvent = intValue / 1000;
            int idAnim = intValue % 1000;
            
            //For debug
            //if (idAnim != 101 && idAnim != 102)
            //    Utils.LogSys.Log(gameObject.name + ", " + idAnim + ", " + strEvent);

            if (onAnimationEvent != null)
            {
                if (idEvent == 1)
                {
                    onAnimationEvent(idAnim, "start");
                }
                else if(idEvent == 2)
                {
                    onAnimationEvent(idAnim, "end");
                }
                else
                {
                    onAnimationEvent(idAnim, idEvent.ToString());
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////

        // 初始化
        void Start()
        {
            if (_objModel == null) return;

            int nAnimID = 0;
            int nParam = 0;
            int nEventID = 0;
            foreach (AnimationState state in GetComponent<Animation>())
            {
                AnimationClip animClip = state.clip;
                string strAnimID = animClip.name.Substring(animClip.name.IndexOf('_') + 1);
                int.TryParse(strAnimID, out nAnimID);

                //动作开始
                nParam = 1 * 1000 + nAnimID;
                AnimationEvent animEventStart = new AnimationEvent();
                animEventStart.time = 0.0f;
                //animEventStart.stringParameter = strAnimID + "," + "start";
                animEventStart.intParameter = nParam;
                animEventStart.functionName = "onAnimationEventHandle";
                animClip.AddEvent(animEventStart);

                ModelAnimationEventData startData = new ModelAnimationEventData();
                startData.idAnimation = int.Parse(strAnimID);
                startData.fTime = 0.0f;
                startData.strEventName = "start";
                _listAnimEvents.Add(startData);


                //动作结束
                nParam = 2 * 1000 + nAnimID;
                AnimationEvent animEventEnd = new AnimationEvent();
                animEventEnd.time = animClip.length; //动作结束有误差
                //animEventEnd.stringParameter = strAnimID + "," + "end";
                animEventStart.intParameter = nParam;
                animEventEnd.functionName = "onAnimationEventHandle";
                animClip.AddEvent(animEventEnd);

                ModelAnimationEventData endData = new ModelAnimationEventData();
                endData.idAnimation = int.Parse(strAnimID);
                endData.fTime = animClip.length;
                endData.strEventName = "end";
                _listAnimEvents.Add(endData);


                foreach (AnimEventInfo animEventInfoItem in _objModel.ModelData._listAnimEvents)
                {
                    string animationName = _objModel.ModelID + "_" + Convert.ToString(animEventInfoItem._nAnimID);
                    if (animClip.name == animationName)
                    {
                        foreach (AnimEventValuePair animEventItem in animEventInfoItem._listEvents)
                        {
                            AnimationEvent animEvent = new AnimationEvent();
                            int.TryParse(animEventItem._strEventID, out nEventID);
                            nParam = nEventID * 1000 + nAnimID;
                            animEvent.time = animEventItem._fTime;
                            //animEvent.stringParameter = animEventInfoItem._nAnimID + "," + animEventItem._strEventID;
                            animEvent.intParameter = nParam;
                            animEvent.functionName = "onAnimationEventHandle";
                            animClip.AddEvent(animEvent);
                           
                            ModelAnimationEventData data = new ModelAnimationEventData();
                            data.idAnimation = animEventInfoItem._nAnimID;
                            data.fTime = animEventItem._fTime;
                            data.strEventName = animEventItem._strEventID;
                            _listAnimEvents.Add(data);
                        }
                    }
                }
            }


            _listAnimEvents.Sort(delegate (ModelAnimationEventData left, ModelAnimationEventData right)
            {
                if (left.fTime >= right.fTime)
                    return 1;
                else
                    return -1;
            });
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////

        // Update 每帧调用一次
        //void Update()
        //{

        //}
    }
}


