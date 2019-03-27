using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BasicDataScructs;

//导出数据
//保存游戏中的模型初始数据，会被打包成assetbundle供游戏中加载使用
public class GameModelDataHolder : ScriptableObject
{
    public List<GameModelData> _listGameModel = new List<GameModelData>();

    public GameModelDataHolder()
    {
    }

    //获取游戏模型信息
    public GameModelData getGameModelData(string strModelIdx)
    {
        for (int i = 0; i < _listGameModel.Count; i++)
        {
            if (_listGameModel[i]._strAssetName == strModelIdx)
            {
                return _listGameModel[i];
            }
        }

        return null;
    }
}
