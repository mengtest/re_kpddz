using UnityEngine;
using System.Collections;
using EventManager;

public class MovePathBase : System.Object
{

    public virtual void GetPointAtTime(float t, out Vector3 temp_vector)
    {
        temp_vector = Vector3.zero;
    }
    public virtual float GetLength(int pointCount = 20)
    {
        return 1f;
    }
    public virtual Vector3 GetLookLinePoint(Vector3 point)
    {
        return Vector3.zero;
    }
}


public class MovePathMono : MonoBehaviour
{
    public enum ECameraPathType
    {
        Straightline,//直线
        Bezier,//贝赛尔曲线
        Arcline,//圆弧线
        Circle,//圆
        CylinderSpirals,//圆柱曲线
    }
    public enum ELookAtType
    {
        None,//不会变镜头朝向
        LookAtPoint,//朝固定点看
        LookAtLine,//朝固定线条上最近点看
        LookAtPointReverse,//朝固定点(反方向)看
        LookAtLineReverse,//朝固定线条上最近点(反方向)看
        LookAtTwoPoints,//begin时看点1，end时看点2（酒馆用到）
    }
    public ECameraPathType ePathType = ECameraPathType.Bezier;
    private ECameraPathType ePathType_Editor = ECameraPathType.Bezier;

    //三个小球触摸对象
    public GameObject pointParam;
    public GameObject pointParam2;
    public GameObject pointBegin;
    public GameObject pointEnd;
    public Transform tfPathObj;//三小球的父结点

    public ELookAtType elookAtType = ELookAtType.None;
    private ELookAtType elookAtType_Editor = ELookAtType.None;
    public GameObject pointLookAt1;
    public GameObject pointLookAt2;
    public float Distance = 2;//轴方向前进距离(非0)编辑用
    public float PerCircleDistance = 0.1f;//每圈前进的距离(非0)编辑用
    public Vector3 PosLookAtOffset;//对所看的目标点做最终偏移,编辑用

    private string pathName = "path_name";
    private Vector3 posParam;
    private Vector3 posParam2;
    private Vector3 posBegin;
    private Vector3 posEnd;
    private Vector3 posLookAt1;
    private Vector3 posLookAt2;
    private Vector3 posLookAtOffset;
    private float distance = 10f;//轴方向前进距离(非0)计算用
    private float perCircleDistance = 1f;//每圈前进的距离(非0)计算用

    private Vector3 posLastCamera;

    public MovePathBase movePathBase;//生成的路径
    private bool bDoCameraAnimation = false;//是否在做镜头动画
    private float fAnimationTime = 0f;//动画时长
    private Quaternion beginRotation;//动画开始时镜头朝向
    private Quaternion finalRotation;//动画结束时镜头朝向
    private DelegateType.CameraAnimationCallback animationCallback;//动画回调
    private bool isDisabled = false;
    private bool isNeedToSave = false;
    GameObject cameraPathsEditor;

    DelegateType.MovePathCallback onAnimationComplete;
    DelegateType.MovePathCallback onAnimationUpdate;
    DelegateType.MovePathCallback onLookToComplete;
    bool isDoingLookTo = false;
    public float curAnimationValue;

    int iLookAtPoint = 1;//1:LookAtPoint1  2:LookAtPoint2

