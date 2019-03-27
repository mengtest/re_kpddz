using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.IO;

namespace Utils
{
    public class AsyncHttpUpDown : Singleton<AsyncHttpUpDown>
    {
        public delegate void delegateOnUpFinsh(int errorCode, string result);
        public delegate void delegateOnDownFinsh(int errorCode, string result);
        public delegate void delegateOnDownTextureFinsh(int errorCode, Texture2D tex);
        public AsyncHttpUpDown()
        {

        }
        /// <summary>
        /// 上传file(post方式)
        /// </summary>
        /// <param name="httpUrl">上传地址</param>
        /// <param name="fileAbsolutePath">本地文件地址</param>
        /// <param name="remoteFile">远端文件名</param>
        /// <param name="callback">上传完成回调</param>
        public void PostUpload(string httpUrl, string fileAbsolutePath, string remoteFile, delegateOnUpFinsh callback)
        {
            if (System.IO.File.Exists(fileAbsolutePath))
            {
                byte[] byteFile = System.IO.File.ReadAllBytes(fileAbsolutePath);
                PostUpload(httpUrl, byteFile, remoteFile, callback);
            }
        }

        //上传data(post方式)
        public void PostUpload(string httpUrl, byte[] byteData, string remoteFile, delegateOnUpFinsh callback)
        {
            StartCoroutine(PostUploadAysnc(httpUrl, byteData, remoteFile, callback));
        }

        IEnumerator PostUploadAysnc(string httpUrl, byte[] byteData, string remoteFile, delegateOnUpFinsh callback)
        {
            WWWForm dataForm = new WWWForm();
            dataForm.AddBinaryData("file", byteData, remoteFile);
            dataForm.AddField("fileName", remoteFile);

            var www = UnityWebRequest.Post(httpUrl, dataForm);
            yield return www.Send();
            if (www.isError)
            {
                www.Dispose();
                yield return new WaitForSeconds(0.5f);
                www = UnityWebRequest.Post(httpUrl, dataForm);
                yield return www.Send();
            }
            if (www.isError)
            {
                www.Dispose();
                yield return new WaitForSeconds(0.5f);
                www = UnityWebRequest.Post(httpUrl, dataForm);
                yield return www.Send();
            }
            if (www.isDone)
            {
                if (!www.isError)
                {
                    if (callback != null)
                    {
                        callback(0, www.downloadHandler.text);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(1, "");
                    }
                }
                www.Dispose();
            }
        }

        //上传file(put方式)
        public void UploadPut()
        {
        }

        //上传data(put方式)
        public void UploadPut(byte[] byteData)
        {

        }

        //下载文件
        public void DownloadFile(string downloadUrl, string saveFilePath, delegateOnDownFinsh callback)
        {
            StartCoroutine(DownloadAsync(downloadUrl, saveFilePath, callback));
        }

        IEnumerator DownloadAsync(string downloadUrl, string saveFilePath, delegateOnDownFinsh callback)
        {
            UnityWebRequest www = UnityWebRequest.Get(downloadUrl);
            yield return www.Send();

            if (www.isError)
            {
                www.Dispose();
                yield return new WaitForSeconds(0.3f);
                www = UnityWebRequest.Get(downloadUrl);
                yield return www.Send();
            }
            if (www.isError)
            {
                www.Dispose();
                yield return new WaitForSeconds(0.3f);
                www = UnityWebRequest.Get(downloadUrl);
                yield return www.Send();
                Debug.Log(www.error);
            }
            if (www.isDone)
            {
                if (!www.isError)
                {
                    byte[] results = www.downloadHandler.data;
                    SaveSoundFile(saveFilePath, results);
                    yield return new WaitForEndOfFrame();
                    if (callback != null)
                    {
                        callback(0, "");
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(1, www.error);
                    }
                }
                www.Dispose();
            }
        }


        //下载图片
        public void DownloadTexture(string downloadUrl, string saveFilePath, delegateOnDownTextureFinsh callback)
        {
            StartCoroutine(DownloadTextureAsync(downloadUrl, saveFilePath, callback));
        }

        IEnumerator DownloadTextureAsync(string downloadUrl, string saveFilePath, delegateOnDownTextureFinsh callback)
        {
            WWW www = new WWW(downloadUrl);//UnityWebRequestTexture.GetTexture
            yield return www;

            if (!string.IsNullOrEmpty(www.error))
            {
                www.Dispose();
                yield return new WaitForSeconds(0.3f);
                www = new WWW(downloadUrl);
                yield return www;
            }
            if (!string.IsNullOrEmpty(www.error))
            {
                www.Dispose();
                yield return new WaitForSeconds(0.3f);
                www = new WWW(downloadUrl);
                yield return www;
            }
            if (www.isDone)
            {
                if (string.IsNullOrEmpty(www.error))
                {
                    Texture2D tex2d = www.texture;
                    byte[] bys = tex2d.EncodeToPNG();//转换图片资源  
                    SaveSoundFile(saveFilePath, bys);
                    yield return new WaitForEndOfFrame();
                    if (callback != null)
                    {
                        callback(0, tex2d);
                    }
                }
                else
                {
                    if (callback != null)
                    {
                        callback(1, null);
                    }
                }
                www.Dispose();
            }
        }

        void SaveSoundFile(string filePath, byte[] fileData)
        {
            //创建临时文件
            string tempFilePath = BetterString.Builder(filePath, "tmp");
            FileStream stream;
            try
            {
                stream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write);
                //stream = File.Create(tempFilePath);
            }
            catch (System.Exception ex)
            {
                Debug.LogException(ex);
                return;
            }

            Utils.LogSys.Log("LoadHead10");
            //写临时文件
            try
            {
                stream.Write(fileData, 0, fileData.Length);
                stream.Flush();
                stream.Close();
            }
            catch (System.Exception e)
            {
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
                Debug.LogException(e);
                return;
            }
        }
    }
}



