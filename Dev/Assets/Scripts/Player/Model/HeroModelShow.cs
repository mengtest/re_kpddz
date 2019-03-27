using UnityEngine;
using System.Collections;
using player;
using Utils;
using HeroData;
using MyExtensionMethod;
using sound;
using Scene;
using UI.Controller;

public class HeroModelShow : MonoBehaviour {
    //说话间隔
    readonly float TALK_INTERVAL = 10.0f;
    public float _fScale = 160f;

    DataHero _data;
    Model _3DModelCreating;
    Model _3DModel;

    public Model HeroModel { get { return _3DModel; } }

    public delegate void CreateModelCallBack(Model pModelObject);//创建成功回调
    public delegate void PressModelCallBack(HeroModelShow heroModelShow, bool begin);//创建成功回调
    public delegate void ClickModelCallBack(HeroModelShow heroModelShow);//点击回调
    public delegate void DragModelCallBack(Vector2 delta);//拖动回调
    CreateModelCallBack _callBack;
    PressModelCallBack _pressCallBack;
    public ClickModelCallBack ClickCallback;
    public DragModelCallBack dragCallBack;
    private bool _willPlayAnimation;
    private float _outLine;
    //点击是否有效
    public bool ClickValid { get; set; }
    public bool needAutoRenderQueue = true;
    bool bWeaponModel = false;

    //英雄说话
    bool _bHeroTalking = false;
    SoundEffect _soundHeroTalk = null;
    GameObject _listener = null;
    bool bMonoBeDestroyed = false;

	// 初始化
	void Awake () {
        ClickValid = true;
        ClickCallback += clickSelf;
        _listener = GameObject.Find("Listener");
	}
    /// <summary>
    /// 设置模型
    /// </summary>
    /// <param name="shaping">外形</param>
    /// <param name="callBack">回调</param>
    public void SetRole(string shaping, CreateModelCallBack callBack, float outLine = 0, bool playAnimation = true) {
//         HeroWeaponConfigItem item = ConfigDataMgr.getInstance().HeroWeaponCfg.GetDataByKey(shaping);
//         string modelId = "";
//         int equipLv = 1;
//         if (item != null) {
//             modelId = item.hero_id.ToString();
//             equipLv = item.weapon1_id;
//         } else {
//             modelId = shaping.Substring(0, shaping.IndexOf(","));
//         }
//         _callBack = callBack;
//         CreateRole(modelId, equipLv, outLine, playAnimation);
    }
    

    public void SetRole(DataHero data, CreateModelCallBack callBack, float outLine = 0, bool playAnimation = true) {
//         _data = data;
//         _callBack = callBack;
//         //设置角色界面技能图标
//         string modelId = _data.Config.view_id;
//         HeroWeaponConfigItem item = ConfigDataMgr.getInstance().GetHeroWeaponCfg(modelId, _data.Phase, _data.ExclusiveLv);
//         if (item == null)
//             return;
// 
//         int equipLv = _data.ExclusiveLv != 0 ? 2 : item.weapon1_id;
// 
//         CreateRole(modelId, equipLv, outLine, playAnimation);
    }

    /// <summary>
    /// 穿件隐去英雄，只显示武器的模型
    /// </summary>
    /// <param name="data"></param>
    /// <param name="callBack"></param>
    /// <param name="outLine"></param>
    public void ShowWeapen(DataHero data, CreateModelCallBack callBack, float outLine = 0) {
//         bWeaponModel = true;
//         string shaping = data.ViewId + ",1," + data.Phase;
//         SetRole(shaping, callBack, outLine, false);
    }

