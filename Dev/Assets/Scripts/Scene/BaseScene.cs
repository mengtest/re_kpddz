/***************************************************************


 *
 *
 * Filename:  	BaseScene.cs	
 * Summary: 	场景基类,提供一些通用的场景操作
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/03/19 16:36
 ***************************************************************/

#region Using
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using network.protobuf;
using EventManager;
#endregion


#region typedef
using boolPersistent = System.Boolean;
using stringSceneName = System.String;
using UI.Controller;
#endregion


namespace Scene
{

    //场景基本组件
    public enum eBaseCompent{
        Cameras,
        UIRoot,
        Lights,
        World,
        DynamicObjects,
    };

    //场景目录结构
    public class SceneStructureData
    {
        //场景基本目录结构
        //<场景目录名字， 是否持久>
        public static Dictionary<stringSceneName, boolPersistent> s_BaseSceneStructure = new Dictionary<stringSceneName, boolPersistent>()
        {
            {"Cameras", false},
            {"Lights", false},
            {"World", false},
            {"DynamicObjects", false},
        };
    };

    /// <summary>
    /// 场景基类
    /// </summary>
    public class BaseScene : MonoBehaviour
    {
        public delegate void LoadUnitsComplete();
        public event LoadUnitsComplete EventLoadUnitsCompete = null;
        //场景ID
        private uint _idScene = 0;
        public Camera sceneCamera;
        public Camera nguiCamera;
        private GameObject clickTarget;
        //拖动
        [HideInInspector]
        public bool bCanDrag = false;

        [HideInInspector]
        public bool bDragDirReverse = false;//反向拖动

        [HideInInspector]
        public float fDragLeftLimit = -14f;

        [HideInInspector]
        public float fDragRightLimit = 14f;
        
        private bool isDraging = false;
        private Vector3 buttonDownPoint;
        private Vector3 sceneCameraPoint;
        
        //点物体
        [HideInInspector]
        public bool bCanClick = false;

        [System.NonSerialized]
        public float fDefaultCameraPos = 0.5f;
        protected float fCurCameraPos = 0.5f;
        private float fLastCameraPos = 0.5f;

        [HideInInspector]
        public MovePathMono movePath = null;
        protected GameObject _sceneCamera;
        //模糊控制
        //BlurEffect blur = null;//场景模糊组件
        private float _curBlurValue;
        ///////////////////////////////////////////////////////////////////
        public DragCameraByPath _dragPath = null;
        public DragCameraInRect _dragInRect = null;

        //点击效果
        static GameObject _clickEffect = null;

        bool _bPressed = false;
        bool _bDraging = false;
        Vector3 _pressedMousePos = Vector3.zero;
        Vector3 temp_pos = Vector3.zero;

        RaycastHit hit = new RaycastHit();
        public virtual void Start()
        {
            //场景开始重置时间
            Time.timeScale = 1.0f;

            fCurCameraPos = fDefaultCameraPos;
            addBaseComponent();
            //string curSceneName = GameSceneManager.sCurSenceName;
            GameObject camObj = transform.Find("Cameras/SceneCamera").gameObject;
            _sceneCamera = camObj;
            if (camObj)
            {
                sceneCamera = camObj.GetComponent<Camera>();
            }

            GameObject camObj2 = GameSceneManager.uiCameraObj;
            if (camObj2)
                nguiCamera = camObj2.GetComponent<Camera>();

        }

        public void SetSceneCamera(GameObject camObj)
        {
            _sceneCamera = camObj;
            if (camObj)
            {
                sceneCamera = camObj.GetComponent<Camera>();
            }
        }

        public virtual void OnDragScene(float value){
            fCurCameraPos = value;
        }

