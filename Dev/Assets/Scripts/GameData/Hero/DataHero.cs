using network;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace HeroData
{
    public class DataHero //: IComparable
    {
        uint _id;       //人物id

        private Dictionary<int, string> _equipDict;
//        HeroConfigItem _config;

//         public DataHero(uint id, HeroConfigItem config = null)
//         {
//             _id = id;
//             _equipDict = new Dictionary<int, string>();
//             if (_config != null){
//            
//                 string[] fateArr = _config.fate_list.Split(',');
//                 for (int i = 0; i < fateArr.Length; i++)
//                 {
// 
//                 }
//             }
//         }



        /// <summary>
        /// 索引器取值，没有对应属性返回取值的 key
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public object this[string name]
        {
            get
            {
                var info = GetType().GetProperty(name);
                return (info != null) ? info.GetValue(this, null) : name;
            }
        }
        /// <summary>
        /// 获取装备uuid
        /// </summary>
        /// <param name="pos">位置 1武器，2防具，3饰品</param>
        /// <returns></returns>
        public string getEquipUuidByPos(int pos) {
            if (_equipDict.ContainsKey(pos)) {
                return _equipDict[pos];
            }
            return "";
        }


    }
}

