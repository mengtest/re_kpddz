/***************************************************************


 *
 *
 * Filename:  	AnimationClipConfig.cs	
 * Summary: 	读取模型配置信息
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/04/09 23:16
 ***************************************************************/


#region Using
using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using INI;
using BasicDataScructs;
#endregion

#region typedef
using AnimClipsList = System.Collections.Generic.List<ModelConfig.AnimClipInfo>;
using AnimEventsList = System.Collections.Generic.List<BasicDataScructs.AnimEventInfo>;
using ModelBpsList = System.Collections.Generic.List<string>;
using ModelDic = System.Collections.Generic.Dictionary<string, ModelConfig.ModelData>;
using System.IO;
using Utils;
#endregion


namespace ModelConfig
{
    //单个动作裁剪信息
    [Serializable]
    public class AnimClipInfo
    {
        public string name;
        public int firstFrame;
        public int lastFrame;
        public bool isloop;

        public AnimClipInfo(string _n, int _f, int _l, bool _i)
        {
            name = _n;
            firstFrame = _f;
            lastFrame = _l;
            isloop = _i;
        }
    }

    //模型数据
    public class ModelData
    {
        //模型资产名
        public string _strAssetName;
        //动作资产名
        public string _strAnimationAssetName;
        //模型名字
        public string _strModelName = default(string);
        //裁剪&&动作列表&&事件&&绑点信息
        public AnimClipsList _listAnimClips;
        public List<string> _listAnimations;
        public AnimEventsList _listAnimEvents;
        public ModelBpsList _listModelBps;
        public Dictionary<string, bool> _dicAnimLoop;//动作是否循环
        //是否已经处理过
        public bool _bInit = false;

        public int _nColliderType = 1;
        public List<float> _nColliderParams;
        public List<float> _nColliderCenter;
        
        public ModelData(string strAssetName)
        {
            _strAssetName = strAssetName;
            _strAnimationAssetName = strAssetName + "@anim";
            _listAnimClips = new AnimClipsList();
            _listAnimEvents = new AnimEventsList();
            _listModelBps = new ModelBpsList();
            _listAnimations = new List<string>();
            _dicAnimLoop = new Dictionary<string, bool>();
            _nColliderParams = new List<float>();
            _nColliderCenter = new List<float>();
        }

        public bool InitFlag
        {
            get
            {
                return _bInit;
            }

            set
            {
                _bInit = value;
                //ModelDataProcess.setInitFlag(_strAssetName, _bInit);
            }
        }
    }

    //模型配置信息读取
    public static class ModelDataProcess
    {
        public static bool isInit = false;
        public static ModelDic _dicModelData = new ModelDic();

