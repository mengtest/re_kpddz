using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using sluaAux;

namespace PokerBase
{
    [SLua.CustomLuaClass]
    public enum ePOKER_TYPE
    {
        none = 0,
        spade = 1,          // 黑桃
        heart = 2,          // 红桃
        club = 3,           // 梅花
        diamond = 4,        // 方片
        king = 5,           // 大小王
    }

    [SLua.CustomLuaClass]
    public enum ePOKER_COUNT
    {
        none = 0,
        suit = 13,          // 一种类型的数量A~K
        except = 52,        // 除去大小王的数量
        both = 54,          // 完整一套牌的数量
    }

    [SLua.CustomLuaClass]
    public class Poker
    {
        private int _num = 0;   
        private ePOKER_TYPE _type = ePOKER_TYPE.none;

        // 在一套扑克牌中的编号（黑红梅方）,与类型、数字互相转换
        private int _index = 0;

        // 在扑克牌堆里的编号
        public int BagIndex = 0;

        // 数字
        public int Num
        {
            get { return _num; }
        }

        // 类型 （黑1， 红2， 梅3， 方4， 王5）
        public ePOKER_TYPE Type
        {
            get { return _type; }
        }

        // 编号
        public int Index
        {
            get { return _index; }
        }

        // 编号转类型
        public static ePOKER_TYPE IndexToType(int index)
        {
            return (ePOKER_TYPE)(index / (int)ePOKER_COUNT.suit + 1);
        }

        // 编号转数字
        public static int IndexToNum(int index)
        {
            return index % (int)ePOKER_COUNT.suit + 1;
        }

        public static int NumTypeToIndex(ePOKER_TYPE type, int num)
        {
            return ((int)type - 1) * (int)ePOKER_COUNT.suit + num;
        }

        public Poker(int index)
        {
            _index = index;
            _type = IndexToType(_index);
            _num = IndexToNum(index);
        }

        public Poker(ePOKER_TYPE type, int num)
        {
            _index = NumTypeToIndex(type, num);
            _type = type;
            _num = num;
        }

        public string toString()
        {
            if (_type == ePOKER_TYPE.king)
            {
                return _type + "(" + (_num == 1 ? "小" : "大") + ")";
            }
            string numStr = null;
            switch (_num)
            {
                case 1:
                    numStr = "A";
                    break;
                case 11:
                    numStr = "J";
                    break;
                case 12:
                    numStr = "Q";
                    break;
                case 13:
                    numStr = "K";
                    break;
                default:
                    numStr = _num.ToString();
                    break;
            }
            return _type + "(" + numStr + ")";
        }
    }

    [SLua.CustomLuaClass]
    public class PokerBag
    {
        private List<Poker> _publicPokers = new List<Poker>();
        private List<Poker> _ownPokers = new List<Poker>();
        private List<List<Poker>> _allPokerSuit = new List<List<Poker>>();

        public List<Poker> PublicPokers
        {
            get { return _publicPokers; }
        }

        public List<Poker> OwnPokers
        {
            get { return _ownPokers; }
        }

        public List<Poker> FinalPokers()
        {
            List<Poker> finalPokers = new List<Poker>();
            finalPokers.AddRange(_publicPokers);
            finalPokers.AddRange(_ownPokers);
            return finalPokers;
        }

        public List<List<Poker>> AllCombination(int cnt)
        {
            List<List<Poker>> finalComb = new List<List<Poker>>();
            List<List<Poker>> allComb = Combinations(cnt);
            for (var i = 0; i < allComb.Count; i++)
            {
                List<Poker> combs = allComb[i];
                List<List<Poker>> perms = Utils.PermutationAndCombination<Poker>.GetPermutation(combs);
                finalComb.AddRange(perms);
            }
            return finalComb;
        }

        public List<List<Poker>> Combinations(int cnt)
        {
            List<List<Poker>> combs = Utils.PermutationAndCombination<Poker>.GetCombination(FinalPokers(), cnt);
            return combs;
        }

        public void CleanPokers()
        {
            _publicPokers.Clear();
            _ownPokers.Clear();
            _allPokerSuit.Clear();
        }

        public int AddPublicPoker(Poker poker)
        {
            _publicPokers.Add(poker);
            return _publicPokers.Count;
        }

        public int AddPublicPoker(ePOKER_TYPE type, int num)
        {
            return AddPublicPoker(new Poker(type, num));
        }

        public int AddPublicPoker(int index)
        {
            return AddPublicPoker(new Poker(index));
        }

        public int AddOwnPoker(Poker poker)
        {
            _ownPokers.Add(poker);
            return _ownPokers.Count;
        }

        public int AddOwnPoker(ePOKER_TYPE type, int num)
        {
            return AddOwnPoker(new Poker(type, num));
        }

        public int AddOwnPoker(int index)
        {
            return AddOwnPoker(new Poker(index));
        }

        public void PrintPokers(List<Poker> pokers)
        {
            string printText = "";
            for (int i = 0; i < pokers.Count; i++)
            {
                Poker poker = pokers[i];
                if ((i + 1) % (int)(ePOKER_COUNT.suit) == 0 || i == pokers.Count - 1)
                {
                    Debug.LogError(printText + poker.toString());
                    printText = "";
                }
                else
                {
                    printText += poker.toString() + " 、 ";
                }
            }
        }
    }

    [SLua.CustomSingletonLuaClass]
    class PokerManager
    {
        // 公共牌堆
        private List<Poker> _publicPokerBag = null;
        // 几副牌
        private int _publicBagCnt = 1;

        static readonly PokerManager instance = new PokerManager();
        public static PokerManager Instance
        {
            get
            {
                return instance;
            }
        }

        // 创建公共牌堆
        public void CreatePokerBag(int bagCnt = 1, ePOKER_COUNT pCnt = ePOKER_COUNT.both)
        {
            _publicBagCnt = bagCnt;
            if (_publicPokerBag != null)
            {
                _publicPokerBag.Clear();
            }
            
            for (int i = 0; i < (int)pCnt * _publicBagCnt; i++)
            {
                Poker poker = new Poker(i);
                AddPokerToBag(poker);  
            }
            Shuffle();
            PrintPokerBag();
        }

        // 添加牌到公共牌堆
        public void AddPokerToBag(Poker poker)
        {
            if (_publicPokerBag == null)
            {
                _publicPokerBag = new List<Poker>();
            }
            _publicPokerBag.Add(poker);
        }

        public void AddPokerToBag(List<Poker> pokers)
        {
            if (_publicPokerBag == null)
            {
                _publicPokerBag = new List<Poker>();
            }
            _publicPokerBag.AddRange(pokers);
        }

        // 洗牌
        public void Shuffle()
        {
            if (_publicPokerBag == null) return;
            System.Random random = new System.Random();
            for (int i = 0; i < _publicPokerBag.Count; i++)
            {
                int willIndex = random.Next(_publicPokerBag.Count);
                Poker temp = _publicPokerBag[willIndex];
                _publicPokerBag[willIndex] = _publicPokerBag[i];
                _publicPokerBag[i] = temp;
            }
        }

        // 打印
        public void PrintPokerBag()
        {
            string printText = "";
            for (int i = 0; i < _publicPokerBag.Count; i++)
            {
                Poker poker = _publicPokerBag[i];
                if ((i + 1) % (int)(ePOKER_COUNT.suit) == 0 || i == _publicPokerBag.Count - 1)
                {
                    Debug.LogError(printText + poker.toString());
                    printText = "";
                }
                else
                {
                    printText += poker.toString() + " 、 "; 
                }   
            }
        }
    }
}