    public string PathName
    {
        get { return pathName; }
    }
    void Awake()
    {
        
    }
    void OnEnable()
    {
        UpdateEditPoints();
    }
    void UpdateEditPoints(bool updatePos = false)
    {
#if UNITY_EDITOR
        isDisabled = false;

        //创建用于编辑路径的结点
        cameraPathsEditor = GameObject.Find("cameraPathEditor");
        if (cameraPathsEditor == null)
            cameraPathsEditor = new GameObject("cameraPathEditor");

        GameObject curPathEditor;
        if (pathName != "path_name")
        {
            Transform tfPath = cameraPathsEditor.transform.FindChild(pathName);
            if (tfPathObj != null && tfPath != tfPathObj)
            {
                Destroy(tfPathObj.gameObject);
                tfPathObj = tfPath;
            }
        }

        if (tfPathObj == null)
            tfPathObj = cameraPathsEditor.transform.FindChild(pathName);
        if (tfPathObj == null)
            curPathEditor = new GameObject(pathName);
        else
            curPathEditor = tfPathObj.gameObject;

        tfPathObj = curPathEditor.transform;
        tfPathObj.gameObject.SetActive(true);
        curPathEditor.transform.parent = cameraPathsEditor.transform;
        if (curPathEditor.transform.FindChild("pointParam") != null)
        {
            pointParam = curPathEditor.transform.FindChild("pointParam").gameObject;
            pointParam2 = curPathEditor.transform.FindChild("pointParam2").gameObject;
            pointBegin = curPathEditor.transform.FindChild("pointBegin").gameObject;
            pointEnd = curPathEditor.transform.FindChild("pointEnd").gameObject;
            pointLookAt1 = curPathEditor.transform.FindChild("pointLookAt1").gameObject;
            pointLookAt2 = curPathEditor.transform.FindChild("pointLookAt2").gameObject;
            if (updatePos)
            {
            }
            return;
        }
        pointParam = new GameObject("pointParam");
        pointParam.SetActive(false);
        pointParam.transform.parent = curPathEditor.transform;
        pointParam2 = new GameObject("pointParam2");
        pointParam2.SetActive(false);
        pointParam2.transform.parent = curPathEditor.transform;
        pointBegin = new GameObject("pointBegin");
        pointBegin.SetActive(false);
        pointBegin.transform.parent = curPathEditor.transform;
        pointEnd = new GameObject("pointEnd");
        pointEnd.SetActive(false);
        pointEnd.transform.parent = curPathEditor.transform;
        pointLookAt1 = new GameObject("pointLookAt1");
        pointLookAt1.SetActive(false);
        pointLookAt1.transform.parent = curPathEditor.transform;
        pointLookAt2 = new GameObject("pointLookAt2");
        pointLookAt2.SetActive(false);
        pointLookAt2.transform.parent = curPathEditor.transform;

#endif
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        isDisabled = true;
        if (cameraPathsEditor == null)
            return;
        Transform tfPathEditor = cameraPathsEditor.transform.FindChild(pathName);
        if (tfPathEditor == null)
            return;
        tfPathEditor.gameObject.SetActive(false);
        //Destroy(tfPathEditor.gameObject);
#endif
    }

    public void CreatePathAnimation(float from, float to, float time, iTween.EaseType eType = iTween.EaseType.linear, DelegateType.MovePathCallback onComplete = null, DelegateType.MovePathCallback onUpdate = null)
    {
        if (to == from)
            return;
        
        Hashtable args = new Hashtable();
        args.Add("from", from);
        args.Add("to", to);
        args.Add("time", time);
        args.Add("easetype", eType);
        args.Add("onupdate", "AnimationOnUpdate");
        args.Add("onupdatetarget", gameObject);
        args.Add("oncomplete", "AnimationOnComplete");
        args.Add("oncompletetarget", gameObject);
        args.Add("ignoretimescale", false);
        iTween.ValueTo(gameObject, args);

        if (from <= 0.1f)
        {
            iLookAtPoint = 2;//ELookAtType.LookAtTwoPoints时才有用
        }
        else if (from >= 0.9f)
        {
            iLookAtPoint = 1;
        }

        onAnimationComplete = onComplete;
        onAnimationUpdate = onUpdate;
    }

    //LookAtTwoPoints专用
    public void LookToAnimation(bool bReverse, float time, iTween.EaseType eType = iTween.EaseType.linear, DelegateType.MovePathCallback onComplete = null)
    {
        if (elookAtType != ELookAtType.LookAtTwoPoints)
            return;

        Hashtable args = new Hashtable();
        args.Add("from", 0f);
        args.Add("to", 1f);
        args.Add("time", time);
        args.Add("easetype", eType);

        beginRotation = gameObject.transform.localRotation;
        if (!bReverse)
        {
            args.Add("onupdate", "LookToAnimationUpdate_ToPos2");
            args.Add("onupdatetarget", gameObject);
            finalRotation = Quaternion.LookRotation(posLookAt2 - posEnd);
        }
        else
        {
            args.Add("onupdate", "LookToAnimationUpdate");
            args.Add("onupdatetarget", gameObject);
            finalRotation = Quaternion.LookRotation(posLookAt1 - posBegin);
        }
        args.Add("oncomplete", "LookToAnimationComplete");
        args.Add("oncompletetarget", gameObject);
        args.Add("ignoretimescale", false);
        iTween.ValueTo(gameObject, args);
        onLookToComplete = onComplete;
        isDoingLookTo = true;
    }

    public void LookToAnimationUpdate_ToPos2(float value)
    {
        Quaternion rotation;
        Quaternion finalRotat = Quaternion.LookRotation(posLookAt2 - transform.position);
        rotation = Quaternion.Slerp(beginRotation, finalRotat, value);
        gameObject.transform.localRotation = rotation;
    }

