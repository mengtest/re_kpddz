/***************************************************************


 *
 *
 * Filename:  	luaProtoMetadata.cs	
 * Summary: 	lua protobuf消息定义元数据描述
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/23 17:33
 ***************************************************************/


using UnityEngine;
using System.Collections;
using ProtoBuf;
using ProtoBuf.Serializers;
using System.Collections.Generic;
using System;
using System.Text;
using Utils;

namespace sluaAux.proto
{
    //字段修饰符
    public enum EFieldModifier
    {
        none,
        required,
        optional,
        repeated,
    }

    //*********************************************************************************

    /// <summary>
    /// 字段描述信息
    /// </summary>
    public class fieldDescriptor
    {
        //修饰符
        public EFieldModifier modifier = EFieldModifier.required;

        //类型代码
        public ProtoTypeCode typeCode = ProtoTypeCode.String;

        //字段名称
        public string name = "";

        //嵌套类型
        public string nestedTypeName = "";

        //wireType
        public WireType wireType = WireType.None;


        /// <summary>
        /// 创建描述信息
        /// </summary>
        /// <param name="modifier">字段修饰符</param>
        /// <param name="typeCode">字段类型</param>
        /// <param name="name">字段名字</param>
        /// <param name="nestedTypeName">嵌套类型的名字</param>
        /// <returns></returns>
        public static fieldDescriptor createDescriptor(EFieldModifier modifier, ProtoTypeCode typeCode, string name, string nestedTypeName = "")
        {
            if (modifier == EFieldModifier.none
                || typeCode == ProtoTypeCode.Empty
                || string.IsNullOrEmpty(name))
                return null;


            return new fieldDescriptor(modifier, typeCode, name, nestedTypeName);
        }

