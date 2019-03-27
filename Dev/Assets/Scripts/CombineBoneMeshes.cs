
#region Using
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion


namespace optimization
{
    //网格骨骼合并
    public class CombineBoneMeshes : MonoBehaviour
    {
        void Awake()
        {
            //合并带骨骼的材质
            SkinnedMeshRenderer[] skinnedMesh = GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMesh.Length > 0)
            {
                List<CombineInstance> combineInstances = new List<CombineInstance>();//网格
                List<Transform> bones = new List<Transform>();//骨骼
                Material[] materials = new Material[1];//材质相同的才能合并，所以只要一个
                Bounds localBounds = new Bounds(new Vector3(0, 1.0f, 0), new Vector3(2, 2, 2));
                for (int i = 0; i < skinnedMesh.Length; i++)
                {
                    SkinnedMeshRenderer smr = skinnedMesh[i];
                    if (smr == null)
                        continue;

                    //Utils.LogSys.Log("#####################################" + smr.gameObject.name + ", smr name: " + smr.name);
                    for (int index = 0; index < smr.materials.Length; index++)
                    {
                        materials[0] = smr.materials[index];
                        //materials[0].shader = Shader.Find("Custom/Animated Vegetation/Simple");
                    }

                    //合并网格
                    for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
                    {
                        CombineInstance ci = new CombineInstance();
                        ci.mesh = smr.sharedMesh;
                        ci.subMeshIndex = sub;
                        combineInstances.Add(ci);
                    }

                    // As the SkinnedMeshRenders are stored in assetbundles that do not
                    // contain their bones (those are stored in the characterbase assetbundles)
                    // we need to collect references to the bones we are using
                    foreach (Transform bone in smr.bones)
                    {
                        bones.Add(bone);
                    }

           
                    smr.gameObject.SetActive(false);
                }

                SkinnedMeshRenderer skmr = gameObject.GetComponent<SkinnedMeshRenderer>();
                if (skmr == null)
                {
                    skmr = gameObject.AddComponent<SkinnedMeshRenderer>();
                }
                skmr.sharedMesh = new Mesh();
                skmr.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, false);
                skmr.rootBone = bones[0];
                skmr.bones = bones.ToArray();
                skmr.materials = materials;
                skmr.localBounds = localBounds;
                skmr.updateWhenOffscreen = true;
            }

            /*

            //合并不带骨骼的材质
            MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
            MeshRenderer[] meshRenderer = GetComponentsInChildren<MeshRenderer>();  //获取自身和所有子物体中所有MeshRenderer组件
            if (meshFilters.Length > 0)
            {
                CombineInstance[] combine = new CombineInstance[meshFilters.Length];
                List<Material> mats = new List<Material>();//材质相同，只会有1~2个

                for (int i = 0; i < meshFilters.Length; i++)
                {
                    if (meshFilters[i].gameObject.activeSelf)
                    {
                        if (mats.Count == 0)
                        {
                            mats.Add(meshRenderer[i].sharedMaterial);                           //获取材质球列表,因为要求材质相同的才能合并，所以只存储一个材质
                        }
                        
                        combine[i].mesh = meshFilters[i].sharedMesh;
                        combine[i].transform = transform.worldToLocalMatrix * meshFilters[i].transform.localToWorldMatrix;  //注意变换矩阵
                        meshFilters[i].gameObject.SetActive(false);
                    }
                }

                if (combine.Length > 0)
                {
                    //检测MeshFilter组件
                    MeshFilter comptMeshFilter = GetComponent<MeshFilter>();
                    if (comptMeshFilter == null)
                    {
                        comptMeshFilter = gameObject.AddComponent<MeshFilter>();
                    }
                    //检测MeshRenderer组件
                    MeshRenderer comptMeshRender = GetComponent<MeshRenderer>();
                    if (comptMeshRender == null)
                    {
                        comptMeshRender = gameObject.AddComponent<MeshRenderer>();
                    }
                    comptMeshFilter.mesh = new Mesh();

                    try
                    {
                        comptMeshFilter.mesh.CombineMeshes(combine, true);//为mesh.CombineMeshes添加一个 false 参数，表示并不是合并为一个网格，而是一个子网格列表
                    }
                    catch (System.Exception ex)
                    {
                        Utils.LogSys.LogError(ex.Message);
                    }
                    comptMeshRender.sharedMaterials = mats.ToArray();          //为合并后的GameObject指定材质
                    transform.gameObject.SetActive(true);
                }
            }
             * */
        }
    }
}

