using UnityEngine;

[AddComponentMenu("NGUI/Interaction/GridCellMgr")]
public class UIGridCellMgr : MonoBehaviour
{
    public delegate void OnInitializeItem(GameObject cellBox, int index, GameObject cell);
    public delegate Vector4 GetItemSize(GameObject cellBox, int index);
    public GameObject cellTemplate;
    public int preloadAmount;
    protected float cellWidth;
    protected float cellHeight;
    protected UIPanel mPanel;
    protected UIScrollView mScroll;
    bool isValidate = true;
    protected UtilPool cellPool;
    int cellBoxCount = 0;
    bool isReshow = false;//强制刷新时，发送所有已显示cell的Show事件
    /// <summary>
    /// UIGridCellMgr 对应的 GameObject
    /// </summary>
    [HideInInspector]
    public GameObject Go;
    /// <summary>
    /// UIGridCellMgr 对应的 UIGrid
    /// </summary>
    [HideInInspector]
    public UIGrid Grid;
    /// <summary>
    /// 回调函数：当Cell刚显示时调用
    /// The 'index' is the index within the child list.
    /// </summary>
    public OnInitializeItem onShowItem;
    public OnInitializeItem onHideItem;
    public GetItemSize getCellRealSize;

    private UIWidget _cellMgrWidget;


    // 初始化
    void Awake()
    {
        //内存池唯一名字必填
        //if (poolName == null)
        //{
        //    isValidate = false;
        //    return;
        //}
        _cellMgrWidget = GameObject.Find("UIRoot/CellMgrWidget/CellMgrWidget").GetComponent<UIWidget>();

        Go = gameObject;
        Grid = transform.GetComponent<UIGrid>();
        if (Grid != null) {
            _defaultWidth = (int)Grid.cellWidth;
            _defaultHeight = (int)Grid.cellHeight;
        }

        //内存池预创建数量必填
        if (preloadAmount <= 0) {
            isValidate = false;
            return;
        }
        //要有UIPanel
        mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
        if (mPanel == null) {
            isValidate = false;
            return;
        }
        //要有UIScrollView
        mScroll = mPanel.GetComponent<UIScrollView>();
        if (mScroll == null) {
            isValidate = false;
            return;
        }
        //要有mGrid
        //mGrid = gameObject.GetComponent<UIGrid>();
        //if (mGrid == null)
        //{
        //    isValidate = false;
        //    return;
        //}

        mPanel.onClipMove += OnMove;
        UIWidget widget = cellTemplate.GetComponent<UIWidget>();
        cellWidth = widget.width;
        cellHeight = widget.height;

        cellPool = gameObject.AddComponent<UtilPool>();
        cellPool.Init(cellTemplate, preloadAmount);
    }

    public void resetCellMgr()
    {
        Awake();
    }

    public UIWidget NewCellsBox(GameObject parentObj, int width, int height)
    {
        UIWidget cellBox = null;
        if (_cellMgrWidget != null)
        {
            cellBox = NGUITools.AddChild(parentObj, _cellMgrWidget.gameObject).GetComponent<UIWidget>();
            //cellBox = Instantiate(_cellMgrWidget);
            //NGUITools.AddChild(parentObj, cellBox.gameObject);
        }
        else
        {
            cellBox = NGUITools.AddWidget<UIWidget>(parentObj, 0);
        }
        
        cellBox.width = width;
        cellBox.height = height;
        cellBox.name = "CellBox" + cellBoxCount;

        //ComponentData data = cellBox.gameObject.AddComponent<ComponentData>();
        ComponentData data = ComponentData.Get(cellBox.gameObject);
        data.Id = cellBoxCount;
        cellBoxCount++;
        return cellBox;
    }


    /// <summary>
    /// 默认cell宽度
    /// </summary>
    private int _defaultWidth;
    /// <summary>
    /// 默认cell高度
    /// </summary>
    private int _defaultHeight;

    public UIWidget NewCellsBox(GameObject parentObj)
    {
        UIWidget cellBox = null;
        if (_cellMgrWidget != null)
        {
            cellBox = NGUITools.AddChild(parentObj, _cellMgrWidget.gameObject).GetComponent<UIWidget>();
           // NGUITools.AddChild(parentObj, _cellMgrWidget.gameObject);
        }
        else
        {
            cellBox = NGUITools.AddWidget<UIWidget>(parentObj, 0);
        }
        
        cellBox.width = _defaultWidth;
        cellBox.height = _defaultHeight;
        cellBox.name = "CellBox" + cellBoxCount;

        //ComponentData data = cellBox.gameObject.AddComponent<ComponentData>();
        ComponentData data = ComponentData.Get(cellBox.gameObject);
        data.Id = cellBoxCount;
        cellBoxCount++;
        return cellBox;
    }

