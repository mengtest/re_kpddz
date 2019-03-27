/***************************************************************


 *
 *
 * Filename:  	PlayerManager.cs	
 * Summary: 	Player数据管理
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/04/16 21:40
 ***************************************************************/

using UnityEngine;
using System;
using System.Collections.Generic;
using Utils;
using effect;
using task;
using asset;

namespace player
{
    //Player事件
    [SLua.CustomLuaClass]
    public class PlayerDelegate{
        public delegate void onPlayerEvent(Model pModelObject);
        public delegate void testEvent();
    }

    //Player数据管理
    [SLua.CustomExLuaClass]
    public class PlayerManager : Singleton<PlayerManager>
    {
        public PlayerDelegate.onPlayerEvent onModelCreateEvent = null;

        //模型数据
        public static GameModelDataHolder _pAllModelData = null;

        //等待创建列表缓冲
        private List<Model> _listWaitingCreateBuffer = new List<Model>();

        /////////////////////////////////////////////////////////////////////////////////////

        //初始化Player类
        public void init()
        {
            onModelCreateEvent = onModelCreateSuccessEvent;
        }

        //获取模型数据
        public static GameModelDataHolder ModelData
        {
            get { return _pAllModelData; }
        }

        /// <summary>
        /// 根据模型ID创建模型
        /// </summary>
        /// <param name="strModelID"></param>
        /// <param name="delegateComplete"></param>
        /// <param name="nWeaponLev"></param>
        /// <param name="bExclusive"></param>
        /// <param name="tf"></param>
        /// <param name="ignoreComponents"></param>
        /// <returns></returns>
        public Model createModel(string strModelID,  //模型id
            PlayerDelegate.onPlayerEvent delegateComplete,  //创建完成回调
            Vector3 birthPos,       //出生位置
            int nWeaponLev = 1,  //武器等级
            bool bExclusive = false,  //是否专属
            params string[] ignoreComponents //忽略的组件名
            )
        {
            Model m = new Model(strModelID, strModelID, birthPos, nWeaponLev, bExclusive, delegateComplete);
            m.setIgnoreComponents(ignoreComponents);
            m.load();
            _listWaitingCreateBuffer.Add(m);
            return m;
        }
        public Model createModel(int playerID,//playerid
            string strModelID,  //模型id
            PlayerDelegate.onPlayerEvent delegateComplete,  //创建完成回调
            Vector3 birthPos       //出生位置
            )
        {
            int nWeaponLev = 1;  //武器等级(已无用)
            Model m = new Model(playerID.ToString(), strModelID, birthPos, nWeaponLev, false, delegateComplete, playerID);
            m.load();
            _listWaitingCreateBuffer.Add(m);
            return m;
        }

        public void createModelTest(PlayerDelegate.onPlayerEvent delegateComplete)
        {
            Debug.Log("createModelTest");
            delegateComplete(null);
        }

        public void createModelTests(int a)
        {
            Debug.Log("createModelTest" + a.ToString());
        }

        //模型创建回调
        public void onModelCreateSuccessEvent(Model pModelObject)
        {
            GameObject pModelRoot = pModelObject.ModelRootObj;
            pModelRoot.layer = LayerMask.NameToLayer("UI");
            pModelObject.playAnimationsAndReset(new int[]{301, 302, 401, 402, 601, 701});
            pModelObject.setScale (0.6f);
            pModelObject.rotate (0.0f, 0.0f, 0.0f);

//             UnityEngine.Object objRes = Resources.Load("Effects/diaochan/gongji/gongji2");
//             if (objRes)
//             {
//                 Utils.LogSys.Log("Create Effect");
//                 GameObject _effectGameObj = UnityEngine.Object.Instantiate(objRes) as GameObject;
//             }

            EffectObject effect = EffectManager.getInstance().addEffect(pModelRoot.transform, "diaochan/gongji/gongji2");
            //effect._effectGameObj.transform.rotation = pModelRoot.transform.rotation;
        }

        //模型是否存在
        public bool isModelExist(int nModelID)
        {
            string strModelID = Convert.ToString(nModelID);
            /*
            ElementConfig cfg = (ElementConfig)ConfigDataMgr.getInstance().GetData(ConfigDataType.ModelElementConfig);
            ElementConfigItem modelData = null;// cfg.GetDataByKey(strModelID);
            if (modelData == null)
            {
                return false;
            }
            */
            return true;
        }


        //////////////////////////////////////////////////////////////////////////////

        #region MONO
        
        // 开始加载
        void Start()
        {
            //获取模型数据
            string assetPath = "GameModelData/GameModelData.asset";
            //assetPath = UtilTools.PathCheck(assetPath);
            _pAllModelData = AssetManager.getInstance().loadAsset<GameModelDataHolder>(assetPath);
        }

        public void LoadFinishedCallback(bool manual, TaskBase currentTask)
        {
            _pAllModelData = (GameModelDataHolder)((AssetBundleLoadTask)currentTask).getTargetAsset();
        }
        // Update 每帧调用一次
        void Update()
        {
            if (_pAllModelData == null)
            {
                return;
            }

            for (int i = _listWaitingCreateBuffer.Count - 1; i >= 0; i--)
            {
                if (_listWaitingCreateBuffer[i].isLoaded())
                {
                    _listWaitingCreateBuffer[i].createModel();
                    //onModelCreateEvent(_listWaitingCreateBuffer[i]);

                    _listWaitingCreateBuffer.RemoveAt(i);
                }
            }
        }

        #endregion
    }
}


