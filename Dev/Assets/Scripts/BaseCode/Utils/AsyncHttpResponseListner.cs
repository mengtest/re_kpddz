using UnityEngine;
using System.Collections;


namespace Utils
{
    public abstract class AsyncHttpResponseListner : MonoBehaviour
    {
        public abstract void onStartAbstract();
        public abstract void onProgressAbstract(long currentSize, long totalSize);
        public abstract void onSuccessAbstract(string statusCode, string filePath, string msg);
        public abstract void onFailureAbstract(string statusCode, string msg);
        public abstract void onFinishAbstract();
        public abstract void onRetryAbstract();

        public void onStart(string msg)
        {
            onStartAbstract();
        }
        public void onProgress(string msg)
        {
            //Debug.Log("onProgress( msg: " + msg + ")");
            var data = new JSONObject(msg);
            onProgressAbstract((long)data["currentSize"].f, (long)data["totalSize"].f);
        }
        public void onSuccess(string msg)
        {
            Debug.Log("onSuccess( msg: " + msg + ")");
            var data = new JSONObject(msg);
            string statusCode = "0";
            string filePath = subStringSlash(data["filePath"].ToString());
            string message = "";
            
            filePath.ToCharArray();
            //Debug.Log("onSuccess( statusCode: " + statusCode + ",filePath:" + filePath + ", message:" + message +")");
            onSuccessAbstract(statusCode, filePath, message);
        }

        public void onFailure(string msg)
        {
            var data = new JSONObject(msg);
            string statusCode = subString(data["statusCode"].ToString());
            string message = subString(data["msg"].ToString());
            onFailureAbstract(statusCode, message);
        }

        public void onFinish(string msg)
        {
            onFinishAbstract();
        }

        public void onRetry(string msg)
        {
            onRetryAbstract();
        }

        string subString(string str)
        {
            return str.Replace("\"", "");
        }

        string subStringSlash(string str)
        {
            return subString(str.Replace("\\", ""));
        }
    }
}



