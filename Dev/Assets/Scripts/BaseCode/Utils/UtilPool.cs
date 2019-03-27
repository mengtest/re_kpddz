using System.Collections.Generic;
using UnityEngine;

public class UtilPool : MonoBehaviour {

    public GameObject cellTemplate;
    //public string poolName;
    public int preloadAmount;


    private List<GameObject> _spawned = new List<GameObject>();

    //void UtilPool(){}

    //void UtilPool(GameObject template, int amount)
    //{
    //    cellTemplate = template;
    //    preloadAmount = amount;
    //}

    public void Init(GameObject template, int amount)
    {
        cellTemplate = template;
        preloadAmount = amount;

        for (int i = 0; i < preloadAmount; i++)
        {
            var parentObj = transform.parent.gameObject;
            GameObject bigCopyItem = NGUITools.AddChild(parentObj, cellTemplate);
            bigCopyItem.name = "Cell";
            bigCopyItem.SetActive(false);
            _spawned.Add(bigCopyItem);
        }

    }

    /// <summary>
    /// 回收所有
    /// </summary>
    public void RecoverSpawn(GameObject cell)
    {
        if (cell == null) return;
        var parent = transform.parent;
        for (int i = 0; i < _spawned.Count; i++)
        {
            GameObject obj = _spawned[i];
            if (cell == obj)
            {
                obj.transform.parent = parent;
                obj.SetActive(false);
                break;
            }
        }
    }

    /// <summary>
    /// 回收所有
    /// </summary>
    public void RecoverAllSpawn()
    {
        var parent = transform.parent;
        for (int i = 0; i < _spawned.Count; i++)
        {
            GameObject obj = _spawned[i];
            obj.transform.parent = parent.transform;
            obj.SetActive(false);
        }
    }

    // 初始化
	void Awake () {
	}
	

    void Destroy()
    {
        for (int i = 0; i < _spawned.Count; i++)
        {
            GameObject obj = _spawned[i];
            GameObject.Destroy(obj);
        }
        _spawned.Clear();
    }

    public GameObject Spawn()
    {
        for (int i = 0; i < _spawned.Count; i++)
        {
            GameObject obj = _spawned[i];
            if (obj.activeSelf == false)
                return obj;
        }
        return null;
    }
}
