using UnityEngine;

public abstract class BaseMono : MonoBehaviour
{
    /// <summary>
    /// UI 界面的根 Transform
    /// </summary>
    private Transform _root;

    /// <summary>
    /// Awake
    /// </summary>
    private void Awake()
    {
        _root = transform;
        ReplaceAwake();
    }

    /// <summary>
    /// 代替子类的 Awake
    /// </summary>
    protected abstract void ReplaceAwake();


    internal GameObject FindGameObject(string path) {
        return _root.Find(path).gameObject;
    }
    internal T FindComponent<T>(string path) where T : Component {
        return _root.Find(path).GetComponent<T>();
        
    }
    internal Transform FindTransform(string path) {
        return _root.Find(path);
    }
    protected void ClearGrid(UIGrid grid)
    {
        while (grid.transform.childCount > 0)
        {
            var item = grid.transform.GetChild(0);
            if (item != null){
                NGUITools.Destroy(item.gameObject);
            }
        }
    }
}