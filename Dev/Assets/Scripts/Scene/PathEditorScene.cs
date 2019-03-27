/***************************************************************


 *
 *
 * Filename:  	StartUpScene.cs
 * Summary: 	开始场景控制脚本
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/03/21 2:26
 ***************************************************************/

#region using

using asset;
using customerPath;
using network.protobuf;
using object_c;
using sdk;
using System.Collections;
using UI.Controller;
using UnityEngine;
using player;
#endregion using

namespace Scene
{
    //开始场景控制脚本
    public class PathEditorScene : BaseScene
    {
//        FishMoveByPath path;
        PathDrawMono mono;
        int pathid = 10000;
        string modelId = "11005";
        Model _3DModelCreating;
        int _startUpStep = 0;
        float _fScale = 100f;
        private bool _bStartUp = false;
        public static Transform _sceneCamera;
        public static Transform _fishCamera;
        private ApplicationMgr _applicationMgr;
        Transform tf_background;
        //加载
        private void Awake()
        {
            mono = GameObject.Find("World/Container").GetComponent<PathDrawMono>();
			Caching.CleanCache ();
            //FollowToSceneCamera follower = null;

            GameObject pSingletonObj = GameObject.Find("Singleton");
            if (pSingletonObj == null) {
                pSingletonObj = new GameObject("Singleton");
                pSingletonObj.transform.position = default(Vector3);
            }

            GameObject camObj1 = GameObject.Find("Scene/Cameras/SceneCamera");
            if (camObj1 && !camObj1.GetComponent<CameraAjustor>()) {
                camObj1.AddComponent<CameraAjustor>();
            }
            GameObject camObj2 = GameObject.Find("UIRoot/UICamera");
            if (camObj2 && !camObj2.GetComponent<CameraAjustor>()) {
                camObj2.SetActive(true);
                camObj2.AddComponent<CameraAjustor>();
            }
            GameSceneManager.uiCameraObj = camObj2;
//            BattleScene2DController.InitAdjustData();
        }
        public bool GetIsRun()
        {
            return _bStartUp;
        }
        void OnDestroy()
        {
            _bStartUp = false;
        }
        void Start()
        {

            _fishCamera = transform.Find("Cameras/FishCamera");
            _sceneCamera = transform.Find("Cameras/SceneCamera");
           AssetManager.getInstance().intialize();
           GameSceneManager.sceneCameraObj = _sceneCamera.gameObject;
            _bStartUp = true;
            PlayerManager.getInstance().init();
            FishPathDataMgr.getInstance().initialize();
            base.Start();
            _startUpStep = 1;
            Transform tr = transform.Find("World/Container/FishBox");
            tr.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            Transform tr_ptct = transform.Find("World/Container/ptct");
            tr_ptct.localRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
            if (_fishCamera != null)
            {
                Transform ct = transform.Find("World/Container");
                float z = ct.localPosition.z;
                float h = Mathf.Tan(3.1415926f / 4f) * z;
                _fishCamera.localPosition = new Vector3(0f, h, 0f);
                UIPanel panel = transform.Find("World/Container/Panel").GetComponent<UIPanel>();
                panel.renderQueue = UIPanel.RenderQueue.StartAt;
                panel.startingRenderQueue = 2000;
                tf_background = transform.Find("World/Container/Panel/Texture");
                tf_background.LookAt(_fishCamera);
            }
            
        }
        void OnCreateModelCB(Model pModelObject) {
            GameObject role = pModelObject.ModelRootObj;
            role.transform.parent = transform;
            Transform fishBox = transform.Find("World/Container/FishBox");
                role.transform.parent = fishBox;
            role.layer = LayerMask.NameToLayer("Default");
            role.transform.localPosition = new Vector3(0, 0, 0);
            pModelObject.setScale(FishPathDataMgr.BASE_SCALE);
            //         if (needAutoRenderQueue)
            //UtilTools.SetModelRenderQueueByUIParent(transform, role.transform, 20);
            /*path = role.AddComponent<FishMoveByPath>();
            if (mono == null)
            {
                path.fishPathID = 10000;
            }
            else
            {
                path.fishPathID = mono.selectPath;
            }
            pathid = path.fishPathID;*/
            UtilTools.SetModelRenderQueue(role.transform, 3000);
        }
        // Update 每帧调用一次
        override public void Update()
        {
            if (!_bStartUp)
                return;
            if (_startUpStep == 1 && AssetManager.getInstance().IsInitComplete())
            {
                ConfigDataMgr.getInstance().initialize();
                _startUpStep = 2;
                
            }
            else if (_startUpStep == 2)
            {
                ConfigDataMgr.getInstance().initialize();
                _startUpStep = 3;
            }
            else if (_startUpStep == 3 && ConfigDataMgr.getInstance().IsAllConfigLoaded())
            {
                if (mono != null)
                {
                    modelId = mono.modelId;
                }
                _3DModelCreating = PlayerManager.getInstance().createModel(modelId, OnCreateModelCB, Vector3.zero);
                _startUpStep = 4;
            }
            else if ((_startUpStep == 4 && mono != null && (pathid != mono.selectPath || modelId != mono.modelId)) )
            {
                FishPathDataMgr.getInstance().initialize();
                //path.fishPathID = mono.selectPath;
                pathid = mono.selectPath;
                modelId = mono.modelId;
                _3DModelCreating.destroy();
                _3DModelCreating = PlayerManager.getInstance().createModel(modelId, OnCreateModelCB, Vector3.zero);
                mono.isChangeModel = false;
            }
            else if (mono.isChangeModel && _startUpStep == 4)
            {
                FishPathDataMgr.getInstance().initialize();
                //path.fishPathID = mono.selectPath;
                pathid = mono.selectPath;
                modelId = mono.modelId;
                _3DModelCreating.destroy();
                _3DModelCreating = PlayerManager.getInstance().createModel(modelId, OnCreateModelCB, Vector3.zero);
                mono.isChangeModel = false;
            }
            base.Update();
        }

        /// <summary>
        /// 在版本更新完成后，才调用该函数，开启游戏初始化。
        /// step1:开始加载配置文件
        /// </summary>
        public void StartUp()
        {
            
            base.Start();
            
        }
    }
}
