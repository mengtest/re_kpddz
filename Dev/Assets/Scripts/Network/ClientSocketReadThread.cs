/***************************************************************


 *
 *
 * Filename:  	ApplicationMgr.cs	
 * Summary: 	网络socket模块：负责连接和收发
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/16 10:22
 ***************************************************************/

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Net.Sockets;
using network;
using EventManager;
using System.Threading;


namespace network.protobuf
{
	/// <summary>
	/// 负责socket连接和收发
	/// </summary>
    public class ClientSocketReadThread
    {
        public MsgStream recieveState;
        public bool bNetworkDisconnect = false;
        public bool toStop = false;

    //    public void BeginListen()               //Socket监听函数, 等下作为创建新线程的参数
    //    {
    //        while (true)                     //这里弄了个死循环来监听端口, 有人会问死循环了,那程序不卡住了, 注意这只是个类, 这里还没有main函数呢.
    //        {
    //            if (toStop)
    //                return;
                
    //            try
    //            {
    //                if (recieveState != null)
    //                {
    //                    int bytesRead = recieveState.workSocket.Receive(recieveState.buffer); //接受client发送过来的数据保存到缓冲区.
    //                    if (bytesRead != 0)
    //                    {
    //                        ClientNetwork.Instance.ReceiveProtoMsg(recieveState, bytesRead);
    //                    }
    //                    bNetworkDisconnect = false;
    //                    Thread.Sleep(5);
    //                }
    //            }

    //            catch (SocketException se)              //捕捉异常,
    //            {
    //                bNetworkDisconnect = true;
    //                //Utils.LogSys.LogError(se.Message);
    //                Thread.Sleep(5);

    //                return;
    //            }
    //        }
    //    }

        public void BeginListen()               //Socket监听函数, 等下作为创建新线程的参数
        {
            while (true)                     //这里弄了个死循环来监听端口, 有人会问死循环了,那程序不卡住了, 注意这只是个类, 这里还没有main函数呢.
            {
                if (toStop)
                    return;

                try
                {
                    if (recieveState != null)
                    {
                        int bytesRead = recieveState.workSocket.Receive(recieveState.buffer); //接受client发送过来的数据保存到缓冲区.
                        if (bytesRead != 0)
                        {
                            ClientNetwork.Instance.ReceiveProtoMsg(recieveState, bytesRead);
                        }
                        bNetworkDisconnect = false;
                    }
                    Thread.Sleep(5);
                }

                catch (SocketException se)              //捕捉异常,
                {
                    if (toStop)
                        return;

                    if (se.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionAborted || se.SocketErrorCode == System.Net.Sockets.SocketError.ConnectionReset)
                    {
                        //被踢或网络断开
                        bNetworkDisconnect = true;
                        return;
                    }
                    UnityEngine.Debug.LogError("Receive Msg Error:" + se.ErrorCode.ToString() + "  " + se.Message);
                    //ClientNetwork.Instance.HeartBeastSwitch(false);
                    Thread.Sleep(5);

                }
            }
        }
    }
}