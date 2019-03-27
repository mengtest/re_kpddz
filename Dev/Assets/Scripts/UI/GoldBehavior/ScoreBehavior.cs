/***************************************************************


 *
 *
 * Filename:  	ItemBehavior.cs	
 * Summary:     物品表现
 *
 * Version:   	1.0.0
 * Author: 		LiuYi
 * Date:   		2016/11/8 11:08
 ***************************************************************/
using UnityEngine;
using System.Collections;
using EventManager;
using MyExtensionMethod;
using UI.Controller;
using Utils;
using Scene;
using Msg;
using network.protobuf;

class ScoreBehavior : MonoBehaviour
{
    void Awake()
    {
    }
    void OnEnable()
    {
        Invoke("endToDisable", 1.0f);
    }

    void endToDisable()
    {
        transform.localPosition = new Vector3(1000, 1000, transform.localPosition.z);
        gameObject.SetActive(false);
        Animator animator = transform.GetComponent<Animator>();
        animator.enabled = false;
    }
}