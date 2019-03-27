/***************************************************************


 *
 *
 * Filename:  	MsgStream.cs	
 * Summary: 	存储socket中取出来的stream消息数据
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/18 14:22
 ***************************************************************/

using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using network.protobuf;

namespace network.protobuf
{
	/// <summary>
	/// 用于读取消息数据
	/// </summary>
	public class MsgStream {
		// Client  socket.
		public Socket workSocket = null;
		// Size of receive buffer.
		public const int BufferSize = 1024;
		// Receive buffer.
		public byte[] buffer = new byte[BufferSize];
		// Received data string.
		public MemoryStream stream = new MemoryStream();
		
		//public SocketClient sc;
	}

    public class MsgHeader
    {
        private uint _sumLen;
        public uint sumLen
        {
            get { return _sumLen; }
        }

        private uint _dlsn;
        public uint dlsn
        {
            get { return _dlsn; }
        }

        private ProtoID _id;
        public ProtoID id
        {
            get { return _id; }
        }
        public void ReadHeaderFromBuffer(byte[] buffer)
        {
            if (buffer.Length < 10)
            {
                return;
            }

            for (int i = 0; i < 4; ++i)
                _sumLen = _sumLen * 256 + (uint)buffer[i];


            for (int j = 4; j < 8; ++j)
                _dlsn = _dlsn * 256 + (uint)buffer[j];

            uint n = 0;
            for (int k = 8; k < 10; ++k)
                n = n * 256 + (uint)buffer[k];

            _id = (ProtoID)n;
        }
    }
}