        public virtual void Update()
        {
            TimerManager.GetInstance().Trigger();
            MsgCallManager.Run();
            EventSystem.update();

            if (sceneCamera != null)
            {
                //点击波纹
                if (nguiCamera != null && Input.GetMouseButtonUp(0))
                {
                    Vector3 p = Input.mousePosition;
                    if (_clickEffect != null)
                    {
                        Ray uiRay = nguiCamera.ScreenPointToRay(Input.mousePosition);
                        RaycastHit uiHit = new RaycastHit();
                        if (Physics.Raycast(uiRay, out uiHit, 1000, 1 << LayerMask.NameToLayer("ValidViewArea")))
                        {
                            _clickEffect.layer = LayerMask.NameToLayer("UI");
                            _clickEffect.transform.position = uiHit.point;
                            _clickEffect.GetComponent<ParticleSystem>().Play();
                        }
                    }
                }
                if (bCanDrag || bCanClick)
                {
                    if (_bPressed)
                    {
                        Vector3 mousePose = Input.mousePosition;//cam.ScreenToWorldPoint(Input.mousePosition);//hitt.transform.position; //
                        float offsetX = mousePose.x - _pressedMousePos.x;
                        if (Mathf.Abs(offsetX) >= 10f)
                        {
                            _bDraging = true;
                        }
                    }
                    if (Input.GetMouseButtonDown(0))//点下去
                    {
                        _pressedMousePos = Input.mousePosition;
                        _bPressed = false;
                        _bDraging = false;
                        clickTarget = null;
                        if (!ClickUI()) //没点到UI
                        {
                            if (bCanClick)
                            {
                                /*if (UIManager.IsWinShow(UIName.BATTLE_SCENE_2D_WIN))
                                {
                                    clickTarget = ClickSeaHorizontal(out temp_pos);
                                }
                                else
                                {
                                    clickTarget = ClickScene(out temp_pos);
                                }*/
                                clickTarget = ClickScene(out temp_pos);
                                EventMultiArgs args = new EventMultiArgs();
                                if (clickTarget != null)//有点到东西
                                {
                                    args.AddArg("target", clickTarget);
                                    args.AddArg("hitPos", temp_pos);
                                }
                                args.AddArg("mousePos", _pressedMousePos);
                                EventSystem.CallEvent(EventID.PRESS_SCENE_TARGET, args, true);
                            }
                            _bPressed = true;
                        }
                    }
                    else if (Input.GetMouseButtonUp(0))//弹起
                    {
                        if (!bCanClick)
                        {
                            //不用处理点击事件
                        }
                        else if (clickTarget == null)
                        {
                            //没点到东西
                            EventSystem.CallEvent(EventID.PRESS_CANCEL_PRESS, new EventMultiArgs(), true);
                        }
                        else if (!bCanDrag && _bDraging)
                        {
                            //手指正在拖动，不用触发点击事件
                            EventSystem.CallEvent(EventID.PRESS_CANCEL_PRESS, new EventMultiArgs(), true);
                        }
                        else if (bCanDrag && _dragPath != null && _dragPath.IsDraged())
                        {
                            //正在拖动，不用触发点击事件
                        }
                        else if (bCanDrag && _dragInRect != null && _dragInRect.IsDraged())
                        {
                            //正在拖动，不用触发点击事件
                        }
                        else
                        {
                            EventMultiArgs args = new EventMultiArgs();
                            args.AddArg("target", clickTarget);
                            EventSystem.CallEvent(EventID.CLICK_SCENE_TARGET, args, true);
                        }
                        EventSystem.CallEvent(EventID.PRESS_REBOUND_PRESS, new EventMultiArgs(), true);
                        


                        clickTarget = null;
                        _bPressed = false;
                        _bDraging = false;
                    }
                    
                }
            }
           
        }

//         public void OnMove() {
//             if (clickTarget != null) {//取消按下事件， click事件无效
//                 GameObject nowTarget = ClickScene();
//                 if (nowTarget != clickTarget) {
//                     EventMultiArgs args = new EventMultiArgs();
//                     args.AddArg("target", clickTarget);
//                     EventSystem.CallEvent(EventID.PRESS_CANCEL_SCENE_TARGET, args, true);
//                     clickTarget = null;
//                 } 
//             }
//         }