    public void SetPressCallBack(PressModelCallBack callback)
    {
        _pressCallBack = callback;
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="modelId"></param>
    /// <param name="equipLv"></param>
    /// <param name="playAnimation">穿件后是否播放动作</param>
    private void CreateRole(string modelId, int equipLv, float outLine = 0, bool playAnimation = true) {
        _willPlayAnimation = playAnimation;
        _outLine = outLine;
        if (_3DModel != null) {
            if (_3DModel.ModelID == modelId && _3DModel.WeaponLev == equipLv) {
                if (_3DModel.ModelRootObj.activeSelf && _willPlayAnimation) {
                    _3DModel.playAnimationsAndReset(new int[] { 701 });
                    if (_callBack != null) _callBack(_3DModel); 
                }
                return;
            }
            Destroy3DModel();
        }
        //LogSys.Log("creating@@@@@@@@@@@@@@@@@@@@@@@");
        //_isCreatingModel = true;
        _3DModelCreating = PlayerManager.getInstance().createModel(modelId, OnCreateModelCB, Vector3.zero, equipLv);
    }

    private void OnCreateModelCB(Model pModelObject) {
        //LogSys.Log("################");
        //_isCreatingModel = false;
        if (bMonoBeDestroyed)
        {
            //Utils.LogSys.LogError("######## OnCreateModelCB Mono be destroyed########");
            return;
        }
        DestroyOld3DModel();
        _3DModel = pModelObject;
        GameObject role = _3DModel.ModelRootObj;
        role.transform.parent = transform;
        role.layer = LayerMask.NameToLayer("UI");
        role.transform.localPosition = new Vector3(0, 0, 0);
        _3DModel.setScale(_fScale);
        if (_willPlayAnimation)
            _3DModel.playAnimationsAndReset(new int[] { 701 });

        setOutLine(_outLine);
        if (needAutoRenderQueue)
            UtilTools.SetModelRenderQueueByUIParent(transform, role.transform, 20);

        //显示模型，则设置英雄透明
        if (bWeaponModel) {
            setHeroTransparent();
        } 
        
        if (_callBack != null) _callBack(_3DModel);


        //英雄说话
        heroTalk(true);
    }

    public void setScale(float fScale) {
        if (_3DModel == null) return;
        _3DModel.setScale(fScale);
    }

    public void rotate(float x, float y, float z) {
        if (_3DModel == null) return;
        _3DModel.rotate(x, y, z);
    }

    public void Destroy3DModel() {
        if (_3DModelCreating != null)
        {
            _3DModelCreating.destroy();
            _3DModelCreating = null;
            _3DModel = null;
            Utils.LogSys.Log("HeroModelShow:---------------->Destroy3DModel");
        }
        else if (_3DModel != null)
        {
            Utils.LogSys.Log("HeroModelShow:---------------->Destroy3DModel->");
            _3DModel.destroy();
            _3DModel = null;
        }

        //停止说话
        if (_soundHeroTalk != null)
        {
            _soundHeroTalk.Stop();
            _soundHeroTalk = null;
        }
    }

    void DestroyOld3DModel()
    {
        if (_3DModel != null)
        {
            Utils.LogSys.Log("HeroModelShow:---------------->Destroy3DModel->");
            _3DModel.destroy();
            _3DModel = null;
        }

        //停止说话
        if (_soundHeroTalk != null)
        {
            _soundHeroTalk.Stop();
            _soundHeroTalk = null;
        }
    }

    public void HideRole() {
        if (_3DModel == null) return;
        _3DModel.ModelRootObj.SetActive(false);
    }

    public void ShowRole() {
        if (_3DModel == null) return;
        _3DModel.ModelRootObj.SetActive(true);
    }

    /// <summary>
    /// 英雄说话
    /// </summary>
    /// <param name="bCreate"></param>
    private void heroTalk(bool bCreate = false)
    {
        if (bCreate) _bHeroTalking = false;

        if (!_bHeroTalking)
        {
            //停止说话
            if (_soundHeroTalk != null)
            {
                _soundHeroTalk.Stop();
                _soundHeroTalk = null;
            }

            if (_data != null) {
//                uint soundRandom = (uint)Random.Range(1, 3);
                string strHeroWord = "Sounds/HeroWords/" + getSoundTalkName(_data);
//                LogSys.LogError("sound----<>"+strHeroWord);
                  _soundHeroTalk = UtilTools.PlaySoundEffect(strHeroWord, 10.0f, false, 1f, _listener);
                _bHeroTalking = true;

                Invoke("resetTalking", TALK_INTERVAL);
            }
        }
    }

    private void OnClick() {
        //英雄说话
        heroTalk();


        if (ClickCallback != null) ClickCallback(this);
    }

    private void clickSelf(HeroModelShow model){
        if (_3DModel == null)
            return;
        int[] actionArray = new int[] { 301, 302, 401, 402, 403, 410, 701 };
        int action = actionArray[Random.Range(0, actionArray.Length)];
        _3DModel.playAnimationsAndReset(new int[] { action });
    }

    private void OnDrag(Vector2 delta) {
        if (!ClickValid) return;
        if (_3DModel == null) return;
        float speed = 1f;
        UICamera.currentTouch.clickNotification = UICamera.ClickNotification.None;
        _3DModel.rotate(0f, -0.5f * delta.x * speed, 0f);

        if (dragCallBack != null) dragCallBack(delta); 
    }

    private void OnPress(bool begin)
    {
        if (_pressCallBack != null)
        {
            _pressCallBack(this, begin);
        }
    }

    public void playAnimationsAndReset(int[] actions){
        if (_3DModel == null) return;
        _3DModel.playAnimationsAndReset(actions);
    }

    public void playBreathAction() {
        if (_3DModel == null) return;
        _3DModel.playAnimationsAndReset(new int[] { 101 });
    }

    /// <summary>
    /// 设置模型描边
    /// </summary>
    /// <param name="width"></param>
    public void setOutLine(float width) {
        if (width == 0) return;
        
        if (_3DModel == null || _3DModel.ModelRootObj == null) return;
        var s = Shader.Find("Custom/Outline");
        _3DModel.setShader(s);

        var skinMeshs = _3DModel.ModelRootObj.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer skin in skinMeshs)
        {
            foreach (Material mat in skin.materials)
                mat.SetFloat("_Outline", width);
        }
    }

