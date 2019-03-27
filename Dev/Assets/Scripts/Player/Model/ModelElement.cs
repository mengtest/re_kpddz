/***************************************************************


 *
 *
 * Filename:  	ModelElement.cs	
 * Summary: 	模型组件信息，模型组件的加载
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/04/16 3:27
 ***************************************************************/

#region Using
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using task;
using customerPath;
#endregion

namespace player
{
    //模型组件
    public class ModelElement
    {
        //部件名字 && 材质名字
        private string _strName = default(string);
        private string _strMaterialName = default(string);

        //部件&&材质URL
        private string _strUrl = default(string);
        private string _strMatUrl = default(string);

        //父模型名字
        private string _strParentName = default(string);

        //蒙皮信息和骨骼
        private SkinnedMeshRenderer _smrInfo = null;
        private string[] _arrBones;

        //是否加载完成
        private bool _bIsLoaded = false;

        //主模型
        private Model _pModel = null;

        //加载任务
        AssetBundleLoadTask _task = null;

        /////////////////////////////////////////////////////////////

        public ModelElement(string strName, string strMaterial, string strParentName, Model mdl)
        {
            if (mdl == null)
            {
                Debug.LogWarning("模型主体不能为空");
                return;
            }

            _strName = strName;
            _strParentName = strParentName;
            _strMaterialName = strMaterial;
            _pModel = mdl;

            _strUrl = createElenentsURL();
            _strMatUrl = createMaterialsURL();

            load();
        }

        //加载
        public void load()
        {

			loadComplete ();
        }

        //加载完成
        public void loadComplete()
        {
			AssetBundle elemtAbd = null;

            /*
            string strFilePath = Application.dataPath + "/" + _strUrl;

            if (!File.Exists(strFilePath))
            {
                return;
            }

            FileStream fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
			if (fs != null)
			{
				byte[] content = new byte[fs.Length];
				fs.Read(content, 0, content.Length);
				fs.Close();
				
				elemtAbd = AssetBundle.CreateFromMemoryImmediate(content);
			}

             * */

            _task = new AssetBundleLoadTask(_strUrl);
            _task.EventFinished += new task.TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
            {
                elemtAbd = ((AssetBundleLoadTask)currentTask).getTargetAssetbundle();
                //模型
                if (elemtAbd != null)
                {
                    //实例化对象
                    GameObject  goPrefab = elemtAbd.LoadAsset<GameObject>(_strName + ".prefab");
                    //goPrefab.GetComponent<Renderer>().sharedMaterial.shader = null;

                    Object pObj = (Object)goPrefab; //elemtAbd.LoadAsset<GameObject>(_strName + ".prefab");
                    GameObject pGameObj = (GameObject)Object.Instantiate(goPrefab, new Vector3(10000f,10000f,10000f), Quaternion.identity);

                    //材质信息
                    pGameObj.GetComponent<Renderer>().material = Resources.Load(_strMatUrl) as Material;
                    //获取蒙皮信息
                    _smrInfo = (SkinnedMeshRenderer)pGameObj.GetComponent<Renderer>();

                    //骨骼信息
					Object pBonesObj = (Object)elemtAbd.LoadAsset<StringHolder>(_strName + "bonenames.asset");
                    StringHolder strholder = (StringHolder)pBonesObj;
                    _arrBones = strholder.content;

                    //加载完成
                    _bIsLoaded = true;

                    //完成
                    if (_pModel != null)
                    {
                        _pModel.elementLoadComplete(_strName);
                    }

					((AssetBundleLoadTask)currentTask).unloadUnusedAssetbundle(false);
                }
            });
        }

        //获取蒙皮信息
        public SkinnedMeshRenderer getSkinnedMeshRender()
        {
            return _smrInfo;
        }

        //获取骨骼信息
        public string[] getBones()
        {
            return _arrBones;
        }


        //生成组件加载URL
        public string createElenentsURL()
        {
            //return "Resources/modelsassetbundles/" + _strParentName + "/" + _strName + ".assetbundle";
            //string platformName = IPath.getPlatformName();
            //return platformName + "/modelsassetbundles/" + _strParentName + "/" + _strName;
			//return "modelsassetbundles/" + _strParentName + "/" + _strName;
			return "resources/modelsassetbundles/" + _strParentName + "/" + _strName + ".prefab";
        }

        //生成材质URL
        public string createMaterialsURL()
        {
            return "Materials/" + _strParentName + "/" + _strMaterialName;
        }

        //是否加载完成
        public bool isLoaded()
        {
            return _bIsLoaded;
        }

        //销毁部件
        public void reset()
        {
            _strName = default(string);
            _strMaterialName = default(string);
            _strUrl = default(string);
            _strMatUrl = default(string);
            _strParentName = default(string);
            _smrInfo = null;
            _arrBones=null;
            _bIsLoaded = false;
            _pModel = null;
        }

        //是否是主体部件
        public bool isMainBody()
        {
            return _strName.Contains("element_body");
        }

        //卸载资源
        public void unloadAssetBundle()
        {
            if (_task != null) _task.unloadUnusedAssetbundle(true);
        }
    }
}

