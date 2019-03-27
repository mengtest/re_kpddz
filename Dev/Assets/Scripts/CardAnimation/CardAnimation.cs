using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SLua;

[CustomLuaClass]
public class CardAnimation : MonoBehaviour
{
    /// <summary>
    /// 移动时间
    /// </summary>
    public static float MOVE_TIME = 0.3f;

    // 序列优先顺序
    public enum ESequencePriority
    {
        eLeft,
        eRight,
    }

    public enum EDirection
    {
        none,
        left,
        right,
    }


    // 卡片对象
    public class Card
    {
        /// <summary>
        /// 当前Slot key
        /// </summary>
        public int slotKey = 0;

        /// <summary>
        /// 下一个Slot key
        /// </summary>
        public int slotNext = 0;

        /// <summary>
        /// 目标Slot
        /// </summary>
        public int slotTarget = 0;

        /// <summary>
        /// 卡片Transform对象
        /// </summary>
        public Transform tf = null;

        // 卡片动画控制器
        public CardAnimation cardAnim = null;

        // 移动中
        public bool moving = false;

        // 移动的时间
        public float eclapse = 0.0f;

        // 开始移动位置
        Vector3 _startPos, _nextPos;

        /// <summary>
        /// 开始移动
        /// </summary>
        public void startMove()
        {
            _startPos = tf.localPosition;
            _nextPos = cardAnim.slot2Position(slotNext);
            eclapse = 0.0f;
            moving = true;

            if (slotNext == 0 && cardAnim.onStartEnterForegroundEvent != null)
            {
                cardAnim.onStartEnterForegroundEvent(tf.gameObject);
            }

            if (slotKey == 0 && cardAnim.onCardStartEnterBackgroundEvent != null)
            {
                cardAnim.onCardStartEnterBackgroundEvent(tf.gameObject);
            }

            if (((slotKey == cardAnim.leftUpperBound && slotNext == cardAnim.rightUpperBound)
                    || (slotKey == cardAnim.rightUpperBound && slotNext == cardAnim.leftUpperBound))
                && cardAnim.onCardAxisSideStartChangeEvent != null
            )
            {
                cardAnim.onCardAxisSideStartChangeEvent(tf.gameObject);
            }
        }

        // 卡片信息更新
        public void update()
        {
            if (cardAnim == null || !moving)
                return;

            eclapse += Time.deltaTime;
            Vector3 pos = Vector3.Lerp(_startPos, _nextPos, Mathf.Min(eclapse, CardAnimation.MOVE_TIME)/ CardAnimation.MOVE_TIME);
            tf.localPosition = pos;

            if (eclapse > CardAnimation.MOVE_TIME)
            {
                slotKey = slotNext;
                if (slotNext == slotTarget)
                {
                    moving = false;
                }
                else
                {
                    if (cardAnim.direction == EDirection.left)
                        slotNext = cardAnim.leftSlotKey(slotKey);
                    else
                        slotNext = cardAnim.rightSlotKey(slotKey);

                    _startPos = tf.localPosition;
                    _nextPos = cardAnim.slot2Position(slotNext);
                    eclapse = 0.0f;
                }
            }
        }

        public void drag(Vector2 delta)
        {
            var pos = new Vector3(delta.x, 0.0f, 0.0f);
            tf.localPosition += (pos*0.5f);
        }

        public void dragStart()
        {
            _startPos = tf.localPosition;
        }

        public void dragStop()
        {

        }
    }

    #region 动画事件

    // 卡片进入前景
    public delegate void onCardStartEnterForeground(GameObject go);

    // 卡片进入后景
    public delegate void onCardStartEnterBackground(GameObject go);

    // 卡片停止
    public delegate void onCardStop();

    // 卡片更新
    public delegate void onCardUpdate(GameObject go, float delta);

    // 改变坐标轴的象限
    public delegate void onCardAxisSideStartChange(GameObject go);

    #endregion // 动画事件

    #region 配置数据

    // 卡牌大小
    public Vector2 cardSize = new Vector2(0.0f, 0.0f);

    // 间隔
    public int interval = 0;

    // 缩放曲线
    public AnimationCurve scaleCurve = null;

    // 位置曲线
    public AnimationCurve posCurve = null;

    // 卡片个数
    public int cardCount = 0;

    // 优先顺序
    public ESequencePriority seqPriority = ESequencePriority.eLeft;

    // 位置偏移
    public float posOffset = 0;

    // 中心额外范围
    public float centerExtraRange = 0;

    // 是否循环
    public bool cycle = false;

    // 是否缩放
    public bool changeScale = true;

    // 开始进入前景事件
    public onCardStartEnterForeground onStartEnterForegroundEvent = null;

    // 开始进入后景事件
    public onCardStartEnterBackground onCardStartEnterBackgroundEvent = null;

    // 坐标轴象限改变事件
    public onCardAxisSideStartChange onCardAxisSideStartChangeEvent = null;

