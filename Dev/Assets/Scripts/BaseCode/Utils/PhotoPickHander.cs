using UnityEngine;
using System.Collections;


namespace Utils
{
    public abstract class PhotoPickHander : MonoBehaviour
    {
        public  abstract void onSuccessObject(string result, string filePath);
        public abstract void onFailureObject(string error, string msg);

        public void onSuccess(string msg)
        {
            var data = new JSONObject(msg);
            string result = subString(data["result"].ToString());
            string filePath = subStringSlash(data["filePath"].ToString());
            onSuccessObject(result, filePath);
        }

        public void onFailure(string msg)
        {
            var data = new JSONObject(msg);
            string error = subString(data["error"].ToString());
            string message = subString(data["msg"].ToString());
            onFailureObject(error, message);
        }       string subString(string str)
        {
            return str.Replace("\"", "");
        }
        string subStringSlash(string str)
        {
            return subString(str.Replace("\\", ""));
        }
    }
}