    public void LookToAnimation(float time, iTween.EaseType eType = iTween.EaseType.linear, DelegateType.MovePathCallback onComplete = null)
    {
//         Hashtable args = new Hashtable();
//         args.Add("looktarget", targetPos);
//         args.Add("time", time);
//         args.Add("easetype", eType);
//         args.Add("oncomplete", "LookToAnimationComplete");
//         args.Add("oncompletetarget", gameObject);
//         iTween.LookTo(gameObject, args);

        Hashtable args = new Hashtable();
        args.Add("from", 0f);
        args.Add("to", 1f);
        args.Add("time", time);
        args.Add("easetype", eType);
        args.Add("onupdate", "LookToAnimationUpdate");
        args.Add("onupdatetarget", gameObject);
        args.Add("oncomplete", "LookToAnimationComplete");
        args.Add("oncompletetarget", gameObject);
        args.Add("ignoretimescale", false);
        iTween.ValueTo(gameObject, args);

        beginRotation = gameObject.transform.localRotation;
        finalRotation = Quaternion.LookRotation(posLookAt1 - posEnd);
        onLookToComplete = onComplete;
        isDoingLookTo = true;
    }

    public void LookToAnimationUpdate(float value)
    {
        Quaternion rotation;
        Quaternion finalRotat = Quaternion.LookRotation(posLookAt1 - transform.position);
        rotation = Quaternion.Slerp(beginRotation, finalRotat, value);
        gameObject.transform.localRotation = rotation;
    }

    public void LookToAnimationComplete()
    {
        if (onLookToComplete != null)
            onLookToComplete(1f);
        isDoingLookTo = false;

    }

    public void AnimationOnUpdate(float value)
    {
        if (enabled == false)
            return;
        
        curAnimationValue = value;
        Vector3 toPos = Vector3.zero;
        movePathBase.GetPointAtTime(value, out toPos);
        gameObject.transform.localPosition = toPos;
        UpdateRotation();
        if (onAnimationUpdate != null)
            onAnimationUpdate(value);
    }
    public void AnimationOnComplete()
    {
        if (enabled == false)
            return;

        if (onAnimationComplete != null)
            onAnimationComplete(1f);

        onAnimationComplete = null;
        onAnimationUpdate = null;
        curAnimationValue = 0f;
    }

    /// <summary>
    /// isStartFromCurPos:为true时表示创建的路径将以当前镜头位置为起点; 为false时表示完全按配置好的路径创建。
    /// </summary>
    /// <param name="pathName"></param>
    /// <param name="isStartFromCurPos"></param>
    public void CreatePathAnimation(string pathName, DelegateType.CameraAnimationCallback fCallback=null, bool isStartFromCurPos = true)
    {
        bDoCameraAnimation = true;
        CreatePath(pathName, isStartFromCurPos);
        float fLength = movePathBase.GetLength();
        if (fLength > 0f)
            fAnimationTime = fLength / 10f;

        beginRotation = gameObject.transform.localRotation;
        Vector3 offset = posLookAt1 - posEnd;
        finalRotation = Quaternion.LookRotation(offset);
        if (Mathf.Abs(finalRotation.x) < 0.1f && Mathf.Abs(finalRotation.y) < 0.1f && Mathf.Abs(finalRotation.z) < 0.1f)
            finalRotation.y = 0.1f;
        Utils.LogSys.Log(beginRotation.ToString() + finalRotation.ToString());
        Hashtable args = new Hashtable();
        args.Add("from", 0f); 
        args.Add("to", 1f);
        args.Add("time", fAnimationTime);
        args.Add("onupdate", "CameraAnimationOnUpdate");
        args.Add("onupdatetarget", gameObject);
        args.Add("oncomplete", "CameraAnimationOnComplete");
        args.Add("oncompletetarget", gameObject);
        args.Add("easetype", iTween.EaseType.easeInQuad);
        args.Add("ignoretimescale", false);
        iTween.ValueTo(gameObject, args);
        if (fCallback != null)
            animationCallback = fCallback;
        
    }

    public void CameraAnimationOnUpdate(float value)
    {
        Vector3 toPos = Vector3.zero;
        movePathBase.GetPointAtTime(value, out toPos);
        gameObject.transform.localPosition = toPos;
        Quaternion rotation;
        rotation = Quaternion.Slerp(beginRotation, finalRotation, value);
        gameObject.transform.localRotation = rotation;

    }
    public void CameraAnimationOnComplete()
    {
        bDoCameraAnimation = false;
        if (animationCallback != null)
            animationCallback(pathName);
    }

