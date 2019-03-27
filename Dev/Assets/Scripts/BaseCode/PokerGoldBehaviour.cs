using UnityEngine;
using System.Collections;
using EventManager;
using MyExtensionMethod;
using UI.Controller;
using Utils;
using Scene;
class PokerGoldBehaviour : MonoBehaviour
{
    bool _beStartCreateObject = false;
    public GameObject _poolPrefab;//模板

    void Update()
    {
        if (!_beStartCreateObject)
            return;



    }


    public void StartCreateObject(GameObject obj,int Count)
    {
        if(obj == null)
        {
            return;
        }
        _beStartCreateObject = true;
    }
}
