
#region Using
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using asset;
using FoundationHelper;
using Utils;
using UnityEngine;
#endregion
public struct FishPathNode
{
    public Vector3 pos;//坐标
    public float speed;//速率倍数
    public bool isBezierParam;
    public FishPathNode(Vector3 node)
    {
        this.pos = node;
        this.speed = 1f;
        this.isBezierParam = false;
    }
    public FishPathNode(Vector3 node, float speed)
    {
        this.pos = node;
        this.speed = speed;
        this.isBezierParam = false;
    }
    public FishPathNode(float x, float y, float z)
    {
        this.pos.x = x;
        this.pos.y = y;
        this.pos.z = z;
        this.speed = 1f;
        this.isBezierParam = false;
    }
    public FishPathNode(float x, float y, float z, float speed)
    {
        this.pos.x = x;
        this.pos.y = y;
        this.pos.z = z;
        this.speed = speed;
        this.isBezierParam = false;
    }
}


public class FishPathDataMgr : Singleton<FishPathDataMgr>
{
    public static float BASE_SPEED = 100f;
    public static float BASE_SCALE = 50f;
    private static readonly Regex matches = new Regex(@"(?<=\{)[+-]?\d+\.?\d*,[+-]?\d+\.?\d*,?[+-]?\d*\.?\d*,?[+-]?\d*\.?\d*,?[a-z]*(?=\})");
    public FishPathDataMgr() { }

    Dictionary<int, List<FishPathNode>> dicPathConfig = new Dictionary<int, List<FishPathNode>>();
    Dictionary<int, float> dicPathTime = new Dictionary<int, float>();
    /// <summary>
    /// 初始化场景管理类
    /// </summary>
    public void initialize()
    {
        dicPathConfig.Clear();
        dicPathTime.Clear();
        try
        {
            XDocument doc = null;
            UnityEngine.Object assets = AssetManager.getInstance().loadAsset("Config/RootPathConfig.xml");//同步加载XML
            if (assets != null)
                doc = XDocument.Parse(assets.ToString());
            if (doc == null)
                return;
            //foreach (XElement item in doc.Root.Descendants("Type")) //得到每一个Sence节点
            //{
                foreach(XElement path in doc.Root.Descendants("Path"))
                {
                    string strName = path.Attribute("name").Value;
                    List<FishPathNode> points = new List<FishPathNode>();
                    int id = 0;
                    if (path.Attribute("id") != null)
                         int.TryParse(path.Attribute("id").Value, out id);
                    if (path.Attribute("nodes") != null)
                    {
                        string nodes = path.Attribute("nodes").Value;
                        ParseNodes(ref nodes, ref points);
                    }
                    if (dicPathConfig.ContainsKey(id))
                        continue;
                    dicPathConfig.Add(id, points);
                    float baseTime = 0f;
                    if (path.Attribute("baseTime") != null)
                    {
                        float.TryParse(path.Attribute("baseTime").Value, out baseTime);
                        dicPathTime.Add(id, baseTime);
                    }
                    else
                    {
                        //异常处理
                        dicPathTime.Add(id, 30f);
                    }
                }
            //}
        }
        catch (Exception e)
        {
            Utils.LogSys.LogError(e.Message);
        }

    }

    //nodes="{0,0};{1,1};{1,1,9};{3,5,0,1.65}"
    void ParseNodes(ref string nodes, ref List<FishPathNode> points)
    {
        float speed = 1f;
        Vector3 pos = new Vector3();
        MatchCollection mc = matches.Matches(nodes);
        foreach (Match m in mc)
        {
            string node = m.Value;
            pos.Set(0f, 0f, 0f);
            speed = 1f;
            string[] val = node.Split(',');
            int length = val.Length;
            bool isBezierParam = val[val.Length - 1].Equals("bezier");
            if (isBezierParam)
                length -= 1;
            if (length >= 4)
            {
                float.TryParse(val[0], out pos.x);
                float.TryParse(val[1], out pos.y);
                float.TryParse(val[2], out pos.z);
                float.TryParse(val[3], out speed);
                FishPathNode temp = new FishPathNode(pos, speed);
                temp.isBezierParam = isBezierParam;
                points.Add(temp);
            }
            else if (length >= 3)
            {
                float.TryParse(val[0], out pos.x);
                float.TryParse(val[1], out pos.y);
                float.TryParse(val[2], out pos.z);
                FishPathNode temp = new FishPathNode(pos, speed);
                temp.isBezierParam = isBezierParam;
                points.Add(temp);
            }
            else if (length >= 2)
            {
                float.TryParse(val[0], out pos.x);
                float.TryParse(val[1], out pos.y);
                FishPathNode temp = new FishPathNode(pos, speed);
                temp.isBezierParam = isBezierParam;
                points.Add(temp);
            }
        }
        
    }

