using UnityEngine;
using UI.Controller;
using System.Collections;
using System.Collections.Generic;

public class randomTips : MonoBehaviour {

	// Use this for initialization
    private List<string> _tips = new List<string>()
    {
        "牛九 > 牛八 > 牛七 > 牛六 > 牛五 > 牛四 > 牛三 > 牛二 > 牛一",
        "五小牛 > 五花牛> 四炸 > 牛牛 > 有牛 > 没牛",
        "K > Q > J > 10 > 9 > 8 > 7 > 6 > 5 > 4 > 3 > 2 > A",
        "牌型数字相同时最大一张牌根据花色：黑桃♠ > 红桃♥ > 梅花♣ > 方块♦ 比较大小",
        "3张手牌（JQK按10计算）相加为10的倍数（如10、20），即为有牛，否则为没牛",
        "3张牌组成牛后再看剩下2张牌相加的个位数，该数字越大则牌型越大",
        "没牛：任意3张牌相加不为10的倍数",
        "有牛：任意3张牌相加为10的倍数",
        "牛牛：任意3张牌相加为10的倍数，另外2张牌相加为10的倍数。",
        "四炸：5张牌中有4张牌数字完全一样。",
        "五花牛：5张牌为花牌( JQK )",
        "五小牛：5张牌均小于5，且相加之和不大于10",
        "千人抢红包：每五分钟内，只要完成10局游戏，就可100%抽取到一个红包。",
        "千人抢红包：5分钟玩10局，必得0.3元、0.6元、1.2元红包之一。",
        "当钻石低于24颗时，进入“千人抢红包”系统将免费补足至24颗，每天一次。"
    };

    void OnEnable()
    {
        //UILabel tip = transform.GetComponent<UILabel>();
        //tip.text = _tips[Random.Range(1, _tips.Count)];
        UIManager.CallLuaFuncCall("RandomTips:OnEnable", gameObject);
    }
}
