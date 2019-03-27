/***************************************************************


 *
 *
 * Filename:  	luaProtoReader.cs	
 * Summary: 	从流中读取数据
 *
 * Version:   	1.0.0
 * Author: 		WP.Chu
 * Date:   		2016/10/24 17:41
 ***************************************************************/

using UnityEngine;
using System.Collections;
using System.IO;
using System;
using ProtoBuf;

namespace sluaAux.proto
{
    public class luaProtoReader : IDisposable
    {
        #region 常量表

        private const long Int64Msb = ((long)1) << 63;
        private const int Int32Msb = ((int)1) << 31;

        #endregion //常量表

        //待读取数据
        MemoryStream _stream = null;

        //缓存
        private byte[] _buff = null;

        //跟踪当前缓存中的读取位置
        int _buffReadPosition = 0;

        //reader流中的当前位置
        int _position = 0;

        //是否固定长度
        bool _isFixedLength = false;

        //缓存中的可用数据长度
        int _available = 0;

        //剩余长度
        int _dataRemaining = 0;

        //块结束
        int _blockEnd = int.MaxValue;

        //深度
        int _depth = 0;

        //当前读取的字段字段索引
        int _fieldNumber = 0;

        //是否缓存字符串
        bool _internStrings = true;
        
        //reader.wireType = WireType.None;
        //reader.trapCount = 1;

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="ms">源内存流</param>
        /// <returns></returns>
        public static luaProtoReader createLuaProtoReader(MemoryStream ms)
        {
            if (ms == null)
                return null;

            return new luaProtoReader(ms);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream"></param>
        public luaProtoReader(MemoryStream stream)
        {
            _stream = stream;
            _buff = BufferPool.GetBuffer();
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 读取lua对象
        /// </summary>
        object readLuaObject(ProtoTypeCode typeCode, WireType wireType, string nestedTypeName)
        {
            if (typeCode == ProtoTypeCode.Empty) return null;
            if (wireType == WireType.None) return null;

            object objRead = null;
            switch (typeCode)
            {
                case ProtoTypeCode.Int32:
                    objRead = readInt32(wireType);
                    break;
                case ProtoTypeCode.UInt32:
                    objRead = readUInt32(wireType);
                    break;
                case ProtoTypeCode.Int64:
                    objRead = readInt64(wireType);
                    break;
                case ProtoTypeCode.UInt64:
                    objRead = readUInt64(wireType);
                    break;
                case ProtoTypeCode.Boolean:
                    objRead = readBoolean(wireType);
                    break;
                case ProtoTypeCode.ByteArray:
                    objRead = readBytes(wireType, null); ;
                    break;
                case ProtoTypeCode.Single:
                    objRead = readSingle(wireType);
                    break;
                case ProtoTypeCode.Double:
                    objRead = readDouble(wireType);
                    break;
                case ProtoTypeCode.String:
                    objRead = readString(wireType);
                    break;
                case ProtoTypeCode.Type:
                    int startBlock = startSubItem(wireType);
                    objRead = readLuaMessage(nestedTypeName);
                    endSubItem(startBlock, wireType);
                    break;
                default:
                    throw createWireTypeException(wireType);
            }

            return objRead;
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取lua消息
        /// </summary>
        /// <param name="idMsg">配置表中的消息索引</param>
        /// <returns></returns>
        public luaMessage readLuaMessage(object idMsg)
        {
            luaMessage msg = (luaMessage)luaMessageCache.createTypeInstance(idMsg);
            if (msg == null)
                return null;

            int fieldCount = msg.fieldsCount();
            fieldDataInfo fieldData = null;
            
            for (int idx=1; idx<=fieldCount; idx++)
            {
                fieldData = (fieldDataInfo)msg[idx];
                if (fieldData == null) continue;

                WireType wireType = fieldData.descriptor.wireType;
                ProtoTypeCode typeCode = fieldData.descriptor.typeCode;
                string nestedTypeName = fieldData.descriptor.nestedTypeName;

                if (tryReadFieldHeader(idx, wireType))
                {
                    //创建值字段
                    if (!fieldData.appendValue())
                        continue;

                    if (fieldData.descriptor.isRepeated())
                    {
                        do
                        {
                            var obj = readLuaObject(typeCode, wireType, nestedTypeName);
                            fieldData.Value.addChild(obj);
                        } while (tryReadFieldHeader(idx, wireType));
                    }
                    else
                    {
                        fieldData.Value.valueL = readLuaObject(typeCode, wireType, nestedTypeName);
                    }
                }
            }

            return msg;
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取下一个字段头信息，如果没有更多的可用字段，返回0。该方法同样遵循子消息
        /// </summary>
        /// <returns>字段索引</returns>
        int readFieldHeader(WireType wireType)
        {
            // at the end of a group the caller must call EndSubItem to release the
            // reader (which moves the status to Error, since ReadFieldHeader must
            // then be called)
            if (_blockEnd <= _position || wireType == WireType.EndGroup) { return 0; }

            uint tag;
            int fieldNumber = 0;
            if (tryReadUInt32Variant(out tag))
            {
                wireType = (WireType)(tag & 7);
                fieldNumber = (int)(tag >> 3);
                if (fieldNumber < 1) throw new ProtoException("Invalid field in source data: " + fieldNumber.ToString());
            }
            else
                fieldNumber = 0;

            if (wireType == ProtoBuf.WireType.EndGroup)
            {
                if (_depth > 0) return 0; // spoof an end, but note we still set the field-number
                throw new ProtoException("Unexpected end-group in source data; this usually means the source data is corrupt");
            }

            //保存当前正在读取的字段索引
            _fieldNumber = fieldNumber;

            return fieldNumber;
        }

        /// <summary>
        /// 查找下一个可读字段的头部信息。 比如repeated字段
        /// 
        /// Looks ahead to see whether the next field in the stream is what we expect
        /// (typically; what we've just finished reading - for example ot read successive list items)
        /// </summary>
        bool tryReadFieldHeader(int field, WireType wireType)
        {
            // check for virtual end of stream
            if (_blockEnd <= _position || wireType == WireType.EndGroup) { return false; }
            uint tag;
            int read = tryReadUInt32VariantWithoutMoving(false, out tag);
            WireType tmpWireType; // need to catch this to exclude (early) any "end group" tokens
            if (read > 0 && ((int)tag >> 3) == field
                && (tmpWireType = (WireType)(tag & 7)) != WireType.EndGroup)
            {
                _fieldNumber = field;
                _position += read;
                _buffReadPosition += read;
                _available -= read;
                return true;
            }
            return false;
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 尝试读取一个Variant编码的无符号32位整数
        /// </summary>
        /// <param name="trimNegative">是否为负数</param>
        /// <param name="value">读取的值(out)</param>
        /// <returns>返回读取的长度</returns>
        int tryReadUInt32VariantWithoutMoving(bool trimNegative, out uint value)
        {
            if (_available < 10) ensure(10, false);
            if (_available == 0)
            {
                value = 0;
                return 0;
            }
            int readPos = _buffReadPosition;
            value = _buff[readPos++];
            if ((value & 0x80) == 0) return 1;
            value &= 0x7F;
            if (_available == 1) throw eof(this);

            uint chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 7;
            if ((chunk & 0x80) == 0) return 2;
            if (_available == 2) throw eof(this);

            chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 14;
            if ((chunk & 0x80) == 0) return 3;
            if (_available == 3) throw eof(this);

            chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 21;
            if ((chunk & 0x80) == 0) return 4;
            if (_available == 4) throw eof(this);

            chunk = _buff[readPos];
            value |= chunk << 28; // can only use 4 bits from this chunk
            if ((chunk & 0xF0) == 0) return 5;

            if (trimNegative // allow for -ve values
                && (chunk & 0xF0) == 0xF0
                && _available >= 10
                    && _buff[++readPos] == 0xFF
                    && _buff[++readPos] == 0xFF
                    && _buff[++readPos] == 0xFF
                    && _buff[++readPos] == 0xFF
                    && _buff[++readPos] == 0x01)
            {
                return 10;
            }
            throw createOverFlowException();
        }

        /// <summary>
        /// 尝试读取一个uint32值
        /// </summary>
        /// <param name="value">读取的值(out)</param>
        /// <returns>返回是否读取成功</returns>
        bool tryReadUInt32Variant(out uint value)
        {
            int read = tryReadUInt32VariantWithoutMoving(false, out value);
            if (read > 0)
            {
                _buffReadPosition += read;
                _available -= read;
                _position += read;
                return true;
            }
            return false;
        }


        /// <summary>
        /// 读取一个无符号32位整数
        /// </summary>
        /// <param name="trimNegative"></param>
        /// <returns></returns>
        uint readUInt32Variant(bool trimNegative)
        {
            uint value;
            int read = tryReadUInt32VariantWithoutMoving(trimNegative, out value);
            if (read > 0)
            {
                _buffReadPosition += read;
                _available -= read;
                _position += read;
                return value;
            }

            throw eof(this);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取一个有符号32位整数
        /// 支持编码类型: Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public int readInt32(WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Variant:
                    return (int)readUInt32Variant(true);
                case WireType.Fixed32:
                    if (_available < 4) ensure(4, true);
                    _position += 4;
                    _available -= 4;
                    return ((int)_buff[_buffReadPosition++])
                        | (((int)_buff[_buffReadPosition++]) << 8)
                        | (((int)_buff[_buffReadPosition++]) << 16)
                        | (((int)_buff[_buffReadPosition++]) << 24);
                case WireType.Fixed64:
                    long l = readInt64(wireType);
                    checked { return (int)l; }
                case WireType.SignedVariant:
                    return zag(readUInt32Variant(true));
                default:
                    throw createWireTypeException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取一个无符号32位整数
        /// 支持编码类型: Variant, Fixed32, Fixed64
        /// </summary>
        public uint readUInt32(WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Variant:
                    return readUInt32Variant(false);
                case WireType.Fixed32:
                    if (_available < 4) ensure(4, true);
                    _position += 4;
                    _available -= 4;
                    return ((uint)_buff[_buffReadPosition++])
                        | (((uint)_buff[_buffReadPosition++]) << 8)
                        | (((uint)_buff[_buffReadPosition++]) << 16)
                        | (((uint)_buff[_buffReadPosition++]) << 24);
                case WireType.Fixed64:
                    ulong val = readUInt64(wireType);
                    checked { return (uint)val; }
                default:
                    throw createWireTypeException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 读取一个Varian编码无符号64位整数
        /// </summary>
        /// <returns></returns>
        private ulong readUInt64Variant()
        {
            ulong value;
            int read = tryReadUInt64VariantWithoutMoving(out value);
            if (read > 0)
            {
                _buffReadPosition += read;
                _available -= read;
                _position += read;
                return value;
            }
            throw eof(this);
        }

        /// <summary>
        /// 尝试读取一个Variant编码的无符号64位整数
        /// </summary>
        /// <param name="value">读取的值(out)</param>
        /// <returns>返回读取的长度</returns>
        private int tryReadUInt64VariantWithoutMoving(out ulong value)
        {
            if (_available < 10) ensure(10, false);
            if (_available == 0)
            {
                value = 0;
                return 0;
            }
            int readPos = _buffReadPosition;
            value = _buff[readPos++];
            if ((value & 0x80) == 0) return 1;
            value &= 0x7F;
            if (_available == 1) throw eof(this);

            ulong chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 7;
            if ((chunk & 0x80) == 0) return 2;
            if (_available == 2) throw eof(this);

            chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 14;
            if ((chunk & 0x80) == 0) return 3;
            if (_available == 3) throw eof(this);

            chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 21;
            if ((chunk & 0x80) == 0) return 4;
            if (_available == 4) throw eof(this);

            chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 28;
            if ((chunk & 0x80) == 0) return 5;
            if (_available == 5) throw eof(this);

            chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 35;
            if ((chunk & 0x80) == 0) return 6;
            if (_available == 6) throw eof(this);

            chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 42;
            if ((chunk & 0x80) == 0) return 7;
            if (_available == 7) throw eof(this);


            chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 49;
            if ((chunk & 0x80) == 0) return 8;
            if (_available == 8) throw eof(this);

            chunk = _buff[readPos++];
            value |= (chunk & 0x7F) << 56;
            if ((chunk & 0x80) == 0) return 9;
            if (_available == 9) throw eof(this);

            chunk = _buff[readPos];
            value |= chunk << 63; // can only use 1 bit from this chunk

            if ((chunk & ~(ulong)0x01) != 0) throw createOverFlowException();
            return 10;
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取一个有符号64位整数
        /// 支持编码类型: Variant, Fixed32, Fixed64, SignedVariant
        /// </summary>
        public long readInt64(WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Variant:
                    return (long)readUInt64Variant();
                case WireType.Fixed32:
                    return readInt32(wireType);
                case WireType.Fixed64:
                    if (_available < 8) ensure(8, true);
                    _position += 8;
                    _available -= 8;

                    return ((long)_buff[_buffReadPosition++])
                        | (((long)_buff[_buffReadPosition++]) << 8)
                        | (((long)_buff[_buffReadPosition++]) << 16)
                        | (((long)_buff[_buffReadPosition++]) << 24)
                        | (((long)_buff[_buffReadPosition++]) << 32)
                        | (((long)_buff[_buffReadPosition++]) << 40)
                        | (((long)_buff[_buffReadPosition++]) << 48)
                        | (((long)_buff[_buffReadPosition++]) << 56);

                case WireType.SignedVariant:
                    return zag(readUInt64Variant());
                default:
                    throw createWireTypeException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取一个有符号64位整数
        /// 支持编码类型: Variant, Fixed32, Fixed64
        /// </summary>
        public ulong readUInt64(WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Variant:
                    return readUInt64Variant();
                case WireType.Fixed32:
                    return readUInt32(wireType);
                case WireType.Fixed64:
                    if (_available < 8) ensure(8, true);
                    _position += 8;
                    _available -= 8;

                    return ((ulong)_buff[_buffReadPosition++])
                        | (((ulong)_buff[_buffReadPosition++]) << 8)
                        | (((ulong)_buff[_buffReadPosition++]) << 16)
                        | (((ulong)_buff[_buffReadPosition++]) << 24)
                        | (((ulong)_buff[_buffReadPosition++]) << 32)
                        | (((ulong)_buff[_buffReadPosition++]) << 40)
                        | (((ulong)_buff[_buffReadPosition++]) << 48)
                        | (((ulong)_buff[_buffReadPosition++]) << 56);
                default:
                    throw createWireTypeException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取布尔变量
        /// 支持编码类型: Variant, Fixed32, Fixed64
        /// </summary>
        /// <returns></returns>
        public bool readBoolean(WireType wireType)
        {
            switch (readUInt32(wireType))
            {
                case 0: return false;
                case 1: return true;
                default: throw createException("Unexpected boolean value");
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取byte数组(byte[])
        /// 支持编码类型: String
        /// </summary>
        public byte[] readBytes(WireType wireType, byte[] value)
        {
            switch (wireType)
            {
                case WireType.String:
                    int len = (int)readUInt32Variant(false);
                    if (len == 0) return value == null ? new byte[0] : value;
                    int offset;
                    if (value == null || value.Length == 0)
                    {
                        offset = 0;
                        value = new byte[len];
                    }
                    else 
                    {
                        offset = value.Length;
                        byte[] tmp = new byte[value.Length + len];
                        Helpers.BlockCopy(value, 0, tmp, 0, value.Length);
                        value = tmp;
                    }

                    // value is now sized with the final length, and (if necessary)
                    // contains the old data up to "offset"
                    _position += len; // assume success
                    while (len > _available)
                    {
                        if (_available > 0)
                        {
                            // copy what we *do* have
                            Helpers.BlockCopy(_buff, _buffReadPosition, value, offset, _available);
                            len -= _available;
                            offset += _available;
                            _buffReadPosition = _available = 0; // we've drained the buffer
                        }

                        //  now refill the buffer (without overflowing it)
                        int count = len > _buff.Length ? _buff.Length : len;
                        if (count > 0) ensure(count, true);
                    }

                    // at this point, we know that len <= available
                    if (len > 0)
                    {   // still need data, but we have enough buffered
                        Helpers.BlockCopy(_buff, _buffReadPosition, value, offset, len);
                        _buffReadPosition += len;
                        _available -= len;
                    }
                    return value;
                default:
                    throw createWireTypeException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取单精度浮点数
        /// 支持编码类型: Fixed32, Fixed64
        /// </summary>
        public
#if !FEAT_SAFE
        unsafe
#endif
        float readSingle(WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Fixed32:
                    {
                        int value = readInt32(wireType);
#if FEAT_SAFE
                        return BitConverter.ToSingle(BitConverter.GetBytes(value), 0);
#else
                        return *(float*)&value;
#endif
                    }
                case WireType.Fixed64:
                    {
                        double value = readDouble(wireType);
                        float f = (float)value;
                        if (Helpers.IsInfinity(f)
                            && !Helpers.IsInfinity(value))
                        {
                            throw createOverFlowException();
                        }
                        return f;
                    }
                default:
                    throw createWireTypeException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取双精度浮点数
        /// 支持编码类型: Fixed32, Fixed64
        /// </summary>
        public
#if !FEAT_SAFE
        unsafe
#endif
        double readDouble(WireType wireType)
        {
            switch (wireType)
            {
                case WireType.Fixed32:
                    return readSingle(wireType);
                case WireType.Fixed64:
                    long value = readInt64(wireType);
#if FEAT_SAFE
                    return BitConverter.ToDouble(BitConverter.GetBytes(value), 0);
#else
                    return *(double*)&value;
#endif
                default:
                    throw createWireTypeException(wireType);
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 从流中读取字符串(UT8编码)
        /// 支持编码类型: String
        /// </summary>
        public string readString(WireType wireType)
        {
            if (wireType == WireType.String)
            {
                int bytes = (int)readUInt32Variant(false);
                if (bytes == 0) return "";
                if (_available < bytes) ensure(bytes, true);
#if MF
                byte[] tmp;
                if(ioIndex == 0 && bytes == ioBuffer.Length) {
                    // unlikely, but...
                    tmp = ioBuffer;
                } else {
                    tmp = new byte[bytes];
                    Helpers.BlockCopy(ioBuffer, ioIndex, tmp, 0, bytes);
                }
                string s = new string(encoding.GetChars(tmp));
#else
                string s = System.Text.Encoding.UTF8.GetString(_buff, _buffReadPosition, bytes);
#endif
                //Avoid GC (return)
                if (_internStrings) { s = intern(s); }
                _available -= bytes;
                _position += bytes;
                _buffReadPosition += bytes;
                return s;
            }
            throw createWireTypeException(wireType);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 开始读取嵌入的子消息
        /// 
        /// Begins consuming a nested message in the stream; supported wire-types: StartGroup, String
        /// </summary>
        /// <remarks>The token returned must be help and used when callining EndSubItem</remarks>
        private int startSubItem(WireType wireType)
        {
            switch (wireType)
            {
                case WireType.StartGroup:
                    _depth++;
                    return _fieldNumber;
                case WireType.String:
                    int len = (int)readUInt32Variant(false);
                    if (len < 0) throw createInvalidOptException();
                    int lastEnd = _blockEnd;
                    _blockEnd = _position + len;
                    _depth++;
                    return lastEnd;
                default:
                    throw createWireTypeException(wireType); // throws
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 读取嵌入子消息结束，检查是否读出成功并还原读取状态。
        /// 
        /// Makes the end of consuming a nested message in the stream; the stream must be either at the correct EndGroup
        /// marker, or all fields of the sub-message must have been consumed (in either case, this means ReadFieldHeader
        /// should return zero)
        /// </summary>
        private void endSubItem(int lastBlockEnd, WireType wireType)
        {
            int value = lastBlockEnd;
            switch (wireType)
            {
                case WireType.EndGroup:
                    if (value >= 0) throw new ArgumentException("token");
                    if (-value != _fieldNumber) throw createException("Wrong group was ended"); // wrong group ended!
                    _depth--;
                    break;
                // case WireType.None: // TODO reinstate once reads reset the wire-type
                default:
                    if (value < _position) throw createException("Sub-message not read entirely");
                    if (_blockEnd != _position && _blockEnd != int.MaxValue)
                    {
                        throw createException("Sub-message not read correctly");
                    }
                    _blockEnd = value;
                    _depth--;
                    break;
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 确保缓存中有足够多的字节数据可供读取，若缓存中的数据不够则从流中拷贝到缓存中。
        /// </summary>
        /// <param name="count">要读取的字节数</param>
        /// <param name="strict">是否必须满足要求字节数; false:不满足也可读取,true:必须有足够多的字节(>=count)</param>
        void ensure(int count, bool strict)
        {
            Helpers.DebugAssert(_available <= count, "Asking for data without checking first");
            if (count > _buff.Length)
            {
                BufferPool.ResizeAndFlushLeft(ref _buff, count, _buffReadPosition, _available);
                _buffReadPosition = 0;
            }
            else if (_buffReadPosition + count >= _buff.Length)
            {
                // need to shift the buffer data to the left to make space
                Helpers.BlockCopy(_buff, _buffReadPosition, _buff, 0, _available);
                _buffReadPosition = 0;
            }

            count -= _available;
            int writePos = _buffReadPosition + _available, bytesRead;
            int canRead = _buff.Length - writePos;
            if (_isFixedLength)
            {   // throttle it if needed
                if (_dataRemaining < canRead) canRead = _dataRemaining;
            }
            while (count > 0 && canRead > 0 && (bytesRead = _stream.Read(_buff, writePos, canRead)) > 0)
            {
                _available += bytesRead;
                count -= bytesRead;
                canRead -= bytesRead;
                writePos += bytesRead;
                if (_isFixedLength) { _dataRemaining -= bytesRead; }
            }
            if (strict && count > 0)
            {
                throw eof();
            }
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 构建wiretype异常
        /// </summary>
        private Exception createWireTypeException(WireType wireType)
        {
            string msg = "Invalid wire-type; this usually means you have over - written a file without truncating or setting the length; see http://stackoverflow.com/q/2152978/23354";
            var exception = new ProtoException(msg);

            if (exception != null && !exception.Data.Contains("protoSource"))
            {
                exception.Data.Add("protoSource", string.Format("tag={0}; wire-type={1}; offset={2}; depth={3}",
                    _fieldNumber, wireType, _position, _depth));
            }

            return exception;
        }

        /// <summary>
        /// 构建普通异常
        /// </summary>
        /// <param name="message"></param>
        private Exception createException(string message)
        {
            return new ProtoException(message);
        }

        /// <summary>
        /// 溢出异常
        /// </summary>
        private Exception createOverFlowException()
        {
            return new OverflowException();
        }

        /// <summary>
        /// 抛出流结束符异常
        /// </summary>
        private Exception eof(luaProtoReader reader = null)
        {
            return new EndOfStreamException();
        }

        /// <summary>
        /// 构建无效操作异常
        /// </summary>
        /// <returns></returns>
        private Exception createInvalidOptException()
        {
            return new InvalidOperationException();
        }

        ///////////////////////////////////////////////////////////////////////////

        private int zag(uint ziggedValue)
        {
            int value = (int)ziggedValue;
            return (-(value & 0x01)) ^ ((value >> 1) & ~Int32Msb);
        }

        private long zag(ulong ziggedValue)
        {
            long value = (long)ziggedValue;
            return (-(value & 0x01L)) ^ ((value >> 1) & ~Int64Msb);
        }

        ///////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// 缓存字符串，防止读取字符串时的实例化和GC
        /// </summary>
        private System.Collections.Generic.Dictionary<string, string> _stringInterner;
        private string intern(string value)
        {
            if (value == null) return null;
            if (value.Length == 0) return "";
            string found;
            if (_stringInterner == null)
            {
                _stringInterner = new System.Collections.Generic.Dictionary<string, string>();
                _stringInterner.Add(value, value);
            }
            else if (_stringInterner.TryGetValue(value, out found))
            {
                value = found;
            }
            else
            {
                _stringInterner.Add(value, value);
            }
            return value;
        }

        ///////////////////////////////////////////////////////////////////////////

        #region IDisposable interface

        void IDisposable.Dispose()
        {
            dispose();
        }

        private void dispose()
        {
            _stream = null;
            BufferPool.ReleaseBufferToPool(ref _buff);
            if (_stringInterner != null) _stringInterner.Clear();
        }

        #endregion //IDisposable interface    
    }
}