    public void ClearCells()
    {
        if (cellPool == null)
            return;

        cellBoxCount = 0;
        cellPool.RecoverAllSpawn();
        foreach (Transform trans in transform) {
            Destroy(trans.gameObject);
        }
        transform.DetachChildren();
    }

    /// <summary>
    /// 强制刷新所有的cell,并触发当前有显示所有cell的show函数
    /// </summary>
    public void UpdateCells()
    {
        isReshow = true;
        ResetCells();
        isReshow = false;
    }

    /// <summary>
    /// Callback triggered by the UIPanel when its clipping region moves (for example when it's being scrolled).
    /// </summary>

    protected virtual void OnMove(UIPanel panel)
    {
        ResetCells();
    }

    private Vector4 getCellShowSize(Transform t)
    {
        if (getCellRealSize != null)
        {
            return getCellRealSize(t.gameObject, t.GetComponent<ComponentData>().Id);
        }
        return new Vector4(t.localPosition.x, t.localPosition.y, 0, 0);
    }

    /// <summary>
    /// 根据位置显示隐藏相应的cell
    /// </summary>
    protected void ResetCells()
    {
        int count = transform.childCount;
        if (count == 0) return;

        Vector3[] corners = mPanel.worldCorners;
        for (int i = 0; i < 4; ++i) {
            Vector3 v = corners[i];
            v = transform.InverseTransformPoint(v);
            corners[i] = v;
        }
        if (mScroll.movement == UIScrollView.Movement.Vertical || mScroll.movement == UIScrollView.Movement.Horizontal) {
            float cellPos = 0f;
            float cellDis = 0f;
            float minPos = 0f;
            float maxPos = 0f;
            float min2 = 0f;
            float max2 = 0f;
            if (mScroll.movement == UIScrollView.Movement.Vertical) {
                minPos = corners[0].y - cellHeight;//最下
                maxPos = corners[2].y + cellHeight;//最上
                min2 = corners[0].y;
                max2 = corners[2].y;

            } else if (mScroll.movement == UIScrollView.Movement.Horizontal) {
                minPos = corners[0].x - cellWidth;//显示区最下
                maxPos = corners[2].x + cellWidth;//显示区最上
                min2 = corners[0].x;
                max2 = corners[2].x;
            }
            int i_start = -1;
            int i_end = -2;

            //先回收不显示的
            for (int i = 0; i < count; ++i) {
                Transform t = transform.GetChild(i);
                if (t.name.Substring(0, 7) != "CellBox") continue;
                Vector4 cellSizeInfo = getCellShowSize(t);
                if (mScroll.movement == UIScrollView.Movement.Vertical) 
                {
                    cellPos = cellSizeInfo.y;
                    cellDis = cellSizeInfo.w * (cellSizeInfo.y > 0 ? 1 : -1);
                }
                else if (mScroll.movement == UIScrollView.Movement.Horizontal)
                {
                    cellPos = cellSizeInfo.x;
                    cellDis = cellSizeInfo.z * (cellSizeInfo.x > 0 ? -1 : 1);
                }

                if ((cellPos + cellDis) > maxPos || cellPos < minPos)//显示区之外的,移除cell
                {
                    Transform cell = t.Find("Cell");
                    if (cell != null) {
                        cellPool.RecoverSpawn(cell.gameObject);
                        if (onHideItem != null) {
                            int id = t.GetComponent<ComponentData>().Id;
                            onHideItem(t.gameObject, id, null);
                        }
                    }
                    continue;
                }
                else if ((cellPos + cellDis) > max2 || cellPos < min2)
                {
                    if (onHideItem != null)
                    {
                        int id = t.GetComponent<ComponentData>().Id;
                        onHideItem(t.gameObject, id, null);
                    }
                }
                if (i_start == -1) {
                    i_start = i;
                }
                i_end = i;
            }

            //再分配要显示的
            for (int k = i_start; k <= i_end; ++k) {
                Transform t = transform.GetChild(k);
                if (t.name.Substring(0, 7) != "CellBox")
                    continue;

                if (isReshow)//强制刷新时，显示区内的所有cell都触发ShowItem
                {
                    Transform cell = t.Find("Cell");
                    GameObject cellObj;
                    if (cell != null)
                        cellObj = cell.gameObject;
                    else
                        cellObj = cellPool.Spawn(); ////从内存池里面取一个GameObjcet

                    if (cellObj != null) {
                        cellObj.SetActive(true);
                        cellObj.transform.parent = t;
                        cellObj.transform.localPosition = new Vector3(0f, 0f, 0f);
                        int id = t.GetComponent<ComponentData>().Id;
                        ShowItem(t, id, cellObj);
                        t.gameObject.SetActive(false);
                        t.gameObject.SetActive(true);//解决新加的cell没有被裁剪问题。
                    }
                } else if (t.Find("Cell") == null)//显示区内的，分配一个cell
                  {
                    ////从内存池里面取一个GameObjcet
                    GameObject cell = cellPool.Spawn();
                    if (cell != null) {
                        cell.SetActive(true);
                        cell.transform.parent = t;
                        cell.transform.localPosition = new Vector3(0f, 0f, 0f);
                        int id = t.GetComponent<ComponentData>().Id;
                        ShowItem(t, id, cell);
                        t.gameObject.SetActive(false);
                        t.gameObject.SetActive(true);//解决新加的cell没有被裁剪问题。
                    }
                }
            }
        }
    }


