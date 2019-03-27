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
using System.Net;
using System.Runtime.InteropServices;


namespace network.protobuf
{
	/// <summary>
	/// 负责socket连接和收发
	/// </summary>
	public class ClientSocket {
		/// <summary>
		/// 发送出的消息编号
		/// </summary>
		private ulong verifySn = 0;
		protected ulong VerifySn
		{
			get
			{
				++verifySn;
				return verifySn;
			}
		}
		private bool isDisposed = false;//是否已被释放
		protected Socket clientSocket;
        Thread readThread;//读消息的线程
        MsgStream recieveState;
        ClientSocketReadThread readThreadObj;
        bool bNetworkDisconnect = false;
        public bool IsNetworkDisconnect
        {
            get
            {
                if (readThreadObj != null)
                    return readThreadObj.bNetworkDisconnect;
                return bNetworkDisconnect;
             }
        }
		public bool HasClientSocket()
		{
			if (clientSocket == null)
				return false;

			return true;
		}

		public bool IsConnected()
		{
			if (clientSocket == null)
				return false;
			
			return clientSocket.Connected;
		}
        DateTime _lostConnectTime;

//		private Timer connectTimer;
		/// <summary>
		/// 连接游戏服务器
        /// 1.登录界面时调用该函数，可以重复调用
        /// 2.重连时要先调用Close()再调用该函数，再在connect回调中发送重连消息。
		/// </summary>
		public void Connect()
		{
            if (IsConnected()) return;//socket正常连接则不再创建
			
			if (isDisposed) return;


			//appStore用Ipv6协议来访问Ipv4
			if (sdk.SDKManager.isAppStoreVersion ()) {
				string serverIp;
				AddressFamily ipType;
				GetIpType(BaseConfig.IP,BaseConfig.port.ToString(), out serverIp, out ipType);  
				clientSocket = new Socket(ipType, SocketType.Stream, ProtocolType.Tcp);
				clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
				IPAddress address = IPAddress.Parse(serverIp);
				IPEndPoint point = new IPEndPoint(address,BaseConfig.port);
				try
				{
					Utils.LogSys.Log("Connection" + serverIp + ":" + BaseConfig.port);
					clientSocket.BeginConnect(point,new AsyncCallback(ConnectCallback), clientSocket);
					Utils.LogSys.Log("等待连接中……");
				}
				catch (Exception e){
					Utils.LogSys.Log(e.ToString());
				}
			} else {
				clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				clientSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);
				try
                {
                    //BaseConfig.IP = "172.26.102.23";
                    //BaseConfig.port = 8991;
					Utils.LogSys.Log("Connection" + BaseConfig.IP + ":" + BaseConfig.port);
					//UtilTools.MessageDialog("连接地址: " + BaseConfig.IP + ", port:" + BaseConfig.port);
					clientSocket.BeginConnect(BaseConfig.IP, BaseConfig.port, new AsyncCallback(ConnectCallback), clientSocket);
					// Start an asynchronous socket to listen for connections.
					Utils.LogSys.Log("等待连接中……");
				}
				catch (Exception e)
				{
					Utils.LogSys.Log(e.ToString());
				}
			}
		}
		
		/// <summary>
		/// 连接回调
		/// </summary>
		/// <param name="ar">Ar.</param>
		public void ConnectCallback(IAsyncResult ar)
		{
			// 从state对象获取socket.  
			Socket client = (Socket)ar.AsyncState;
			bool result = true;
			try
			{
                ClientNetwork._bLoginOut = false;
				client.EndConnect(ar);
				// 完成连接.  
				Utils.LogSys.Log("连接成功！");
				result = true;

                Socket handler = client;
                recieveState = new MsgStream();
				
				if (isDisposed)
					return;

				recieveState.workSocket = handler;

                if (readThreadObj != null)
                {
                    readThreadObj.toStop = true;//让该线程自然结束
                }
                readThreadObj = new ClientSocketReadThread();
                readThreadObj.recieveState = recieveState;

                readThread = new Thread(new ThreadStart(readThreadObj.BeginListen));          //创建一个新的线程专门用于处理监听
                readThread.Start();
			}
			catch (SocketException e)
			{
				// 连接失败
				// TODO: 展示连接失败
                Utils.LogSys.Log("连接失败！" + e.Message);

				result = false;
			}
			
			if (isDisposed)
				return;

			EventMultiArgs args = new EventMultiArgs ();
			args.AddArg("result", result);
            
			EventSystem.CallEvent (EventID.SOKECT_CONNECT_RESULT, args);
		}


