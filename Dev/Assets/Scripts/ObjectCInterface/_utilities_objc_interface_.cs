using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace _object_c_interface_
{
	/// <summary>
	/// 系统通用操作
	/// </summary>
	public class SystemUtilsOperation
	{
		/// <summary>
		/// 从相册捡取图片
		/// </summary>
		/// <param name="compressFormat">压缩格式(png,jpg, jpeg)</param>
		/// <param name="crop">是否裁剪.</param>
		/// <param name="outputX">输出X（宽度）.</param>
		/// <param name="outputY">输出Y（高度）.</param>
		/// <param name="gameObjectName">unity回调对象名称.</param>
		[DllImport("__Internal")]
		public static extern void pickPictureFromAlbum (string compressFormat, bool crop, int outputX, int outputY, string gameObjectName); 

		/// <summary>
		/// 从相册捡取图片
		/// </summary>
		/// <param name="compressFormat">压缩格式(png,jpg, jpeg)</param>
		/// <param name="crop">是否裁剪.</param>
		/// <param name="outputX">输出X（宽度）.</param>
		/// <param name="outputY">输出Y（高度）.</param>
		/// <param name="gameObjectName">unity回调对象名称.</param>
		[DllImport("__Internal")]
		public static extern void pickPictureFromCamera (string compressFormat, bool crop, int outputX, int outputY, string gameObjectName);

		/// <summary>
		/// 获取电池剩余电量（0.0～1.0）
		/// </summary>
		/// <returns>The battery remaining power.</returns>
		[DllImport("__Internal")]
		public static extern float getBatteryRemainingPower ();


		/// <summary>
		/// 震动设备
		/// </summary>
		[DllImport("__Internal")]
		public static extern void vibrateDevice();

		/// <summary>
		/// 请求支付
		/// </summary>
		/// <param name="goodsID">Goods I.</param>
		/// <param name="goodsName">Goods name.</param>
		/// <param name="goodsDesc">Goods desc.</param>
		/// <param name="quantifier">Quantifier.</param>
		/// <param name="cpOrderID">Cp order I.</param>
		/// <param name="callbackUrl">Callback URL.</param>
		/// <param name="extrasParams">Extras parameters.</param>
		/// <param name="price">Price.</param>
		/// <param name="amount">Amount.</param>
		/// <param name="count">Count.</param>
		/// <param name="serverName">Server name.</param>
		/// <param name="serverID">Server I.</param>
		/// <param name="gameRoleName">Game role name.</param>
		/// <param name="gameRoleID">Game role I.</param>
		/// <param name="gameRoleBalance">Game role balance.</param>
		/// <param name="vipLevel">Vip level.</param>
		/// <param name="gameRoleLevel">Game role level.</param>
		/// <param name="partyName">Party name.</param>
		[DllImport("__Internal")]
		public static extern void requestPay (string goodsID, string goodsName, string goodsDesc, string quantifier,
		                                     string cpOrderID, string callbackUrl, string extrasParams, string price, string amount, string count,
		                                     string serverName, string serverID, string gameRoleName, string gameRoleID, string gameRoleBalance, 
		                                     string vipLevel, string gameRoleLevel, string partyName);
        [DllImport("__Internal")]
		public static extern void InitPaySdk(string loginAppId, string payAppId, string partnerId);

		[DllImport("__Internal")]
		public static extern void avmSign(string input, int type);


        
	}


	////////////////////////////////////////////////////////////////////////////////////////


	/// <summary>
	/// iOS语音处理
	/// 
	/// NOTE: 目前只支持amr格式的音频文件
	/// </summary>
	public class GameAudioManager
	{
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="audiosTargetDir"> 语音文件目标文件目录 </param>
		[DllImport("__Internal")]
		public static extern void initAudio (string audiosTargetDir);

		/// <summary>
		/// 设置最小时间
		/// </summary>
		/// <param name="minimumSeconds"> 最小时间，秒为单位 </param>
		[DllImport("__Internal")]
		public static extern void setMinimumTime (float minimumSeconds);

		/// <summary>
		/// 清空语音文件目录
		/// </summary>
		[DllImport("__Internal")]
		public static extern void clear ();

		/// <summary>
		/// 删除指定语音
		/// </summary>
		/// <param name="audioName"> 录音名字(可带扩展名)</param>
		[DllImport("__Internal")]
		public static extern void deleteAudio (string audioName);

		/// <summary>
		/// 添加音频名称
		/// </summary>
		/// <param name="audio">录音名字(可带扩展名)</param>
		[DllImport("__Internal")]
		public static extern void addAudionName (string audio);

		/// <summary>
		/// 开始录音
		/// </summary>
		/// <param name="audioName">录音名字(可带扩展名)</param>
		[DllImport("__Internal")]
		public static extern void startRecord (string audioName);

		/// <summary>
		/// 停止录音
		/// </summary>
		[DllImport("__Internal")]
		public static extern void stopRecord ();

		/// <summary>
		/// 播放音频
		/// </summary>
		/// <param name="audioName">录音名字(可带扩展名)</param>
		[DllImport("__Internal")]
		public static extern void playAudio (string audioName);
	}


	////////////////////////////////////////////////////////////////////////////////////////


	/// <summary>
	/// 异步HTTP
	/// 
	/// @Note: 如果无特别指明是绝对路径，则下载文件的根本目录在app的临时目录
	/// </summary>
	public class AsyncHttpClient
	{
		/// <summary>
		/// 初始化异步HTTP客户端
		/// </summary>
		/// <param name="baseUrl">Base URL.</param>
		[DllImport("__Internal")]
		public static extern void initAsyncClient (string baseUrl);

		/// <summary>
		/// 异步HTTP下载文件
		/// </summary>
		/// <param name="url">下载URL地址.</param>
		/// <param name="relativeUrl">是否相对地址，是则取 baseUrl+url</param>
		/// <param name="destPath">目标文件路径</param>
		/// <param name="destFileName">目标文件名</param>
		/// <param name="vibrator">下载完成震动时间，0不震动</param>
		/// <param name="gameObjectName">unity回调对象名称</param>
		[DllImport("__Internal")]
		public static extern void  downloadFileDefault (string url, bool relativeUrl, string destPath, string destFileName, int vibrator, string gameObjectName);

        /// <summary>
        /// 异步HTTP下载文件
        /// </summary>
        /// <param name="url">下载URL地址.</param>
        /// <param name="relativeUrl">是否相对地址，是则取 baseUrl+url</param>
        /// <param name="destPath">目标文件路径</param>
        /// <param name="relativePath">是否是相对路径</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="vibrator">下载完成震动时间，0不震动</param>
        /// <param name="gameObjectName">unity回调对象名称</param>
        [DllImport("__Internal")]
		public static extern void  downloadFile (string url, bool relativeUrl, string destPath, bool relativePath, string destFileName, int vibrator, string gameObjectName);


		/// <summary>
		/// 上传文件（流）到服务器
		/// </summary>
		/// <param name="url">URL地址.</param>
		/// <param name="relativeUrl">是否相对地址.</param>
		/// <param name="destFileName">目标文件名.</param>
		/// <param name="fileAbsolutePath">文件绝对路径.</param>
		/// <param name="gameObjectName">unity回调对象名称</param>
		[DllImport("__Internal")]
		public static extern void uploadFile (string url, bool relativeUrl, string destFileName, string fileAbsolutePath, string gameObjectName);
	}
}
