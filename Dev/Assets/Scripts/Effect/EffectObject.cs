using UnityEngine;
using System.Collections;
using Utils;
using asset;
using task;

namespace effect
{
    public enum EEffectBehaviour
    {
        eDefault,  //无
        eLineTrack, //直线轨迹
        eParabolaTrack, //抛物线轨迹
    }

    [SLua.CustomLuaClass]
    public class EffectObject
    {
        //光效回调
        public delegate void delegateEffect(EffectObject target);
        public event delegateEffect OnEffectEvent = null;
        public delegate void LoadComplete(EffectObject effect);
        public LoadComplete _loadComplete = null;
        public LoadComplete _removeComplete = null;
        public int param_int = 0;//外部存数据用,一般在_loadComplete回调里用.
        public Transform param_transform = null;//外部存数据用,一般在_loadComplete回调里用.
        public Vector3 param_vector3 = Vector3.zero;//外部存数据用,一般在_loadComplete回调里用.

        //光效销毁事件
        public event delegateEffect onEffectDestroy = null;

        //光效对象
        private GameObject _effectGameObj= null;

        //光效名字
        private string _effectName = "";
        private string _transformName = "";
        private bool _bSetLocalScale = false;
        private Vector3 _transformLocalScale;
        private bool _bSetLocalRotation = false;
        private Quaternion _transformLocalRotation;
        private bool _gameobjectActive = true;
        private float _autoDestroyDelay = -1f;//n秒后,直接删除
        //是否循环
        private bool _bIsLoop = false;

        //是否播放完自动销毁（单次特效）
        private bool _bIsAutoDestroy = true;//单次播完后自动删除

        //行为类型
        private EEffectBehaviour _eBehavourType = EEffectBehaviour.eDefault;

        //父亲
        Transform _tfParent;
        //目标
        Transform _tfTarget;
        //是否被停止
        bool _bStop;

        //是否已开始
        bool _bStart;
        //是否设置position
        bool _bPosition;
        Vector3 _vPosition = Vector3.zero;
        //是否设置localposition
        bool _bLocalPosition;
        Vector3 _vLocalPosition = Vector3.zero;
        //是否已销毁
        bool _bDestroy = false;
        //layer层
        int _iLayer = 0;
        //是否正在下载
        bool _loading = false;
        private int _offset = 20;
        private int _renderQueue = 0;

        //播放开始时间
        float _fPlayStartTime = 0.0f;
        float _fScale = 1f;
        float _fSpeed = 1f;
        ///////////////////////////////////////////////////////////////////////////////////

