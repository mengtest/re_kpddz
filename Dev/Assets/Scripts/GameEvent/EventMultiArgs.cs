/***************************************************************


 *
 *
 * Filename:  	EventMultiArgs.cs	
 * Summary: 	事件参数
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/17 19:22
 ***************************************************************/

using System;
using System.Collections.Generic;
using Utils;

namespace EventManager
{
    [SLua.CustomLuaClass]
    public class EventMultiArgs : EventArgs
    {
        private Dictionary<string, object> dicArgsValue = new Dictionary<string, object>();

        public void AddArg(string key, object value)
        {
            dicArgsValue[key] = value;
        }

        public T GetArg<T>(string key, T defaultValue)
        {
            if (dicArgsValue.ContainsKey(key))
            {
                object val = dicArgsValue[key];
                if (val is T)
                {
                    return (T)val;
                }
                LogSys.LogError("GetArg Error!! the event arg '" + key + "' type is " + val.GetType() + " not " + defaultValue.GetType());
            }
            else
            {
                LogSys.LogError("GetArg Error!! the event arg '" + key + "' not exist");
            }
            return defaultValue;
        }

        public T GetArg<T>(string key)
        {
            if (dicArgsValue.ContainsKey(key))
            {
                object val = dicArgsValue[key];
                if (val is T)
                {
                    return (T)val;
                }

                if (key != "EVENT_ID")
                {
                    uint id = GetArg<uint>("EVENT_ID");
                    LogSys.LogError("GetArg Error!! the event " + id + "arg type error: '" + key + "' type is " + val.GetType() + ", not " + default(T).GetType());
                }
            }
            else
            {
                LogSys.LogError("GetArg Error!! the event arg '" + key + "' not exist");
            }
            return default(T);
        }

        public bool ContainsKey(string sKey)
        {
            return dicArgsValue.ContainsKey(sKey);
        }

        internal Dictionary<string, object> GetDictionary()
        {
            return dicArgsValue;
        }
    }

}