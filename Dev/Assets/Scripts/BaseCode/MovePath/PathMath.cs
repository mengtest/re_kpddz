using UnityEngine;
using System.Collections;


/// <summary>
/// //贝赛尔曲线算法
/// </summary>
public class PathBezier : MovePathBase
{

    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;
    public Vector3 p3;

    public float ti = 0f;

    private Vector3 b0 = Vector3.zero;
    private Vector3 b1 = Vector3.zero;
    private Vector3 b2 = Vector3.zero;
    private Vector3 b3 = Vector3.zero;

    private float Ax;
    private float Ay;
    private float Az;
    private float Bx;
    private float By;
    private float Bz;
    private float Cx;
    private float Cy;
    private float Cz;

    // Init function v0 = 1st point, v1 = handle of the 1st point , v2 = handle of the 2nd point, v3 = 2nd point
    // handle1 = v0 + v1
    // handle2 = v3 + v2
    //*****************************临时变量,用来减小频繁开辟内存******************
    float temp_t2;
    float tmep_t3;
    Vector3 temp_vector = Vector3.zero;
    //****************************************************************************


    public PathBezier(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        this.p0 = v0;
        this.p1 = v1;
        this.p2 = v2;
        this.p3 = v3;
    }

    public void Reset(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 v3)
    {
        this.p0 = v0;
        this.p1 = v1;
        this.p2 = v2;
        this.p3 = v3;
    }

    // 0.0 >= t <= 1.0
    public override void GetPointAtTime(float t, out Vector3 temp_vector)
    {
        this.CheckConstant();
        temp_t2 = t * t;
        tmep_t3 = t * t * t;
        temp_vector.x = this.Ax * tmep_t3 + this.Bx * temp_t2 + this.Cx * t + p0.x;
        temp_vector.y = this.Ay * tmep_t3 + this.By * temp_t2 + this.Cy * t + p0.y;
        temp_vector.z = this.Az * tmep_t3 + this.Bz * temp_t2 + this.Cz * t + p0.z;

    }

    /// <summary>
    /// 计算Bezier曲线的大概长度（pointCount:分为多少段来计算长度，分得越细越准）
    /// </summary>
    /// <param name="pointCount"></param>
    /// <returns></returns>
    public override float GetLength(int pointCount = 20)
    {
        this.CheckConstant();
        Vector3 lastPoint = Vector3.zero;
        Vector3 curPoint = Vector3.zero;
        GetPointAtTime(0f, out lastPoint);
        float fLength = 0f;
        for (int i = 1; i <= pointCount; i++)
        {
            GetPointAtTime((float)i / (float)pointCount, out curPoint);
            fLength = fLength + Vector3.Distance(lastPoint, curPoint);
            lastPoint = curPoint;
        }

        return fLength;
    }
    private void SetConstant()
    {
        this.Cx = 3f * ((this.p0.x + this.p1.x) - this.p0.x);
        this.Bx = 3f * ((this.p3.x + this.p2.x) - (this.p0.x + this.p1.x)) - this.Cx;
        this.Ax = this.p3.x - this.p0.x - this.Cx - this.Bx;
        this.Cy = 3f * ((this.p0.y + this.p1.y) - this.p0.y);
        this.By = 3f * ((this.p3.y + this.p2.y) - (this.p0.y + this.p1.y)) - this.Cy;
        this.Ay = this.p3.y - this.p0.y - this.Cy - this.By;
        this.Cz = 3f * ((this.p0.z + this.p1.z) - this.p0.z);
        this.Bz = 3f * ((this.p3.z + this.p2.z) - (this.p0.z + this.p1.z)) - this.Cz;
        this.Az = this.p3.z - this.p0.z - this.Cz - this.Bz;

    }

    // Check if p0, p1, p2 or p3 have changed

    private void CheckConstant()
    {
        if (this.p0 != this.b0 || this.p1 != this.b1 || this.p2 != this.b2 || this.p3 != this.b3)
        {
            this.SetConstant();
            this.b0 = this.p0;
            this.b1 = this.p1;
            this.b2 = this.p2;
            this.b3 = this.p3;
        }
    }
}



/// <summary>
/// 圆柱螺旋曲线算法
/// </summary>
public class PathCylinderSpirals : MovePathBase
{
    //编辑用参数
    public Vector3 p0;//起始点
    public Vector3 p1;//中心轴线点1
    public Vector3 p2;//中心轴线点2
    public float distance;//轴方向前进距离(非0)
    //public float liftAngle;//升角(0~360不包含0和360)
    public float perCircleDistance;//每圈前进的距离

