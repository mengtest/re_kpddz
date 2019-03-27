/***************************************************************


 *
 *
 * Filename:  	MeshCombine.cs	
 * Summary: 	网格合并工具，合并指定节点下的子节点
 *              合并原则：使用相同的材质和同一张贴图
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/07/20 1:16
 ***************************************************************/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

public class Optimization {
    [MenuItem("WoDong/优化/移除Animation组件")]
    public static void removeUnusedAnimationWrapper()
    {
        GameObject[] objs = Selection.gameObjects;
        foreach (GameObject obj in objs)
        {
            Animation[] anims = obj.GetComponentsInChildren<Animation>();
            for (int i = 0; i < anims.Length; i++)
            {
                Animation anim = anims[i];
                int n = anim.GetClipCount();
                if (n <= 0)
                {
                    GameObject.DestroyImmediate(anim.gameObject.GetComponent<Animation>());
                }
            }
        }
    }
}
