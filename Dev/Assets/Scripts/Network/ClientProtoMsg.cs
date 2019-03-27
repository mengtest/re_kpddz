/***************************************************************


 *
 *
 * Filename:  	ClientProtoMsg.cs	
 * Summary: 	网络protobuf模块：负责消息的序列化和反序列化，加密和解密
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
using System.Collections.Generic;
using network;
using network.protobuf;
using EventManager;

namespace network.protobuf
{
    /// <summary>
    /// 处理proto消息
    /// </summary>
    public class ClientProtoMsg
    {
        /// <summary>
        /// 服务端发的消息的编号
        /// </summary>
        private ulong serverDlsn = 0;

        public static double totalMsgSize = 0; //单位kb

        public ulong ServerDlsn
        {
            get { return serverDlsn; }
            set
            {
                serverDlsn = value;

                if (serverDlsn != 0 && serverDlsn % 10 == 0){
                    EventSystem.CallEvent(EventID.SERVER_MSG_DLSN_TEN_MUL, null);
                }
            }
        }

        /// <summary>
        /// 客户端发的消息的编号
        /// </summary>
        protected ulong clientSn = 0;

        public ulong ClientSn
        {
            get
            {
                ++clientSn;
                return clientSn;
            }
        }

        /// <summary>
        /// 存储需要回包的回调函数
        /// </summary>
        protected string _reconnectKey;

        protected byte[] _encryptKey;
        private bool isDisposed = false;

        public string ReconnectKey
        {
            get { return _reconnectKey; }
            set { _reconnectKey = value; }
        }

        public byte[] EncryptKey
        {
            get { return _encryptKey; }
            set { _encryptKey = value; }
        }

        /// <summary>
        /// 消息解释
        /// 消息结构：4Byte的消息长度（含前8Byte）+ 4Byte的消息ID + 内容
        /// </summary>
        /// <param name="state">State.</param>
        /// <param name="bytesRead">Bytes read.</param>
        public void ReceiveProtoMsg(MsgStream state, int bytesRead)
        {
            if (isDisposed){
                state.stream = new MemoryStream();
                return;
            }

            if (state.stream.Length > 0){
//                 state.stream = new MemoryStream();
//             }
//             else
//             {
                // There  might be more data, so store the data received so far.
                // 将新接收的数据写入stream尾（消息可能被分包，组好后才能解释）
                state.stream.Position = state.stream.Length;
            }
            state.stream.Write(state.buffer, 0, bytesRead);

            //Utils.LogSys.Log("ReceiveProtoMsg:----------------------->" + state.stream.Length);

            try{
                // Check for end-of-file tag. If it is not there, read
                // more data.
                if (state.stream.Length >= 10){
                    state.stream.Position = 0;
                    uint sumLen = ReadLong(state.stream);

                    long leftLength = state.stream.Length;
                    while (leftLength - 4 >= sumLen) //至少有一个完整的消息时才能解释
                    {
                        float kb_size = (float) sumLen / 1024f;
                        totalMsgSize += (double) kb_size;

                        // All the data has been read from the
                        // client. Display it on the console.
                        uint dlSn = ReadLong(state.stream);
                        if (dlSn == 226){
                            int test = 1;
                        }
                        if (dlSn == 0 || ServerDlsn == dlSn - 1){
                            // 正常
                            if (dlSn != 0) ServerDlsn = dlSn;

                            int iProtoType = ReadWord(state.stream);
                            ProtoID protoType = (ProtoID) iProtoType;
                            //state.stream.Position = state.stream.Position + 8;
                            if (state.stream.Length < sumLen - 6 + state.stream.Position){
                                Utils.LogSys.LogWarning("Receive Msg Too Less: " + iProtoType);
                                Utils.LogSys.LogWarning("Receive Msg Too Less: count: " + sumLen);
                                Utils.LogSys.LogWarning("Receive Msg Too Less: Position: " + state.stream.Position);
                                Utils.LogSys.LogWarning("Receive Msg Too Less: Length: " + state.stream.Length);
                            }
                            MemoryStream protoStream = new MemoryStream(state.stream.ToArray(),
                                (int) state.stream.Position, (int) sumLen - 6);
                            protoStream.Position = 0;

                            if (!Enum.IsDefined(typeof(ProtoID), iProtoType)){
                                Utils.LogSys.LogWarning("Receive Msg Not Defined: " + iProtoType);
                                MsgCallManager.addMsgObj(protoType, protoStream);
                            }
                            else{
#if UNITY_EDITOR
                                Utils.LogSys.Log("Receive Msg : " + protoType + "(size:" + sumLen + ")");
#endif
                                if (ProtoEncryptList.protoEncryptList.Contains(protoType)){
                                    byte[] bs =
                                        EncryptionUtils.DecryptXor(protoStream, (int) (_encryptKey[0] + dlSn) % 255);
                                    protoStream.Dispose();
                                    protoStream = new MemoryStream(bs, 0, (int) sumLen - 6);
                                }
                                object protoMsg = null;
                                try{
                                    protoMsg = ProtoSerializer.ParseFrom(protoType, protoStream);
                                } catch (Exception e){
                                    Utils.LogSys.LogError(protoType);
                                    Debug.LogException(e);
                                }
                                if (protoType == ProtoID.SC_LOGIN_REPLY){
                                    _reconnectKey = (protoMsg as sc_login_reply).reconnect_key;
                                    _encryptKey = (protoMsg as sc_login_reply).proto_key;
                                }
#if UNITY_EDITOR
                                if (protoType != ProtoID.SC_COMMON_HEARTBEAT_REPLY)
                                    Utils.LogSys.Log("Got " + protoType + " proto Len:" + sumLen + ". dlSn: " + dlSn);
#endif
                                protoStream.Seek(0, SeekOrigin.Begin);
                                MsgCallManager.addMsgObj(protoType, protoStream);
                            }
//                            protoStream.Dispose();
                        }
                        else if (ServerDlsn >= dlSn){
                            // 过期
                            ProtoID protoType = (ProtoID) ReadWord(state.stream);
#if UNITY_EDITOR
                            Utils.LogSys.LogWarning("收到过期包：" + dlSn + "(protoID:" + protoType.ToString() +
                                                    "),不再处理,当前已处理的最新包编号是：" + ServerDlsn);
#endif
                        }
                        else{
                            // 丢包
                            ProtoID protoType = (ProtoID) ReadWord(state.stream);
#if UNITY_EDITOR
                            Utils.LogSys.LogError("丢失包:（编号" + (ServerDlsn + 1) + "--" + (dlSn - 1) +
                                                  ") 这次到达的是protoID:" + protoType + " ,已重新请求");
#endif
                            EventSystem.CallEvent(EventID.SERVER_MSG_MISSING, null);
                        }
                        state.stream.Position = state.stream.Position + sumLen - 6;
                        leftLength = state.stream.Length - state.stream.Position;
                        if (leftLength > 0){
                            bool hasHoleMsg = true; //字节流中是否还有完整的消息
                            if (leftLength < 10){
                                hasHoleMsg = false;
                            }
                            else{
                                sumLen = ReadLong(state.stream);
                                state.stream.Position = state.stream.Position - 4;
                                if (sumLen > leftLength - 4){
                                    hasHoleMsg = false;
                                }
                            }
                            //1.未解释完，将剩余的字符流存下来
                            if (!hasHoleMsg){
                                MemoryStream newStream = new MemoryStream();
                                newStream.Write(state.stream.ToArray(), (int) state.stream.Position, (int) leftLength);
                                state.stream.Dispose();
                                state.stream = newStream;
                                state.stream.Position = 0;
                            }
                            //2.解释完成，继续解释字符流中的下一个消息
                            else{
                                state.stream.Position = state.stream.Position + 4;
                            }
                        }
                        else{
                            //3.解释完成，new一个空字符流下一个消息
                            state.stream.Dispose();
                            state.stream = new MemoryStream();
                            break;
                        }
                    }
                }
            } catch (Exception e){
                Debug.LogException(e);
            }
        }

        protected virtual int ReadWord(Stream stream)
        {
            int n = stream.ReadByte();
            n = n * 256 + stream.ReadByte();
            return n;
        }

        protected virtual int Read3Bytes(Stream stream)
        {
            int n = 0;

            for (int i = 0; i < 3; ++i) n = n * 256 + stream.ReadByte();

            return n;
        }

        protected uint ReadLong(Stream stream)
        {
            uint n = 0;

            for (int i = 0; i < 4; ++i) n = n * 256 + (uint) stream.ReadByte();

            return n;
        }

        //消息发送接口
        public virtual MsgSaving SendProtoMsg(ProtoID protoType, object msg)
        {
            if (isDisposed) return default(MsgSaving);

            var save = GenerateStreamFromProto(protoType, msg);

            if (!ClientNetwork.Instance.IsConnected()){
                //Utils.LogSys.Log("Going to Connect");
                Utils.LogSys.Log("开始连接服务器");
                //test._obj._txt = "Going to Connect";
                // 仍未连接，先连接服务器，在连接成功后发送
                // 先保存，确认成功发送后删除
                ClientNetwork.Instance.ReconnectWithoutAsk(false);
            }
            else if (ClientNetwork.Instance.IsConnected()){
#if UNITY_EDITOR
                Utils.LogSys.Log("Send " + protoType + " proto. ulSn: " + save.Sn);
#endif
                SendSavingMsg(save);
            }
            /*
        else if (ClientNetwork.Instance.IsConnected() && protoType == ProtoID.CS_LOGIN_RECONNECTION)
        {
#if UNITY_EDITOR
            Utils.LogSys.Log("Send " + protoType + " proto. ulSn: " + save.Sn);
#endif
            SendSavingMsg(save);
        }
             * */
            return save;
        }

        /// <summary>
        /// 发送Lua消息
        /// </summary>
        /// <param name="idMsg"></param>
        /// <param name="stream"></param>
        public virtual void SendLuaProtoMsg(int idMsg, MemoryStream stream)
        {
            if (isDisposed) return; // default(MsgSaving);

            int sumLen = (int) stream.Length + 2;
            if (sumLen > 65536) throw new Exception("Message Length is too large");

            var msgSave = new MsgSaving((ProtoID)idMsg, stream, ClientSn);
            if (!ClientNetwork.Instance.IsConnected()){
                Utils.LogSys.Log("开始连接服务器");
                // 仍未连接，先连接服务器，在连接成功后发送
                // 先保存，确认成功发送后删除
                ClientNetwork.Instance.ReconnectWithoutAsk(false);
            }
            else if (ClientNetwork.Instance.IsConnected()){
#if UNITY_EDITOR
                Utils.LogSys.Log("Send " + idMsg + " proto. ulSn: " + msgSave.Sn);
#endif
                SendSavingMsg(msgSave);
            }

            //return msgSave;
        }

        //加密
        public void SendSavingMsg(MsgSaving save)
        {
            if (isDisposed) return;

            // 先保存，确认成功发送后删除
            ClientNetwork.Instance.SendStreamMsg(save.GetStreamToSend(_encryptKey));
        }

        //序列化
        protected virtual MsgSaving GenerateStreamFromProto(ProtoID protoType, object msg)
        {
            MemoryStream serializeStream = new MemoryStream();

            ProtoSerializer.Serialize(protoType, serializeStream, msg);

            int sumLen = (int) serializeStream.Length + 2;
            if (sumLen > 65536) throw new Exception("Message Length is too large");
            /*
            if (protoType == ProtoID.CS_LOGIN_RECONNECTION || protoType == ProtoID.CS_LOGIN || protoType == ProtoID.CS_COMMON_HEARTBEAT
                || protoType == ProtoID.CS_COMMON_PROTO_CLEAN || protoType == ProtoID.CS_COMMON_PROTO_COUNT)
            {
                return new MsgSaving(protoType, serializeStream, 0);
            }
            else
             * */
            return new MsgSaving(protoType, serializeStream, ClientSn);
        }

        //清数据
        public void Dispose()
        {
            isDisposed = true;
        }
    }
}