    /// <summary>
    /// 设置英雄透明
    /// </summary>
    public void setHeroTransparent() {
        if (_3DModel == null || _3DModel.ModelRootObj == null) return;
        SkinnedMeshRenderer skmr = _3DModel.ModelRootObj.GetComponent<SkinnedMeshRenderer>();
        Material mat = skmr.materials[0];
        Shader sd = Resources.Load("Shaders/LightShader") as Shader;
        mat.shader = sd;
    }

    public void ClickModel()
    {
        OnClick();
    }
    public void DragModel(Vector2 delta)
    {
        OnDrag(delta);
    }

    /// <summary>
    /// 重置说话状态
    /// </summary>
    void resetTalking()
    {
        _bHeroTalking = false;
    }

    void OnDestroy()
    {
        Destroy3DModel();
        bMonoBeDestroyed = true;
    }
    /// <summary>
    /// 获取声音文件的名字(判断现在应该播放哪一个)
    /// </summary>
    /// <param name="data"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private string getSoundTalkName(DataHero data)
    {
        return "";
//         uint first = 1;
//         uint secend = 2;
//         if (ClientDefine.heroSoundDic.ContainsKey(data.Id) ==false) {
//             ClientDefine.heroSoundDic[data.Id] = first;
//         }else {
//             uint id = ClientDefine.heroSoundDic[data.Id];
//             ClientDefine.heroSoundDic[data.Id] = (id == first ? secend : first);
//         }
//         uint type = ClientDefine.heroSoundDic[data.Id];
//         string soundName = data.Config.speak_1;
//         switch (type)
//         {
//             case 1:
//                 soundName = data.Config.speak_1;
//                 break;
//             case 2:
//                 soundName = data.Config.speak_2;
//                 break;
//         }
//         return soundName;
    }
}
