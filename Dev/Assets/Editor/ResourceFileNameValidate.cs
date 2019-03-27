#region

using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

#endregion

/// <summary>
///     限制项目Resources目录下的文件及文件不能还有非法字符，避免Resources.load出错
/// </summary>
public class ResourceFileNameValidate : AssetPostprocessor
{
    private static readonly Regex Reg = new Regex(@"^([0-9a-zA-Z_])([0-9a-zA-Z-_@\.()\s])*(\.(0-9a-zA-Z){1,}){0,1}$");
    public static bool bSwitch = false;//开关控制
    /// <summary>
    ///     该方法是由AssetPostprocessor的回调方法
    /// </summary>
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (!bSwitch)
        {
            return;
        }
        foreach (var importedAsset in importedAssets) {
            Debug.Log("importedAssets:" + importedAsset);
            if (ChekFileIsNotChianese(importedAsset)) continue;
            //删除文件
            //AssetDatabase.DeleteAsset(importedAsset);
            //FileUtil.DeleteFileOrDirectory(importedAsset + ".meta");
            var msg = string.Format("本项目Resources目录下的文件及文件不能还有非法字符，请修改【{0}】文件", importedAsset);
            EditorUtility.DisplayDialog("错误", msg, "我知道了");
        }
    }

    /// <summary>
    ///     检查文件是否属于Resources目录，并且文件为中文
    /// </summary>
    private static bool ChekFileIsNotChianese(string path)
    {
        if (!path.Contains("Resources")) return true;
        var fileName = Path.GetFileName(path);
        var isNormal = Reg.IsMatch(fileName);
        return isNormal;
    }

    /// <summary>
    ///     检测文件名
    /// </summary>
    [MenuItem("WoDong/检测文件名")]
    private static void CheckFileName()
    {
        var directiory = new DirectoryInfo(Application.dataPath + "/Resources");
        var infos = directiory.GetFiles("*", SearchOption.AllDirectories);
        foreach (var file in infos) {
            var a = Reg.IsMatch(file.Name);
            if (!a && !file.FullName.Contains("Shaders")) {
                Debug.LogError(file.FullName);
            }
        }
        Debug.LogError("文件名检查完成");
    }
}