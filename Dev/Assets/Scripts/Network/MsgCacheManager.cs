/***************************************************************


 *
 *
 * Filename:  	MsgCacheManager.cs	
 * Summary: 	网络消息缓存模块
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/18 14:22
 ***************************************************************/

using UnityEngine;
using System.Collections;
using System.Threading;
using network;
using network.protobuf;
using EventManager;

/// <summary>
/// 管理消息缓存
/// 实现丢包处理
/// </summary>
public class MsgCacheManager  {
	private CircularQueue<MsgSaving> msgsToSend;
	protected Timer _sendDlSnCleanTimer;
	protected Timer _sendDlSnCountTimer;

	public MsgCacheManager()
	{
	}

	public void Dispose()
	{
	}

	public void AddSaving(MsgSaving save)
	{
	}

	
	//清客户端消息缓存
	private void OnMsgClientProtoClean(object msg)
	{
	}
	
	//重新发送缓存中的消息
	private void OnMsgClientProtoCount(object msg)
	{
	}

    //清掉编号<=ulSn的消息
	protected void CleanSaving(ulong ulSn)
	{

	}
	
	public virtual void SendAllMsgBlock()
	{
	}
	
	//每接收10就通知服务端清空消息缓存
	public void OnEventServerMsgTenMultiple(EventMultiArgs args)
	{
	}
	//发送清空服务端缓存消息
	private void SendDlSnClean()
	{
	}
	//丢包重新请求
	public void OnEventServerMsgMissing(EventMultiArgs args)
	{
	}
	//发送丢包消息
	private void SendDlSnCount()
	{
	}

}
