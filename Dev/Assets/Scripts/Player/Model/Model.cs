/***************************************************************


 *
 *
 * Filename:  	Model.cs
 * Summary: 	游戏人物模型创建
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/04/15 17:09
 ***************************************************************/

using Object = UnityEngine.Object;

#region Using

using UnityEngine;
using System.Collections.Generic;
using BasicDataScructs;
using System;
using task;

#endregion Using

#region Typedef

using AnimEventsList = System.Collections.Generic.List<BasicDataScructs.AnimEventInfo>;
using asset;
using customerPath;

#endregion Typedef

namespace player
{
    //模型数据
    public class Model
    {
        //模型名字
        private string _strModelName = default(string);

        //模型索引
        private string _strModelID = default(string);

        //唯一id
        public int _playerID = 0;

        public int _pathID = 0;

        public int _birthTime = 0;//出生的时间戳

        public int _attackActionTime = 0;//被攻击时间
        private string _strFBXName = default(string);

        //模型静态数据
        private GameModelData _pObjModelData = null;

        //组件列表
        private List<ModelElement> _listElement = new List<ModelElement>();

        //动作列表
        private List<string> _listAnimations = new List<string>();

        //绑点列表
        private List<string> _listBindPoints = new List<string>();

        //动画事件列表
        private AnimEventsList _listAnimEvent = new AnimEventsList();

        //武器等级 && 是否专属
        private int _nWeaponLev = 1;

        private bool _bExclusive = false;

        //组件未完成列表
        private List<string> _listUncomplete = new List<string>();

        //基础部件是否加载完成
        private bool _bIsBaseLoaded = false;

        //模型
        private GameObject _pModelRoot = null;

        //创建完成回调
        private PlayerDelegate.onPlayerEvent _onCreateComplete = null;

        //初始位置
        private Vector3 _tfDefaultPos = Vector3.zero;

        //动作速度控制
        private Dictionary<string, float> _dictAnimationSpeed = new Dictionary<string, float>();

        //忽略部件列表
        List<string> _listIgnoreComponents = new List<string>();

        //默认休闲动作201
        int _idAnimatIdle = 201;

        BothCondition _bothLoadComplete = new BothCondition();
        AssetBundleLoadTask _baseAssetbundleLoadTask = null;

        //绑点查找辅助列表
        Dictionary<string, Transform> _dictAuxBps = new Dictionary<string, Transform>();

        List<Material> materials = null;

        //动作暂停中
        bool _animtionPause = false;

        public bool _bDead = false;
        public bool _bFrozon = false;//是否被冻住(技能将被打断)
        public int _renderqueque = 2000;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 休闲动作
        /// </summary>
        public int AnimatIdle
        {
            get { return _idAnimatIdle; }
            set { _idAnimatIdle = value; }
        }

