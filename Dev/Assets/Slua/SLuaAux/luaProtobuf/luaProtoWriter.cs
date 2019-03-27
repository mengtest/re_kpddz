/***************************************************************


 *
 *
 * Filename:  	luaProtoReader.cs	
 * Summary: 	向流中写入数据
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/24 17:41
 ***************************************************************/

using UnityEngine;
using System.Collections;
using System.IO;
using ProtoBuf;
using System;

namespace sluaAux.proto
{
    public class luaProtoWriter : IDisposable
    {
        //待写入流
        MemoryStream _stream = null;

        //缓存
        private byte[] _buff = null;

        //当前写入位置
        int _buffPosition = 0;

        //缓存位置
        int _position = 0;

        private int _flushLock = 0;

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="ms">目标内存流</param>
        /// <returns></returns>
        public static luaProtoWriter createLuaProtoWriter(MemoryStream ms)
        {
            if (ms == null)
                return null;

            return new luaProtoWriter(ms);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ms">目标内存流</param>
        public luaProtoWriter(MemoryStream ms)
        {
            _stream = ms;
            _buff = BufferPool.GetBuffer(); 
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入字段头
        /// </summary>
        /// <param name="fieldNumber"></param>
        /// <param name="wireType"></param>
        private void writeFieldHeader(int fieldNumber, WireType wireType)
        {
            uint header = (((uint)fieldNumber) << 3)
                | (((uint)wireType) & 7);
            writeUInt32Variant(header);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入lua对象
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="value"></param>
        /// <param name="wireType"></param>
        private void writeLuaObject(ProtoTypeCode typeCode, object value, WireType wireType)
        {
            if (typeCode == ProtoTypeCode.Empty) return;
            if (value == null) return;
            if (wireType == WireType.None) return;

            switch (typeCode)
            {
                case ProtoTypeCode.Int32:
                    writeInt32((int)value, wireType);
                    break;
                case ProtoTypeCode.UInt32:
                    writeUInt32((uint)value, wireType);
                    break;
                case ProtoTypeCode.Int64:
                    writeInt64((long)value, wireType);
                    break;
                case ProtoTypeCode.UInt64:
                    writeUInt64((ulong)value, wireType);
                    break;
                case ProtoTypeCode.Boolean:
                    writeBoolean((bool)value, wireType);
                    break;
                case ProtoTypeCode.ByteArray:
                    byte[] arr = (byte[])value;
                    writeBytes(arr, wireType);
                    break;
                case ProtoTypeCode.Single:
                    writeSingle((float)value, wireType);
                    break;
                case ProtoTypeCode.Double:
                    writeDouble((double)value, wireType);
                    break;
                case ProtoTypeCode.String:
                    writeString((string)value, wireType);
                    break;
                case ProtoTypeCode.Type:
                    int startIdx = startSubMessage(wireType, true);
                    writeLuaMessage((luaMessage)value);
                    endSubMessage(startIdx, PrefixStyle.Base128);
                    break;
                default:
                    throw createException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入消息字段
        /// </summary>
        /// <param name="number">字段索引</param>
        /// <param name="wireType">WireType代码</param>
        /// <param name="typeCode">类型代码</param>
        /// <param name="value">要写入的值</param>
        private void writeField(int number, WireType wireType, ProtoTypeCode typeCode, object value)
        {
            if (value == null)
                return;

            //字段头
            writeFieldHeader(number, wireType);
            //值
            writeLuaObject(typeCode, value, wireType);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool writeLuaMessage(luaMessage msg)
        {
            if (msg == null)
                return false;

            int nFieldCount = msg.count();
            for (int fieldIdx=1; fieldIdx<=nFieldCount; fieldIdx++)
            {
                var field = (fieldDataInfo)msg[fieldIdx];
                if (field.Value == null)
                    continue;

                var wireType = field.descriptor.wireType;
                var typeCode = field.descriptor.typeCode;
                
                //写入数据
                if (field.descriptor.isRepeated())
                {
                    int listCount = field.Value.count();
                    for (int i=0; i<listCount; i++)
                        writeField(field.number, wireType, typeCode, field.Value[i]);
                }
                else
                    writeField(field.number, wireType, typeCode, field.Value.valueL);
            }

            return true;
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入有符号32位整数
        /// 支持类型：Fixed32
        /// </summary>
        void writeInt32ToBuffer(int value, int idx=0)
        {
            int pos = (idx == 0) ?_buffPosition : idx;

            _buff[pos] = (byte)value;
            _buff[pos + 1] = (byte)(value >> 8);
            _buff[pos + 2] = (byte)(value >> 16);
            _buff[pos + 3] = (byte)(value >> 24);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入有符号32位整数
        /// 支持类型：Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public void writeInt32(int value, WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Fixed32:
                    demandSpace(4);
                    writeInt32ToBuffer(value);
                    incrementedAndReset(4);
                    return;
                case WireType.Fixed64:
                    demandSpace(8);
                    _buff[_buffPosition] = (byte)value;
                    _buff[_buffPosition + 1] = (byte)(value >> 8);
                    _buff[_buffPosition + 2] = (byte)(value >> 16);
                    _buff[_buffPosition + 3] = (byte)(value >> 24);
                    _buff[_buffPosition + 4] = _buff[_buffPosition + 5] =
                        _buff[_buffPosition + 6] = _buff[_buffPosition + 7] = 0;
                    incrementedAndReset(8);
                    return;
                case WireType.SignedVariant:
                    writeUInt32Variant(zig(value));
                    return;
                case WireType.Variant:
                    if (value >= 0)
                    {
                        writeUInt32Variant((uint)value);
                    }
                    else
                    {
                        demandSpace(10);
                        _buff[_buffPosition] = (byte)(value | 0x80);
                        _buff[_buffPosition + 1] = (byte)((value >> 7) | 0x80);
                        _buff[_buffPosition + 2] = (byte)((value >> 14) | 0x80);
                        _buff[_buffPosition + 3] = (byte)((value >> 21) | 0x80);
                        _buff[_buffPosition + 4] = (byte)((value >> 28) | 0x80);
                        _buff[_buffPosition + 5] = _buff[_buffPosition + 6] =
                            _buff[_buffPosition + 7] = _buff[_buffPosition + 8] = (byte)0xFF;
                        _buff[_buffPosition + 9] = (byte)0x01;
                        incrementedAndReset(10);
                    }
                    return;
                default:
                    throw createException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入一个无符号32位整数到流中
        /// 支持wireType : Variant, SignedVariant
        /// </summary>
        void writeUInt32Variant(uint value)
        {
            demandSpace(5);
            int count = 0;
            do
            {
                _buff[_buffPosition++] = (byte)((value & 0x7F) | 0x80);
                count++;
            } while ((value >>= 7) != 0);
            _buff[_buffPosition - 1] &= 0x7F;
            _position += count;
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入无符号32位整数
        /// 支持类型：Variant, Fixed32, Fixed64
        /// </summary>
        public void writeUInt32(uint value, WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Fixed32:
                    writeInt32((int)value, wireType);
                    return;
                case WireType.Fixed64:
                    writeInt64((int)value, wireType);
                    return;
                case WireType.Variant:
                    writeUInt32Variant(value);
                    return;
                default:
                    throw createException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入无符号64位整数
        /// 支持wireType : SignedVariant, Variant
        /// </summary>
        /// <param name="value"></param>
        void writeUInt64Variant(ulong value)
        {
            demandSpace(10);
            int count = 0;
            do
            {
                _buff[_buffPosition++] = (byte)((value & 0x7F) | 0x80);
                count++;
            } while ((value >>= 7) != 0);
            _buff[_buffPosition - 1] &= 0x7F;
            _position += count;
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入一个有符号64位整数到流中
        /// 支持wireType : Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public void writeInt64(long value, WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Fixed64:
                    demandSpace(8);
                    _buff[_buffPosition] = (byte)value;
                    _buff[_buffPosition + 1] = (byte)(value >> 8);
                    _buff[_buffPosition + 2] = (byte)(value >> 16);
                    _buff[_buffPosition + 3] = (byte)(value >> 24);
                    _buff[_buffPosition + 4] = (byte)(value >> 32);
                    _buff[_buffPosition + 5] = (byte)(value >> 40);
                    _buff[_buffPosition + 6] = (byte)(value >> 48);
                    _buff[_buffPosition + 7] = (byte)(value >> 56);
                    incrementedAndReset(8);
                    return;
                case WireType.SignedVariant:
                    writeUInt64Variant(zig(value));
                    return;
                case WireType.Variant:
                    if (value >= 0)
                    {
                        writeUInt64Variant((ulong)value);
                    }
                    else
                    {
                        demandSpace(10);
                        _buff[_buffPosition] = (byte)(value | 0x80);
                        _buff[_buffPosition + 1] = (byte)((int)(value >> 7) | 0x80);
                        _buff[_buffPosition + 2] = (byte)((int)(value >> 14) | 0x80);
                        _buff[_buffPosition + 3] = (byte)((int)(value >> 21) | 0x80);
                        _buff[_buffPosition + 4] = (byte)((int)(value >> 28) | 0x80);
                        _buff[_buffPosition + 5] = (byte)((int)(value >> 35) | 0x80);
                        _buff[_buffPosition + 6] = (byte)((int)(value >> 42) | 0x80);
                        _buff[_buffPosition + 7] = (byte)((int)(value >> 49) | 0x80);
                        _buff[_buffPosition + 8] = (byte)((int)(value >> 56) | 0x80);
                        _buff[_buffPosition + 9] = 0x01; // sign bit
                        incrementedAndReset(10);
                    }
                    return;
                case WireType.Fixed32:
                    checked { writeInt32((int)value, wireType); }
                    return;
                default:
                    throw createException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入无符号64位整数
        /// 支持类型：Variant, Fixed32, Fixed64
        /// </summary>
        public void writeUInt64(ulong value, WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Fixed64:
                    writeInt64((long)value, wireType);
                    return;
                case WireType.Variant:
                    writeUInt64Variant(value);
                    return;
                case WireType.Fixed32:
                    checked { writeUInt32((uint)value, wireType); }
                    return;
                default:
                    throw createException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入布尔变量
        /// </summary>
        /// <param name="value"></param>
        /// <param name="wireType"></param>
        public void writeBoolean(bool value, WireType wireType)
        {
            writeUInt32(value ? (uint)1 : (uint)0, wireType);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入bytes数组
        /// </summary>
        public void writeBytes(byte[] dat, WireType wireType)
        {
            if (dat == null) throw new ArgumentNullException("data");
            writeBytes(dat, 0, dat.Length, wireType);
        }

        /// <summary>
        /// 写入bytes数组
        /// </summary>
        /// <param name="dat"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <param name="wireType"></param>
        public void writeBytes(byte[] dat, int offset, int length,  WireType wireType)
        {
            if (dat == null) throw new ArgumentNullException("data");
            switch (wireType)
            {
                case WireType.Fixed32:
                    if (length != 4) throw new ArgumentException("length");
                    goto CopyFixedLength;  // ugly but effective
                case WireType.Fixed64:
                    if (length != 8) throw new ArgumentException("length");
                    goto CopyFixedLength;  // ugly but effective
                case WireType.String:
                    writeUInt32Variant((uint)length);
                    if (length == 0) return;
                    if (_flushLock != 0 || length <= _buff.Length) // write to the buffer
                    {
                        goto CopyFixedLength; // ugly but effective
                    }
                    // writing data that is bigger than the buffer (and the buffer
                    // isn't currently locked due to a sub-object needing the size backfilled)
                    flush(); // commit any existing data from the buffer
                    // now just write directly to the underlying stream
                    _stream.Write(dat, offset, length);
                    _position += length; // since we've flushed offset etc is 0, and remains
                                               // zero since we're writing directly to the stream
                    return;
            }
            throw createException(wireType);
            CopyFixedLength: // no point duplicating this lots of times, and don't really want another stackframe
            demandSpace(length);
            Helpers.BlockCopy(dat, offset, _buff, _buffPosition, length);
            incrementedAndReset(length);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入单精度浮点数
        /// 支持格式：Fixed32， Fixed64
        /// </summary>
#if !FEAT_SAFE
        unsafe
#endif
        public void writeSingle(float value, WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Fixed32:
#if FEAT_SAFE
                    ProtoWriter.WriteInt32(BitConverter.ToInt32(BitConverter.GetBytes(value), 0), writer);
#else
                    writeInt32(*(int*)&value, wireType);
#endif
                    return;
                case WireType.Fixed64:
                    writeDouble((double)value, wireType);
                    return;
                default:
                    throw createException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入双精度浮点数
        /// 支持格式：Fixed32， Fixed64
        /// </summary>
#if !FEAT_SAFE
        unsafe
#endif
        public void writeDouble(double value, WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Fixed32:
                    float f = (float)value;
                    if (Helpers.IsInfinity(f)
                        && !Helpers.IsInfinity(value))
                    {
                        throw new OverflowException();
                    }
                    writeSingle(f, wireType);
                    return;
                case WireType.Fixed64:
#if FEAT_SAFE
                    ProtoWriter.WriteInt64(BitConverter.ToInt64(BitConverter.GetBytes(value), 0), writer);
#else
                    writeInt64(*(long*)&value, wireType);
#endif
                    return;
                default:
                    throw createException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 写入字符串到流中
        /// 支持类型: String
        /// </summary>
        public void writeString(string value, WireType wireType)
        {
            if (wireType != WireType.String) throw createException(wireType);
            if (value == null) throw new ArgumentNullException("value"); // written header; now what?
            int len = value.Length;
            if (len == 0)
            {
                writeUInt32Variant(0);
                return; // just a header
            }
#if MF
            byte[] bytes = encoding.GetBytes(value);
            int actual = bytes.Length;
            writer.WriteUInt32Variant((uint)actual);
            writer.Ensure(actual);
            Helpers.BlockCopy(bytes, 0, writer.ioBuffer, writer.ioIndex, actual);
#else
            int predicted = System.Text.Encoding.UTF8.GetByteCount(value);
            writeUInt32Variant((uint)predicted);
            demandSpace(predicted);
            int actual = System.Text.Encoding.UTF8.GetBytes(value, 0, value.Length, _buff, _buffPosition);
            Helpers.DebugAssert(predicted == actual);
#endif
            incrementedAndReset(actual);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 消息中的嵌入写入开始
        /// 注：不做嵌套层数检查
        /// </summary>
        int startSubMessage(WireType wireType, bool allowFixed)
        {
            switch (wireType)
            {
                case WireType.StartGroup:
                    return _buffPosition; // new SubItemToken(-writer.fieldNumber);
                case WireType.String:
                    demandSpace(32); // make some space in anticipation...
                    _flushLock++;
                    _position++;
                    return _buffPosition++; // leave 1 space (optimistic) for length
                case WireType.Fixed32:
                    {
                        if (!allowFixed) throw createException(wireType);
                        demandSpace(32); // make some space in anticipation...
                        _flushLock++;
                        incrementedAndReset(4); // leave 4 space (rigid) for length
                        return _buffPosition;
                    }
                default:
                    throw createException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 消息中的嵌入写入结束
        /// 注：不做嵌套层数检查
        /// </summary>
        void endSubMessage(int startIdx, PrefixStyle style)
        {
            int value = startIdx;
            int len;
            switch (style)
            {
                case PrefixStyle.Fixed32:
                    len = (int)((_buffPosition - value) - 4);
                    writeInt32ToBuffer(len, value);
                    break;
                case PrefixStyle.Fixed32BigEndian:
                    len = (int)((_buffPosition - value) - 4);
                    writeInt32ToBuffer(len, value);
                    // and swap the byte order
                    byte b = _buff[value];
                    _buff[value] = _buff[value + 3];
                    _buff[value + 3] = b;
                    b = _buff[value + 1];
                    _buff[value + 1] = _buff[value + 2];
                    _buff[value + 2] = b;
                    break;
                case PrefixStyle.Base128:
                    // string - complicated because we only reserved one byte;
                    // if the prefix turns out to need more than this then
                    // we need to shuffle the existing data
                    len = (int)((_buffPosition - value) - 1);
                    int offset = 0;
                    uint tmp = (uint)len;
                    while ((tmp >>= 7) != 0) offset++;
                    if (offset == 0)
                    {
                        _buff[value] = (byte)(len & 0x7F);
                    }
                    else
                    {
                        demandSpace(offset);
                        byte[] blob = _buff;
                        Helpers.BlockCopy(blob, value + 1, blob, value + 1 + offset, len);
                        tmp = (uint)len;
                        do
                        {
                            blob[value++] = (byte)((tmp & 0x7F) | 0x80);
                        } while ((tmp >>= 7) != 0);
                        blob[value - 1] = (byte)(blob[value - 1] & ~0x80);
                        _position += offset;
                        _buffPosition += offset;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException("style");
            }

            // and this object is no longer a blockage - also flush if sensible
            const int ADVISORY_FLUSH_SIZE = 1024;
            if (--_flushLock == 0 && _buffPosition >= ADVISORY_FLUSH_SIZE)
                flush();
        }

        ///////////////////////////////////////////////////////////////////////////

        uint zig(int value)
        {
            return (uint)((value << 1) ^ (value >> 31));
        }

        ulong zig(long value)
        {
            return (ulong)((value << 1) ^ (value >> 63));
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 位置增长
        /// </summary>
        /// <param name="length"></param>
        /// <param name="writer"></param>
        void incrementedAndReset(int length)
        {
            Helpers.DebugAssert(length >= 0);
            _buffPosition += length;
            _position += length;
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 申请空间
        /// </summary>
        /// <param name="required">申请大小（字节）</param>
        void demandSpace(int required)
        {
            // check for enough space
            if ((_buff.Length - _buffPosition) < required)
            {
                if (_flushLock == 0)
                {
                    flush();
                    if ((_buff.Length - _buffPosition) >= required) return;
                }
                // either can't empty the buffer, or that didn't help; need more space
                BufferPool.ResizeAndFlushLeft(ref _buff, required + _buffPosition, 0, _buffPosition);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 生成异常
        /// </summary>
        Exception createException(WireType wireType)
        {
            return new ProtoException("Invalid serialization operation with wire-type " + wireType.ToString() + " at position " + _position.ToString());
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 将缓存数据写入流
        /// </summary>
        void flush()
        {
            if (_flushLock == 0 && _buffPosition != 0)
            {
                _stream.Write(_buff, 0, _buffPosition);
                _buffPosition = 0;
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void close()
        {
            Dispose();
        }

        #region IDisposable interface

        void IDisposable.Dispose()
        {
            Dispose();
        }

        private void Dispose()
        {   // importantly, this does **not** own the stream, and does not dispose it
            if (_stream != null)
            {
                flush();
                _stream = null;
            }
        }

        #endregion //IDisposable interface
    }
}