    /// <summary>
    /// isStartFromCurPos:为true时表示创建的路径将以当前镜头位置为起点; 为false时表示完全按配置好的路径创建。
    /// </summary>
    /// <param name="pathName"></param>
    /// <param name="isStartFromCurPos"></param>
    public void CreatePathAnimation(GameObject targetObj, GameObject camera)
    {
        Hashtable args = new Hashtable();
        Vector3 pos = targetObj.transform.position;
        args.Add("looktarget", pos);
        args.Add("axis", "y");
        args.Add("time", 0.6f);
        args.Add("easetype", iTween.EaseType.easeOutQuart);
        args.Add("oncomplete", "LookToComplete");
        args.Add("oncompletetarget", gameObject);
        args.Add("ignoretimescale", false);
        iTween.LookTo(camera, args);

        Vector3 posCam = camera.transform.position;
        Hashtable argsMove = new Hashtable();
        argsMove.Add("position", Vector3.Lerp(posCam, pos, 0.7f));
        argsMove.Add("time", 2f);
        //argsMove.Add("easetype", iTween.EaseType.easeInQuart);
        argsMove.Add("oncomplete", "MoveToComplete");
        argsMove.Add("oncompletetarget", gameObject);
        argsMove.Add("ignoretimescale", false);
        iTween.MoveTo(camera, argsMove);
    }

    void LookToComplete()
    {
//         Hashtable args = new Hashtable();
//         args.Add("from", 0f);
//         args.Add("to", 1f);
//         args.Add("time", 10f);
//         args.Add("onupdate", "CameraBlurOnUpdate");
//         args.Add("onupdatetarget", gameObject);
//         args.Add("easetype", iTween.EaseType.easeOutQuart);
//         iTween.ValueTo(gameObject, args);
    }
    void MoveToComplete()
    {
        Time.timeScale = 1f;
    }

    void CameraBlurOnUpdate(float value)
    {
       // UtilTools.SetMainCityBlur(value * 5f);
    }
    /// <summary>
    /// isStartFromCurPos:为true时表示创建的路径将以当前镜头位置为起点; 为false时表示完全按配置好的路径创建。
    /// </summary>
    /// <param name="name"></param>
    /// <param name="isStartFromCurPos"></param>
    public void CreatePath(string name, bool isStartFromCurPos=false)
    {
        PathConfig config = new PathConfig();
        if (!PathXMLMgr.getInstance().GetPath(name, ref config))
            return;

        CreatePath(name, config, isStartFromCurPos);
    }
     
    public void CreatePath(string name, PathConfig config, bool isStartFromCurPos=false)
    {
        pathName = name;
        UpdateEditPoints();
        int iType = config.pathType;
        int iLookAtType = config.lookAtType;
        if (iType == 0)
        {
            ePathType = ECameraPathType.Straightline;
        }
        else if (iType == 1)
        {
            posParam = config.pointParam1;
            if (isStartFromCurPos)
                posBegin = gameObject.transform.position;
            else
                posBegin = config.pointBegin;
            posEnd = config.pointEnd;
            movePathBase = new PathBezier(posBegin, posParam - posBegin, new Vector3(0f, 0f, 0f), posEnd);
            posLookAt1 = config.pointLookAt1;
            posLookAt2 = config.pointLookAt2;
            ePathType = ECameraPathType.Bezier;
//            Utils.LogSys.Log("CreatePath:" + name);
            //Utils.LogSys.Log("CreatePath posBegin：" + posBegin.ToString() + "posEnd:" + posEnd.ToString());
        }
        else if (iType == 2)
        {
            ePathType = ECameraPathType.Arcline;
        }
        else if (iType == 3)
        {
            ePathType = ECameraPathType.Circle;
        }
        else if (iType == 4)
        {
            posParam = config.pointParam1;
            posParam2 = config.pointParam2;
            if (isStartFromCurPos)
                posBegin = gameObject.transform.position;
            else
                posBegin = config.pointBegin;

            distance = config.distance;//轴方向前进距离(非0)
            perCircleDistance = config.perCircleDistance;//每圈前进的距离(非0)
            posLookAtOffset = config.pointLookAtOffset;//对所看的目标点做最终偏移
            if (Mathf.Abs(distance) > 0.01f)
                Distance = distance;
            if (Mathf.Abs(perCircleDistance) > 0.01f)
                PerCircleDistance = perCircleDistance;
            PosLookAtOffset = posLookAtOffset;
            movePathBase = new PathCylinderSpirals(posBegin, posParam, posParam2, Distance, PerCircleDistance);
//            Utils.LogSys.Log("CreatePath:" + name);
            ePathType = ECameraPathType.CylinderSpirals;
        }

        if (iLookAtType == 0)
        {
        }
        else if (iLookAtType == 1)
        {
            posLookAt1 = config.pointLookAt1;
            elookAtType = ELookAtType.LookAtPoint;
        }
        else if (iLookAtType == 2)
        {
            posLookAt1 = config.pointLookAt1;
            posLookAt2 = config.pointLookAt2;
            elookAtType = ELookAtType.LookAtLine;
        }
        else if (iLookAtType == 3)
        {
            posLookAt1 = config.pointLookAt1;
            elookAtType = ELookAtType.LookAtPointReverse;
        }
        else if (iLookAtType == 4)
        {
            posLookAt1 = config.pointLookAt1;
            posLookAt2 = config.pointLookAt2;
            elookAtType = ELookAtType.LookAtLineReverse;
        }
        else if (iLookAtType == 5)
        {
            posLookAt1 = config.pointLookAt1;
            posLookAt2 = config.pointLookAt2;
            elookAtType = ELookAtType.LookAtTwoPoints;
        }

        ePathType_Editor = ePathType;
        elookAtType_Editor = elookAtType;
        UpdatePoints(true);
    }