    // 停止事件
    public onCardStop onCardStopEvent = null;

    // 卡片更新
    onCardUpdate _onCardUpdateEvent = null;

    #endregion // 配置数据

    #region 变量区域

    // 插槽位置
    Dictionary<int, Vector3> _slots = new Dictionary<int, Vector3>();

    // 中心插槽索引
    int _idxOfCenterSlot = 0;

    // 卡片列表
    List<Card> _cards = new List<Card>();

    int _leftLowerBound, _leftUpperBound;
    int _rightLowerBound, _rightUpperBound;

    // 是否移动状态
    bool _moving = false;

    // 移动方向
    EDirection _direction = EDirection.none;

    #endregion // 变量区域


    #region 属性

    /// <summary>
    /// 最左边界
    /// </summary>
    public int leftUpperBound
    {
        get
        {
            return _leftUpperBound;
        }
    }

    /// <summary>
    /// 最右边界
    /// </summary>
    public int rightUpperBound
    {
        get
        {
            return _rightUpperBound;
        }
    }

    /// <summary>
    /// 移动方向
    /// </summary>
    public EDirection direction
    {
        get
        {
            return _direction;
        }
    }

    /// <summary>
    /// 当前激活的索引
    /// </summary>
    public GameObject activeIndex
    {
        get
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (_cards[i].slotKey == 0)
                    return _cards[i].tf.gameObject;
            }

