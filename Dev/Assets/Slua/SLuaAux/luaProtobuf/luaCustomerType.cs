/***************************************************************


 *
 *
 * Filename:  	luaCustomerType.cs	
 * Summary: 	自定义值类型
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/27 5:13
 ***************************************************************/


using UnityEngine;
using System.Collections;

namespace sluaAux.proto
{
    /// <summary>
    /// 废弃提示信息
    /// </summary>
    class obsoleteMsg
    {
        public const string MSG = "This extension type is not to use";
    }

    /// <summary>
    /// Lua类型基类
    /// </summary>
    [System.Obsolete(obsoleteMsg.MSG)]
    public abstract class luaVar
    {
        protected object _value;

        public object Value
        {
            get { return _value; }
        }

        public luaVar(object value)
        {}
    }

    /// <summary>
    /// lua有符号32位整数
    /// </summary>
    [System.Obsolete(obsoleteMsg.MSG)]
    public class luaInt32 : luaVar
    {
        public luaInt32(object luaNumber)
            :base(luaNumber)
        {
            if (luaNumber.GetType() == typeof(double) || luaNumber.GetType() == typeof(float))
            {
                double d = (double)luaNumber;
                _value = (int)d;
            }
            else 
                _value = (int)luaNumber;
        }
    }

    /// <summary>
    /// lua字符串值
    /// </summary>
    [System.Obsolete(obsoleteMsg.MSG)]
    public class luaValueString
    {
        string _value = null;

        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public luaValueString()
        {
        }

        public luaValueString(string str)
        {
            _value = str;
        }
    }

    /// <summary>
    /// 字节流
    /// </summary>
    [System.Obsolete(obsoleteMsg.MSG)]
    public class luaValueByteArray
    {
        byte[] _value = null;

        public byte[] Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public luaValueByteArray()
        {
        }

        public luaValueByteArray(byte[] arr)
        {
            _value = arr;
        }
    }

}
