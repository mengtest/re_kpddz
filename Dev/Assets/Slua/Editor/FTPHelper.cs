using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Globalization;
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace FtpLib
{

    public class FtpWeb
    {
        string ftpServerIP;
        string ftpRemotePath;
        string ftpUserID;
        string ftpPassword;
        string ftpURI;

        /// <summary>
        /// 连接FTP
        /// </summary>
        /// <param name="FtpServerIP">FTP连接地址</param>
        /// <param name="FtpRemotePath">指定FTP连接成功后的当前目录, 如果不指定即默认为根目录</param>
        /// <param name="FtpUserID">用户名</param>
        /// <param name="FtpPassword">密码</param>
        public FtpWeb(string FtpServerIP, string FtpRemotePath, string FtpUserID, string FtpPassword)
        {
            ftpServerIP = FtpServerIP;
            ftpRemotePath = FtpRemotePath;
            ftpUserID = FtpUserID;
            ftpPassword = FtpPassword;
            ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
        }

        public void UploadDirectory(string path, string dirName)
        {
            string[] files = Directory.GetFiles(path + dirName, "*.lua", SearchOption.AllDirectories);
            string orginPath = ftpRemotePath;
            List<string> createdPaths = new List<string>();
            foreach (string file in files)
            {
                string ftpFile = file.Replace(path, "");
                ftpFile = ftpFile.Replace("\\", "/");
                string ftpFilePath = Path.GetDirectoryName(ftpFile);
                string ftpFileName = Path.GetFileName(ftpFile);
                Debug.LogError("Upload->" + ftpFile);
                // 创建文件夹
                var pathArr = ftpFilePath.Split('/');
                for (var i = 0; i < pathArr.Length; i++)
                {
                    string p = pathArr[i];
                    if (!createdPaths.Contains(p))
                    {
                        MakeDir(p);
                        createdPaths.Add(p);
                    }
                    GotoDirectory(p, false);
                }
                
                // 上传文件
                Upload(path + ftpFile);
                GotoDirectory(orginPath, true);
            }
        }

        public void UploadPath(string file, string path)
        {
            string orginPath = ftpRemotePath;
            string ftpFile = file.Replace(path, "");
            ftpFile = ftpFile.Replace("\\", "/");
            string ftpFilePath = Path.GetDirectoryName(ftpFile);
            string ftpFileName = Path.GetFileName(ftpFile);
            Debug.Log("Upload->" + ftpFile);
            // 创建文件夹
            var pathArr = ftpFilePath.Split('/');
            for (var i = 0; i < pathArr.Length; i++)
            {
                string p = pathArr[i];
                MakeDir(p);
                GotoDirectory(p, false);
            }

            // 上传文件
            Upload(path + ftpFile);
            GotoDirectory(orginPath, true);
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="filename"></param>
        public void Upload(string filename)
        {
            FileInfo fileInf = new FileInfo(filename);
            string uri = ftpURI + fileInf.Name;
            FtpWebRequest reqFTP;
            //Debug.LogError(uri);
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
            reqFTP.KeepAlive = false;
            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
            reqFTP.UseBinary = true;
            reqFTP.ContentLength = fileInf.Length;
            int buffLength = 10000;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream fs = fileInf.OpenRead();
            try
            {
                Stream strm = reqFTP.GetRequestStream();
                contentLen = fs.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Upload Error --> " + ex.Message);
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        public void Download(string filePath, string fileName)
        {
            FtpWebRequest reqFTP;
            try
            {
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 10000;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Download Error --> " + ex.Message);
            }
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName"></param>
        public void Delete(string fileName)
        {
            try
            {
                string uri = ftpURI + fileName;
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Delete Error --> " + ex.Message + "  文件名:" + fileName);
            }
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="folderName"></param>
        public void RemoveDirectory(string folderName)
        {
            try
            {
                string uri = ftpURI + folderName;
                FtpWebRequest reqFTP;
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.KeepAlive = false;
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;

                string result = String.Empty;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                long size = response.ContentLength;
                Stream datastream = response.GetResponseStream();
                StreamReader sr = new StreamReader(datastream);
                result = sr.ReadToEnd();
                sr.Close();
                datastream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "Delete Error --> " + ex.Message + "  文件名:" + folderName);
            }
        }

        /// <summary>
        /// 获取当前目录下明细(包含文件和文件夹)
        /// </summary>
        /// <returns></returns>
        public string[] GetFilesDetailList()
        {
            string[] downloadFiles;
            try
            {
                StringBuilder result = new StringBuilder();
                FtpWebRequest ftp;
                ftp = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI));
                ftp.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = ftp.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);

                //while (reader.Read() > 0)
                //{

                //}
                string line = reader.ReadLine();
                //line = reader.ReadLine();
                //line = reader.ReadLine();

                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf("\n"), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                Insert_Standard_ErrorLog.Insert("FtpWeb", "GetFilesDetailList Error --> " + ex.Message);
                return downloadFiles;
            }
        }

        /// <summary>
        /// 获取当前目录下文件列表(仅文件)
        /// </summary>
        /// <returns></returns>
        public string[] GetFileList(string mask)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI));
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (mask.Trim() != string.Empty && mask.Trim() != "*.*")
                    {

                        string mask_ = mask.Substring(0, mask.IndexOf("*"));
                        if (line.Substring(0, mask_.Length) == mask_)
                        {
                            result.Append(line);
                            result.Append("\n");
                        }
                    }
                    else
                    {
                        result.Append(line);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                downloadFiles = null;
                if (ex.Message.Trim() != "远程服务器返回错误: (550) 文件不可用(例如，未找到文件，无法访问文件)。")
                {
                    Insert_Standard_ErrorLog.Insert("FtpWeb", "GetFileList Error --> " + ex.Message.ToString());
                }
                return downloadFiles;
            }
        }

        /// <summary>
        /// 获取当前目录下所有的文件夹列表(仅文件夹)
        /// </summary>
        /// <returns></returns>
        public string[] GetDirectoryList()
        {
            string[] drectory = GetFilesDetailList();
            string m = string.Empty;
            foreach (string str in drectory)
            {
                int dirPos = str.IndexOf("<DIR>");
                if (dirPos > 0)
                {
                    /*判断 Windows 风格*/
                    m += str.Substring(dirPos + 5).Trim() + "\n";
                }
                else if (str.Trim().Substring(0, 1).ToUpper() == "D")
                {
                    /*判断 Unix 风格*/
                    string dir = str.Substring(54).Trim();
                    if (dir != "." && dir != "..")
                    {
                        m += dir + "\n";
                    }
                }
            }

            char[] n = new char[] { '\n' };
            return m.Split(n);
        }

        /// <summary>
        /// 判断当前目录下指定的子目录是否存在
        /// </summary>
        /// <param name="RemoteDirectoryName">指定的目录名</param>
        public bool DirectoryExist(string RemoteDirectoryName)
        {
            string[] dirList = GetDirectoryList();
            foreach (string str in dirList)
            {
                if (str.Trim() == RemoteDirectoryName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断当前目录下指定的文件是否存在
        /// </summary>
        /// <param name="RemoteFileName">远程文件名</param>
        public bool FileExist(string RemoteFileName)
        {
            string[] fileList = GetFileList("*.*");
            foreach (string str in fileList)
            {
                if (str.Trim() == RemoteFileName.Trim())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="dirName"></param>
        public void MakeDir(string dirName)
        {
            FtpWebRequest reqFTP;
            try
            {
                // dirName = name of the directory to create.
                //Debug.LogError("URI:" + ftpURI + dirName);
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + dirName));
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "MakeDir Error --> " + ex.Message);
            }
        }

        /// <summary>
        /// 获取指定文件大小
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public long GetFileSize(string filename)
        {
            FtpWebRequest reqFTP;
            long fileSize = 0;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + filename));
                reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                fileSize = response.ContentLength;

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "GetFileSize Error --> " + ex.Message);
            }
            return fileSize;
        }

        /// <summary>
        /// 改名
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        public void ReName(string currentFilename, string newFilename)
        {
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpURI + currentFilename));
                reqFTP.Method = WebRequestMethods.Ftp.Rename;
                reqFTP.RenameTo = newFilename;
                reqFTP.UseBinary = true;
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();

                ftpStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                Insert_Standard_ErrorLog.Insert("FtpWeb", "ReName Error --> " + ex.Message);
            }
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="currentFilename"></param>
        /// <param name="newFilename"></param>
        public void MovieFile(string currentFilename, string newDirectory)
        {
            ReName(currentFilename, newDirectory);
        }

        /// <summary>
        /// 切换当前目录
        /// </summary>
        /// <param name="DirectoryName"></param>
        /// <param name="IsRoot">true 绝对路径   false 相对路径</param>
        public void GotoDirectory(string DirectoryName, bool IsRoot)
        {
            if (IsRoot)
            {
                ftpRemotePath = DirectoryName;
            }
            else
            {
                ftpRemotePath += DirectoryName + "/";
            }
            ftpURI = "ftp://" + ftpServerIP + "/" + ftpRemotePath + "/";
        }

        /// <summary>
        /// 删除订单目录
        /// </summary>
        /// <param name="ftpServerIP">FTP 主机地址</param>
        /// <param name="folderToDelete">FTP 用户名</param>
        /// <param name="ftpUserID">FTP 用户名</param>
        /// <param name="ftpPassword">FTP 密码</param>
        public static void DeleteOrderDirectory(string ftpServerIP, string folderToDelete, string ftpUserID, string ftpPassword)
        {
            try
            {
                if (!string.IsNullOrEmpty(ftpServerIP) && !string.IsNullOrEmpty(folderToDelete) && !string.IsNullOrEmpty(ftpUserID) && !string.IsNullOrEmpty(ftpPassword))
                {
                    FtpWeb fw = new FtpWeb(ftpServerIP, folderToDelete, ftpUserID, ftpPassword);
                    //进入订单目录
                    fw.GotoDirectory(folderToDelete, true);
                    //获取规格目录
                    string[] folders = fw.GetDirectoryList();
                    foreach (string folder in folders)
                    {
                        if (!string.IsNullOrEmpty(folder) || folder != "")
                        {
                            //进入订单目录
                            string subFolder = folderToDelete + "/" + folder;
                            fw.GotoDirectory(subFolder, true);
                            //获取文件列表
                            string[] files = fw.GetFileList("*.*");
                            if (files != null)
                            {
                                //删除文件
                                foreach (string file in files)
                                {
                                    fw.Delete(file);
                                }
                            }
                            //删除冲印规格文件夹
                            fw.GotoDirectory(folderToDelete, true);
                            fw.RemoveDirectory(folder);
                        }
                    }

                    //删除订单文件夹
                    string parentFolder = folderToDelete.Remove(folderToDelete.LastIndexOf('/'));
                    string orderFolder = folderToDelete.Substring(folderToDelete.LastIndexOf('/') + 1);
                    fw.GotoDirectory(parentFolder, true);
                    fw.RemoveDirectory(orderFolder);
                }
                else
                {
                    throw new Exception("FTP 及路径不能为空！");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("删除订单时发生错误，错误信息为：" + ex.Message);
            }
        }
    }


    public class Insert_Standard_ErrorLog
    {
        public static void Insert(string x, string y)
        {

        }
    }

    public class LuaFtpEditorWindow : EditorWindow
    {
        //Ftp路径
        string _strFtpDirectory = "";

        //Ftp地址
        string _strFtpAddress = "ksyx.update.iwodong.com";

        //Ftp账号
        string _strFtpAccount = "ksyx";

        //Ftp密码
        string _strFtpPassword = "ksyx@014";

        //帮助信息
        string _strHelpMsg = "luaCode将要上传的路径";
        MessageType _msgType = MessageType.Info;

        bool _toggle = false;

        // 操作步骤 0未开始，1生存完成，2上传中，3完成
        int _operStep = 0;

        string _operStr = "等待生成文件";

        string _platformPath = null;

        string _tempFile = null;

        string _uploadPath = "luaCode/";

        List<string> _fileList = null;

        FtpWeb _ftpWeb = null;

        int _allFileCnt = 0;
        int _uploadCnt = 0;
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //初始化
        public void initialize(string directory)
        {
            _ftpWeb = new FtpWeb(_strFtpAddress, _strFtpDirectory, _strFtpAccount, _strFtpPassword);
            _strFtpDirectory = directory;
            _platformPath = Application.streamingAssetsPath + "/" + customerPath.IPath.getPlatformName() + "/";
        }

        private void moveFile(string copyFile, string endFilePath)
        {
            string direName = Path.GetDirectoryName(endFilePath);
            if (!System.IO.Directory.Exists(direName))
            {
                System.IO.Directory.CreateDirectory(direName);
            }
            System.IO.File.Copy(copyFile, endFilePath);
        }

        private void CloseWin()
        {
            this.Close();
            if (_toggle)
            {
                if (System.IO.Directory.Exists(_platformPath + "luaCode"))
                {
                    System.IO.Directory.Delete(_platformPath + "luaCode", true);
                }
            }
        }

        //绘制窗口内容
        void OnGUI()
        {
            EditorGUILayout.HelpBox(_strHelpMsg, _msgType, true);
            EditorGUILayout.Separator();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("路径：" + _strFtpAddress);
            _strFtpDirectory = EditorGUILayout.TextField(_strFtpDirectory, GUILayout.Width(300), GUILayout.ExpandWidth(true));
            if (_ftpWeb != null)
            {
                _ftpWeb.GotoDirectory(_strFtpDirectory, true);
            }
            
            EditorGUILayout.EndHorizontal();

            if (_operStep != 2)
            {
                EditorGUILayout.BeginHorizontal();
                _toggle = EditorGUILayout.Toggle("", _toggle, GUILayout.Width(15));
                EditorGUILayout.LabelField("删除本地临时luaCode文件夹");
                EditorGUILayout.EndHorizontal();
            }
            
            //操作按钮
            EditorGUILayout.BeginHorizontal();

            switch (_operStep)
            {
                case 0:
                    if (GUILayout.Button("生成luaCode", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
                    {
                        _operStep = 1;
                        
                        if (System.IO.Directory.Exists(_platformPath + "luaCode"))
                        {
                            System.IO.Directory.Delete(_platformPath + "luaCode", true);
                        }
                        string[] metaFiles = Directory.GetFiles(_platformPath + "luaScripts", "*.lua", SearchOption.AllDirectories);
                        string fileCodePath = _platformPath + "fileCode.lua";
                        if (System.IO.File.Exists(fileCodePath))
                        {
                            moveFile(fileCodePath, _platformPath + "luaCode/fileCode.lua");
                        }

                        foreach (string file in metaFiles)
                        {
                            string copyFile = file.Replace("\\", "/");
                            string endFilePath = copyFile.Replace(_platformPath, _platformPath + "luaCode/");
                            moveFile(copyFile, endFilePath);
                        }
                        _operStr = "文件生成成功，等待上传";
                    }

                    if (GUILayout.Button("取消", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
                    {
                        this.CloseWin();
                    }
                    break;
                case 1:
                    //this.position = new Rect(960, 540, 515, 145);
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("要上传的文件夹：", GUILayout.Width(100));
                    _uploadPath = EditorGUILayout.TextField(_uploadPath, GUILayout.Width(300), GUILayout.ExpandWidth(true));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("上传", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
                    {
                        string[] files = Directory.GetFiles(_platformPath + _uploadPath, "*.lua", SearchOption.AllDirectories);
                        _fileList = new List<string>();
                        foreach (string file in files)
                        {
                            _fileList.Add(file);
                        }
                        _allFileCnt = _fileList.Count;
                        _operStep = 2;
                    }

                    if (GUILayout.Button("取消", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
                    {
                        this.CloseWin();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    break;
                case 2:
                    {
                        if (_fileList.Count > 0)
                        {
                            if (_tempFile == null)
                            {
                                _tempFile = _fileList[0];
                                _uploadCnt += 1;
                                _operStr = "上传->" + _tempFile.Replace(_platformPath, "") + "   ....................(" + _uploadCnt + "/" + _allFileCnt + ")";
                                //this.Repaint();
                            }
                            else
                            {
                                _ftpWeb.UploadPath(_tempFile, _platformPath);
                                _fileList.RemoveAt(0);
                                _tempFile = null;
                            }
                        }
                        else
                        {
                            _operStep = 3;
                            _operStr = "上传完成";
                        }
                        //this.position = new Rect(960, 540, 515, 110);
                    }
                    break;
                case 3:
                    //this.position = new Rect(960, 540, 515, 135);
                    if (GUILayout.Button("完成", GUILayout.Width(100), GUILayout.ExpandWidth(true)))
                    {
                        this.CloseWin();
                    }
                    break;
            }
            

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(_operStr);
        }


    }
}



