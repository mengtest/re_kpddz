/***************************************************************


 *
 *
 * Filename:  	MsgSaving.cs	
 * Summary: 	结构体：用于存储已发送的消息数据
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/18 14:22
 ***************************************************************/

using System;
using System.IO;
using UnityEngine;
using network.protobuf;

namespace network.protobuf
{
	/// <summary>
	/// 用于存储发送的消息数据
	/// </summary>
	public struct MsgSaving
	{
		ProtoID _protoType;
	    MemoryStream _stream;
		ulong _sn;

		public MsgSaving(ProtoID protoType, MemoryStream stream, ulong sn)
	    {
	        _protoType = protoType;
	        _stream = stream;
	        _sn = sn;
	    }

		public ulong Sn
	    {
	        get { return _sn; }
	    }

	    public MemoryStream GetStreamToSend(byte[] encryptKey)
	    {
	        var returnStream = new MemoryStream();

	        MemoryStream stream;
	        if (ProtoEncryptList.protoEncryptList.Contains(_protoType))
	        {
	            var s = EncryptionUtils.Encrypt(_stream, encryptKey, encryptKey);
	            stream = new MemoryStream(s, 0, (int)_stream.Length);
	        }
	        else
	        {
	            stream = _stream;
	        }

	        int sumLen = (int)stream.Length + 6;

	        byte[] sumLenBytes = BitConverter.GetBytes(sumLen);
	        for (int i = 3; i >= 0; --i)
	            returnStream.WriteByte(sumLenBytes[i]);

	        byte[] snBytes = BitConverter.GetBytes(_sn);
	        for (int i = 3; i >= 0; --i)
	            returnStream.WriteByte(snBytes[i]);

	        byte[] protoTypeBytes = BitConverter.GetBytes((int)_protoType);
	        returnStream.WriteByte(protoTypeBytes[1]);
	        returnStream.WriteByte(protoTypeBytes[0]);

	        stream.WriteTo(returnStream);

	        return returnStream;
	    }

	    public bool NeedSave
	    {
	        get
	        {
	            return _sn > 0;
	        }
	    }
	}
}