        public Vector3 _birthPos = Vector3.zero;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //构造函数
        public Model(string strModelName, string strModelID, Vector3 bithPos, int nWeaponLev = 1, bool bExclusive = false, PlayerDelegate.onPlayerEvent delegateComplete = null, int playerID=0)
        {
            _strModelName = strModelName;
            _strModelID = strModelID;
            _strFBXName = "shape" + _strModelID;

            _nWeaponLev = Mathf.Clamp(nWeaponLev, 1, 3);
            _bExclusive = bExclusive;
            _onCreateComplete = delegateComplete;
            _tfDefaultPos = bithPos;
            _playerID = playerID;
            //load();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 组件加载完成回调
        /// </summary>
        public void elementLoadComplete(string strElementName)
        {
            _listUncomplete.Remove(strElementName);

            if (_listUncomplete.Count <= 0)
            {
                _bothLoadComplete.ConditionOK(1, bothLoadComplete);
                //createModel();
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 加载基础部件
        /// </summary>
        public void loadBaseElement()
        {
            onLoadBaseElementComplete();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 加载基础部件完成
        /// </summary>
        public void onLoadBaseElementComplete()
        {
            string strUrl = createBaseURL();

            _baseAssetbundleLoadTask = new AssetBundleLoadTask(strUrl);
            _baseAssetbundleLoadTask.EventFinished += new task.TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
            {
                _bothLoadComplete.ConditionOK(2, bothLoadComplete);
            });

            /*
			FileStream fs = new FileStream(Application.dataPath + "/" + strUrl, FileMode.Open, FileAccess.Read);
			if (fs != null)
			{
				byte[] content = new byte[fs.Length];
				fs.Read(content, 0, content.Length);
				fs.Close();

				AssetBundle baseAdb = AssetBundle.CreateFromMemoryImmediate(content);

				GameObject objBase = baseAdb.mainAsset as GameObject;

				//基础对象
				_pModelRoot = (GameObject)UnityEngine.Object.Instantiate(objBase);
				_pModelRoot.name = _strModelName;

                //删除assetbundle镜像
                baseAdb.Unload(false);
				_bIsBaseLoaded = true;
			}
             * */
        }

        private void bothLoadComplete()
        {
            if (_onCreateComplete == null)
            {
                Utils.LogSys.Log("创建模型:---------------->_onCreateComplete == null" + _baseAssetbundleLoadTask._taskName);
                return;
            }
            //Utils.LogSys.Log("创建模型:---------------->bothLoadComplete" + _baseAssetbundleLoadTask._taskName);
            AssetBundle assetbundle = _baseAssetbundleLoadTask.getTargetAssetbundle();
            if (assetbundle == null)
            {
                Utils.LogSys.LogError("创建模型失败：" + _baseAssetbundleLoadTask._taskName);
                return;
			}
			UnityEngine.Object assetObj = (Object)assetbundle.LoadAsset<GameObject>("base.prefab");//assetbundle.mainAsset;
            if (assetObj != null)
            {
                GameObject objBase = assetObj as GameObject;
                _baseAssetbundleLoadTask.unloadUnusedAssetbundle(false);

                //基础对象
                _pModelRoot = (GameObject)UnityEngine.Object.Instantiate(objBase, _tfDefaultPos, Quaternion.identity);

                _pModelRoot.name = _strModelName;
                _bIsBaseLoaded = true;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 模型是否加载完成
        /// </summary>
        /// <returns></returns>
        public bool isLoaded()
        {
            if (_listUncomplete.Count <= 0 && _bIsBaseLoaded)
            {
                return true;
            }

            return false;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 创建模型
        /// </summary>
        public void createModel()
        {
            if (_pModelRoot == null)
            {
                return;
            }

            //合并网格
            List<CombineInstance> combineInstances = new List<CombineInstance>();
            List<Transform> bones = new List<Transform>();
            materials = new List<Material>();
            Bounds localBounds = new Bounds(new Vector3(0, 1.0f, 0), new Vector3(2, 2, 2));

            Transform[] transforms = _pModelRoot.GetComponentsInChildren<Transform>();

            if (true)
            {
                foreach (ModelElement element in _listElement)
                {
                    if (element == null)
                        continue;

                    SkinnedMeshRenderer smr = element.getSkinnedMeshRender();
                    if (smr == null)
                        continue;
                   
                    materials.AddRange(smr.materials);

                    //保存主体部件的AABB盒
                    if (element.isMainBody())
                    {
                        //localBounds = smr.bounds;
                    }

                    //合并网格
                    for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
                    {
                        CombineInstance ci = new CombineInstance();
                        ci.mesh = smr.sharedMesh;
                        ci.subMeshIndex = sub;
                        combineInstances.Add(ci);
                    }

                    // As the SkinnedMeshRenders are stored in assetbundles that do not
                    // contain their bones (those are stored in the characterbase assetbundles)
                    // we need to collect references to the bones we are using
                    foreach (string bone in element.getBones())
                    {
                        foreach (Transform transform in transforms)
                        {
                            if (transform.name != bone) continue;
                            bones.Add(transform);
                            break;
                        }
                    }

                    UnityEngine.Object.Destroy(smr.gameObject);
                }

                SkinnedMeshRenderer skmr = _pModelRoot.GetComponent<SkinnedMeshRenderer>();
                skmr.sharedMesh = new Mesh();
                skmr.sharedMesh.CombineMeshes(combineInstances.ToArray(), false, false);
                skmr.bones = bones.ToArray();
                skmr.materials = materials.ToArray();
                skmr.localBounds = localBounds;
            }
                
            //AABB盒设置和element_body的相同
            //武器不包含在内
            //if (!localBounds.size.Equals(new Vector3(-1, -1, -1)))
            //{
            //    skmr.localBounds =  localBounds;
            //}

            rotate(0, 180, 0);

            //保存绑点信息（查找优化）
            List<string> _listBps = new List<string>() { "bp_head", "bp_l_shoulder", "bp_r_shoulder", "bp_body", "bp_l_hand", "bp_r_hand", "bp_l_foot", "bp_r_foot", "bp_root", "bp_primary_weapon", "bp_sub_weapon", "bp_overhead" };
            foreach (Transform tf in _pModelRoot.GetComponentsInChildren<Transform>())
            {
                if (_listBps.Contains(tf.name))
                {
                    _dictAuxBps[tf.name] = tf;
                }
            }

            //添加模型对象行为控制脚本
            ModelObjectBehaviour csModelObjBhv = _pModelRoot.AddComponent<ModelObjectBehaviour>();
            if (csModelObjBhv != null)
            {
                csModelObjBhv._objModel = this;
            }

            //完成回调
            if (_onCreateComplete != null)
            {
                _onCreateComplete(this);
            }
            else
            {
                Utils.LogSys.Log("创建模型:---------------->destroy");
                destroy();
                return;
            }

            //武器专属特效
            if (_nWeaponLev > 1)
            {
                //双武器
                if (_dictAuxBps.ContainsKey("bp_primary_weapon") && _dictAuxBps.ContainsKey("bp_sub_weapon"))
                {
//                     GameObject objPrim = Resources.Load("Effects/juesewuqi/" + _strModelID + "_wuqi01") as GameObject;
//                     GameObject objSub = Resources.Load("Effects/juesewuqi/" + _strModelID + "_wuqi02") as GameObject;
                    GameObject objPrim = asset.AssetManager.getInstance().loadAsset<GameObject>("Effects/juesewuqi/" + _strModelID + "_wuqi01.prefab");//同步加载资源
                    GameObject objSub = asset.AssetManager.getInstance().loadAsset<GameObject>("Effects/juesewuqi/" + _strModelID + "_wuqi02.prefab");//同步加载资源
                    if (objPrim != null && objSub != null)
                    {
                        var effectPrim = GameObject.Instantiate<GameObject>(objPrim);
                        if (effectPrim != null)
                        {
                            effectPrim.transform.parent = _dictAuxBps["bp_primary_weapon"];
                            UtilTools.SetLayerRecursive(effectPrim.transform.gameObject, _pModelRoot.layer);
                            effectPrim.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                            effectPrim.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                            effectPrim.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                            
                        var effectSub = GameObject.Instantiate<GameObject>(objSub);
                        if (effectSub != null)
                        {
                            effectSub.transform.parent = _dictAuxBps["bp_sub_weapon"];
                            UtilTools.SetLayerRecursive(effectSub.transform.gameObject, _pModelRoot.layer);
                            effectSub.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                            effectSub.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                            effectSub.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                    }

                    //GameObject __objPrim = Resources.Load("Effects/juesewuqi/" + _strModelID + "_wuqi") as GameObject;
                    GameObject __objPrim = asset.AssetManager.getInstance().loadAsset<GameObject>("Effects/juesewuqi/" + _strModelID + "_wuqi.prefab");//同步加载资源
                    if (__objPrim == null)
                        __objPrim = objPrim;

                    if (__objPrim != null)
                    {
                        var __effectPrim = GameObject.Instantiate<GameObject>(__objPrim);
                        if (__effectPrim != null)
                        {
                            __effectPrim.transform.parent = _dictAuxBps["bp_primary_weapon"];
                            UtilTools.SetLayerRecursive(__effectPrim.transform.gameObject, _pModelRoot.layer);
                            __effectPrim.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                            __effectPrim.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                            __effectPrim.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }

                        var __effectSub = GameObject.Instantiate<GameObject>(__objPrim);
                        if (__effectSub != null)
                        {
                            __effectSub.transform.parent = _dictAuxBps["bp_sub_weapon"];
                            UtilTools.SetLayerRecursive(__effectSub.transform.gameObject, _pModelRoot.layer);
                            __effectSub.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                            __effectSub.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                            __effectSub.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                    }
                }
                else if (_dictAuxBps.ContainsKey("bp_primary_weapon") && !_dictAuxBps.ContainsKey("bp_sub_weapon"))
                {
                    //GameObject objPrim = Resources.Load("Effects/juesewuqi/" + _strModelID + "_wuqi") as GameObject;
                    GameObject objPrim = asset.AssetManager.getInstance().loadAsset<GameObject>("Effects/juesewuqi/" + _strModelID + "_wuqi.prefab");//同步加载资源
                    if (objPrim != null)
                    {
                        var effectPrim = GameObject.Instantiate<GameObject>(objPrim);
                        if (effectPrim != null)
                        {
                            effectPrim.transform.parent = _dictAuxBps["bp_primary_weapon"];
                            UtilTools.SetLayerRecursive(effectPrim.transform.gameObject, _pModelRoot.layer);
                            effectPrim.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                            effectPrim.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
                            effectPrim.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        }
                    }
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 获取模型对象
        /// </summary>
        public GameObject ModelRootObj
        {
            get { return _pModelRoot; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 模型静态数据
        /// </summary>
        public GameModelData ModelData
        {
            get { return _pObjModelData; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 等级转换为模型或者贴图索引
        /// 不同等级对应不同的贴图和武器外形
        /// </summary>
        /// <returns></returns>
        public string levToWeaponAndMaterialIdx()
        {
            return Convert.ToString(_nWeaponLev);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 生成BASE组件URL
        /// </summary>
        /// <returns></returns>
        public string createBaseURL()
        {
            //return "Resources/modelsassetbundles/" + _strModelID + "/base.assetbundle";
            //string platformName = IPath.getPlatformName();
            //return platformName + "/modelsassetbundles/" + _strModelID + "/base";
            return "resources/modelsassetbundles/" + _strModelID + "/base.prefab";
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 加载
        /// </summary>
        public void load()
        {
            if (_strModelID.Length == 0)
            {
                return;
            }

            _pObjModelData = PlayerManager.ModelData.getGameModelData(_strModelID);
            if (_pObjModelData == null)
            {
                //Utils.LogSys.LogWarning("Model config data is missed. " + _strModelID);
                return;
            }

            //获取数据
            _listBindPoints = _pObjModelData._listModelBps;
            _listAnimEvent = _pObjModelData._listAnimEvents;
            _listAnimations = _pObjModelData._listAnimations;

            //获取模型部件列表
            //模型 = 部件加载 + 部件组装 + 材质
            
            ElementConfig cfg = (ElementConfig)ConfigDataMgr.getInstance().GetData(ConfigDataType.ModelElementConfig);
            ElementConfigItem modelData = cfg.GetDataByKey(_strModelID);
            if (modelData == null)
            {
                Utils.LogSys.LogWarning(" ElementConfigItem is null " + _strModelID);
            }

            string strLev = levToWeaponAndMaterialIdx();
            List<string> listTemp = new List<string>();

            for (int i = 0; i < modelData.getKeys().Count; i++)
            {
                var strKey = modelData.getKeys()[i];
                if (strKey == "key") continue;
                if (!(bool)modelData[strKey]) continue;
                var strComponent = strKey.ToLower().Contains("weapon") && strLev != "" ? strKey + "_" + strLev : strKey;

                //忽略部件不创建
                if (!_listIgnoreComponents.Contains(strComponent))
                {
                    _listUncomplete.Add(strComponent);
                    listTemp.Add(strComponent);
                }
            }
            for (int i = 0; i < listTemp.Count; i++)
            {
                string strKey = listTemp[i];
                _listElement.Add(new ModelElement(strKey, strKey, _strModelID, this));
            }
            

            loadBaseElement();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 销毁接口
        /// </summary>
        public void destroy()
        {
            if (materials != null)
            {
                for (int i = 0; i < materials.Count; i++)
                {
                    Object.Destroy(materials[i]);
                }
                materials.Clear();
            }
            _listElement.Clear();
            _listAnimations = null;
            _listBindPoints = null;
            _listAnimEvent = null;
            _nWeaponLev = 1;
            _bExclusive = false;
            _listUncomplete.Clear();
            _bIsBaseLoaded = false;
            _onCreateComplete = null;

            if (_pModelRoot != null)
            {
                SkinnedMeshRenderer skmr = _pModelRoot.GetComponent<SkinnedMeshRenderer>();
                if (skmr != null && skmr.sharedMesh != null)
                {
                    Object.Destroy(skmr.sharedMesh);
                    skmr.sharedMesh = null;
                }
                if (skmr != null && skmr.materials != null)
                {
                    int count = skmr.materials.Length;
                    for (int i = 0; i < count; i++)
                    {
                        Object.Destroy(skmr.materials[i]);
                    }
                }
                Object.Destroy(_pModelRoot);
                _pModelRoot = null;
            }


            //卸载资源
            if (_baseAssetbundleLoadTask != null) _baseAssetbundleLoadTask.unloadUnusedAssetbundle(true);
            foreach (var abTask in _listElement)
            {
                abTask.unloadAssetBundle();
            }

            //Resources.UnloadUnusedAssets();
            //AssetManager.getInstance().UnloadUnusedResourcesAssets();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 模型ID
        /// </summary>
        public string ModelID
        {
            get { return _strModelID; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 模型FBX文件名
        /// </summary>
        public string FBXName
        {
            get { return _strFBXName; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 武器品质
        /// </summary>
        public int WeaponLev
        {
            get { return _nWeaponLev; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 是否专属
        /// </summary>
        public bool IsExclusive
        {
            get { return _bExclusive; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 设置缩放
        /// </summary>
        /// <param name="fScale"></param>
        public void setScale(float fScale = 1.0f)
        {
            if (_pModelRoot != null)
            {
                Vector3 vecPreScale = _pModelRoot.transform.localScale;
                _pModelRoot.transform.localScale = new Vector3(fScale, fScale, fScale);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 设置缩放X轴
        /// </summary>
        /// <param name="fScale"></param>
        public void setScaleX(float fScale = 1.0f)
        {
            if (_pModelRoot != null)
            {
                Vector3 vecPreScale = _pModelRoot.transform.localScale;
                _pModelRoot.transform.localScale = new Vector3(fScale, vecPreScale.y, vecPreScale.z);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 设置缩放Y轴
        /// </summary>
        /// <param name="fScale"></param>
        public void setScaleY(float fScale = 1.0f)
        {
            if (_pModelRoot != null)
            {
                Vector3 vecPreScale = _pModelRoot.transform.localScale;
                _pModelRoot.transform.localScale = new Vector3(vecPreScale.x, fScale, vecPreScale.z);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="fAngleX"></param>
        /// <param name="fAngleY"></param>
        /// <param name="fAngleZ"></param>
        public void rotate(float fAngleX, float fAngleY, float fAngleZ)
        {
            if (_pModelRoot != null)
            {
                Quaternion quat = Quaternion.Euler(new Vector3(fAngleX, fAngleY, fAngleZ)) * _pModelRoot.transform.rotation;
                _pModelRoot.transform.rotation = quat;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="nAnimationId"></param>
        /// <param name="mode"></param>
        /// <param name="fSpeed"></param>
        public void playAnimation(int nAnimationId, WrapMode mode = WrapMode.Once, float fSpeed = 1.0f, float blendTime = 0.3f)
        {
            if (_pModelRoot == null) return;
            if (_animtionPause) return;

            string strAnimationID = Convert.ToString(nAnimationId);
            string strAnimationName = _strModelID + "_" + strAnimationID;

            if (_dictAnimationSpeed.ContainsKey(nAnimationId.ToString()))
                fSpeed = _dictAnimationSpeed[nAnimationId.ToString()];

            if (_listAnimations.Contains(strAnimationID))
            {
                AnimationState animState = _pModelRoot.GetComponent<Animation>()[strAnimationName];
                if (animState != null)
                {
                    animState.speed = fSpeed;
                    if (fSpeed < 0.0f) animState.normalizedTime = 1.0f;
                }

                _pModelRoot.GetComponent<Animation>().wrapMode = mode;
                _pModelRoot.GetComponent<Animation>().CrossFade(strAnimationName, blendTime);
            }
            else
            {
                //Utils.LogSys.LogWarning("不存在的动作: " + strAnimationID + " " + _strModelID);
            }
        }

        /// <summary>
        /// 休闲动作
        /// </summary>
        public void idle()
        {
            if (_animtionPause) return;
            playAnimation(_idAnimatIdle, WrapMode.Loop);
        }

        /// <summary>
        /// 暂停动作
        /// </summary>
        /// <param name="pause"></param>
        public void pauseAnimation(bool pause)
        {
            if (_pModelRoot == null) return;
            _animtionPause = pause;

            var anim = _pModelRoot.GetComponent<Animation>();
            if (anim == null) return;
           
            foreach (AnimationState state in anim)
            {
                if (pause)
                {
                    state.speed = 0.0f;
                }
                else
                {
                    float fSpeed = 1.0f;
                    string animId = state.name.Substring(state.name.LastIndexOf("_"));
                    if (_dictAnimationSpeed.ContainsKey(animId))
                        fSpeed = _dictAnimationSpeed[animId];
            
                    state.speed = fSpeed;
                }
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 播放动画，没有差值
        /// </summary>
        /// <param name="nAnimationId"></param>
        /// <param name="mode"></param>
        /// <param name="fSpeed"></param>
        public void playAnimationWithoutCrossFade(int nAnimationId, WrapMode mode = WrapMode.Once, float fSpeed = 1.0f)
        {
            if (_pModelRoot == null) return;
            if (_animtionPause) return;

            string strAnimationID = Convert.ToString(nAnimationId);
            string strAnimationName = _strModelID + "_" + strAnimationID;

            if (_dictAnimationSpeed.ContainsKey(nAnimationId.ToString()))
                fSpeed = _dictAnimationSpeed[nAnimationId.ToString()];

            if (_listAnimations.Contains(strAnimationID))
            {
                AnimationState animState = _pModelRoot.GetComponent<Animation>()[strAnimationName];
                if (animState != null)
                {
                    animState.speed = fSpeed;
                }

                _pModelRoot.GetComponent<Animation>().wrapMode = mode;
                _pModelRoot.GetComponent<Animation>().Stop();
                _pModelRoot.GetComponent<Animation>().Play(strAnimationName);
            }
            else
            {
                Utils.LogSys.LogWarning("不存在的动作: " + strAnimationID);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 停止动画
        /// </summary>
        public void stopAnimation()
        {
            if (_pModelRoot != null)
                _pModelRoot.GetComponent<Animation>().Stop();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 播放动画序列然后回到休闲动作
        /// </summary>
        /// <param name="arrAnimationId"></param>
        public void playAnimationsAndReset(int[] arrAnimationId, int resetAction = 0, float blendTime = 0.3f)
        {
            if (_pModelRoot == null) return;
            if (_animtionPause) return;
            if (resetAction == 0)
            {
                resetAction = AnimatIdle;
            }
            for (int i = 0; i < arrAnimationId.Length; i++)
            {
                string strAnimationID = Convert.ToString(arrAnimationId[i]);
                string strAnimationName = _strModelID + "_" + strAnimationID;

                if (_listAnimations.Contains(strAnimationID))
                {
                    //动作速度控制
                    if (_dictAnimationSpeed.ContainsKey(arrAnimationId[i].ToString()))
                    {
                        float fSpeed = _dictAnimationSpeed[arrAnimationId[i].ToString()];
                        AnimationState animState = _pModelRoot.GetComponent<Animation>()[strAnimationName];
                        if (animState != null)
                        {
                            animState.speed = fSpeed;
                        }
                    }

                    if (i == 0)
                    {
                        _pModelRoot.GetComponent<Animation>().wrapMode = WrapMode.Default;
                        if (Utils.LocalMathf.Approximately(0.3f, blendTime))
                        {
                            _pModelRoot.GetComponent<Animation>().Play(strAnimationName);// 此处不用CrossFade是因为会导致动作第一次之后就不能正常播放了
                        }
                        else
                        {
                            _pModelRoot.GetComponent<Animation>().CrossFade(strAnimationName, blendTime);
                        }
                    }
                    else
                    {
                        _pModelRoot.GetComponent<Animation>().PlayQueued(strAnimationName);
                    }
                }
                else
                {
#if UNITY_EDITOR
                    Utils.LogSys.LogWarning("不存在的动作: " + strAnimationID);
#endif
                }
            }

            if (_listAnimations.Contains(resetAction.ToString()))
            {
                string strIdleAnimation = _strModelID + "_" + resetAction.ToString();
                AnimationState aniState = _pModelRoot.GetComponent<Animation>().PlayQueued(strIdleAnimation);
                aniState.wrapMode = WrapMode.Loop;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 添加刚体
        /// </summary>
        /// <param name="bKinematic"></param>
        public void addRigidbody(bool bKinematic)
        {
            if (_pModelRoot == null)
            {
                return;
            }

            Rigidbody pPreRigidbodyComponent = _pModelRoot.GetComponent(typeof(Rigidbody)) as Rigidbody;
            if (pPreRigidbodyComponent != null)
            {
                return;
            }

            pPreRigidbodyComponent = _pModelRoot.AddComponent<Rigidbody>();
            pPreRigidbodyComponent.isKinematic = bKinematic;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 添加碰撞盒
        /// </summary>
        public void addBoxCollider()
        {
            if (_pModelRoot == null)
                return;

            BoxCollider pPreBoxColliderComponent = _pModelRoot.GetComponent(typeof(BoxCollider)) as BoxCollider;
            if (pPreBoxColliderComponent != null)
            {
                return;
            }

            pPreBoxColliderComponent = _pModelRoot.AddComponent<BoxCollider>();
            //pPreBoxColliderComponent.center = new Vector3(0.0f, 0.0f, 0.0f);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 添加NavMeshAgent寻路组件
        /// </summary>
        /// <param name="bObstacleAvoid"></param>
        public void addNavMeshAgent(bool bObstacleAvoid)
        {
            if (_pModelRoot == null)
            {
                return;
            }

            NavMeshAgent pPreNavMeshAgentComponent = _pModelRoot.GetComponent(typeof(NavMeshAgent)) as NavMeshAgent;
            if (pPreNavMeshAgentComponent != null)
            {
                return;
            }

            pPreNavMeshAgentComponent = _pModelRoot.AddComponent<NavMeshAgent>();
            pPreNavMeshAgentComponent.baseOffset = -0.04f;
            pPreNavMeshAgentComponent.radius = 1.0f;
            pPreNavMeshAgentComponent.height = 2.5f;
            pPreNavMeshAgentComponent.acceleration = 50.0f;
            pPreNavMeshAgentComponent.speed = 8.0f;
            pPreNavMeshAgentComponent.angularSpeed = 240.0f;
            pPreNavMeshAgentComponent.stoppingDistance = 2.0f;

            if (bObstacleAvoid)
            {
                pPreNavMeshAgentComponent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
            }
            else
            {
                pPreNavMeshAgentComponent.obstacleAvoidanceType = ObstacleAvoidanceType.MedQualityObstacleAvoidance;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 注册动作事件
        /// </summary>
        /// <param name="pHandle"></param>
        public void registerAnimationEvent(ModelObjectBehaviour.animationEventDelegate pHandle)
        {
            if (pHandle != null && _pModelRoot != null)
            {
                ModelObjectBehaviour csModelObjBhv = _pModelRoot.GetComponent<ModelObjectBehaviour>();
                if (csModelObjBhv != null)
                {
                    csModelObjBhv.onAnimationEvent = pHandle;
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 获取绑点
        /// </summary>
        public Transform getBp(string strName)
        {
            if (_pModelRoot == null) return null;
            if (!_pModelRoot.activeSelf) return _pModelRoot.transform;

            //先查找辅助表
            if (_dictAuxBps.ContainsKey(strName)) return _dictAuxBps[strName];


            foreach (Transform tf in _pModelRoot.GetComponentsInChildren<Transform>())
            {
                if (tf.name == strName)
                {
                    return tf;
                }
            }

#if UNITY_EDITOR
            if (!string.IsNullOrEmpty(strName))
            {
                Utils.LogSys.LogWarning(string.Format("绑点未找到 绑点：{0} 模型：{1}", strName, _strFBXName));
            }
#endif
            return _pModelRoot.transform;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 设置动作速度
        /// </summary>
        public void setAnimationSpeed(int idAnim, float fSpeed)
        {
            _dictAnimationSpeed[idAnim.ToString()] = fSpeed;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 设置shader
        /// </summary>
        /// <param name="rhsShader"></param>
        public void setShader(Shader rhsShader)
        {
            if (_pModelRoot == null || rhsShader == null) return;

            foreach (SkinnedMeshRenderer skin in _pModelRoot.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                foreach (Material mat in skin.materials)
                {
                    mat.shader = rhsShader;
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 获取动作事件
        /// </summary>
        /// <returns></returns>
        public List<ModelAnimationEventData> getAnimEvents(int idAnimation)
        {
            var list = new List<ModelAnimationEventData>();
            if (_pModelRoot == null) return list;
            var mbh = _pModelRoot.GetComponent<ModelObjectBehaviour>();
            if (mbh == null) return list;
            for (int i = 0; i < mbh.AnimEvents.Length; i++)
            {
                var v = mbh.AnimEvents[i];
                if (v.idAnimation != idAnimation) continue;
                //根据动作速度做缩放
                float fScale = 1.0f;
                if (_dictAnimationSpeed.ContainsKey(v.idAnimation.ToString()))
                    fScale = _dictAnimationSpeed[v.idAnimation.ToString()];

                //var isChange = _dictAnimationSpeed.TryGetValue(v.idAnimation, out fScale) && (fScale < 0.00000001f);
                //fScale = isChange ? 0.00000001f : 1f;
                v.fTime /= fScale;
                list.Add(v);
            }

            return list;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 设置忽略部件
        /// </summary>
        /// <param name="components"></param>
        public void setIgnoreComponents(string[] components)
        {
            foreach (var c in components)
            {
                _listIgnoreComponents.Add(c);
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 设置动作裁剪模式
        /// </summary>
        /// <param name="animCullType"></param>
        public void setAnimationCullingType(AnimationCullingType animCullType)
        {
            if (_pModelRoot != null)
                _pModelRoot.GetComponent<Animation>().cullingType = animCullType;
        }
    }
}