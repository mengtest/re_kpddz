using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TextUtils
{
    public static string GetString(byte[] bytes)
    {
#if UNITY_WP8
        return UTF8Encoding.UTF8.GetString(bytes, 0, bytes.Length);
#else
        return Encoding.UTF8.GetString(bytes);
#endif
    }

    public static byte[] GetBytes(string str)
    {
#if UNITY_WP8
        return UTF8Encoding.UTF8.GetBytes(str);
#else
        return Encoding.UTF8.GetBytes(str);
#endif
    }

    public static string MD5(byte[] emailBytes)
    {
        if (emailBytes == null || emailBytes.Length <= 0) return "";
        var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashedEmailBytes = md5.ComputeHash(emailBytes);
        StringBuilder sb = new StringBuilder();
        foreach (var b in hashedEmailBytes)
        {
            sb.Append(b.ToString("x2").ToLower());
        }
        return sb.ToString();
    }

    public static string MD5(string stringToHash)
    {
        byte[] emailBytes = Encoding.UTF8.GetBytes(stringToHash.ToLower());
        return MD5(emailBytes);
    }
}