    /// <summary>
    /// 取节点坐标
    /// </summary>
    /// <param name="id">路径id</param>
    /// <param name="index">节点序号</param>
    /// <returns></returns>
    public bool GetNodePos(int id, int index, out Vector3 pos, out float speed, out bool isBezierParam)
    {
        if (dicPathConfig.ContainsKey(id))
        {
            if (dicPathConfig[id].Count > index)
            {
                pos = dicPathConfig[id][index].pos;
                speed = dicPathConfig[id][index].speed * BASE_SPEED;
                isBezierParam = dicPathConfig[id][index].isBezierParam;
                return true;
            }
        }
        pos = Vector3.zero;
        speed = BASE_SPEED;
        isBezierParam = false;
        return false;
    }


    /// <summary>
    /// 通过鱼的年龄, 计算当前的位置
    /// nodeIndex:当前所处位置的前个结点
    /// nodeTime:已经从前个结点出发了多久
    /// 返回:是否还没到最终点
    /// </summary>
    /// <returns></returns>
    public bool GetPosByAge(int id, float fAge, float fSpeedScale, out int nodeIndex, out float nodeTime, out Vector3 pos)
    {
        nodeIndex = -1;
        nodeTime = 0f;
        pos = new Vector3(10000f, 10000f, 10000f);
        if (dicPathConfig.ContainsKey(id))
        {
            int nodeCount = dicPathConfig[id].Count;
            if (dicPathTime[id]/ fSpeedScale <= fAge)
            {
                //如果时间已超, 返回最后一个节点
                //return dicPathConfig[id][nodeCount - 1].pos;
                return false;
            }

            float totalTime = 0f;//累加每个节点时间
            float tempTime = 0f;//当前节点要花的时间
            float tempDis = 0f;//当前节点的路程
            float pathSpeed = BASE_SPEED * fSpeedScale;//这个路径的基础速度
            float tempSpeed = pathSpeed;//当前节点的真实速度.
            if (nodeCount > 0)
            {
                pos = dicPathConfig[id][0].pos;
            }
            for (int i=1; i< nodeCount; i++ )
            {
                nodeIndex = i - 1;
                if (dicPathConfig[id][i].isBezierParam)//曲线
                {
                    tempSpeed = pathSpeed * dicPathConfig[id][i-1].speed;
                    tempTime = dicPathConfig[id][i].speed / tempSpeed;

                    if (totalTime + tempTime >= fAge)
                    {
                        //如果该时间在两点之间, 按比例取两点间直线上的点.
                        //pos = dicPathConfig[id][i].pos + (dicPathConfig[id][i].pos - dicPathConfig[id][i - 1].pos) * ((fAge - totalTime) / tempTime);
                        //return pos;
                        nodeTime = fAge - totalTime;
                        return true;
                    }
                    if (dicPathConfig[id][i + 1].isBezierParam)//如果下一点还是曲线参数
                    {
                        i++;
                        i++;
                    }
                    else
                    {
                        i++;
                    }
                    totalTime += tempTime;
                }
                else//直线
                {
                    tempSpeed = pathSpeed * dicPathConfig[id][i].speed;
                    tempDis = Vector3.Distance(dicPathConfig[id][i - 1].pos, dicPathConfig[id][i].pos);
                    tempTime = tempDis / tempSpeed;
                    if (totalTime + tempTime >= fAge)
                    {
                        //如果该时间在两点之间, 按比例取两点间直线上的点.
                        pos = dicPathConfig[id][i-1].pos + (dicPathConfig[id][i].pos - dicPathConfig[id][i - 1].pos) * ((fAge - totalTime) / tempTime);
                        //return pos;
                        nodeTime = fAge - totalTime;
                        return true;
                    }
                    totalTime += tempTime;
                }
            }
            if (totalTime < fAge)
            {
                //如果时间已超, 返回最后一个节点
                //return dicPathConfig[id][nodeCount - 1].pos;
            }
        }
        //return pos;
        return true;
    }

    //鱼已经老死了
    public bool IsFishTimeOver(int idPath, int birthTime, float fSpeed)
    {
        int curTime = UtilTools.GetServerTime();
        float fAge = curTime - birthTime;
        if (fAge < 0)
            return false;
        if (dicPathTime.ContainsKey(idPath))
        {
            if (dicPathTime[idPath] / fSpeed <= fAge)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsPathExist(int idPath)
    {
        if (dicPathTime.ContainsKey(idPath))
        {
            return true;
        }
        return false;
    }
}