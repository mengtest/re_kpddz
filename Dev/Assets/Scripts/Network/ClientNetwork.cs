/***************************************************************


 *
 *
 * Filename:  	ClientNetwork.cs	
 * Summary: 	网络管理类:总类
 *
 * Version:   	1.0.0
 * Author: 		XMG
 * Date:   		2015/03/16 10:22
 ***************************************************************/
using EventManager;
using Scene;
using System.IO;


namespace network.protobuf
{
    public enum NetworkError
    {
        Unkown,
        SocketConnectFailed,

    }
	public class ClientNetwork{
        public static bool _bLoginOut = false;
        public static bool _bLoginOtherDev = false;
		static ClientNetwork instance;
		/// <summary>
		/// 取单例
		/// </summary>
		/// <returns>The instance.</returns>
		public static ClientNetwork Instance
		{
			get
			{
				if (instance == null) {
					instance = new ClientNetwork ();
				}
				return instance;
			}
		}
		/// <summary>
		/// 登出时清空数据
		/// </summary>
		public static void ResetAllData()
		{
            if (instance == null)
                return;

			instance.Dispose();
			instance = null;
			instance = new ClientNetwork();
			instance.Init();
		}
		ClientSocket clientSocket;//socket消息相关
		ClientProtoMsg clientProtoMsg;//消息序列化和反序列化
		internal  HeartBeat heartBeatCheck;//心跳检测
		//MsgCacheManager msgCacheMgr;//消息缓存管理
		MsgVerifyManager msgVerifyMgr;//消息回包管理
        
		private bool disposed = false;

		public ClientNetwork() {}
		
		public void Init()
		{
            Dispose();
            disposed = false;
			
			//创建相关容器
			clientSocket = new ClientSocket ();//只有发重连消息时，才能换socket。
			clientProtoMsg = new ClientProtoMsg ();
			heartBeatCheck = new HeartBeat ();
			//msgCacheMgr = new MsgCacheManager();
			msgVerifyMgr = new MsgVerifyManager ();
			
			//注册相关事件
			EventSystem.RegisterEvent (EventID.SOKECT_CONNECT_RESULT, OnEventSocketConnectOK);
            //MsgCallManager.AddCallback(ProtoID.SC_LOGIN_RECONNECTION_REPLY, ReconnectSuccess);
			//响应相关消息
		}

		//清数据
		public void Dispose()
		{
			disposed = true;
            EventSystem.RemoveEvent(EventID.SOKECT_CONNECT_RESULT, OnEventSocketConnectOK);
            //MsgCallManager.RemoveCallback(ProtoID.SC_LOGIN_RECONNECTION_REPLY, ReconnectSuccess);
            if (heartBeatCheck != null)
			    heartBeatCheck.Dispose ();
			heartBeatCheck = null;
            if (clientSocket != null)
			    clientSocket.Dispose ();
			clientSocket = null;
            if (clientProtoMsg != null)
			    clientProtoMsg.Dispose ();
			clientProtoMsg = null;
//             if (msgCacheMgr != null)
// 			    msgCacheMgr.Dispose ();
// 			msgCacheMgr = null;
            if (msgVerifyMgr != null)
			    msgVerifyMgr.Dispose ();
			msgVerifyMgr = null;

		}

		/// <summary>
        /// 连接服务器
        /// 1.登录界面时调用该函数，失败时可以直接重复调用
        /// 2.重连时要先调用Disconnect()再调用该函数，再在connect回调中发送重连消息。
		/// </summary>
		public void Connect()
		{
			clientSocket.Connect ();
			//_reConnectTimer = new Timer(20, Disconnect, 0, false);改为重连时调用
		}

        public void NewClientSocket()
        {

            if (clientSocket != null)
                clientSocket.Dispose();
            clientSocket = new ClientSocket();
        }
        

