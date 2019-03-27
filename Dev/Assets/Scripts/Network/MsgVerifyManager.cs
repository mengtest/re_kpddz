/***************************************************************


 *
 *
 * Filename:  	MsgVerifyManager.cs	
 * Summary: 	网络消息回包模块：当要确认服务端已处理要发送的消息时，请用消息回包。
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/18 14:22
 ***************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EventManager;
using network;
using network.protobuf;

/// <summary>
/// 管理消息的回包
/// </summary>
public class MsgVerifyManager {
	/// <summary>
	/// 记录回包消息的函数的下标
	/// </summary>
	protected uint verifySn = 0;
	public uint VerifySn
	{
		get
		{
			++verifySn;
			return verifySn;
		}
	}
	protected Timer _verifyTimer;            // 确认计时器，过长时间不响应确认信息则认为是掉线
	private Dictionary<ulong, DelegateType.OperationVerify> verifyWaiting;


	public MsgVerifyManager()
	{
	}

	public void Dispose()
	{
	}

	//添加
	public void AddVerifyCallback(DelegateType.OperationVerify verifyCallBack)
	{
	}
	
	/// <summary>
	/// 回包消息处理函数
	/// </summary>
	/// <param name="msg">Message.</param>
	protected virtual void OnMsgVerifyCallback(object msg)
	{
	}

	public void CloseTimer()
	{
	}

	private void Disconnect()
	{
	}

}