    private void UpdatePoints(bool bUpdatePos = false)
    {
#if UNITY_EDITOR
        //刷新路径的名字。
        if (tfPathObj != null && "path_name" == tfPathObj.name)
        {
            tfPathObj.name = pathName;
        }
        else if (tfPathObj != null && pathName == tfPathObj.name)
        {
        }
        else if (tfPathObj != null && "path_name" != tfPathObj.name)
        {
            Destroy(tfPathObj.gameObject);
            return;
        }

        //刷新对应的编辑结点
        pointParam.SetActive(false);
        pointParam2.SetActive(false);
        pointBegin.SetActive(false);
        pointEnd.SetActive(false);
        pointLookAt1.SetActive(false);
        pointLookAt2.SetActive(false);
        if (ePathType == ECameraPathType.Straightline)
        {
        }
        else if (ePathType == ECameraPathType.Bezier)
        {
            if (bUpdatePos)
            {
                pointParam.transform.position = posParam;
                pointBegin.transform.position = posBegin;
                pointEnd.transform.position = posEnd;
            }
            pointParam.SetActive(true);
            pointBegin.SetActive(true);
            pointEnd.SetActive(true);
        }
        else if (ePathType == ECameraPathType.Arcline)
        {
        }
        else if (ePathType == ECameraPathType.Circle)
        {
        }
        else if (ePathType == ECameraPathType.CylinderSpirals)
        {
            if (bUpdatePos)
            {
                pointParam.transform.position = posParam;
                pointParam2.transform.position = posParam2;
                pointBegin.transform.position = posBegin;
            }
            pointParam.SetActive(true);
            pointParam2.SetActive(true);
            pointBegin.SetActive(true);
        }

        if (elookAtType == ELookAtType.LookAtPoint)
        {
            if (bUpdatePos)
            {
                pointLookAt1.transform.position = posLookAt1;
            }
            pointLookAt1.SetActive(true);
        }
        else if (elookAtType == ELookAtType.LookAtLine)
        {
            if (bUpdatePos)
            {
                pointLookAt1.transform.position = posLookAt1;
                pointLookAt2.transform.position = posLookAt2;
            }
            pointLookAt1.SetActive(true);
            pointLookAt2.SetActive(true);
        }
        else if (elookAtType == ELookAtType.LookAtPointReverse)
        {
            if (bUpdatePos)
            {
                pointLookAt1.transform.position = posLookAt1;
            }
            pointLookAt1.SetActive(true);
        }
        else if (elookAtType == ELookAtType.LookAtLineReverse)
        {
            if (bUpdatePos)
            {
                pointLookAt1.transform.position = posLookAt1;
                pointLookAt2.transform.position = posLookAt2;
            }
            pointLookAt1.SetActive(true);
            pointLookAt2.SetActive(true);
        }
        else if (elookAtType == ELookAtType.LookAtTwoPoints)
        {
            if (bUpdatePos)
            {
                pointLookAt1.transform.position = posLookAt1;
                pointLookAt2.transform.position = posLookAt2;
            }
            pointLookAt1.SetActive(true);
            pointLookAt2.SetActive(true);
        }
#endif
    }

    /// <summary>
    /// fTime:0~1
    /// </summary>
    /// <param name="fTime"></param>
    public Vector3 GetPointAtTime(float fTime)
    {
        Vector3 point = Vector3.zero;
        movePathBase.GetPointAtTime(fTime, out point);
        return point;
    }

    // Update 每帧调用一次
	public void Update ()
    {
       UpdateConfigToXML();
	}

