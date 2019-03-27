using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace _object_c_interface_ 
{
	/// <summary>
	/// 微信sdk相关接口 - ios调用
	/// </summary>
	public class WXSDK
	{
#if UNITY_IOS && !UNITY_EDITOR
		/// <summary>
		/// 设置sdk回调对象名
		/// </summary>
		/// <param name="gameObjectName">SDK回调游戏对象名.</param>
		[DllImport("__Internal")]
		public static extern void setUnityGameObjectName (string gameObjectName);

		/// <summary>
		/// 微信登陆接口
		/// </summary>
		[DllImport("__Internal")]
		public static extern void requestWXLogin();

        /// <summary>
		/// 微信分享接口
		/// </summary>
		[DllImport("__Internal")]
		public static extern void requestWXWebShare(int scene, string shareTitle, string shareText, string iconPath, string webUrl);


		/// <summary>
		/// 微信分享接口
		/// </summary>
		[DllImport("__Internal")]
		public static extern void requestWXImageShare(int scene, string iconPath, string imagePath);



		/// <summary>
		/// 微信分享接口
		/// </summary>
		[DllImport("__Internal")]
		public static extern void ShareMutilPic(int flag, string desc, string img1, string img2, string img3, string img4, string img5, string img6);

#else
        public static void setUnityGameObjectName (string gameObjectName){}

		public static void requestWXLogin(){}

        public static void requestWXWebShare(int scene, string shareTitle, string shareText, string iconPath, string webUrl) { }

		public static void requestWXImageShare(int scene, string iconPath, string imagePath) { }

		public static void ShareMutilPic(int flag, string desc, string img1, string img2, string img3, string img4, string img5, string img6) { }
#endif
    }
}
