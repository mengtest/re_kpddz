/***************************************************************

 *
 *
 * Filename:  	BasicDataStructs.cs	
 * Summary: 	游戏中的基本数据结构
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/04/10 3:23
 ***************************************************************/


#region Using
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#endregion


#region typedef
using AnimEventsList = System.Collections.Generic.List<BasicDataScructs.AnimEventInfo>;
using ModelBpsList = System.Collections.Generic.List<string>;
#endregion


namespace BasicDataScructs
{
    //动作事件[关键帧-事件]数据对
    [Serializable]
    public struct AnimEventValuePair
    {
        public float _fTime;
        public string _strEventID;

        public AnimEventValuePair(float fTime, string strEventID)
        {
            _fTime = fTime;
            _strEventID = strEventID;
        }
    }

    //单个动作事件信息
    [Serializable]
    public class AnimEventInfo
    {
        public int _nAnimID = 0;
        public List<AnimEventValuePair> _listEvents;

        public AnimEventInfo(int nAnimID)
        {
            _nAnimID = nAnimID;
            _listEvents = new List<AnimEventValuePair>();
        }

        //插入[关键帧-事件]数据对
        public void insertValuePair(int nKeyFrameIdx, string strEventID)
        {
            //由于配置的数值为帧索引，而UNITTY事件的触发点是以时间(s)为度量
            //所以要转换为时间(30fps)
            float fTime = nKeyFrameIdx / 30.0f;
            _listEvents.Add(new AnimEventValuePair(fTime, strEventID));
        }
    }

    //游戏中用到的模型数据
    [Serializable]
    public class GameModelData
    {
        //模型资产名
        public string _strAssetName;
        //碰撞体类型
        public int nColliderType = 1;
        //碰撞体参数
        public List<float> nColliderParams;
        //碰撞体位置
        public List<float> colliderCenter; 
        //动作资产名
        public string _strAnimationAssetName;
        //模型名字
        public string _strModelName = default(string);
        //动作列表&&事件&&绑点信息
        public List<string> _listAnimations;
        public AnimEventsList _listAnimEvents;
        public ModelBpsList _listModelBps;
    }  
}