    public void UpdateRotation()
    {
        if (isDoingLookTo)
            return;

        //镜头移动时刷新朝向(直接看向目标)
        Vector3 change = posLastCamera - gameObject.transform.position;
        if (Mathf.Abs(change.x) >= 0.001f || Mathf.Abs(change.y) >= 0.001f || Mathf.Abs(change.z) >= 0.001f)
        {
            posLastCamera = gameObject.transform.position;
            if (elookAtType == ELookAtType.LookAtPoint)
            {
                Vector3 offset = posLookAt1 - posLastCamera;
                gameObject.transform.localRotation = Quaternion.LookRotation(offset);
            }
            else if (elookAtType == ELookAtType.LookAtLine)
            {
                Vector3 offset = GetLookLinePoint(posLastCamera) - posLastCamera;
                gameObject.transform.localRotation = Quaternion.LookRotation(offset);
            }
            else if (elookAtType == ELookAtType.LookAtPointReverse)
            {
                Vector3 offset = posLastCamera - posLookAt1;
                gameObject.transform.localRotation = Quaternion.LookRotation(offset);
            }
            else if (elookAtType == ELookAtType.LookAtLineReverse)
            {
                Vector3 offset = posLastCamera - GetLookLinePoint(posLastCamera);
                gameObject.transform.localRotation = Quaternion.LookRotation(offset);
            }
            else if (elookAtType == ELookAtType.LookAtTwoPoints)
            {
                if (iLookAtPoint == 1)
                {
                    Vector3 offset = posLookAt1 - posLastCamera;
                    gameObject.transform.localRotation = Quaternion.LookRotation(offset);
                }
                else if (iLookAtPoint == 2)
                {
                    Vector3 offset = posLookAt2 - posLastCamera;
                    gameObject.transform.localRotation = Quaternion.LookRotation(offset);
                }
            }
        }
    }

