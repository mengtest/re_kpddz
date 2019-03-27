using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using Utils;
using asset;
using Scene;

public struct PathConfig
{
    public int pathType;
    public Vector3 pointBegin;
    public Vector3 pointEnd;
    public Vector3 pointParam1;
    public Vector3 pointParam2;
    public int lookAtType;
    public Vector3 pointLookAt1;
    public Vector3 pointLookAt2;
    public Vector3 pointLookAtOffset;
    public float distance;
    public float perCircleDistance;
}
public class PathXMLMgr: Singleton<PathXMLMgr> {

    private XDocument doc;
    private Dictionary<string, PathConfig> dicPathCofig;
    private bool bInit = false;
    
    public void init(XDocument docTemp = null)
    {
        if (bInit && docTemp == null)
            return;

        bInit = true;
        dicPathCofig = new Dictionary<string, PathConfig>();
        try
        {
            if (docTemp == null)
            {
                UnityEngine.Object assets = AssetManager.getInstance().loadAsset("Config/PathConfig.xml");//同步加载XML
                if (assets != null)
                    docTemp = XDocument.Parse(assets.ToString());
                if (docTemp == null)
                    return;
            }
            doc = docTemp;
            foreach (XElement item in doc.Root.Descendants("Path")) //得到每一个Sence节点
            {
                string strName = item.Attribute("name").Value;
                PathConfig config = new PathConfig();
                foreach (XElement param in item.Descendants("Property")) //得到每一个Sence节点
                {
                    if (param.Attribute("pathType") != null)
                    {
                        int.TryParse(param.Attribute("pathType").Value, out config.pathType);
                    }
                    else if (param.Attribute("pointBegin") != null)
                    {
                        ParsePos(param.Attribute("pointBegin").Value, ref config.pointBegin);
                    }
                    else if (param.Attribute("pointEnd") != null)
                    {
                        ParsePos(param.Attribute("pointEnd").Value, ref config.pointEnd);
                    }
                    else if (param.Attribute("pointParam1") != null)
                    {
                        ParsePos(param.Attribute("pointParam1").Value, ref config.pointParam1);
                    }
                    else if (param.Attribute("pointParam2") != null)
                    {
                        ParsePos(param.Attribute("pointParam2").Value, ref config.pointParam2);
                    }
                    else if (param.Attribute("lookAtType") != null)
                    {
                        int.TryParse(param.Attribute("lookAtType").Value, out config.lookAtType);
                    }
                    else if (param.Attribute("pointLookAt1") != null)
                    {
                        ParsePos(param.Attribute("pointLookAt1").Value, ref config.pointLookAt1);
                    }
                    else if (param.Attribute("pointLookAt2") != null)
                    {
                        ParsePos(param.Attribute("pointLookAt2").Value, ref config.pointLookAt2);
                    }
                    else if (param.Attribute("pointLookAtOffset") != null)
                    {
                        ParsePos(param.Attribute("pointLookAtOffset").Value, ref config.pointLookAtOffset);
                    }
                    else if (param.Attribute("distance") != null)
                    {
                        float.TryParse(param.Attribute("distance").Value, out config.distance);
                    }
                    else if (param.Attribute("perCircleDistance") != null)
                    {
                        float.TryParse(param.Attribute("perCircleDistance").Value, out config.perCircleDistance);
                    }
                }
                dicPathCofig.Add(strName, config);
            }
        }
        catch (Exception e)
        {
            Utils.LogSys.LogError(e.Message);
        }
    }

    private void ParsePos(string sPos, ref Vector3 outPos)
    {
        int cutCount = Mathf.Max(0, sPos.Length - 2);
        string str = sPos.Substring(1, cutCount);
        string[] sNum = str.Split(new char[] { ',' });
        float.TryParse(sNum[0], out outPos.x);
        float.TryParse(sNum[1], out outPos.y);
        float.TryParse(sNum[2], out outPos.z);
    }