        /// <summary>
        /// 拷贝创建描述信息
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static fieldDescriptor copyCreateDescriptor(fieldDescriptor other)
        {
            if (other == null)
                return null;

            return createDescriptor(other.modifier, other.typeCode, other.name, other.nestedTypeName);
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="modifier">字段修饰符</param>
        /// <param name="typeCode">字段类型</param>
        /// <param name="name">字段名字</param>
        /// <param name="nestTypeName">嵌套类型的名字</param>
        private fieldDescriptor(EFieldModifier modifier, ProtoTypeCode typeCode, string name, string nestedTypeName = "")
        {
            this.modifier = modifier;
            this.typeCode = typeCode;
            this.name = name;
            wireType = luaProtoHelper.getWireType(typeCode);
            this.nestedTypeName = nestedTypeName;
        }

        /// <summary>
        /// 是否为列表
        /// </summary>
        public bool isRepeated()
        {
            return modifier == EFieldModifier.repeated;
        }

        /// <summary>
        /// 是否为嵌入类型
        /// </summary>
        /// <returns></returns>
        public bool haveNestedType()
        {
            return !string.IsNullOrEmpty(nestedTypeName);
        }

        /// <summary>
        /// TODO: 描述信息缓存，通过hash值来索引，减少描述信息对象的创建
        /// </summary>
        /// <returns></returns>

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    //*********************************************************************************

    /// <summary>
    /// 值类型接口
    /// </summary>
    public interface IFieldValue
    {
        object valueL { get; set; }
        object defaultValue();
        void addChild(object child);
        int count();
        object this[int idx] { get; }
    }

    //*********************************************************************************


    /// <summary>
    /// requied字段值类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class requiedFieldValue<T> : IFieldValue
    {
        T _value;

        public object valueL
        {
            get { return _value; }
            set { _value = (T)value; }
        }

        public object defaultValue()
        {
            return null;
        }

        public void addChild(object child)
        {
        }

        public int count()
        {
            return 1;
        }

        public object this[int idx]
        {
            get { return _value; }
        }
    }

    //*********************************************************************************

    /// <summary>
    /// optional字段值类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class optionalFieldValue<T> : IFieldValue
    {
        T _value;

        public object valueL
        {
            get { return _value; }
            set { _value = (T)value; }
        }

        public object defaultValue()
        {
            return default(T);
        }

        public void addChild(object child)
        {
        }

        public int count()
        {
            return 1;
        }

        public object this[int idx]
        {
            get { return _value; }
        }
    }

    //*********************************************************************************

    /// <summary>
    /// repeated字段值类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class repeatedFieldValue<T> : IFieldValue
    {
        List<T> _value = new List<T>();

        public object valueL
        {
            get { return _value; }
            set { _value = (List<T>)value; }
        }

        public object defaultValue()
        {
            return new List<T>();
        }

        public void addChild(object child)
        {
            if (child.GetType() == typeof(double))
                child = (double)child;

            _value.Add((T)child);
        }

        public int count()
        {
            return _value.Count;
        }

        public object this[int idx]
        {
            get {
                if (idx < _value.Count)
                    return _value[idx];

                return null;
            }
        }

    }

    //*********************************************************************************

    /// <summary>
    /// 字段信息
    /// 
    /// 两个部分组成，描述信息和值
    ///    描述信息： 读取配置文件生成
    ///    值： 序列化或反序列化动态附加
    /// </summary>
    public class fieldDataInfo
    {
        //索引
        int _number = 0;

        //描述信息
        fieldDescriptor _decriptor = null;

        //值信息
        IFieldValue _value = null;

        #region 属性

        /// <summary>
        /// 索引
        /// </summary>
        public int number
        {
            get { return _number; }
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public fieldDescriptor descriptor
        {
            get { return _decriptor; }
        }

        //值信息
        public IFieldValue Value
        {
            get { return _value; }
        }

        #endregion

        ////////////////////////////////////////////////////////

        /// <summary>
        /// 创建字段信息
        /// </summary>
        public static fieldDataInfo createFieldData(int number, fieldDescriptor descriptor)
        {
            if (number <= 0 || descriptor == null)
                return null;

            return new fieldDataInfo(number, descriptor);
        }


        /// <summary>
        /// 构造函数，只需传入描述信息
        /// </summary>
        private fieldDataInfo(int number, fieldDescriptor descriptor)
        {
            _number = number;
            _decriptor = descriptor;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// 附加值
        /// </summary>
        public bool appendValue()
        {
            if (_decriptor == null)
                return false;

            var valueFctry = valueFactroyManager.getFactory(_decriptor.typeCode);
            if (valueFctry != null)
            {
                _value = valueFctry.createValue(_decriptor.modifier);
                return true;
            }

            return false;
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// 清理值信息
        /// </summary>
        public void clear()
        {
            _value = null;
        }
    }

    //*********************************************************************************

    /// <summary>
    /// Lua proto消息定义
    /// </summary>
    public class luaMessage : IFieldValue
    {
        //消息key值
        object _idMsg = null;

        //字段信息
        Dictionary<int, fieldDataInfo> _fields = null;

        #region 属性

        public object idMsg
        {
            get { return _idMsg; }
            set { _idMsg = value; }
        }

        #endregion

        ////////////////////////////////////////////////////////

        public luaMessage(object idMsg)
        {
            _idMsg = idMsg;
            _fields = new Dictionary<int, fieldDataInfo>();
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// 添加字段
        /// </summary>
        /// <param name="number">字段索引</param>
        /// <param name="field">字段信息</param>
        public void addField(int number, fieldDataInfo field)
        {
            if (field == null)
                return;

            if (_fields.ContainsKey(number))
                return;

            _fields.Add(number, field);
        }

        ////////////////////////////////////////////////////////

        /// <summary>
        /// 字段个数
        /// </summary>
        /// <returns></returns>
        public int fieldsCount()
        {
            return _fields.Count;
        }

        //打印内容
        public string logStr(int s = 0)
        {
            string space = new string(' ', s);
            StringBuilder strBld = new StringBuilder();
            strBld.Append(space + "{\r\n");

            foreach (var kvp in _fields)
            {
                string modifier = kvp.Value.descriptor.modifier.ToString();
                string name = kvp.Value.descriptor.name;

                strBld.Append(space + space + modifier);
                strBld.Append(" ");
                strBld.Append(name);
                strBld.Append(" = ");

                if (kvp.Value != null)
                {
                    if (kvp.Value.descriptor.isRepeated())
                    {
                        int count = kvp.Value.Value.count();
                        strBld.Append("[\n");
                        strBld.Append(space + space);
                        for (int i=0; i<count; i++)
                        {
                            if (kvp.Value.descriptor.haveNestedType())
                                strBld.Append(((luaMessage)kvp.Value.Value[i]).logStr(s + 4)).Append(",\n");
                            else
                                strBld.Append(kvp.Value.Value[i].ToString()).Append(", ");
                        }

                        strBld.Append(System.Environment.NewLine).Append(space + space + "]");
                    }
                    else
                    {
                        if (kvp.Value.descriptor.haveNestedType())
                            strBld.Append(((luaMessage)kvp.Value.Value.valueL).logStr(s + 4));
                        else
                        {
                            if (kvp.Value.Value.valueL.GetType() == typeof(byte[]))
                                strBld.Append(System.Text.Encoding.UTF8.GetString((byte[])kvp.Value.Value.valueL));
                            else
                                strBld.Append(kvp.Value.Value.valueL.ToString());
                        }
                    }
                }

                strBld.Append(";").Append(System.Environment.NewLine);
            }

            strBld.Append(space + "}");


            return (strBld.ToString());
        }

        #region IFieldValue接口

        public object valueL
        {
            get { return _fields; }
            set { _fields = (Dictionary<int, fieldDataInfo>)value; }
        }

        public object defaultValue()
        {
            return new Dictionary<int, fieldDataInfo>();
        }

        public void addChild(object value)
        { }

        public int count()
        {
            return _fields.Count;
        }

        /// <summary>
        /// 索引器,获取字段信息
        /// </summary>
        /// <param name="number">字段索引</param>
        /// <returns></returns>
        public object this[int idx]
        {
            get
            {
                if (idx <= 0)
                    return null;

                fieldDataInfo fld = null;
                if (_fields.TryGetValue(idx, out fld))
                    return fld;

                return null;
            }
        }

        #endregion //IFieldValue
    }

}


