/***************************************************************

 *
 *
 * Filename:  	Pool.cs	
 * Summary:     缓冲池
 *
 * Version:   	1.0.0
 * Author: 		LiuYi
 * Date:   		2017/2/6 17:59
 ***************************************************************/
using UnityEngine;
using System.Collections;
using EventManager;
using MyExtensionMethod;
using UI.Controller;
using Utils;
using Scene;

public class PoolTimeObject
{
    //需要设置回不被激活的GameObject
    public GameObject instance;

    //time秒后设置
    public float time;
}
public class Pool : MonoBehaviour
{


    public GameObject _poolPrefab;//模板
    public int _totalCount = 0;//当前总的个数
    public int _preLoad = 0;//初始创建个数
    public bool _limit = false;//是否限制创建个数
    public int _maxCount;//限制的最大个数
    public GameObject _poolParent;//父节点
    public Vector3 _initPosition;//初始位置
    public BetterList<GameObject> _active = new BetterList<GameObject>();//被激活的
    public BetterList<GameObject> _inactive = new BetterList<GameObject>();//未激活的


    public void Awake()
    {
        if (_poolPrefab == null)
            return;

        PoolManager.getInstance().Add(this);

//        PreLoad();
    }

    void Start()
    {
    }


    void Update()
    {
        //每帧预加载一个
        if (_poolPrefab == null)
            return;
        if (_totalCount < _preLoad)
        {
            GameObject obj = GameObject.Instantiate(_poolPrefab) as GameObject;
            obj.transform.parent = _poolParent.transform;
            obj.transform.position = _initPosition;
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.SetActive(false);

            _inactive.Add(obj);
        }
    }

    //创建GameObject
//     public void PreLoad()
//     {
//         if (_poolPrefab == null)
//             return;
// 
//         for (int i = _totalCount; i < _preLoad; i++)
//         {
//             GameObject obj = GameObject.Instantiate(_poolPrefab) as GameObject;
//             obj.transform.parent = _poolParent.transform;
//             obj.transform.position = _initPosition;
//             obj.transform.localScale = new Vector3(1, 1, 1);
//             obj.SetActive(false);
// 
//             _inactive.Add(obj);
//         }
//     }

    //获取一个未被激活的GameObject,如果没有则创建一个
    public GameObject Spawn()
    {
        GameObject obj = null;
        if(_inactive.size > 0)
        {
            obj = _inactive[0];
            _inactive.RemoveAt(0);
        }
        else
        {
            if (_limit && _active.size >= _maxCount)
                return obj;
            obj = GameObject.Instantiate(_poolPrefab) as GameObject;
            obj.transform.parent = _poolParent.transform;
            obj.transform.position = _initPosition;
            obj.transform.localScale = new Vector3(1, 1, 1);
        }
        _active.Add(obj);
        obj.SetActive(true);

        return obj;
    }

    public GameObject Spawn(Vector3 position, Quaternion rotation)
    {
        GameObject obj = null;
        if (_inactive.size > 0)
        {
            obj = _inactive[0];
            _inactive.RemoveAt(0);
        }
        else
        {
            if (_limit && _active.size >= _maxCount)
                return obj;
            obj = GameObject.Instantiate(_poolPrefab) as GameObject;
            obj.transform.parent = _poolParent.transform;
            obj.transform.position = _initPosition;
            obj.transform.localScale = new Vector3(1, 1, 1);
        }
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        _active.Add(obj);
        obj.SetActive(true);

        return obj;
    }

    //将GameObject设置回不被激活的状态
    public void DeSpawn(GameObject go)
    {
        if (!_active.Contains(go))
            return;
        _active.Remove(go);
        _inactive.Add(go);
        go.SetActive(false);
    }

    ////根据时间将GameObject设置回不被激活
    public void DeSpawn(GameObject instance, float time)
    {
        //create new class PoolTimeObject to keep track of the instance
        PoolTimeObject timeObject = new PoolTimeObject();
        //assign time and instance variable of this class
        timeObject.instance = instance;
        timeObject.time = time;

        //start timed deactivation using the created properties
        StartCoroutine(DespawnInTime(timeObject));
    }

    IEnumerator DespawnInTime(PoolTimeObject timeObject)
    {
        GameObject instance = timeObject.instance;

        float timer = Time.time + timeObject.time;
        while (instance.activeInHierarchy && Time.time < timer)
            yield return null;

        if (!instance.activeInHierarchy) yield break;
        DeSpawn(instance);
    }

    //清理不被使用的GameObject
    public void DestroyUnused(bool limitToPreLoad)
    {
        if (limitToPreLoad)
        {
            for (int i = _inactive.size - 1; i >= _preLoad; i--)
            {
                Destroy(_inactive[i]);
            }
            if (_inactive.size > _preLoad)
            {
                int nDeletCount = 0;
                while (nDeletCount <= _inactive.size - _preLoad)
                {
                    if (_inactive.size < _preLoad)
                    {
                        _inactive.RemoveAt(_preLoad);
                    }
                    nDeletCount++;
                }
            }
        }
        else
        {
            for (int i = 0; i < _inactive.size; i++)
            {
                Destroy(_inactive[i]);
            }
            _inactive.Clear();
        }
    }

    //按个数删除GameObject
    public void DestroyCount(int count)
    {
        //如果要删除的个数大于现有个数则全部删除
        if (count > _inactive.size)
        {
            DestroyUnused(false);
            return;
        }

        for (int i = _inactive.size - 1; i >= _inactive.size - count; i--)
        {
            Destroy(_inactive[i]);
        }

        int nDeletCount = 0;
        while (nDeletCount <= count)
        {
            if (_inactive.size < _inactive.size - count)
            {
                _inactive.RemoveAt(_inactive.size - count);
            }
            nDeletCount++;
        }

    }

    private int totalCount
    {
        get
        {
            int count = 0;
            count += _active.size;
            count += _inactive.size;
            return count;
        }
    }
    void OnDestroy()
    {
        _active.Clear();
        _inactive.Clear();
    }
}