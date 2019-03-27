using UnityEngine;
using asset;
//using HeroData;
using player;
using task;

public class ModelToUITexture : MonoBehaviour
{

    private int AntiAliasing = 1;
    private UITexture uiTexture;
    private RenderTexture mTex = null; //即为显示用的RenderTexutre
    private Material mat = null;
    private GameObject HeroModelBox;
    private HeroModelShow _heroModel;//3D模型
    private ModelRotationDirection _heroModelFloor = null;//英雄模型底座控制脚本
    private GameObject TaiZi;
    private ModelRotationDirection taiziRotation;
    private GameObject pCamera;
    static int nCount = 1;//用来控制每个模型的偏移位置 。
    Transform child;
    private HeroModelShow.CreateModelCallBack _callBack;
    private bool bInited = false;
    public bool bNeedTaiZi = false;
    // Use this for initialization
    private Vector3 cameraPos = new Vector3(0f, 2.66f, -5.5f);
    private CameraAjustor cameraAjustor = null;
    private Quaternion cameraRotation = Quaternion.Euler(18.2f, 0f, 0f);

    public Model HeroModel
    {
        get { return _heroModel.HeroModel; }
    }
    public HeroModelShow ModelShow {
        get { return _heroModel; }
    }

    public GameObject CameraObj
    {
        get { return pCamera; }
    }
    public GameObject ModelBoxObj
    {
        get { return HeroModelBox; }
    }
    public CameraAjustor CameraAjustor
    {
        get { return cameraAjustor; }
    }

    void Init()
    {
        if (bInited)
            return;

        bInited = true;
        Vector2 screen;
        if (Screen.width > Screen.height)
            screen = new Vector2(Screen.width, Screen.height);
        else
            screen = new Vector2(Screen.height, Screen.width);
        //创建renderTexture给镜头用
        uiTexture = gameObject.GetComponent<UITexture>();

        mTex = new RenderTexture((int)screen.x, (int)screen.y, 16);//16或24
        //mTex = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, RenderTextureFormat.ARGB32);
        mTex.name = transform.name + GetInstanceID();
        mTex.anisoLevel = AntiAliasing;
        Utils.LogSys.Log("ModelToUITexture: RenderTexure format:" + mTex.format.ToString());
        //创建材质球给uiTexture用。
        Shader shader = Shader.Find("Unlit/Transparent Colored");//Shader.Find("Custom/FuckOutline");//
        mat = new Material(shader);
        uiTexture.material = mat;

        //创建放模型和镜头的父结点
        HeroModelBox = new GameObject("HeroModelBox");
        HeroModelBox.transform.parent = gameObject.transform;
        HeroModelBox.transform.localPosition = new Vector3((float)nCount * 100000f, 0f, 0f);
        if (nCount > 100)
            nCount = 1;
        else
            nCount++;

        //创建镜头
        pCamera = new GameObject("ModelCamera");
        Camera mCamera = pCamera.AddComponent<Camera>();
        mCamera.clearFlags = CameraClearFlags.SolidColor;// CameraClearFlags.Depth;//
        mCamera.backgroundColor = new Color(0f,0f,0f,0f);
        mCamera.cullingMask = (1 << LayerMask.NameToLayer("UI"));
        mCamera.targetTexture = mTex;
        pCamera.transform.parent = HeroModelBox.transform;
        pCamera.transform.localPosition = cameraPos;
        pCamera.transform.localRotation = cameraRotation;
        //cameraAjustor = pCamera.AddComponent<CameraAjustor>();

        float adjust_width = (float)uiTexture.height * screen.x / screen.y;
        uiTexture.width = (int)adjust_width;
        //创建底座
        TaiZi = new GameObject();// (GameObject)Instantiate(taizi);
        TaiZi.transform.name = "TaiZi";
        TaiZi.transform.parent = HeroModelBox.transform;
        TaiZi.transform.localPosition = Vector3.zero;

        //创建模型管理器
        _heroModel = HeroModelBox.AddComponent<HeroModelShow>();
        _heroModel.ClickCallback = onClickModel;
        _heroModel.dragCallBack = onDragModel;
        _heroModel._fScale = 0.92f;
        _heroModel.needAutoRenderQueue = false;
    }

