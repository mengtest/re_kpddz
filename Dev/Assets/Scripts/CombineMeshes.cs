/***************************************************************


 *
 *
 * Filename:  	CombineMeshes.cs	
 * Summary: 	网格合并脚本，目前只支持相同材质网格合并，运行时合并
 *              注意：该脚本为美术制作场景时进行物件分组时使用
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/07/20 5:33
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
#endregion


namespace optimization
{
    //网格合并
    public class CombineMeshes : MonoBehaviour
    {
        void Start()
        {
            //静态部件不合并
            if (gameObject.isStatic) return;

            //检测MeshFilter组件
            MeshFilter comptMeshFilter = GetComponent<MeshFilter>();
            if (comptMeshFilter == null)
            {
                gameObject.AddComponent<MeshFilter>();
            }

            //检测MeshRenderer组件
            MeshRenderer comptMeshRender = GetComponent<MeshRenderer>();
            if (comptMeshRender == null)
            {
                gameObject.AddComponent<MeshRenderer>();
            }
            
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            MeshRenderer[] meshRenderer = GetComponentsInChildren<MeshRenderer>();  //获取自身和所有子物体中所有MeshRenderer组件
            Material[] mats = new Material[1];                    //新建材质球数组，材质相同，只声明大小为1个

            for (int i = 0; i < meshFilters.Length; i++)
            {
                mats[0] = meshRenderer[i].sharedMaterial;                           //获取材质球列表,因为要求材质相同的才能合并，所以只存储一个材质

                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;  //注意变换矩阵
                meshFilters[i].gameObject.SetActive(false);
            }

            transform.GetComponent<MeshFilter>().mesh = new Mesh();
            try
            {
                transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true);//为mesh.CombineMeshes添加一个 false 参数，表示并不是合并为一个网格，而是一个子网格列表
            }
            catch (System.Exception ex)
            {
                Utils.LogSys.LogError(ex.Message);
            }
            transform.GetComponent<MeshRenderer>().sharedMaterials = mats;          //为合并后的GameObject指定材质

            transform.gameObject.SetActive(true);
            transform.gameObject.isStatic = true;
        }
    }
}


