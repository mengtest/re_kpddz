/***************************************************************


 *
 *
 * Filename:  	LoadFileList.cs	
 * Summary: 	下载指定的文件列表,并保存到本地目录
 *
 * Version:    	1.0.0
 * Author: 	    XMG
 * Date:   	    2015/07/09 10:03
 ***************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;  
using task;
using version;

public class LoadFileList {

    /// 协程终止订阅者代理。当且仅当显示调用stop函数终止协程时
    /// manual才为true
    public delegate void LoadFinishedHandler(string sVersion, int loadFileCount);
    public delegate void LoadErrorHandler(VersionData.ErrorCode errorCode);
    /// 终止事件，当协程结束时触发
    public event LoadFinishedHandler EventFinished = null;
    public event LoadErrorHandler EventError = null;
    
    /// <summary>
    /// 已下载的资源文件列表
    /// </summary>
    List<string> _loadedFilesList = new List<string>();

    /// <summary>
    /// 要下载的资源文件列表
    /// </summary>
    List<FileMD5Node> _waittingFilesList = new List<FileMD5Node>();

    List<WWWLoadTask> _tasks = new List<WWWLoadTask>();

    public int LoadedCount
    {
        get { return _loadedFilesList.Count; }
    }

    private string _sVersion = "";
    public LoadFileList(List<FileMD5Node> fileList, string sVersion, string downloadUrl)
    {
        _sVersion = sVersion;
        for (int i = 0; i < fileList.Count; i++ )
        {
            _waittingFilesList.Add(fileList[i]);

            //下载资源
            string _strFileUrl = downloadUrl + sVersion + "/" + fileList[i]._path;
            WWWLoadTask task = new WWWLoadTask(fileList[i]._path, _strFileUrl, fileList[i]._md5);
            _tasks.Add(task);
            task.EventFinished += new task.TaskBase.FinishedHandler(delegate(bool manual, TaskBase currentTask)
            {
                if (manual)
                    return;

                WWW _download = currentTask.GetWWW();
                if (_download == null || _download.bytes.Length == 0)
                {
                    LoadError(VersionData.ErrorCode.DownLoadFileFailed);
                    return;
                }
                WWWLoadTask wwwTask = currentTask as WWWLoadTask;
                LoadFileCallback(_download, wwwTask._strTaskName, wwwTask._strMD5);
            });
        }
    }

    private void LoadFileCallback(WWW download, string fileDir, string serverMD5)
    {
        //对比MD5
        string md5 = UtilTools.GetFileMD5(download.bytes);
        if (serverMD5 != md5)
        {
             LoadError(VersionData.ErrorCode.DownLoadFileMD5Error);
             return;
        }

        //创建临时文件
        string filePath = Application.persistentDataPath + "/" + ClientDefine.LOCAL_PROGRAM_VERSION + "/" + fileDir;
        string tempFilePath = Application.persistentDataPath + "/" + ClientDefine.LOCAL_PROGRAM_VERSION + "/" + fileDir + "temp";
        string dirPath = Path.GetDirectoryName(filePath);
        FileStream stream;
        try
        {
            System.IO.Directory.CreateDirectory(dirPath);
            System.IO.File.SetAttributes(dirPath, FileAttributes.Normal);
            stream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
            //stream = File.Create(tempFilePath);
        }
        catch (System.Exception ex)
        {
            LoadError(VersionData.ErrorCode.CreateFileFailed);
            Debug.LogException(ex);
            return;
        }

        //写临时文件
        try
        {
            stream.Write(download.bytes, 0, download.bytes.Length);
            stream.Flush();
            stream.Close();
        }
        catch (System.Exception e)
        {
            LoadError(VersionData.ErrorCode.WriteFileFailed);
            Debug.LogException(e);
            return;
        }

        //名字改为正式
        try
        {
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.SetAttributes(filePath, FileAttributes.Normal);
                System.IO.File.Delete(filePath);
            }
            System.IO.File.Move(tempFilePath, filePath);
        }
        catch (System.Exception e)
        {
            LoadError(VersionData.ErrorCode.RenameFileFailed);
            Debug.LogException(e);
            return;
        }

        if (!_loadedFilesList.Contains(fileDir))
        {
            _loadedFilesList.Add(fileDir);
        }

        if (_loadedFilesList.Count == _waittingFilesList.Count)//完成
        {
            if (EventFinished != null)
                EventFinished(_sVersion, _loadedFilesList.Count);
        }
    }

    private void LoadError(VersionData.ErrorCode errorCode)
    {
        for (int i = 0; i < _tasks.Count; i++)
        {
            TaskBase task = _tasks[i] as TaskBase;
            if (task != null && task.Running)
                task.stop();
        }
        if (EventError != null)
            EventError(errorCode);
    }


}
