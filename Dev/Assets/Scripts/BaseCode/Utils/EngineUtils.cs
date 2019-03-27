using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using UnityEngine;

public static class EngineUtils
{
    public delegate void SelectionFun();

    public static int ToInt(string value)
    {
        switch (value)
        {
            case "true":
                return 1;
            case "True":
                return 1;
            case "False":
                return 0;
            case "false":
                return 0;
            default:
                return string.IsNullOrEmpty(value) ? 0 : Convert.ToInt32(value);
        }
    }


    public static float ToFloat(string value)
    {
        switch (value)
        {
            case "true":
                return 1f;
            case "True":
                return 1f;
            case "False":
                return 0f;
            case "false":
                return 0f;
            default:
                return string.IsNullOrEmpty(value) ? 0f: Convert.ToSingle(value);
        }
    }

    public static bool ToBoolean(string value)
    {
        switch (value)
        {
            case "true":
                return true;
            case "True":
                return true;
            case "1":
                return true;
            case "0":
                return false;
            case "False":
                return false;
            case "false":
                return false;
            case "TRUE":
                return true;
            case "FALSE":
                return false;
            default:
                return true;
        }
    }

    public static bool VersionsCompare(string local, string server)
    {
        if (local == string.Empty) return true;
        local = local.Split('_')[0];
        string[] localArr = local.Split('.');
        server = server.Split('_')[0];
        string[] serverArr = server.Split('.');
        int index = 0;
        while (index < 4)
        {
            if (Convert.ToInt32(serverArr[index]) > Convert.ToInt32(localArr[index]))
            {
                return true;
            }
            index++;
        }
        return false;
    }

    public static string Domain2ip(string str)
    {
        return str;
        string ip = "";
#if UNITY_WP8
        //WP8 域名转IP 找不到对应API
        return str;
#else
        ip = str.Replace("http://", "");
        int index = ip.IndexOf(":");
        string port = index != -1 ? ip.Substring(index) : "";
        if (!"".Equals(port.Trim())) ip = ip.Replace(port, "");
        try
        {
            IPHostEntry hostinfo = Dns.GetHostEntry(ip);
            IPAddress[] aryIP = hostinfo.AddressList;
            ip = aryIP[0].ToString();
        }
        catch (Exception)
        {
            return str;
        }
        if (str.Contains("http://")) ip = "http://" + ip;
        ip = ip + port;
        return ip;
#endif
    }
}
