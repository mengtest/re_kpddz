using UnityEngine;
using System;
using System.Collections.Generic;


//资源依赖关系保存
public class ResDependenciesHolder : ScriptableObject
{
    //所有文件的依赖列表
    public List<AssetDepRecord> listAllAssetsDependencies = new List<AssetDepRecord>();

    //材质shader信息
    public List<MatsShaderInfo> listMatsShaders = new List<MatsShaderInfo>();

    //获取指定资源的依赖项
    //资源名：相对Resources的路径，包括扩展名
    public AssetDepRecord getAssetDependencies(string strAssetName)
    {
        foreach (var assetDepRecord in listAllAssetsDependencies)
        {
            if (assetDepRecord._strTitle == strAssetName)
            {
                return assetDepRecord;
            }
        }

        return null;
    }

    //添加依赖项
    public void addAssetDependencies(AssetDepRecord assetDepRecord)
    {
        foreach (var value in listAllAssetsDependencies)
        {
            //若已经存在则更新
            if (assetDepRecord._strTitle == value._strTitle)
            {
                value._listDependencies = assetDepRecord._listDependencies;
                return;
            }
        }

        //添加新的
        listAllAssetsDependencies.Add(assetDepRecord);
    }

    //添加材质的shader信息
    public void addMatShader(string strMatName, string strShaderName)
    {
        MatsShaderInfo ms = new MatsShaderInfo();
        ms.strMatName = strMatName;
        ms.strShaderName = strShaderName;

        listMatsShaders.Add(ms);
    }

    //获取材质的shader信息
    public string getMatsShaderName(string strMatName)
    {
        foreach(var v in listMatsShaders)
        {
            if (v.strMatName == strMatName)
            {
                return v.strShaderName;
            }
        }

        return "Mobile/Diffuse";
    }
};


//单个文件的依赖记录
[Serializable]
public class AssetDepRecord
{
    /// <summary>
    /// Asset路径（相对于Resources文件夹）
    /// </summary>
    public string _strTitle;

    /// <summary>
    /// 依赖列表
    /// </summary>
    public List<string> _listDependencies;


    public AssetDepRecord(string strTitle)
    {
        _strTitle = strTitle;
        _listDependencies = new List<string>();
    }
};

//材质的shader信息
[Serializable]
public class MatsShaderInfo
{
    /// <summary>
    /// 材质名字
    /// </summary>
    public string strMatName;

    /// <summary>
    /// shader名字
    /// </summary>
    public string strShaderName = "Diffuse";
}