            return null;
        }
    }

    /// <summary>
    /// 事件
    /// </summary>
    public onCardUpdate onCardUpdateEvent
    {
        set
        {
            _onCardUpdateEvent = value;
            updateEvent();
        }

        get
        {
            return _onCardUpdateEvent;
        }
    }

    #endregion  // 属性


    /////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 根据插槽索引找位置
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Vector3 slot2Position(int key)
    {
        Vector3 pos = new Vector3();
        _slots.TryGetValue(key, out pos);
        return pos;
    }

    protected Plane _plane;
    protected Vector3 _lastPos;
    public void press(bool pressed)
    {
        if (pressed)
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].dragStart();
            }

            _lastPos = UICamera.lastWorldPosition;
            _plane = new Plane(Vector3.back, _lastPos);
        }
        else
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].dragStop();
            }
        }
    }

    public void drag(Vector2 delta)
    {
        if (_moving)
            return;

        Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.currentTouch.pos);
        float dist = 0f;
        if (_plane.Raycast(ray, out dist))
        {
            Vector3 currentPos = ray.GetPoint(dist);
            Vector3 offset = currentPos - _lastPos;
            _lastPos = currentPos;

            if (offset.x != 0f || offset.y != 0f || offset.z != 0f)
            {
                offset = transform.InverseTransformDirection(offset);

                // 只有X方向的移动
                offset.y = 0f;
                offset.z = 0f;

                offset = transform.TransformDirection(offset);
                Vector3 a = transform.InverseTransformPoint(offset);
                Vector3 b = transform.InverseTransformPoint(Vector3.zero);

                //Debug.Log("offset: " + (a - b).ToString());

                //for (int i = 0; i < _cards.Count; i++)
                //{
                //    _cards[i].drag(a-b);
                //}
            }
        }

        if (delta.x > 0)
        {
            moveRight();
        }
        else
        {
            moveLeft();
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void init()
    {
        initSlots();
        initCards();
        updateScale();
        updateEvent();
    }

    public void selectLeft()
    {
        moveRight();
    }

    public void selectRight()
    {
        moveLeft();
    }

    /////////////////////////////////////////////////////////////////////////////////

    // 初始化插槽
    void initSlots()
    {
        if (cardCount <= 0)
            return;

        _slots.Clear();
        float posY = posOffset;

        // 加入0点
        _slots.Add(0, new Vector3(0.0f, posY, 0.0f));

        if (cycle) // 循环滚动
        {
            if (cardCount % 2 == 0)
            {
                if (seqPriority == ESequencePriority.eLeft)
                {
                    _leftLowerBound = -1;
                    _leftUpperBound = -cardCount / 2;
                    _rightLowerBound = 1;
                    _rightUpperBound = (cardCount / 2) - 1;
                }
                else
                {
                    _leftLowerBound = -1;
                    _leftUpperBound = -(cardCount / 2) + 1;
                    _rightLowerBound = 1;
                    _rightUpperBound = cardCount / 2;
                }
            }
            else
            {
                _leftLowerBound = -1;
                _leftUpperBound = -cardCount / 2;
                _rightLowerBound = 1;
                _rightUpperBound = cardCount / 2;
            }
        }
        else
        {
            _leftLowerBound = -1;
            _leftUpperBound = -cardCount + 1;
            _rightLowerBound = 1;
            _rightUpperBound = cardCount - 1;
        }

        // 计算插槽位置
        for (int i=_leftLowerBound; i>= _leftUpperBound; i--)
        {
            float posX = -centerExtraRange + i * (interval + cardSize.x);
            _slots.Add(i, new Vector3(posX, posY, 0.0f));
        }

        for (int i= _rightLowerBound; i<= _rightUpperBound; i++)
        {
            float posX = centerExtraRange + i * (interval + cardSize.x);
            _slots.Add(i, new Vector3(posX, posY, 0.0f));
        }
    }

    // 初始化卡片
    void initCards()
    {
        _cards.Clear();
        int childCount = transform.childCount;
        for (int idx=0; idx< childCount; idx++)
        {
            var tf = transform.GetChild(idx);
            if (!tf.gameObject.activeSelf || tf.GetComponent<ComponentData>() == null)
                continue;

            var c = new Card();
            c.cardAnim = this;
            c.tf = transform.GetChild(idx);
            c.slotKey = c.tf.GetComponent<ComponentData>().Id;
            c.tf.localPosition = _slots[c.slotKey];

            _cards.Add(c);
        }
    }

    // 左边的插槽
    int leftSlotKey(int slotKey)
    {
        if (cycle)
        {
            if (slotKey == leftUpperBound)
                return rightUpperBound;
            else
                return --slotKey;
        }
        else
        {
            if (isLeftMost())
                return slotKey;
            else
                return --slotKey;
        }
    }

    // 右边的插槽
    int rightSlotKey(int slotKey)
    {
        if (cycle)
        {
            if (slotKey == rightUpperBound)
                return leftUpperBound;
            else
                return ++slotKey;
        }
        else
        {
            if (isRightMost())
                return slotKey;
            else
                return ++slotKey;
        }
    }

    // 是否到最左边
    bool isLeftMost()
    {
        if (cycle)
            return false;

        for (int i=0; i<_cards.Count; i++)
        {
            if (_cards[i].slotKey > 0)
                return false;
        }

        return true;
    }

    // 是否到最右边
    bool isRightMost()
    {
        if (cycle)
            return false;

        for (int i = 0; i < _cards.Count; i++)
        {
            if (_cards[i].slotKey < 0)
                return false;
        }

        return true;
    }

    // 向左移动
    bool moveLeft()
    {
        if (isLeftMost())
            return false;

        Card c = null;
        for (int i=0; i<_cards.Count; i++)
        {
            c = _cards[i];
            c.slotNext = leftSlotKey(c.slotKey);
            c.slotTarget = c.slotNext;
        }

        startMove(EDirection.left);
        return true;
    }

    // 向右移动
    bool moveRight()
    {
        if (isRightMost())
            return false;

        Card c = null;
        for (int i = 0; i < _cards.Count; i++)
        {
            c = _cards[i];
            c.slotNext = rightSlotKey(c.slotKey);
            c.slotTarget = c.slotNext;
        }

        startMove(EDirection.right);
        return true;
    }

    // 开始移动
    void startMove(EDirection direction)
    {
        _moving = true;
        _direction = direction;

        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].startMove();
        }
    }

    // 更新模型大小
    void updateScale()
    {
        if (!changeScale)
            return;

        for (int i = 0; i < _cards.Count; i++)
        {
            float fDis = Vector3.Distance(_cards[i].tf.localPosition, new Vector3(0.0f, 0.0f, 0.0f));
            float delta = fDis / cardSize.x;
            float scale = scaleCurve.Evaluate(delta);

            _cards[i].tf.localScale = new Vector3(scale, scale, scale);
        }
    }

    // 更新位置信息
    void updatePosition()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].update();
        }
    }

    // 更新事件
    void updateEvent()
    {
        if (_onCardUpdateEvent == null)
            return;

        for (int i = 0; i < _cards.Count; i++)
        {
            float fDis = Vector3.Distance(_cards[i].tf.localPosition, new Vector3(0.0f, 0.0f, 0.0f));
            float delta = fDis / cardSize.x;

            _onCardUpdateEvent(_cards[i].tf.gameObject, delta);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////

    #region MONO

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!_moving)
            return;

        updatePosition();
        updateScale();
        updateEvent();

        bool moving = false;
        for (int i = 0; i < _cards.Count; i++)
        {
            moving |= _cards[i].moving;
        }
        _moving = moving;

        // 停止事件
        if (!_moving)
        {
            _direction = EDirection.none;
            if (onCardStopEvent != null)
                onCardStopEvent();
        }
    }

#if UNITY_EDITOR
    private void OnGUI()
    {
        if (GUI.Button(new Rect(0.0f, 0.0f, 100.0f, 50.0f), "left"))
        {
            moveLeft();
        }

        if (GUI.Button(new Rect(180.0f, 0.0f, 100.0f, 50.0f), "right"))
        {
            moveRight();
        }
    }
#endif

    public void OnDestroy()
    {
        onStartEnterForegroundEvent = null;
        onCardStartEnterBackgroundEvent = null;
        onCardAxisSideStartChangeEvent = null;
        onCardStopEvent = null;
        _onCardUpdateEvent = null;
    }

    #endregion //MONO


}
