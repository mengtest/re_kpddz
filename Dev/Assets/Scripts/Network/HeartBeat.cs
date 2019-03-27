/***************************************************************


 *
 *
 * Filename:  	HeartBeat.cs	
 * Summary: 	网络心跳包（每60秒）
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/18 14:22
 ***************************************************************/

using UnityEngine;
using System.Collections;
using network;
using network.protobuf;
using System;
using Msg;
using Utils;

/// <summary>
/// 心跳检测
/// </summary>
public class HeartBeat {	
	private bool heartBeatReplyWaiting;// 充值心跳等待
	private PeriodicTimer heartBeatTimer;
    internal int NowTime;
    internal int OffsetTime;//客户端与服务端的时间差
    float beatTime = 30f;//每60秒发一次;
    float checkTime = 10f;//发送完后10秒检测是否有回心跳包;
    protected Timer checkTimer;

    public HeartBeat()
	{
	}
	
	protected virtual void CheckHeartBeat()
	{
        if (ClientNetwork.Instance.IsConnected())
        {
            if (heartBeatReplyWaiting)
            {
                ClientOffline();
                heartBeatReplyWaiting = false;
            }
            //float play_time = Time.realtimeSinceStartup;
            //ReYunUtils.Game_SetEvent("total_msg_size", play_time.ToString(), ClientProtoMsg.totalMsgSize.ToString());//热云，统计消息流量kb单位
        }
        else
        {
			Utils.LogSys.Log("--------ClientOffline--- ");
            ClientOffline();
        }
	}

    //心跳包检测到掉线时的修理
    private void ClientOffline()
    {
        ClientNetwork.Instance.ReconnectWithoutAsk(false);
    }
	
	public void OnMsgHeartBeat(object proto)
	{
        NowTime = (int)((sc_common_heartbeat_reply)proto).now_time;
        heartBeatReplyWaiting = false;

        int clientTime = UtilTools.GetClientTime();//时间戳表达
        OffsetTime = clientTime - NowTime;
        Utils.LogSys.Log(NowTime + "=========OnMsgHeartBeat:===========" + OffsetTime);
	}
	
	public void SendHeartBeat()
	{
        if (ClientNetwork.Instance.IsConnected())
        {
            heartBeatReplyWaiting = true;
            var heartBeatMsg = new cs_common_heartbeat();
            ClientNetwork.Instance.SendMsg(ProtoID.CS_COMMON_HEARTBEAT, heartBeatMsg, null);
            checkTimer = new Timer(checkTime, CheckHeartBeat, 0, false);
            HomeKey._isPause = false;
        }
        else
        {
            ClientOffline();
        }
	}

	public void Start()
    {
        Continue();
        MsgCallManager.AddCallback(ProtoID.SC_COMMON_HEARTBEAT_REPLY, OnMsgHeartBeat);
	}
		

	public void Dispose()
	{
        Pause();
        MsgCallManager.RemoveCallback(ProtoID.SC_COMMON_HEARTBEAT_REPLY, OnMsgHeartBeat);
	}

    /// <summary>
    ///  暂停心跳
    /// </summary>
    public void Pause()
    {
        if (heartBeatTimer != null)
        {
            heartBeatTimer._active = false;
            heartBeatTimer = null;
        }
        if (checkTimer != null)
        {
            checkTimer._active = false;
            checkTimer = null;
        }
        heartBeatReplyWaiting = false;
    }

    /// <summary>
    /// 继续心跳
    /// </summary>
    public void Continue()
    {
        if (heartBeatTimer == null)
        {
            heartBeatTimer = new PeriodicTimer(beatTime, SendHeartBeat, beatTime, 0, 0, false);

            heartBeatReplyWaiting = false;
            SendHeartBeat();
        }
    }

}