        public EffectObject(string strEffectName, bool bAutoDestroy = true, EEffectBehaviour eBehaviour = EEffectBehaviour.eDefault)
        {
            _eBehavourType = eBehaviour;
            _effectName = strEffectName;
            _bIsAutoDestroy = bAutoDestroy;

            EffectManager.getInstance().addList(this);

//            string strEffectPath = EffectManager.getEffectFilePath(_effectName);
//             if (strEffectName.Contains("Effects/")) {
//                 strEffectPath = _effectName.Substring(0, _effectName.LastIndexOf('.'));
//             }

            if (!strEffectName.Contains("Effects/") && !strEffectName.Contains("effect/"))//“Effects/”下才是光效
            {
                return;
            }

            //if (AssetManager.getInstance().IsFirstUseStreamingAssets)
            if (AssetManager.getInstance().IsStreamingAssets(_effectName))
            {
                _loading = true;
                //加载StreamingAssets下资源
                _effectName = UtilTools.PathCheck(_effectName);
                AssetBundleLoadTask task = new AssetBundleLoadTask(_effectName);
                task.EventFinished += (manual, currentTask) =>
                {
                    _loading = false;
                    AssetManager.getInstance().minusAssetbundleRefCount(_effectName);
                    if (_bDestroy)
                    {
                        return;
                    }
                    Object assetObj = ((AssetBundleLoadTask)currentTask).getTargetAsset();
                    if (assetObj == null) return;

                    _effectGameObj = UnityEngine.Object.Instantiate(assetObj) as GameObject;
                    if (_effectGameObj == null)
                    {
                        //Utils.LogSys.Log("Effect game object is NULL");
                        return;
                    }
                    UtilTools.UpdateShaders(_effectGameObj);
                    InitEffect();
                    if (_loadComplete != null)
                        _loadComplete(this);
                };
            }
            else
            {
                /*
                string strEffectPath = _effectName;// EffectManager.getEffectFilePath(_effectName);
                if (strEffectName.Contains("Effects/"))//“Effects/”下才是光效
                {
                    strEffectPath = _effectName.Substring(0, _effectName.LastIndexOf('.'));
                    if (strEffectPath.IndexOf("Resources/") == 0)//有“Resources/”就去掉
                    {
                        strEffectPath = strEffectPath.Substring(10);
                    }

                    UnityEngine.Object objRes = Resources.Load(strEffectPath);
                    if (objRes)
                    {
                        _effectGameObj = UnityEngine.Object.Instantiate(objRes) as GameObject;
                        if (_effectGameObj == null)
                        {
                            Utils.LogSys.Log("Effect game object is NULL");
                            return;
                        }
                        InitEffect();
                        if (_loadComplete != null)
                            _loadComplete(this);
                    }
                }*/
                _loading = true;
                AssetManager.getInstance().loadAssetAsync(_effectName,
                (bool manual, TaskBase currentTask) =>
                {
                    _loading = false;
                    AssetManager.getInstance().minusAssetbundleRefCount(_effectName);
                    if (_bDestroy)
                    {
                        return;
                    }
                    Object assetObj = AssetManager.getInstance().getAsset(_effectName);
                    if (assetObj == null) return;

                    _effectGameObj = UnityEngine.Object.Instantiate(assetObj) as GameObject;
                    if (_effectGameObj == null)
                    {
                        //Utils.LogSys.Log("Effect game object is NULL");
                        return;
                    }
                    UtilTools.UpdateShaders(_effectGameObj);
                    InitEffect();
                    if (_loadComplete != null)
                        _loadComplete(this);
                });
            }

        }

        void InitEffect()
        {
            if (!string.IsNullOrEmpty(_transformName))
            {
                _effectGameObj.name = _transformName;
            }
            if (_tfParent != null)
            {
                setParent(_tfParent);
            }
            else {

                _autoDestroyDelay = 0.5f;
            }
            if (_bSetLocalRotation)
                _effectGameObj.transform.localRotation = _transformLocalRotation;
            if (_bPosition)
            {
                setPosition(_vPosition);
            }
            else if (_bLocalPosition)
            {
                setLocalPosition(_vLocalPosition);
            }
            if (_bIsLoop)
            {
                Loop = _bIsLoop;
            }
            if (_eBehavourType != EEffectBehaviour.eDefault)
            {
                setEffectBehaviourType(_eBehavourType);
            }
            if (_tfTarget != null)
            {
                setTarget(_tfTarget);
            }
            if (_iLayer != 0)
            {
                setLayer(_iLayer);
            }
            if(_fScale != 1)
            {
                SetScale(_fScale);
            }
            else if (_bSetLocalScale)
            {
                _effectGameObj.transform.localScale = _transformLocalScale;
            }
            if(_fSpeed != 1)
            {
                SetSpeed(_fSpeed);
            }
            if (_bStop || !_bStart)
            {
                disablePlayOnAwake(false);
            }
            else
            {
                play();
            }

            if (_autoDestroyDelay > 0)
            {
                _effectGameObj.AddComponent<AutoDestroy>().Duration = _autoDestroyDelay;
            }
            _effectGameObj.SetActive(_gameobjectActive);

        }

        //是否自动销毁
        public bool isAutoDestroy()
        {
            return _bIsAutoDestroy;
        }

        public float AutoDestroyDelay
        {
            set
            {
                _autoDestroyDelay = value;
            }
        }

        //设置循环
        public bool Loop
        {
            get { return _bIsLoop;  }
            set 
            { 
                _bIsLoop = value;

                if (_effectGameObj == null)
                {
                    return;
                }

                ParticleSystem[] arr = _effectGameObj.GetComponentsInChildren<ParticleSystem>(true);
                foreach (var ps in arr)
                {
                    ps.loop = value;
                }
            }
        }

        public string TransformName
        {
            set {
                _transformName = value;
            }
        }
        
        
        public bool GameobjectActive
        {
            set
            {
                _gameobjectActive = value;
            }
        }

        public int Offset
        {
            set { _offset = value; }
        }

