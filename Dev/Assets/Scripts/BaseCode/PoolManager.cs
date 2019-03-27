/***************************************************************

 *
 *
 * Filename:  	PoolManager.cs	
 * Summary:     缓冲池管理
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
using System.Collections.Generic;
[SLua.CustomSingletonLuaClass]
public class PoolManager : Singleton<PoolManager>
{
    public Dictionary<GameObject, Pool> _pools = new Dictionary<GameObject, Pool>();
    public PoolManager()
    {
    }

    public void Add(Pool pool)
    {
        if (pool._poolPrefab == null)
            return;
        if (_pools.ContainsKey(pool._poolPrefab))
            return;
        _pools.Add(pool._poolPrefab, pool);
    }

    public void CreatePool(GameObject prefab, GameObject parentObj, int preLoad, bool limit, int maxCount, Vector3 initPos)
    {
        if (_pools.ContainsKey(prefab))
            return;

        Pool poolObj = new Pool();
        poolObj._poolPrefab = prefab;
        poolObj._preLoad = preLoad;
        poolObj._limit = limit;
        poolObj._maxCount = maxCount;
        poolObj._initPosition = initPos;
        poolObj._poolParent = parentObj;
        poolObj.Awake();
    }

    //获取缓冲池
    public Pool GetPool(GameObject go)
    {
        foreach (GameObject prefab in _pools.Keys)
        {
            if (_pools[prefab]._active.Contains(go))
                return _pools[prefab];
        }
        return null;
    }

    //获取一个未被激活的GameObject,如果没有则创建一个
    public GameObject Spawn(GameObject prefab)
    {
        if (!_pools.ContainsKey(prefab))
        {
            CreatePool(prefab, prefab.transform.parent.gameObject, 0, false, 0,Vector3.zero);
        }

        //spawn instance in the corresponding Pool
        return _pools[prefab].Spawn();
    }

    //将GameObject设置回不被激活的状态
    public void Despawn(GameObject go)
    {
        GetPool(go).DeSpawn(go);
    }

    //根据时间将GameObject设置回不被激活
    public  void Despawn(GameObject instance, float time)
    {
        if (time > 0) GetPool(instance).DeSpawn(instance, time);
        else GetPool(instance).DeSpawn(instance);
    }

    //将所有GameObject解除激活状态
    public void DeactivatePool(GameObject prefab)
    {
        if (!_pools.ContainsKey(prefab))
            return;

        //cache active count
        int count = _pools[prefab]._active.size;
        //loop through each active instance
        for (int i = count - 1; i > 0; i--)
        {
            _pools[prefab].DeSpawn(_pools[prefab]._active[i]);
        }
    }

    //删除所有未被使用的GameObject
    public void DestroyAllInactive(bool limitToPreLoad)
    {
        foreach (GameObject prefab in _pools.Keys)
            _pools[prefab].DestroyUnused(limitToPreLoad);
    }

    //删除缓冲池
    public void DestroyPool(GameObject prefab)
    {
        if (!_pools.ContainsKey(prefab))
            return;

        Destroy(_pools[prefab].gameObject);
        _pools.Remove(prefab);
    }

    //删除所有缓冲池
    public void DestroyAllPools()
    {
        foreach (GameObject prefab in _pools.Keys)
            DestroyPool(_pools[prefab].gameObject);
    }

    //清空缓冲池
    void OnDestroy()
    {
        _pools.Clear();
    }
}
