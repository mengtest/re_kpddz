/***************************************************************


 *
 *
 * Filename:  	MsgObject.cs	
 * Summary: 	结构体：存储要分发的消息数据
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/18 14:22
 ***************************************************************/

using UnityEngine;
using System.Collections;
using network.protobuf;

namespace network.protobuf
{
	/// <summary>
	/// 保存接收的消息数据
	/// </summary>
	public struct MsgObject {
		public ProtoID _protoType;
		public object _proto;
		public MsgObject(ProtoID protoType, object proto)
		{
			_protoType = protoType;
			_proto = proto;
		}

		public void RunCallback()
		{
			MsgCallManager.RunCallback (_protoType, _proto);
		}
	}
}