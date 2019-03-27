/***************************************************************


 *
 *
 * Filename:  	iOSPath.cs	
 * Summary: 	Android路径管理
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/06/03 23:51
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
using System.IO;
using Mono.Xml;
using System.Security;
using System.Text;
#endregion


namespace customerPath
{
    public class AndroidPath : IPath
    {
        public AndroidPath()
        {
            LoadAssetsPathsInfo();//加载包内资源目录
            LoadPersistentPathInfo();//加载内更资源目录
        }

        private string GetNameFromPath(string path)
        {
            string[] names = path.Split(new char[] { '/' });
            if (names.Length > 0)
            {
                return names[names.Length - 1];
            }

            return path;
        }

        /// <summary>
        /// 读取Resources下资源时，要把"Resources/"去掉
        /// </summary>
        /// <param name="strAssetPath"></param>
        /// <returns></returns>
        private string PathCheck(string strAssetPath)
        {
//             if (strAssetPath.Length > 10)
//             {
//                 string pre_str = strAssetPath.Substring(0, 10);
//                 if (pre_str == "Resources/" || pre_str == "resources/")
//                 {
//                     return strAssetPath.Substring(10);
//                }
//             }
            return strAssetPath;
        }
        #region 实现IPath的接口

        public override string getLatestVersionPath(string strFilePath)
        {
            //规范化路径名
            strFilePath = strFilePath.Replace("\\", "/");
            //保存返回结果
            string strRlt = "";

            //首先尝试从缓存中查找，找到直接返回，找不到则继续搜索资源路径
            //对应三处资源位置的全路径
            EAssetPathType eType = getAssetPathType(strFilePath.ToLower());
            if (eType == EAssetPathType.ePersistent)
            {
                return persistentDataPath() + "/" + strFilePath;// + ".assetbundle";
            }
            else if (eType == EAssetPathType.eStreamingAssets)
            {
                return streamingAssetsPathPlatform() + "/" + strFilePath;// + ".assetbundle";
            }
            else//(eType == EAssetPathType.eNone || eType == EAssetPathType.eResources)
            {
                //Resource目录
                string strResourcePath = resourcesPath();
                if (strResourcePath != "")
                    strResourcePath = strResourcePath + "/";

                //所在文件夹目录
                string strDirectoryPath = Path.GetDirectoryName(strFilePath);
                if (strDirectoryPath != "")
                    strDirectoryPath = strDirectoryPath + "/";
                //strDirectoryPath = PathCheck(strDirectoryPath);
                return strResourcePath + strDirectoryPath + Path.GetFileNameWithoutExtension(strFilePath);
            }

            //规范化路径
            //return strRlt;//.Replace("\\", "/");
        }

        public override string urlForWWW(string strFilePath)
        {
            //规范化路径名
            strFilePath = strFilePath.Replace("\\", "/");
            EAssetPathType eType = getAssetPathType(strFilePath);
            if (eType == EAssetPathType.ePersistent)
            {
                return "file://" + getLatestVersionPath(strFilePath);
            }
            else if (eType == EAssetPathType.eStreamingAssets)
            {
                return getLatestVersionPath(strFilePath);//Application.streamingAssetsPath会自动添加协议jar:file://
            }
            else
            {
                return "jar:file://" + Application.dataPath + "/assets/" + strFilePath;
            }
        }

        #endregion //实现IPath的接口
    }
}