        /// <summary>
        /// 如果socket断开，直接弹提示框
        /// </summary>
        public void CheckIsLostConnect()
        {
            if (readThreadObj != null && readThreadObj.bNetworkDisconnect && bNetworkDisconnect == false)
            {
                bNetworkDisconnect = false;

                if (ClientNetwork._bLoginOtherDev || ClientNetwork._bLoginOut)//如果是被踢,就不重连
                 {
                     return;
                 }
                Close();
                ClientNetwork.Instance.ReconnectWithoutAsk(false);
            }
        }

        void AskToReconnect()
        {
            bNetworkDisconnect = false;

            if (_lostConnectTime.AddMinutes(5) <= DateTime.Now) //离开超过5分钟, 不能重连要退出重登.
            {
                UtilTools.ReturnToLoginScene();
            }
            else
            {
                ClientNetwork.Instance.ReconnectWithoutAsk();
            }
        }

		/// <summary>
		/// 发送消息
		/// </summary>
		/// <param name="stream">Stream.</param>
		public void Send(MemoryStream stream)
		{
			if (isDisposed)
				return;

			clientSocket.BeginSend(stream.ToArray(), 0, (int)stream.Length, 0,
			                 new AsyncCallback(SendCallback), clientSocket);
		}

		/// <summary>
		/// 发送成功的回调
		/// </summary>
		/// <param name="ar">Ar.</param>
		private void SendCallback(IAsyncResult ar)
		{
			int bytesSent;
			try
			{
				// Retrieve the socket from the state object.
				Socket handler = (Socket)ar.AsyncState;
				
				// Complete sending the data to the remote device.
				bytesSent = handler.EndSend(ar);
			}
			catch (SocketException e)
			{
				Debug.LogException(e);
				// TODO: 展示发送失败
				Close();
				return;
			}
			Utils.LogSys.Log("Send " + bytesSent.ToString() + " Bytes");
		}
	
		/// <summary>
		/// 关闭连接
		/// </summary>
		public void Close()
		{
            if (readThreadObj != null)
            {
                readThreadObj.toStop = true;
                readThreadObj = null;
            }
			if (clientSocket != null)
			{
				try
				{
					clientSocket.Shutdown(SocketShutdown.Both);
					clientSocket.Close();
				}
				catch
				{   
				}
				clientSocket = null;
			}
			
		}
		
		public void Dispose()
		{
			Close ();
			isDisposed = true;
		}

		private static string GetIPv6(string host, string port)  
		{  
			//#if UNITY_IPHONE && !UNITY_EDITOR  
			return object_c.ObjectCInterface.getIPv6(host, port);  
			//#endif  
			return host + "&&ipv4";  
		}  

		public void GetIpType(string serverIp,string serverPort,out  string newServerIp,out AddressFamily newServerAddressFamily)  
		{
			newServerAddressFamily = AddressFamily.InterNetwork;  
			newServerIp = serverIp;  
			try  
			{  
				string mIPv6 = GetIPv6(serverIp, serverPort);  
				if (!string.IsNullOrEmpty(mIPv6))  
				{
					string[] strTemp = System.Text.RegularExpressions.Regex.Split(mIPv6, "&&");
					if (strTemp.Length >= 2)  
					{  
						string type = strTemp[1];  
						if (type == "ipv6")  
						{  
							newServerIp = strTemp[0];  
							newServerAddressFamily = AddressFamily.InterNetworkV6;  
						}
						else if (type == "ipv4")
						{
							newServerIp = strTemp[0];
							newServerAddressFamily = AddressFamily.InterNetwork;
						}
					}  
				}  
			}  
			catch (Exception e)  
			{  
				Debug.LogError(e.Message);  
			}  
		}  

	}
	
}