        [MenuItem("Model/模型数据/第1步-导入INI文件)")]
        public static void excute()
        {
            //if (isInit)
            //return;
            //isInit = true;
            _dicModelData.Clear();


            //读取INI文件
            if (!File.Exists(Application.dataPath + "/Resources/AnimationClipConfig.ini")) {
                LogSys.LogError("配置文件不存在 " + Application.dataPath + "/Resources/AnimationClipConfig.ini");
                return;
            }
            string iniPath = "Assets/Resources/AnimationClipConfig.ini";
            
            IniFiles pIniFile = new IniFiles(iniPath);
            if (pIniFile != null)
            {
                StringCollection strCollect = new StringCollection();
                pIniFile.ReadSections(strCollect);
                foreach (object obj in strCollect)
                {
                    string strSectionName = obj as string;

                    //读取节点
                    NameValueCollection valueCollection = new NameValueCollection();
                    pIniFile.ReadSectionValues(strSectionName, valueCollection);

                    //创建ModelInfo
                    ModelData tempModel = new ModelData(strSectionName);

                    //保存每个动作的开始帧，用于计算事件的时间
                    Dictionary<string, int> startFrameDic = new Dictionary<string, int>();
                    Dictionary<string, int> endFrameDic = new Dictionary<string, int>();

                    //遍历取key-value
                    string strModelName = "";
                    string strAnimationID = "";
                    foreach (string key in valueCollection.AllKeys)
                    {
                        //Debug.Log(key + ", " + valueCollection[key]);
                        string strValue = valueCollection[key];

                        //模型名字
                        if (key.Contains("modelname"))
                        {
                            strModelName = strValue;
                            tempModel._strModelName = strModelName;
                            //Debug.Log(valueCollection[key]);
                        }
                        else if (key.Contains("anim"))
                        {
                            string strAnimID = key.Substring(key.LastIndexOf('_') + 1);
                            strAnimationID = strSectionName + "_" + strAnimID;
                            string[] split = strValue.Split(new char[] { ',' });
                            int nStartFrame = Convert.ToInt32(split[0]);
                            int nEndFrame = Convert.ToInt32(split[1]);
                            bool bLoop = Convert.ToBoolean(split[2]);

                            //Debug.Log("" + nStartFrame + ", " + nEndFrame + ", " + Convert.ToString(bLoop));
                            tempModel._dicAnimLoop[strAnimID] = bLoop;
                            AnimClipInfo pAnimClip = new AnimClipInfo(strAnimationID, nStartFrame, nEndFrame, bLoop);
                            tempModel._listAnimClips.Add(pAnimClip);
                            tempModel._listAnimations.Add(strAnimID);

                            startFrameDic.Add(key, nStartFrame);
                            endFrameDic.Add(key, nEndFrame);
                            if (ModelBindTest.bCheckModel) {
                                if (nStartFrame >= nEndFrame) {
                                    LogSys.LogError(string.Format("动作起始错误 {0} {1}", tempModel._strAssetName, strAnimID));
                                }
                            } 
                        }
                        else if (key.Contains("event")) //事件
                        {
                            string strAnimID = key.Substring(key.LastIndexOf('_')+1);
                            string animKey = "anim_" + strAnimID;
                            if (startFrameDic.ContainsKey(animKey))
                            {
                                //Debug.Log(strValue);
                                int nAnimStartFrame = startFrameDic[animKey];
                                AnimEventInfo animEventInfo = new AnimEventInfo(Convert.ToInt32(strAnimID));
                                
                                string[] arrEvents = strValue.Split(new char[]{','});
                                for (int i=0; i<arrEvents.Length; i++)
                                {
                                    string[] arrStrEventData = arrEvents[i].Split(new char[] { '-' });
                                    int nKeyFrame = Convert.ToInt32(arrStrEventData[0]);
                                    string strEventID = arrStrEventData[1];

                                    if (ModelBindTest.bCheckModel) {
                                        int nAnimEndFrame = endFrameDic[animKey];
                                        if (nKeyFrame < nAnimStartFrame || nKeyFrame > nAnimEndFrame) {
                                            LogSys.LogError(string.Format("关键帧错误 {0} {1}", tempModel._strAssetName, animKey));
                                        }
                                    } 
                                    
                                    //Debug.Log(nKeyFrame);
                                    
                                    if (nKeyFrame > nAnimStartFrame)  //触发帧要大于起始帧
                                    {
                                        animEventInfo.insertValuePair(nKeyFrame - nAnimStartFrame, strEventID);
                                    }
                                }

                                tempModel._listAnimEvents.Add(animEventInfo);
                            }
                        }
                        else if (key.Contains("bps"))
                        {
                            string[] arrBps = strValue.Split(new char[] { ',' });
                            for (int i = 0; i < arrBps.Length; i++)
                            {
                                tempModel._listModelBps.Add(arrBps[i]);
                            }
                        }
                        else if (key.Contains("init"))
                        {
                            bool bInit = Convert.ToBoolean(strValue);
                            tempModel.InitFlag = bInit;
                        }
                        else if (key.Contains("colliderType"))
                        {
                            tempModel._nColliderType = Convert.ToInt32(strValue);
                        }
                        else if (key.Contains("colliderParams"))
                        {
                            string[] arrCP = strValue.Split(new char[] { ',' });
                            for (int i = 0; i < arrCP.Length; i++)
                            {
                                tempModel._nColliderParams.Add(float.Parse(arrCP[i]));
                            }
                        }
                        else if (key.Contains("colliderCenter"))
                        {
                            string[] arrCP = strValue.Split(new char[] { ',' });
                            for (int i = 0; i < arrCP.Length; i++)
                            {
                                tempModel._nColliderCenter.Add(float.Parse(arrCP[i]));
                            }
                        }
                    }


                    //添加到列表
                    _dicModelData[strSectionName] = tempModel;
                }
            }
            else
            {
                Debug.LogError("Model configure file is not exist!!");
            }
        }

        //获取模型数据
        public static ModelData getModelDataByIdx(string strAssetIdx)
        {
            if (_dicModelData.ContainsKey(strAssetIdx))
            {
                return _dicModelData[strAssetIdx];
            }
            else
            {
                return null;
            }
        }

        //设置初始化标示
        public static void setInitFlag(string strIniSectionIdx, bool bValue)
        {
            IniFiles pIniFile = new IniFiles("Assets/Resources/AnimationClipConfig.ini");
            if (pIniFile != null)
            {
                pIniFile.WriteBool(strIniSectionIdx, "init", bValue);
            }
        }
    }
}


