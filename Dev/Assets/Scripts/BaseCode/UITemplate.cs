#region

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

#endregion

[DisallowMultipleComponent]
public class UITemplate : MonoBehaviour
{
#if UNITY_EDITOR
    [HideInInspector]
    public int GUID = 0;

    [HideInInspector]
    [NonSerialized]
    public List<GameObject> searPrefabs = new List<GameObject>();

    public void InitGUID()
    {
        if (GUID != 0) return;
        GUID = Random.Range(int.MinValue, int.MaxValue);
    }
#endif
}