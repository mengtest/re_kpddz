/***************************************************************


 *
 *
 * Filename:  	luaValueFactory.cs	
 * Summary: 	工厂模块，用于动态创建值类型
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/25 3:35
 ***************************************************************/


using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

namespace sluaAux.proto
{
    /// <summary>
    /// 工厂接口类
    /// </summary>
    internal interface IValueFactory
    {
        IFieldValue createValue(EFieldModifier modifier);
    }

    ////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 值类型工厂
    /// </summary>
    /// <typeparam name="T">值类型</typeparam>
    internal class valueFactory<T> : IValueFactory
    {
        /// <summary>
        /// 创建值对象
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public IFieldValue createValue(EFieldModifier modifier)
        {
            switch (modifier)
            {
                case EFieldModifier.required:
                    return new requiedFieldValue<T>();
                case EFieldModifier.optional:
                    return new optionalFieldValue<T>();
                case EFieldModifier.repeated:
                    return new repeatedFieldValue<T>();
                default:
                    return null;
            }
        }
    }

    ////////////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// 工厂管理
    /// </summary>
    public class valueFactroyManager
    {
        //工厂集合
        static Dictionary<ProtoTypeCode, IValueFactory> _factories = new Dictionary<ProtoTypeCode, IValueFactory>();

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 添加工厂类
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="factory"></param>
        static internal void addFactory(ProtoTypeCode typeCode, IValueFactory factory)
        {
            if (factory == null)
                return;

            if (_factories.ContainsKey(typeCode))
                return;

            _factories[typeCode] = factory;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 获取对应类型的工厂
        /// </summary>
        /// <param name="typeCode"></param>
        /// <returns></returns>
        static internal IValueFactory getFactory(ProtoTypeCode typeCode)
        {
            IValueFactory value = null;
            if (_factories.TryGetValue(typeCode, out value))
                return value;

            return null;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 初始化工厂
        /// </summary>
        static internal void initialize()
        {
            addFactory(ProtoTypeCode.Boolean, new valueFactory<bool>());
            addFactory(ProtoTypeCode.Int16, new valueFactory<short>());
            addFactory(ProtoTypeCode.UInt16, new valueFactory<ushort>());
            addFactory(ProtoTypeCode.Int32, new valueFactory<int>());
            addFactory(ProtoTypeCode.UInt32, new valueFactory<uint>());
            addFactory(ProtoTypeCode.Int64, new valueFactory<long>());
            addFactory(ProtoTypeCode.UInt64, new valueFactory<ulong>());
            addFactory(ProtoTypeCode.Single, new valueFactory<float>());
            addFactory(ProtoTypeCode.Double, new valueFactory<double>());
            addFactory(ProtoTypeCode.String, new valueFactory<string>());
            addFactory(ProtoTypeCode.ByteArray, new valueFactory<byte[]>());
            addFactory(ProtoTypeCode.Type, new valueFactory<luaMessage>());
        }
    }
}