    public void updatePath(string pathName, PathConfig config)
    {
        init();
        if (doc == null)
            return;

        XElement targetItem = null;
        foreach (XElement item in doc.Root.Descendants("Path")) //得到每一个Sence节点
        {
            if (item.Attribute("name").Value == pathName)
            {
                targetItem = item;
                targetItem.RemoveNodes();
                //targetItem.RemoveAttributes();
                break;
            }
        }
        if (targetItem == null)
        {
            targetItem = new XElement("Path");
            doc.Root.Add(targetItem);
        }
        targetItem.SetAttributeValue("name", pathName);
        XElement pathType = new XElement("Property");
        pathType.SetAttributeValue("pathType", config.pathType);
        targetItem.Add(pathType);
        XElement pointBegin = new XElement("Property");
        string elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointBegin.x, config.pointBegin.y, config.pointBegin.z);
        pointBegin.SetAttributeValue("pointBegin", elementValue);
        targetItem.Add(pointBegin);
        XElement pointEnd = new XElement("Property");
        elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointEnd.x, config.pointEnd.y, config.pointEnd.z);
        pointEnd.SetAttributeValue("pointEnd", elementValue);
        targetItem.Add(pointEnd);
        if (config.pathType == 0)
        {
        }
        else if (config.pathType == 1 || config.pathType == 2 || config.pathType == 3)
        {
            XElement pointParam1 = new XElement("Property");
            elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointParam1.x, config.pointParam1.y, config.pointParam1.z);
            pointParam1.SetAttributeValue("pointParam1", elementValue);
            targetItem.Add(pointParam1);
        }
        else if (config.pathType == 4 )
        {
            XElement pointParam1 = new XElement("Property");
            elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointParam1.x, config.pointParam1.y, config.pointParam1.z);
            pointParam1.SetAttributeValue("pointParam1", elementValue);
            targetItem.Add(pointParam1);
            XElement pointParam2 = new XElement("Property");
            elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointParam2.x, config.pointParam2.y, config.pointParam2.z);
            pointParam2.SetAttributeValue("pointParam2", elementValue);
            targetItem.Add(pointParam2);
            XElement pointLookAtOffset = new XElement("Property");
            elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointLookAtOffset.x, config.pointLookAtOffset.y, config.pointLookAtOffset.z);
            pointLookAtOffset.SetAttributeValue("pointLookAtOffset", elementValue);
            targetItem.Add(pointLookAtOffset);
            XElement distance = new XElement("Property");
            distance.SetAttributeValue("distance", config.distance);
            targetItem.Add(distance);
            XElement perCircleDistance = new XElement("Property");
            perCircleDistance.SetAttributeValue("perCircleDistance", config.perCircleDistance);
            targetItem.Add(perCircleDistance);
        }
        else
        {
            XElement pointParam1 = new XElement("Property");
            elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointParam1.x, config.pointParam1.y, config.pointParam1.z);
            pointParam1.SetAttributeValue("pointParam1", elementValue);
            targetItem.Add(pointParam1);
            XElement pointParam2 = new XElement("Property");
            elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointParam2.x, config.pointParam2.y, config.pointParam2.z);
            pointParam2.SetAttributeValue("pointParam2", elementValue);
            targetItem.Add(pointParam2);
        }
        //LookAt
        XElement lookAtType = new XElement("Property");
        lookAtType.SetAttributeValue("lookAtType", config.lookAtType);
        targetItem.Add(lookAtType);
        
        if (config.lookAtType == 0)
        {

        }
        else if (config.lookAtType == 1 || config.lookAtType == 3)
        {
            XElement pointLookAt1 = new XElement("Property");
            elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointLookAt1.x, config.pointLookAt1.y, config.pointLookAt1.z);
            pointLookAt1.SetAttributeValue("pointLookAt1", elementValue);
            targetItem.Add(pointLookAt1);
        }
        else
        {
            XElement pointLookAt1 = new XElement("Property");
            elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointLookAt1.x, config.pointLookAt1.y, config.pointLookAt1.z);
            pointLookAt1.SetAttributeValue("pointLookAt1", elementValue);
            targetItem.Add(pointLookAt1);
            XElement pointLookAt2 = new XElement("Property");
            elementValue = string.Format("[{0:0.00},{1:0.00},{2:0.00}]", config.pointLookAt2.x, config.pointLookAt2.y, config.pointLookAt2.z);
            pointLookAt2.SetAttributeValue("pointLookAt2", elementValue);
            targetItem.Add(pointLookAt2);
        }
    }

    public bool GetPath(string pathName, ref PathConfig outConfig)
    {
        init();
        if (dicPathCofig.ContainsKey(pathName))
        {
            outConfig = dicPathCofig[pathName];
            return true;
        }
        Utils.LogSys.LogError("GetPath Error: not find the path " + pathName + "!");
        return false;
    }

	public void WriteXML()
    {
        init();
        if (doc == null)
            return;

        doc.Save("Assets/Resources/Config/PathConfig.xml");
        init(doc);
    }

    public void AddAllPathToCamera(GameObject camObj)
    {
        init();
        foreach (KeyValuePair<string, PathConfig> keyValue in dicPathCofig)
        {
            string pathName = keyValue.Key;
            MovePathMono movePath = camObj.AddComponent<MovePathMono>();
            movePath.CreatePath(pathName);
            movePath.enabled = false;
        }
        
    }
}