        public MovePathMono AddCameraPath(string pathName, bool willResetPos = true, DragCameraByPath.EDragType eType = DragCameraByPath.EDragType.MOMENTUM, List<float> listPagePoint = null)
        {
            if (movePath != null && movePath.PathName == pathName) {
                return null;
            }
            GameObject camObj = _sceneCamera;
            if (camObj == null)
                return null;

            //movePath = camObj.GetComponent<MovePathMono>();
            //if (movePath != null)
            //    RemoveCameraPath();

            movePath = camObj.AddComponent<MovePathMono>();

            movePath.CreatePath(pathName);
            SetCameraDragble(movePath, fCurCameraPos, eType, listPagePoint);//给镜头拖动表现指定轨迹
            if (willResetPos) {
                sceneCamera.transform.position = movePath.GetPointAtTime(fCurCameraPos);
                movePath.UpdateRotation();
                OnDragScene(fCurCameraPos);
            }

            return movePath;
        }
        public MovePathMono AddCameraPath(string pathName, PathConfig config, DragCameraByPath.EDragType eType = DragCameraByPath.EDragType.MOMENTUM, List<float> listPagePoint = null)
        {
            GameObject camObj = _sceneCamera;
            if (camObj == null)
                return null;

            movePath = camObj.GetComponent<MovePathMono>();
            if (movePath != null)
                RemoveCameraPath();

            movePath = camObj.AddComponent<MovePathMono>();
            movePath.CreatePath(pathName, config);
            sceneCamera.transform.position = movePath.GetPointAtTime(fCurCameraPos);
            movePath.UpdateRotation();
            SetCameraDragble(movePath, fCurCameraPos, eType, listPagePoint);//给镜头拖动表现指定轨迹
            OnDragScene(fCurCameraPos);

            return movePath;
        }

        public void SetCameraDragble(MovePathMono _moveMono, float curPos, DragCameraByPath.EDragType eType = DragCameraByPath.EDragType.MOMENTUM, List<float> listPagePoint = null)
        {
            //_dragPath = _sceneCamera.GetComponent<DragCameraByPath>();
            if (_dragPath == null)
                _dragPath = _sceneCamera.AddComponent<DragCameraByPath>();
            _dragPath.SetMovePath(_moveMono);//给镜头拖动表现指定轨迹
            _dragPath._onDragEvent = OnDragScene;
            _dragPath._bDragDirReverse = bDragDirReverse;
            _dragPath._dragType = eType;
            _dragPath._pageViewPoints.Clear();
            _dragPath._curCameraPos = curPos;
            if (listPagePoint != null)
            {
                for (int i = 0; i < listPagePoint.Count; i++ )
                {
                    _dragPath._pageViewPoints.Add(listPagePoint[i]);
                }
            }
        }

        public void SetCameraDragInRect(Vector3 minPoint, Vector3 maxPoint, float fSpeed)
        {
            if (_dragInRect)
            {
                Destroy(_dragInRect);
                _dragInRect = null;
            }
            _dragInRect = GameSceneManager.sceneCameraObj.AddComponent<DragCameraInRect>();
            _dragInRect._minPoint = minPoint;
            _dragInRect._maxPoint = maxPoint;
            _dragInRect._dragSpeedVec = new Vector3(fSpeed, 0f, fSpeed);// fSpeed;
        }

        public void RemoveCameraPath()
        {
            GameObject camObj = GameSceneManager.sceneCameraObj;
            if (camObj != null && movePath != null)
            {
                Destroy(movePath);
                movePath = null;
            }
        }
        protected bool ClickUI()
        {
            if (nguiCamera == null)
                return false;
            
            Ray uiRay = nguiCamera.ScreenPointToRay(Input.mousePosition);//点到UI
            //RaycastHit uiHit = new RaycastHit();
            if (Physics.Raycast(uiRay, out hit, 1000, 1 << LayerMask.NameToLayer("UI"))) 
            //if (nguiCamera.isOrthoGraphic) 
                return true;

            return false;
        }

        public GameObject ClickScene(out Vector3 hitPos)
        {
            hitPos = Vector3.zero;
            if (sceneCamera == null)
                return null;

            //Vector3 pushPoint = Input.mousePosition;//hitt.transform.position;//cam.ScreenToWorldPoint(Input.mousePosition);//
            //Vector3 cameraPoint = sceneCamera.transform.position;
            Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("Default")))
            {
                hitPos = hit.point;
                return hit.transform.gameObject;
            }