    public void UpdateConfigToXML()
    {
#if UNITY_EDITOR
        if (isDisabled)
            return;

        OnEnable();
        UpdatePoints();
        //每帧判断是否要保存编辑好的路径
        if (ePathType == ECameraPathType.Bezier)
        {
            if (pointParam.transform.position != posParam || pointBegin.transform.position != posBegin || pointEnd.transform.position != posEnd)
            {
                posParam = pointParam.transform.position;
                posBegin = pointBegin.transform.position;
                posEnd = pointEnd.transform.position;

                //中间的标志点分别减去左右两边的标志点，计算出曲线的X Y 的点
                float y = (posParam.y - posBegin.y);
                float x = (posParam.x - posBegin.x);
                float z = (posParam.z - posBegin.z);

                //因为我们是通过3个点来确定贝塞尔曲线， 所以参数3 设置为0 即可。
                //这样参数1 表示起点 参数2表示中间点 参数3 忽略 参数4 表示结束点
                movePathBase = new PathBezier(posBegin, new Vector3(x, y, z), new Vector3(0f, 0f, 0f), posEnd);
                Utils.LogSys.Log("UpdateConfigToXML:" + pathName);

                //SavePathToXML();
                isNeedToSave = true;
            }
        }
        else if (ePathType == ECameraPathType.Straightline){}
        else if (ePathType == ECameraPathType.Arcline){}
        else if (ePathType == ECameraPathType.Circle) { }
        else if (ePathType == ECameraPathType.CylinderSpirals) 
        {
            pointLookAt1.transform.position = posLookAt1;
            pointLookAt2.transform.position = posLookAt2;
            if (pointParam.transform.position != posParam || pointParam2.transform.position != posParam2 || pointBegin.transform.position != posBegin || Mathf.Abs(Distance - distance) >= 0.01f || Mathf.Abs(PerCircleDistance - perCircleDistance) >= 0.01f)
            {
                posParam = pointParam.transform.position;
                posParam2 = pointParam2.transform.position;
                posBegin = pointBegin.transform.position;
                posEnd = pointEnd.transform.position;
                distance = Distance;
                perCircleDistance = PerCircleDistance;
                posLookAt1 = posParam;
                posLookAt2 = posParam2;
                pointLookAt1.transform.position = posLookAt1;
                pointLookAt2.transform.position = posLookAt2;
                movePathBase = new PathCylinderSpirals(posBegin, posParam, posParam2, distance, perCircleDistance);
                //SavePathToXML();
                isNeedToSave = true;
            }
        }

        if (elookAtType == ELookAtType.LookAtPoint || elookAtType == ELookAtType.LookAtPointReverse)
        {
            //每帧判断是否要保存编辑好的路径
            Vector3 changeLookAt = posLookAt1 - pointLookAt1.transform.position;
            if (Mathf.Abs(changeLookAt.x) >= 0.01f || Mathf.Abs(changeLookAt.y) >= 0.01f || Mathf.Abs(changeLookAt.z) >= 0.01f)
            {
                posLookAt1 = pointLookAt1.transform.position;
                //SavePathToXML();
                isNeedToSave = true;
            }
        }
        else if (elookAtType == ELookAtType.LookAtLine || elookAtType == ELookAtType.LookAtLineReverse)
        {
            bool bNeedSave = false;
            //每帧判断是否要保存编辑好的路径
            Vector3 changeLookAt = posLookAt1 - pointLookAt1.transform.position;
            if (Mathf.Abs(changeLookAt.x) >= 0.01f || Mathf.Abs(changeLookAt.y) >= 0.01f || Mathf.Abs(changeLookAt.z) >= 0.01f)
            {
                bNeedSave = true;
                posLookAt1 = pointLookAt1.transform.position;
            }
            changeLookAt = posLookAt2 - pointLookAt2.transform.position;
            if (Mathf.Abs(changeLookAt.x) >= 0.01f || Mathf.Abs(changeLookAt.y) >= 0.01f || Mathf.Abs(changeLookAt.z) >= 0.01f)
            {
                bNeedSave = true;
                posLookAt2 = pointLookAt2.transform.position;
            }
            if (PosLookAtOffset != posLookAtOffset)
            {
                bNeedSave = true;
                posLookAtOffset = PosLookAtOffset;
            }
            if (bNeedSave)
                isNeedToSave = true;
                //SavePathToXML();
        }
        else if (elookAtType == ELookAtType.LookAtTwoPoints)
        {
            Vector3 changeLookAt = posLookAt1 - pointLookAt1.transform.position;
            if (Mathf.Abs(changeLookAt.x) >= 0.01f || Mathf.Abs(changeLookAt.y) >= 0.01f || Mathf.Abs(changeLookAt.z) >= 0.01f)
            {
                posLookAt1 = pointLookAt1.transform.position;
                //SavePathToXML();
                isNeedToSave = true;
            } 
            changeLookAt = posLookAt2 - pointLookAt2.transform.position;
            if (Mathf.Abs(changeLookAt.x) >= 0.01f || Mathf.Abs(changeLookAt.y) >= 0.01f || Mathf.Abs(changeLookAt.z) >= 0.01f)
            {
                posLookAt2 = pointLookAt2.transform.position;
                //SavePathToXML();
                isNeedToSave = true;
            }
        }

        //每帧判断是否要保存编辑好的路径
        if (ePathType_Editor != ePathType || elookAtType_Editor != elookAtType)
        {
            ePathType_Editor = ePathType;
            elookAtType_Editor = elookAtType;
            isNeedToSave = true;
            //SavePathToXML();
        }
#endif
    }
    public Vector3 GetLookLinePoint(Vector3 cameraPos)
    {
        if (ePathType == ECameraPathType.CylinderSpirals)
        {
            posLookAt1 = posParam;
            posLookAt2 = posParam2;
        }

        float k1 = (posLookAt1.x - cameraPos.x) * (posLookAt2.x - posLookAt1.x) + (posLookAt1.y - cameraPos.y) * (posLookAt2.y - posLookAt1.y) + (posLookAt1.z - cameraPos.z) * (posLookAt2.z - posLookAt1.z);
        //Vector3 b1b2 = posLookAt1 - posLookAt2;
        float k2 = Mathf.Pow(Vector3.Distance(posLookAt1, posLookAt2),2f);//b1b2.sqrMagnitude;
        float k = -k1 / k2;

        float x = k * (posLookAt2.x - posLookAt1.x) + posLookAt1.x;
        float y = k * (posLookAt2.y - posLookAt1.y) + posLookAt1.y;
        float z = k * (posLookAt2.z - posLookAt1.z) + posLookAt1.z;
        return new Vector3(x, y, z) + PosLookAtOffset;
    }

#if UNITY_EDITOR
    /// <summary>
    /// 画线
    /// </summary>
    void OnDrawGizmos ()
    {
        if (tfPathObj != null && !tfPathObj.gameObject.activeInHierarchy)
            return;

        if (isDisabled)
            return;
        
        if (movePathBase == null)
            return;

        if (ePathType == ECameraPathType.Bezier)
        {
            Gizmos.color = new Color(255f, 255f, 255f);
            Gizmos.DrawLine(pointParam.transform.position, pointBegin.transform.position);
            Gizmos.DrawLine(pointParam.transform.position, pointEnd.transform.position);

            Vector3 tempPoint = Vector3.zero;
            Gizmos.color = new Color(0f, 255f, 0f);
            Vector3 vec = Vector3.zero;
            //绘制贝塞尔曲线
            for (int i = 1; i <= 100; i++)
            {
                movePathBase.GetPointAtTime((float)(i * 0.01), out vec);
                if (tempPoint != Vector3.zero)
                {
                    Gizmos.DrawLine(tempPoint, vec);
                }
                tempPoint = vec;
            }
        }
        else if (ePathType == ECameraPathType.CylinderSpirals)
        {
            Gizmos.color = new Color(255f, 255f, 255f);
            Gizmos.DrawLine(pointParam.transform.position, pointParam2.transform.position);

            Vector3 tempPoint = pointBegin.transform.position;
            Gizmos.color = new Color(0f, 255f, 0f);
            Vector3 vec = Vector3.zero;
            //圆柱螺旋曲线
            for (int i = 1; i <= 1000; i++)
            {
                if (i == 500)
                    Gizmos.color = new Color(255f, 255f, 0f);
                movePathBase.GetPointAtTime((float)(i * 0.001), out vec);
                if (tempPoint != Vector3.zero)
                {
                    Gizmos.DrawLine(tempPoint, vec);
                }
                tempPoint = vec;
            }
        }

        if (elookAtType == ELookAtType.LookAtPoint || elookAtType == ELookAtType.LookAtPointReverse)
        {
            Gizmos.color = new Color(0f, 0f, 0f);
            Gizmos.DrawLine(transform.position, pointLookAt1.transform.position);
        }
        else if (elookAtType == ELookAtType.LookAtLine || elookAtType == ELookAtType.LookAtLineReverse)
        {
            Gizmos.color = new Color(0f, 0f, 0f);
            Gizmos.DrawLine(transform.position, GetLookLinePoint(transform.position));
        }
        else if (elookAtType == ELookAtType.LookAtTwoPoints)
        {
            Gizmos.color = new Color(0f, 0f, 0f);
            Gizmos.DrawLine(transform.position, pointLookAt1.transform.position);
            Gizmos.DrawLine(transform.position, pointLookAt2.transform.position);
        }
    }
#endif