    //计算用参数
    private Vector3 b0 = Vector3.zero;
    private Vector3 b1 = Vector3.zero;
    private Vector3 b2 = Vector3.zero;
    public float height;//轴方向前进距离(非0)
    //public float angle;//升角(0~360不包含0和360)
    public float h;
    //float k;
    float r;
    float Pi = 3.1415926f;
    Vector3 start = Vector3.zero;
    Vector3 direc = Vector3.zero;

    // 起始点、中心轴线、轴方向前进距离、升角。
    public PathCylinderSpirals(Vector3 v0, Vector3 v1, Vector3 v2, float dis, float perDis)
    {
        p0 = v0;
        p1 = v1;
        p2 = v2;
        distance = dis;
        perCircleDistance = perDis;
    }

    // 0.0 >= t <= 1.0
    public override void GetPointAtTime(float t, out Vector3 temp_vector)
    {
        this.CheckConstant();
        float y = height * t;//this.b0.z + 
        float theta = 2*Pi*y / h;
        float z = r * Mathf.Cos(theta);
        float x = r * Mathf.Sin(theta);
        //return new Vector3(x, y, z);
        //Quaternion q = Quaternion.LookRotation(new Vector3(0f, height, 0f) - direc);
        Quaternion q = Quaternion.LookRotation(direc, b2 - b1);//镜头方向，up方向
        Matrix4x4 rot = new Matrix4x4();
        rot.SetTRS(start, q, new Vector3(1, 1, 1));//位置pos，旋转q和缩放s。
        temp_vector = rot.MultiplyPoint3x4(new Vector3(x, y, z));
    }

    /// <summary>
    /// 计算曲线的长度
    /// </summary>
    /// <param name="pointCount"></param>
    /// <returns></returns>
    public override float GetLength(int pointCount = 20)
    {
        this.CheckConstant();
        return this.height / Mathf.Sin(Mathf.Atan(h / (2 * Pi * r)));
    }
    /// <summary>
    /// 取轴上最近的一点
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
//     public override Vector3 GetLookLinePoint(Vector3 point)
//     {
//         float x = k * (b2.x - b1.x) + b1.x;
//         float y = k * (b2.y - b1.y) + b1.y;
//         float z = k * (b2.z - b1.z) + b1.z;
//         return new Vector3(x, y, z);
//     }
    private void SetConstant()
    {
        h = perCircleDistance;
        float k1 = (b1.x - b0.x)*(b2.x - b1.x) + (b1.y - b0.y)*(b2.y - b1.y) + (b1.z - b0.z)*(b2.z - b1.z);
        direc = b1 - b2;
        float k2 = direc.sqrMagnitude;
        float k = -k1 / k2;
        start.x = k * (b2.x - b1.x) + b1.x;
        start.y = k * (b2.y - b1.y) + b1.y;
        start.z = k * (b2.z - b1.z) + b1.z;
        r = Vector3.Distance(b0, start);
        direc = (b0 - start);
        //direc =  (new Vector3(0f, 0f, Vector3.Distance(b1, b2)) - (b1 - b2));
    }

    // Check if p0, p1, p2 or p3 have changed

    private void CheckConstant()
    {
        if (this.p0 != this.b0 || this.p1 != this.b1 || this.p2 != this.b2 || this.height != this.distance || this.h != this.perCircleDistance)
        {
            this.b0 = this.p0;
            this.b1 = this.p1;
            this.b2 = this.p2;
            this.height = this.distance;
            this.h = this.perCircleDistance;
            this.SetConstant();
        }
    }
}

public class MyBezier : MovePathBase
{
    public Vector3 p0;
    public Vector3 p1;
    public Vector3 p2;

    private Vector3 a;
    private Vector3 b;
    private Vector3 c;

    public MyBezier(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        p0 = v0;
        p1 = v1;
        p2 = v2;

        a = p0;
        b = -2 * p0 + 2 * p1;
        c = p0 - 2 * p1 + p2;
    }

    // 0.0 >= t <= 1.0

    public override void GetPointAtTime(float t, out Vector3 temp_vector)
    {
        float t2 = t * t;
        temp_vector =  a + b*t + c*t2;
    }
}