		/// <summary>
		/// 是否已连接
		/// </summary>
		/// <returns><c>true</c> if this instance is connected; otherwise, <c>false</c>.</returns>
		public bool IsConnected()
		{
			return clientSocket.IsConnected ();
		}

		/// <summary>
		/// 是否有socket
		/// </summary>
		/// <returns><c>true</c> if this instance has client socket; otherwise, <c>false</c>.</returns>
		public bool HasClientSocket()
		{
			return clientSocket.HasClientSocket ();	
		}

        /// <summary>
        /// 检查是否“远端关闭了一个连接”
        /// </summary>
        public void CheckIsLostConnect()
        {
            if (clientSocket != null)
                clientSocket.CheckIsLostConnect();
            
        }
        /// <summary>
        /// 暂停心跳包
        /// </summary>
//         public void PauseHeartBeast()
//         {
//             if (heartBeatCheck != null)
//                 heartBeatCheck.Pause();
//         }

        /// <summary>
        /// 继续心跳包
        /// </summary>
//         public void ContinueHeartBeast()
//         {
//             if (heartBeatCheck != null)
//                 heartBeatCheck.Continue();
//         }

        /// <summary>
        /// 心跳包开关
        /// </summary>
        public void HeartBeastSwitch(bool bFlag)
        {
            if (heartBeatCheck == null)
                return;
            if (bFlag)
            {
                heartBeatCheck.Start();
            }
            else
            {
                heartBeatCheck.Dispose();
            }
        }

		/// <summary>
		/// 取已收到的最新服务消息的编号
		/// </summary>
		/// <returns>The server dlsn.</returns>
		public ulong GetServerDlsn()
		{
			return clientProtoMsg.ServerDlsn;
		}

		/// <summary>
		/// 发送消息统一接口
		/// </summary>
		/// <param name="protoType">消息ID</param>
		/// <param name="msg">消息数据</param>
		/// <param name="verifyCallBack">消息回包的响应函数</param>
		public void SendMsg(ProtoID protoType, object msg, DelegateType.OperationVerify verifyCallBack)
		{
			SendMsg (protoType, msg);
		}
		
		/// <summary>
		/// 发送消息统一接口
		/// </summary>
		/// <param name="protoType">消息ID</param>
		/// <param name="msg">消息数据</param>
		public void SendMsg(ProtoID protoType, object msg)
		{
            clientProtoMsg.SendProtoMsg(protoType, msg);
            //			MsgSaving saving = clientProtoMsg.SendProtoMsg(protoType, msg);

            // 			if (saving.NeedSave)
            // 			{
            // 				msgCacheMgr.AddSaving(saving);
            // 			}
        }

        /// <summary>
        /// 发送Lua消息
        /// </summary>
        /// <param name="idMsg"></param>
        /// <param name="stream"></param>
        public void SendLuaMsg(int idMsg, MemoryStream stream)
        {
            clientProtoMsg.SendLuaProtoMsg(idMsg, stream);
//            MsgSaving saving = clientProtoMsg.SendLuaProtoMsg(idMsg, stream);

//             if (saving.NeedSave)
//             {
//                 msgCacheMgr.AddSaving(saving);
//             }
        }


		//发送前进行加密
		public void SendSavingMsg(MsgSaving save)
		{
			clientProtoMsg.SendSavingMsg (save);
		}

		//发送加密后的字符流
		public void SendStreamMsg(MemoryStream stream)
		{
			clientSocket.Send (stream);
		}

		//接收到消息
		public void ReceiveProtoMsg(MsgStream state, int bytesRead)
		{
			clientProtoMsg.ReceiveProtoMsg (state, bytesRead);
		}

		//连接socket的回调事件
		public void OnEventSocketConnectOK(EventMultiArgs args)
		{
// 			bool result = args.GetArg<bool> ("result", true);
// 			if (result)
// 			{
// 				HeartBeastSwitch(true);
// 			}
		}


	    public void StartHeatBeast()
	    {
	        if (IsConnected()){
	            HeartBeastSwitch(true);
	        }
	    }