    public void LoadFinishedCallback(bool manual, TaskBase currentTask)
    {
        Object taizi = (Object)((AssetBundleLoadTask)currentTask).getTargetAsset();
        GameObject new_taizi = (GameObject)Instantiate(taizi);
        new_taizi.layer = LayerMask.NameToLayer("UI");
        new_taizi.transform.parent = TaiZi.transform;
        InitTaiZi(new_taizi.transform);
        UtilTools.UpdateShaders(new_taizi);
    }

    private void InitTaiZi(Transform trans)
    {
        float taiziScale = 1.1f;
        trans.localPosition = new Vector3(0f, 0f, 0f);
        trans.localScale = new Vector3(taiziScale, taiziScale, taiziScale * 0.64f);
        trans.rotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));

    }

    // Update is called once per frame
    void Update()
    {
        if (mat != null)
            mat.mainTexture = mTex;
    }
    private void onDragModel(Vector2 delta)
    {
        if (_heroModelFloor != null)
        {
            _heroModelFloor.Rotate(0.5f * delta.x);
        }
    }

    private void onClickModel(HeroModelShow heroModelShow)
    {
        Init();
        _heroModel.playAnimationsAndReset(new int[] { 701 });
    }
    public void SetModel(string shaping,
        HeroModelShow.CreateModelCallBack callBack,
        Vector3 camera_pos,
        Quaternion camera_rotation,
        bool need_taizi = false,
        float outLine = 0, 
        bool playAnimation = true)
    {
        cameraPos = camera_pos;
        cameraRotation = camera_rotation;
        bNeedTaiZi = need_taizi;
        Init();
        _callBack = callBack;
        if (_heroModel == null)
        {
            //创建模型管理器
            _heroModel = HeroModelBox.AddComponent<HeroModelShow>();
            _heroModel.ClickCallback = onClickModel;
            _heroModel.dragCallBack = onDragModel;
            _heroModel._fScale = 0.92f;
            _heroModel.needAutoRenderQueue = false;
        }
        _heroModel.SetRole(shaping, callBack, outLine, playAnimation);
    }
    /*
    public void SetRole(DataHero data,
        HeroModelShow.CreateModelCallBack callBack,
        Vector3 camera_pos,
        Quaternion camera_rotation,
        bool need_taizi = false,
        float outLine = 0,
        bool playAnimation = true)
    {
        cameraPos = camera_pos;
        cameraRotation = camera_rotation;
        bNeedTaiZi = need_taizi;
        Init();
        _callBack = callBack;
        if (_heroModel == null)
        {
            //创建模型管理器
            _heroModel = HeroModelBox.AddComponent<HeroModelShow>();
            _heroModel.ClickCallback = onClickModel;
            _heroModel.dragCallBack = onDragModel;
            _heroModel._fScale = 0.92f;
            _heroModel.needAutoRenderQueue = false;
        }
        _heroModel.SetRole(data, CreateModelCB, outLine, playAnimation);
    
    }*/
    private void CreateModelCB(Model pModelObject) {
        pModelObject.ModelRootObj.transform.parent = TaiZi.transform;
        if (_callBack != null) {
            _callBack(pModelObject);
        }
    }

    public void Destroy3DModel()
    {
        Init();
        if (_heroModel != null)
        {
            _heroModel.Destroy3DModel();
            _heroModel = null;
        }
//         if (mat != null && mat.mainTexture != null)
//         {
//             Destroy(mat.mainTexture);
//             mat.mainTexture = null;
//         }
//         if (mat != null)
//         {
//             Destroy(mat);
//             mat = null;
//         }
        if (mTex != null)
        {
            Destroy(mTex);
            mTex = null;
        }
    }

    void OnDestroy()
    {
        Destroy3DModel();
    }

    /// <summary>
    /// 庆祝动作
    /// </summary>
    public void RoleCelebrate()
    {
        Init();
        _heroModel.playAnimationsAndReset(new int[] { 701 });
    }


    private void OnDrag(Vector2 delta)
    {
        Init();
        _heroModel.DragModel(delta);
    }

    private void ClickModel()
    {
        Init();
        _heroModel.ClickModel();
    }

    public void Rotate(float delta)
    {
        Init();
        if (taiziRotation != null)
            taiziRotation.Rotate(delta);
        else if(_heroModel != null)
            _heroModel.rotate(0, -delta, 0);
    }
}