    /// <summary>
    /// Want to update the content of items as they are scrolled? Override this function.
    /// </summary>
    protected virtual void ShowItem(Transform itemBox, int index, GameObject item)
    {
        if (onShowItem != null) {
            onShowItem(itemBox.gameObject, index, item);
        }
    }
    protected virtual void HideItem(Transform itemBox, int index)
    {
        if (onHideItem != null) {
            onHideItem(itemBox.gameObject, index, null);
        }
    }

    public void RecoverSpawn(GameObject cell)
    {
        cellPool.RecoverSpawn(cell);
    }

    /// <summary>
    /// 删除最后一个节点
    /// </summary>
    public void DelteLastNode()
    {
        var nodeName = "CellBox" + (cellBoxCount - 1);
        var lastNode = transform.Find(nodeName);
        if (lastNode == null) return;
        var cell = lastNode.Find("Cell");
        if (cell != null) {
            cellPool.RecoverSpawn(cell.gameObject);
        }
        var grid = transform.GetComponent<UIGrid>();
        if (grid == null) return;
        grid.GetChildList().Remove(lastNode);
        lastNode.parent = transform.parent;
        Destroy(lastNode.gameObject);
        cellBoxCount--;
    }

    /// <summary>
    /// 添加一个节点
    /// </summary>
    public void AddNewNode()
    {
        NewCellsBox(gameObject, (int)cellWidth, (int)cellHeight);
    }

    /// <summary>
    /// cell 数量
    /// </summary>
    public int CellCount { get { return cellBoxCount; } }


    #region 动作
    /// <summary>
    /// 注：横向移动未测试
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="animation"></param>
    private void _scrollToPosition(Vector2 targetPos, int animation = 1000)
    {
        Vector3 targetPosition = new Vector3(targetPos.x, targetPos.y);
        SpringPanel.Begin(mScroll.panel.gameObject, targetPosition, animation);
    }

    /// <summary>
    /// 滚动到一个坐标点
    /// 注：横向移动未测试
    /// </summary>
    /// <param name="targetPos"></param>
    /// <param name="animation"></param>
    public void ScrollToPosition(Vector2 targetPos, int animation = 1000)
    {
        _scrollToPosition(targetPos, animation);
    }

    /// <summary>
    /// 滚动到某个cell位置
    /// 注：横向移动未测试
    /// </summary>
    /// <param name="cellIndex"></param>
    /// <param name="toIndex"></param>
    /// <param name="animation"></param>
    public void ScrollCellToIndex(int cellIndex, int toIndex, int animation = 1000)
    {
        float posx = 0f;
        float posy = 0f;
        if (mScroll.movement == UIScrollView.Movement.Vertical)
        {
            posx = mScroll.panel.transform.localPosition.x;
            posy = cellIndex * _defaultHeight - toIndex * _defaultHeight;
        }
        else if (mScroll.movement == UIScrollView.Movement.Horizontal)
        {
            posx = cellIndex * _defaultWidth - toIndex * _defaultWidth;
            posy = mScroll.panel.transform.localPosition.y;
        }
        Vector2 targetPos = new Vector2(posx, posy);
        _scrollToPosition(targetPos, animation);
    }

    /// <summary>
    /// 滚动到顶点
    /// 注：横向移动未测试
    /// </summary>
    /// <param name="index"></param>
    /// <param name="animation"></param>
    public void ScrollCellToTop(int index, int animation = 1000)
    {
        ScrollCellToIndex(index, 0, animation);
    }

    public void ScrollCellToTop(GameObject cell, int animation = 1000)
    {
        ScrollCellToTop(ComponentData.Get(cell.transform.parent.gameObject).Id, animation);
    }

    #endregion
}