        //-----------------------------------------------------------------------------
        //重连功能
        //-----------------------------------------------------------------------------
        #region

        /// <summary>
        /// 开始重连
        /// 1.心跳包停止时，重连
        /// 2.转菊花超时时，重连
        /// 3.发消息时如果socket断开，重连。
        /// </summary>
        public void ReconnectWithoutAsk(bool isReconnect = false)
        {
//             if (disposed)
//                 return;
//             if (clientSocket.IsNetworkDisconnect)//如果socket断开，要等玩家点确定后开始重连。
//                 return;
//             if (_reconnecting)
//                 return;
// 
//             if (ClientNetwork._bLoginOtherDev)//如果是被踢,就不重连
//                  return;
//             if (ClientNetwork._bLoginOut)//如果是注销,就不重连
//                  return;
//             if (string.IsNullOrEmpty(GameSceneManager.sCurSenceName) || GameSceneManager.sCurSenceName == SceneName.s_StartupScene)//如果是登录场景,就不重连
//                 return;

            HeartBeastSwitch(false);
            clientSocket.Close();
//            _reconnecting = true;
            // 重置确认超时计时器，掉线后计时器失效
            msgVerifyMgr.CloseTimer();

//            UtilTools.ShowWaitWin(WaitFlag.ReconnectSocketFist, 15f);
//            new Timer(1f, Connect, 0, false);
			if (version.VersionData.IsReviewingVersion ()) {
				LoginInputController.LoginAccountServer();
				return;
			}
            if (isReconnect)
            {
                LoginInputController.LoginAccountServer();
            }
            else
            {
                UtilTools.MessageDialog(GameText.GetStr("socket_connect_bad"), "904c1d", "Center", ConnectFailed_OnClickOK);
                //重新连接失败，请检查网络后重新登录。
                //UtilTools.ErrorMessageDialog(GameText.GetStr("socket_connect_bad"), "614d46", "Center", ConnectFailed_OnClickOK);
            }
            
        }

        void ConnectFailed_OnClickOK()
        {
            //UtilTools.ReturnToLoginScene();
            UI.Controller.UIManager.CallLuaFuncCall("OnApplicationFocus", null);
            ClientNetwork.Instance.CloseSocket();
            ClientNetwork.Instance.Init();
            LoginInputController.LoginAccountServer();
        }

        /// <summary>
        /// 通知服务端进行重连
        /// </summary>
        /// <param name="user">User.</param>
        /// <param name="sdkID">Sdk I.</param>
        public virtual void SendReconnect(string user, uint sdkID)
		{
// 			cs_login_reconnection proto = new cs_login_reconnection();
// 			proto.user = user;
// 			proto.platform_flag = sdkID;
// 			proto.reconnect_key = clientProtoMsg.ReconnectKey;
// 			SendMsg(ProtoID.CS_LOGIN_RECONNECTION, proto, null);
		}

        
        void ReconnectFailed_OnClickOK()
        {
            UtilTools.ReturnToLoginScene();
        }


        public static void ToLoginScecond()
        {

        }

        //第二次发送ReconnectMsg
        public static void ToSendReconnectMsgSecond()
        {
            string userID = "";// GameDataMgr.PLAYER_DATA.PlayerUuid; //GameDataMgr.LOGIN_DATA.userName;
//             EngineManager engine = EngineManager.GetInstance();
//             int _sdkId = engine.GetSDKId();
//             ClientNetwork.Instance.SendReconnect(userID, (uint)_sdkId);
        }

        public static void ToReconnectSocketSecond()
        {
            ClientNetwork.Instance.CloseSocket();
            ClientNetwork.Instance.Connect();//第一次reconnect超时，直接尝试第二次连接socket。
        }


        #endregion
        
        public void CloseSocket()
        {
            if (clientSocket != null)
            {
                HeartBeastSwitch(false);
                clientSocket.Close();
            }
        }
	}
}