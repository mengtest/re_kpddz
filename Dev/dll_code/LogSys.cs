/***************************************************************
 * Copyright (c) 2013 福建沃动计算机技术有限公司 
 *         All rights reserved. 
 *
 *
 * Filename:  	LogSys.cs	
 * Summary: 	自定义log系统，支持配置LOG输出方式
 *              TODO: 增加 logConfig.xml  
 * 
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2015/03/23 17:29
 ***************************************************************/

#region Using
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#endregion

namespace Utils
{
    public class LogSys : MonoBehaviour
    {
        /// <summary>
        /// 用于显示的LOG列表
        /// </summary>
        static public List<string> _listLogs = new List<string>();

        /// <summary>
        /// 用于显示的LOG颜色
        /// </summary>
        static public List<Color> _listLogColor = new List<Color>();

        /// <summary>
        /// 是否开启LOG
        /// </summary>
        static public bool _bEnableLog = true;

        /// <summary>
        /// 是否开启屏幕LOG
        /// </summary>
        static public bool _bEnableScreenLog = false;

        /// <summary>
        /// LOG文件名
        /// </summary>
        static public string _strLogFileName = "/log.txt";

        static private List<string> _listWriteText = new List<string>();

        ///////////////////////////////////////////////////////////////////


        // Update 每帧调用一次
        void Update()
        {

        }

        public static void OnGUI()
        {
            if (!_bEnableScreenLog)
            {
                return;
            }

            //打印LOG
            for (int i = 0; i < _listLogs.Count; i++)
            {
                GUI.color = _listLogColor[i];
                GUILayout.Label(_listLogs[i]);
            }
        }

        ///////////////////////////////////////////////////////////////////

        /// <summary>
        /// 静态初始化
        /// </summary>
        static LogSys()
        {
            Application.RegisterLogCallback(HandleLog);

            //LOG文件处理,只保留最近一次启动的LOG
            string strLogPath = Application.persistentDataPath + _strLogFileName;
            if (System.IO.File.Exists(strLogPath))
            {
                System.IO.File.Delete(strLogPath);
            }
        }

		static public void Log(object message)
        {
            Log(message, null);
        }
		
        static public void Log(object message, Object context)
        {
            if (_bEnableLog)
            {
                Debug.Log(message, context);
            }
        }


        static public void LogError(object message)
        {
            LogError(message, null);
        }
        
        static public void LogError(object message, Object context)
        {
            if (_bEnableLog)
            {
                Debug.LogError(message, context);
            }
        }
       
        static public void LogWarning(object message)
        {
            LogWarning(message, null);
        }
       
        static public void LogWarning(object message, Object context)
        {
            if (_bEnableLog)
            {
                Debug.LogWarning(message, context);
            }
        }

        //LOG处理句柄
        static void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (_listLogs.Count > 20)
            {
                _listLogs.RemoveAt(0);
                _listLogColor.RemoveAt(0);
            }

           
            string strPrefix = "[Normal] ";

            //颜色
            switch (type)
            {
                case LogType.Error:
                    strPrefix = "[ERROR] ";
                    _listLogColor.Add(Color.red);
                    break;
                case LogType.Warning:
                    strPrefix = "[WARNING] ";
                    _listLogColor.Add(Color.yellow);
                    break;
                default:
                     _listLogColor.Add(Color.white);
                    break;
            }

            logString = strPrefix + logString;
            _listLogs.Add(logString);
            _listWriteText.Add(logString);
        }

        /// <summary>
        /// 输出到LOG文件
        /// </summary>
        static public void outputLogFile()
        {
            if (_listWriteText.Count > 0)
            {
                string strLogPath = Application.persistentDataPath + _strLogFileName;
                LogError(strLogPath);

                string[] temp = _listWriteText.ToArray();
                foreach (string t in temp)
                {
                    using (System.IO.StreamWriter writer = new System.IO.StreamWriter(strLogPath, true, System.Text.Encoding.UTF8))
                    {
                        writer.WriteLine(t);
                    }
                    _listWriteText.Remove(t);
                }
            }
        }
    }

}