    public void SavePathToXML()
    {
        if (isDisabled)
            return;

        if (!isNeedToSave)
            return;

        PathConfig pathConfig = new PathConfig();
        pathConfig.pathType = (int)ePathType;
        pathConfig.pointBegin = posBegin;
        pathConfig.pointEnd = posEnd;
        if (ePathType == ECameraPathType.Straightline)
        {

        }
        else if (ePathType == ECameraPathType.Arcline)
        {
            pathConfig.pointParam1 = posParam;
        }
        else if (ePathType == ECameraPathType.Bezier)
        {
            pathConfig.pointParam1 = posParam;
        }
        else if (ePathType == ECameraPathType.Circle)
        {
            pathConfig.pointParam1 = posParam;
        }
        else if (ePathType == ECameraPathType.CylinderSpirals)
        {
            pathConfig.pointParam1 = posParam;
            pathConfig.pointParam2 = posParam2;
            pathConfig.distance = distance;
            pathConfig.perCircleDistance = perCircleDistance;
            if (Mathf.Abs(distance) < 0.01f || Mathf.Abs(perCircleDistance) < 0.01f)
                return;
        }

        if (elookAtType == ELookAtType.LookAtPoint)
        {
            pathConfig.lookAtType = 1;
            pathConfig.pointLookAt1 = posLookAt1;
        }
        else if (elookAtType == ELookAtType.LookAtLine)
        {
            pathConfig.lookAtType = 2;
            pathConfig.pointLookAt1 = posLookAt1;
            pathConfig.pointLookAt2 = posLookAt2;
            pathConfig.pointLookAtOffset = posLookAtOffset;
        }
        else if (elookAtType == ELookAtType.LookAtPointReverse)
        {
            pathConfig.lookAtType = 3;
            pathConfig.pointLookAt1 = posLookAt1;
        }
        else if (elookAtType == ELookAtType.LookAtLineReverse)
        {
            pathConfig.lookAtType = 4;
            pathConfig.pointLookAt1 = posLookAt1;
            pathConfig.pointLookAt2 = posLookAt2;
            pathConfig.pointLookAtOffset = posLookAtOffset;
        }
        else if (elookAtType == ELookAtType.LookAtTwoPoints)
        {
            pathConfig.lookAtType = 5;
            pathConfig.pointLookAt1 = posLookAt1;
            pathConfig.pointLookAt2 = posLookAt2;
        }
        pathConfig.pointBegin = posBegin;
        PathXMLMgr.getInstance().updatePath(pathName, pathConfig);
        PathXMLMgr.getInstance().WriteXML();
        Utils.LogSys.Log("Saved Camera Path!");
    }
}