        //停止播放
        public void stop()
        {
            _bStart = false;
            _bStop = true;
            if (_effectGameObj == null)
            {
                return;
            }

            ParticleSystem[] arr = _effectGameObj.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in arr)
            {
                ps.Stop(true);
            }
        }

        //初始不播放，等待调用start播放
        public void disablePlayOnAwake(bool bValue)
        {
            if (_effectGameObj == null)
            {
                return;
            }

            ParticleSystem[] arr = _effectGameObj.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in arr)
            {
                ps.Pause(true);
            }
        }

        //设置光效行为类型
        public void setEffectBehaviourType(EEffectBehaviour eType)
        {
            _eBehavourType = eType;
            if (_effectGameObj == null)
            {
                return;
            }

            EffectDefaultBehaviour pBhScript = null;

            switch (eType)
            {
                case EEffectBehaviour.eLineTrack:
                    pBhScript = _effectGameObj.AddComponent<EffectLineBehaviour>();
                    break;
                case EEffectBehaviour.eParabolaTrack:
                    pBhScript = _effectGameObj.AddComponent<EffectDefaultBehaviour>();
                    break;
                default:
                    pBhScript = _effectGameObj.AddComponent<EffectDefaultBehaviour>();
                    break;
            }
        }

        //设置目标
        public void setTarget(Transform tf)
        {
            _tfTarget = tf;
            if (_effectGameObj == null)
            {
                return;
            }

            EffectDefaultBehaviour pBhScript = _effectGameObj.GetComponent<EffectDefaultBehaviour>();
            if (pBhScript != null)
            {
                pBhScript.setTarget(tf);
            }

        }

        //设置父亲
        public void setParent(Transform tf)
        {
            _tfParent = tf;
            if (_effectGameObj != null && tf != null)
            {
                _effectGameObj.transform.parent = tf;
                _effectGameObj.transform.rotation = tf.rotation;
                _effectGameObj.transform.localPosition = Vector3.zero;

                // 改变renderqueue
                UIPanel[] panels = _effectGameObj.GetComponentsInParent<UIPanel>();
                foreach (var p in panels)
                {
                    if (p.startingRenderQueue > _renderQueue)
                    {
                        _renderQueue = p.startingRenderQueue;
                    }
                }
                if (_renderQueue == 0)
                    _renderQueue = 3000;
                _renderQueue += _offset;
                ParticleSystem[] arr = _effectGameObj.GetComponentsInChildren<ParticleSystem>(true);
                foreach (var ps in arr)
                {
                    ps.GetComponent<Renderer>().material.renderQueue = _renderQueue;
                }
                MeshRenderer[] arr_mesh = _effectGameObj.GetComponentsInChildren<MeshRenderer>(true);
                foreach (var ps in arr_mesh)
                {
                    ps.GetComponent<Renderer>().material.renderQueue = _renderQueue-1;
                }
                //Utils.LogSys.Log("create effect success: " + _effectName);
            }
            else if (_effectGameObj != null &&  tf == null)
            {
                //Utils.LogSys.Log("create effect failed: " + _effectName);
                _autoDestroyDelay = 0.5f;
                _effectGameObj.transform.parent = tf;
            }
        }

        public void SetScale(float fScale)
        {
            _fScale = fScale;
            if (_effectGameObj == null) return;
            Vector3 oldScale = _effectGameObj.transform.localScale;
            _effectGameObj.transform.localScale = oldScale * fScale;

            ParticleSystem[] arr = _effectGameObj.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in arr)
            {
                ps.startSize = ps.startSize * fScale;
                //ps.velocityOverLifetime = 
                if (ps.shape.enabled == true && ps.shape.shapeType == ParticleSystemShapeType.Box)
                {
                    var sh = ps.shape;
                    sh.box = sh.box * fScale;
                }
                else if (ps.shape.enabled == true && ps.shape.shapeType == ParticleSystemShapeType.Mesh)
                {
                    ps.transform.localScale *= fScale;
                }
                else if (ps.shape.enabled == true && ps.shape.shapeType == ParticleSystemShapeType.Sphere)
                {
                    var sh = ps.shape;
                    sh.radius = sh.radius * fScale;
                }
//                 if (ps.velocityOverLifetime.enabled == true)
//                 {
//                     var lifeTime = ps.velocityOverLifetime;
//                     if (lifeTime.x.mode == ParticleSystemCurveMode.Constant )
//                     {
//                         var lifeTime_x = lifeTime.x;
//                         lifeTime_x.constantMax *= fScale;
//                         lifeTime_x.constantMin *= fScale;
//                         lifeTime_x.curveScalar *= fScale;
//                     }
//                     if (lifeTime.y.mode == ParticleSystemCurveMode.Constant )
//                     {
//                         var lifeTime_y = lifeTime.y;
//                         lifeTime_y.constantMax *= fScale;
//                         lifeTime_y.constantMin *= fScale;
//                         lifeTime_y.curveScalar *= fScale;
//                     }
//                     if (lifeTime.z.mode == ParticleSystemCurveMode.Constant)
//                     {
//                         var lifeTime_z = lifeTime.z;
//                         lifeTime_z.constantMax *= fScale;
//                         lifeTime_z.constantMin *= fScale;
//                         lifeTime_z.curveScalar *= fScale;
//                     }
//                 }
            }

        }
        public void SetSpeed(float fSpeed)
        {
            _fSpeed = fSpeed;
            if (_effectGameObj == null) return;
            ParticleSystem[] arr = _effectGameObj.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in arr)
            {
                ps.startSpeed = ps.startSpeed * fSpeed;
            }
        }

        //设置位置
        public void setPosition(Vector3 pos)
        {
            _bPosition = true;
            _bLocalPosition = false;
            _vPosition = pos;
            if (_effectGameObj != null)
            {
                _effectGameObj.transform.position = pos;
            }
        }

        /// <summary>
        /// 设置localposition
        /// </summary>
        /// <param name="pos"></param>
        public void setLocalPosition(Vector3 pos)
        {
            _bPosition = false;
            _bLocalPosition = true;
            _vLocalPosition = pos;
            if (_effectGameObj != null) {
                _effectGameObj.transform.localPosition = pos;
            } 
            
        }

        public void setLocalRotation(Quaternion rotation)
        {
            _bSetLocalRotation = true;
            _transformLocalRotation = rotation;
        }
        
        public void setLocalScale(Vector3 scale)
        {
            _bSetLocalScale = true;
            _transformLocalScale = scale;
        }

        

        //获取光效游戏对象
        public GameObject EffectGameObj
        {
            get { return _effectGameObj; }
        }

        //是否播放完成
        public bool isStopped()
        {
            if (_effectGameObj == null)
            {
                if (_loading)
                {
                    return false;
                }
                return true;
            }

            if (_bIsLoop) return false;


            bool bStop = true;

            //粒子
            ParticleSystem[] arr = _effectGameObj.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in arr)
            {
                float fTime = ps.time;
                bool bTimeout = (Time.time - _fPlayStartTime) > ps.duration;
                bStop = bStop && (ps.isStopped || bTimeout);
            }

            return bStop;
        }

        //播放
        public void play()
        {
            _bStart = true;
            _bStop = false;
            if (_effectGameObj == null)
            {
                return; 
            }

            ParticleSystem[] arr = _effectGameObj.GetComponentsInChildren<ParticleSystem>(true);
            foreach (var ps in arr)
            {
                ps.Play(true);
            }

            _fPlayStartTime = Time.time;
        }

        //销毁
        public void destroy()
        {
            _bDestroy = true;
            if (_removeComplete != null)
            {
                _removeComplete(this);
            }
            _removeComplete = null;
            _loadComplete = null;

            if (_effectGameObj != null)
                Object.DestroyObject(_effectGameObj);
            _effectGameObj = null;
            if (onEffectDestroy != null)
            {
                onEffectDestroy(this);
                onEffectDestroy = null;
            }

            OnEffectEvent = null;
            onEffectDestroy = null;


            //EffectManager.getInstance().removeFromList(this);
            //Resources.UnloadUnusedAssets();
            //AssetManager.getInstance().UnloadUnusedResourcesAssets();
        }

        //设置层级
        public void setLayer(int nLayerMask) 
        {
            _iLayer = nLayerMask;
            if (_effectGameObj == null)
                return;
            Transform[] arr = _effectGameObj.GetComponentsInChildren<Transform>(true);
            foreach (var tf in arr) {
                tf.gameObject.layer = nLayerMask;
            }
        }
    }
}