            return null;
        }

        public GameObject ClickFish(out Vector3 hitPos)
        {
            hitPos = Vector3.zero;
            if (sceneCamera == null)
                return null;

            //Vector3 pushPoint = Input.mousePosition;//hitt.transform.position;//cam.ScreenToWorldPoint(Input.mousePosition);//
            //Vector3 cameraPoint = sceneCamera.transform.position;
            Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10000, LayerMask.NameToLayer("Fish")))
            {
                hitPos = hit.point;
                return hit.transform.gameObject;
            }

            return null;
        }

        public GameObject ClickSeaHorizontal(out Vector3 hitPos)
        {
            hitPos = Vector3.zero;
            if (nguiCamera == null)
                return null;

            //Vector3 pushPoint = Input.mousePosition;//hitt.transform.position;//cam.ScreenToWorldPoint(Input.mousePosition);//
            //Vector3 cameraPoint = sceneCamera.transform.position;
            Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10000, 1 << LayerMask.NameToLayer("SeaHorizontal")))
            {
                hitPos = hit.point;
                return hit.transform.gameObject;
            }

            return null;
        }

        ///////////////////////////////////////////////////////////////////


        //禁用默认构造函数
        public BaseScene() { }

        public BaseScene(uint idScene)
        {
            _idScene = idScene;
        }

        public uint id
        {
            get { return _idScene; }
        }

        /// <summary>
        /// 添加场景基础组件
        /// </summary>
        private void addBaseComponent()
        {
            Transform curScene = GameSceneManager.getInstance().CurSceneTransform;
            //创建目录结构
            foreach (KeyValuePair<stringSceneName, boolPersistent> kvp in SceneStructureData.s_BaseSceneStructure)
            {
                Transform pTransform = curScene.FindChild(kvp.Key);
                if (pTransform == null)
                {
                    GameObject pObj = new GameObject(kvp.Key);
                    pObj.transform.position = default(Vector3);
                    pObj.transform.parent = curScene;
                    if (kvp.Value)
                    {
                        DontDestroyOnLoad(pObj);
                    }
                }
                else
                {
                    if (kvp.Value)
                    {
                        DontDestroyOnLoad(pTransform.gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// 场景加载结束
        /// </summary>
        public virtual void loadComplete()
        {
            //Utils.LogSys.Log("BaseScene load complete");
        }

        protected virtual void OnDestroy()
        {
            clickTarget = null;
        }


        /// <summary>
        /// 开启场景模糊
        /// </summary>
        public void BlurOpen()
        {
            /*
            if (blur == null)
            {
                GameObject camObj = GameSceneManager.sceneCameraObj;
                if (camObj != null)
                {
                    _curBlurValue = 0.7f;
                    blur = camObj.AddComponent<BlurEffect>();
                    blur.iterations = 3;
                    blur.blurSpread = _curBlurValue;
                    blur.blurShader = Shader.Find("Hidden/BlurEffectConeTap");
                    UtilTools.ShowScreenshot();
                }
            }
             * */
        }
        public void OpenBlurOnUpdate(float value)
        {
            /*
            if (blur == null)
                return;

            _curBlurValue = value;
            blur.blurSpread = value;
            */
        }

        public void OpenBlurOnComplete()
        {
            UtilTools.ShowScreenshot();
        }

        /// <summary>
        /// 关闭场景模糊
        /// </summary>
        public void BlurClose()
        {
            /*
            GameObject camObj = GameSceneManager.sceneCameraObj;
            if (camObj == null)
                return;
            _curBlurValue = 0f;
            if (blur != null)
            {
                Destroy(blur);
                blur = null;
                return;
            }
             * */
        }

        /// <summary>
        /// 直接设置模糊度
        /// </summary>
        /// <param name="blurValue"></param>
        public void BlurValue(float blurValue)
        {
            /*
            BlurOpen();
            if (blur == null)
                return;
            blur.blurSpread = blurValue;
             */
        }

        //加载部件完成
        public virtual void LoadUnits()
        {
            if (EventLoadUnitsCompete != null)
                EventLoadUnitsCompete();
        }

        /// <summary>
        /// 模拟点击，新手引导中用
        /// </summary>
        /// <param name="go"></param>
        public static void VirtualClick(GameObject go) {
            EventMultiArgs args = new EventMultiArgs();
            args.AddArg("target", go);
            EventSystem.CallEvent(EventID.CLICK_SCENE_TARGET, args, true);
        }
    };

   
}