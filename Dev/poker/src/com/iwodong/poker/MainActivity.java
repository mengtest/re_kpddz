package com.iwodong.poker;

import java.io.ByteArrayOutputStream;
import java.io.InputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.List;
import java.util.Map;

import com.alipay.sdk.app.PayTask;
import com.alipay.sdk.util.H5PayResultModel;
import com.iwodong.unityplugin.PhotoPicker;
import com.unity3d.player.UnityPlayerNativeActivity;

import android.annotation.SuppressLint;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.text.TextUtils;
import android.util.Log;
import android.view.View;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.Toast;

public class MainActivity extends UnityPlayerNativeActivity 
{
	
	/******************* 支付宝支付相关配置  START ******************/
	
	// 支付宝后台配置appid
	public static final String APPID = "";
	
	/** 支付宝账户登录授权业务：入参pid值 */
	public static final String PID = "";
	/** 支付宝账户登录授权业务：入参target_id值 */
	public static final String TARGET_ID = "";

	/** 商户私钥，pkcs8格式 */
	/** 如下私钥，RSA2_PRIVATE 或者 RSA_PRIVATE 只需要填入一个 */
	/** 如果商户两个都设置了，优先使用 RSA2_PRIVATE */
	/** RSA2_PRIVATE 可以保证商户交易在更加安全的环境下进行，建议使用 RSA2_PRIVATE */
	/** 获取 RSA2_PRIVATE，建议使用支付宝提供的公私钥生成工具生成， */
	/** 工具地址：https://doc.open.alipay.com/docs/doc.htm?treeId=291&articleId=106097&docType=1 */
	public static final String RSA2_PRIVATE = "";
	public static final String RSA_PRIVATE = "";
	
	private static final int SDK_PAY_FLAG = 1;
	private static final int SDK_AUTH_FLAG = 2;
	
	
	//H5转native支付组件
	private WebView _webView = null;
	
	/******************* 支付宝支付相关配置  END ******************/
	
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	@SuppressLint("SetJavaScriptEnabled")
	@Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }
	
	
	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data)
	{
		super.onActivityResult(requestCode, resultCode, data);
		
		if (requestCode == PhotoPicker.ACTIVITY_REQUEST_CODE_ALBUM 
			|| requestCode == PhotoPicker.ACTIVITY_REQUEST_CODE_CAMERA
			|| requestCode == PhotoPicker.ACTIVITY_REQUEST_CODE_CROP)
		{
			PhotoPicker.onPickedResult(requestCode, resultCode, data);
			return;
		}
		
		if (data == null) {
				return;
			}
			String respCode = data.getExtras().getString("respCode");
			String respMsg = data.getExtras().getString("respMsg");
			String errorCode = data.getExtras().getString("errorCode");
			AlertDialog.Builder builder = new AlertDialog.Builder(this);
			builder.setTitle("支付结果通知");
			StringBuilder temp = new StringBuilder();
			if (respCode.equals("00")) {
				temp.append("交易状态:成功");
			} else if (respCode.equals("02")) {
				temp.append("交易状态:取消");
			} else if (respCode.equals("01")) {
				temp.append("交易状态:失败").append("\n").append("错误码:")
						.append(errorCode).append("原因:" + respMsg);
			} else if (respCode.equals("03")) {
				temp.append("交易状态:未知").append("\n").append("原因:" + respMsg);
			} else {
				temp.append("respCode=").append(respCode).append("\n")
						.append("respMsg=").append(respMsg);
			}
			builder.setMessage(temp.toString());
			builder.setInverseBackgroundForced(true);
			builder.setNegativeButton("确定", new DialogInterface.OnClickListener() {
				@Override
				public void onClick(DialogInterface dialog, int which) {
					dialog.dismiss();
				}
			});
			builder.create().show();
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	/**
	 * C#调用接口
	 */
	
	// 请求支付
	public void requestPay(final String goodsID, final String goodsName, final String goodsDesc, final String quantifier,
	  		final String cpOrderID, final String callbackUrl, final String extrasParams, final String price, final String amount, final String count,
	  		final String serverName, final String serverID, final String gameRoleName, final String gameRoleID, final String gameRoleBalance, 
	  		final String vipLevel, final String gameRoleLevel, final String partyName)
	{
		if (extrasParams.equals("1"))  //支付宝
	  	{
	  		// 由于支付宝支付比较特殊，需要在服务端生成订单信息，保存在cpOder
	  		aliPay(cpOrderID);  
	  	}
	  	else if (extrasParams.equals("2"))  //微信
	  	{
	  		weChatPay(cpOrderID);
		}
	}
	
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	/**
	 * 支付宝支付相关业务
	 */
	
	
	@SuppressLint("SetJavaScriptEnabled")
	private void initH5Pay() 
	{
		// 初始化支付宝支付组件
        _webView = new WebView(getApplicationContext());
        _webView.setVisibility(View.VISIBLE);

		WebSettings settings = _webView.getSettings();
		settings.setJavaScriptEnabled(true);
		settings.setJavaScriptCanOpenWindowsAutomatically(true);
		settings.setAllowFileAccess(false);
		//settings.setCacheMode(WebSettings.LOAD_NO_CACHE); //不使用缓存
		
		_webView.setWebViewClient(new WebViewClient(){
				//重写此方法拦截http连接
				@Override
		        public boolean shouldOverrideUrlLoading(final WebView view, String url)
		        { 
					if (!(url.startsWith("http") || url.startsWith("https"))) {
						return true;
					}

					final PayTask task = new PayTask(MainActivity.this);
					final String ex = task.fetchOrderInfoFromH5PayUrl(url);
					if (!TextUtils.isEmpty(ex)) {
						System.out.println("paytask:::::" + url);
						new Thread(new Runnable() {
							public void run() {
								System.out.println("payTask:::" + ex);
								final H5PayResultModel result = task.h5Pay(ex, true);
								if (!TextUtils.isEmpty(result.getReturnUrl())) {
									MainActivity.this.runOnUiThread(new Runnable() {
										
										@Override
										public void run() {
											view.loadUrl(result.getReturnUrl());
										}
									});
								}
							}
						}).start();
					} else {
						view.loadUrl(url);
					}
					return true;
		        }
		});
	}
	
	// 回调处理句柄
	@SuppressLint("HandlerLeak")
	private Handler mHandler = new Handler() {
		@SuppressWarnings("unused")
		public void handleMessage(Message msg) {
			switch (msg.what) {
			case SDK_PAY_FLAG: {
				@SuppressWarnings("unchecked")
				PayResult payResult = new PayResult((Map<String, String>) msg.obj);
				/**
				 对于支付结果，请商户依赖服务端的异步通知结果。同步通知结果，仅作为支付结束的通知。
				 */
				String resultInfo = payResult.getResult();// 同步返回需要验证的信息
				String resultStatus = payResult.getResultStatus();
				// 判断resultStatus 为9000则代表支付成功
				if (TextUtils.equals(resultStatus, "9000")) {
					// 该笔订单是否真实支付成功，需要依赖服务端的异步通知。
					Toast.makeText(MainActivity.this, "支付成功", Toast.LENGTH_SHORT).show();
				} else {
					// 该笔订单真实的支付结果，需要依赖服务端的异步通知。
					Toast.makeText(MainActivity.this, "支付失败", Toast.LENGTH_SHORT).show();
				}
				break;
			}
			case SDK_AUTH_FLAG: {
				@SuppressWarnings("unchecked")
				AuthResult authResult = new AuthResult((Map<String, String>) msg.obj, true);
				String resultStatus = authResult.getResultStatus();

				// 判断resultStatus 为“9000”且result_code
				// 为“200”则代表授权成功，具体状态码代表含义可参考授权接口文档
				if (TextUtils.equals(resultStatus, "9000") && TextUtils.equals(authResult.getResultCode(), "200")) {
					// 获取alipay_open_id，调支付时作为参数extern_token 的value
					// 传入，则支付账户为该授权账户
					Toast.makeText(MainActivity.this,
							"授权成功\n" + String.format("authCode:%s", authResult.getAuthCode()), Toast.LENGTH_SHORT)
							.show();
				} else {
					// 其他状态值则为授权失败
					Toast.makeText(MainActivity.this,
							"授权失败" + String.format("authCode:%s", authResult.getAuthCode()), Toast.LENGTH_SHORT).show();

				}
				break;
			}
			default:
				break;
			}
		};
	};

	// 支付宝支付接口
	private void aliPay(String orderInfo) 
	{
		/**
		 * 这里只是为了方便直接向商户展示支付宝的整个支付流程；所以Demo中加签过程直接放在客户端完成；
		 * 真实App里，privateKey等数据严禁放在客户端，加签过程务必要放在服务端完成；
		 * 防止商户私密数据泄露，造成不必要的资金损失，及面临各种安全风险； 
		 * 
		 * orderInfo的获取必须来自服务端；
		 */
		
		final String order = orderInfo;
		
		// H5支付
		if (_webView != null)
		{
			String url = orderInfo;
			if (!(url.startsWith("http") || url.startsWith("https"))) {
				return;
			}

			final PayTask task = new PayTask(MainActivity.this);
			final String ex = task.fetchOrderInfoFromH5PayUrl(url);
			if (!TextUtils.isEmpty(ex)) {
				System.out.println("paytask:::::" + url);
				new Thread(new Runnable() {
					public void run() {
						System.out.println("payTask:::" + ex);
						task.h5Pay(ex, true);
					}
				}).start();
			}
			else
			{
				Toast.makeText(getApplicationContext(), "错误的支付链接", Toast.LENGTH_LONG).show();
			}
			
			return;
		}

		Log.i("thirdPay", ">>>>>>>>>>>>>>>>>>>>>>>>>> alipay: " + order);
		
		Runnable payRunnable = new Runnable() {

			@Override
			public void run() {
				PayTask alipay = new PayTask(MainActivity.this);
				Map<String, String> result = alipay.payV2(order, true);
				Log.i("thirdPay", result.toString());
				
				Message msg = new Message();
				msg.what = SDK_PAY_FLAG;
				msg.obj = result;
				mHandler.sendMessage(msg);
			}
		};

		Thread payThread = new Thread(payRunnable);
		payThread.start();
	}

	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	/**
	 * 微信支付相关业务
	 */
	
	// 微信支付接口
	private void weChatPay(String cpOrder)
	{

		// 请求获取支付串
		MyAsyncTask asyncTask = new MyAsyncTask();
		asyncTask.execute(cpOrder);
	}
	
	public static String getMD5(String content) 
	{
		try 
		{
			MessageDigest digest = MessageDigest.getInstance("MD5");
			digest.update(content.getBytes());
			return getHashString(digest);

		} catch (NoSuchAlgorithmException e) 
		{
			e.printStackTrace();
		}
		return null;
	}

	private static String getHashString(MessageDigest digest) 
	{
		StringBuilder builder = new StringBuilder();
		for (byte b : digest.digest()) 
		{
			builder.append(Integer.toHexString((b >> 4) & 0xf));
			builder.append(Integer.toHexString(b & 0xf));
		}
		
		return builder.toString();
	}
	
	private void PullWX(String pay_str) {
		if (isWeixinAvilible()) {
			Uri uri = Uri.parse(pay_str);
			
			Log.i("thirdPay", "************************************** pay_str: " + pay_str);
			Log.i("thirdPay", "************************************** uri: " + uri.toString());
			
			Intent intent = new Intent(Intent.ACTION_VIEW, uri);
			startActivity(intent);
		} else {
			Toast.makeText(getApplicationContext(), "微信未安装,不能进行支付", Toast.LENGTH_LONG)
					.show();
		}

	}
	
	
	// 是否安装微信
		public boolean isWeixinAvilible() {
			final PackageManager packageManager = getPackageManager();// 获取packagemanager
			List<PackageInfo> pinfo = packageManager.getInstalledPackages(0);// 获取所有已安装程序的包信息
			if (pinfo != null) {
				for (int i = 0; i < pinfo.size(); i++) {
					String pn = pinfo.get(i).packageName;
					if (pn.equals("com.tencent.mm")) {
						return true;
					}
				}
			}

			return false;
		}
	
	class MyAsyncTask extends AsyncTask<String, Void, String> {

		@Override
		protected String doInBackground(String... arg0) {
			return ServerToClient(arg0[0]);
		}

		@Override
		protected void onPostExecute(String result) {
			super.onPostExecute(result);
			
			
			Log.i("thirdPay", "******************************** ret: " + result);
			
			
			/**
			 * 获取到响应串之后自行截取，本示例只是演示一种截取方法。截取内容为XML中的url对应的value
			 * 
			 */
			String index_start = "<item name=\"url\" value=\"";
			String index_end = "\" /></items></fill>";
			String pay_str = result.substring(result.indexOf(index_start)
					+ index_start.length(), result.indexOf(index_end));
			PullWX(pay_str);

		}

	}
	
	public static String ServerToClient(String str) {
		String result = "";
		try {
			URL url = new URL(str);
			HttpURLConnection conn = (HttpURLConnection) url.openConnection();
			conn.setRequestMethod("GET");
			if (conn.getResponseCode() == 200) {
				InputStream is = conn.getInputStream();// 得到网络返回的输入流
				result = readData(is, "GBK");
				conn.disconnect();
			}
		} catch (Exception e) {

			e.printStackTrace();
		}
		return result;
	}

	public static String readData(InputStream inSream, String charsetName)
			throws Exception {
		ByteArrayOutputStream outStream = new ByteArrayOutputStream();
		byte[] buffer = new byte[1024];
		int len = -1;
		while ((len = inSream.read(buffer)) != -1) {
			outStream.write(buffer, 0, len);
		}
		byte[] data = outStream.toByteArray();
		outStream.close();
		inSream.close();
		return new String(data, charsetName);
	}
	
	
	public static byte[] getJavaArray()
	{
		byte[] bs = new byte[]{'a', 'c', 'd', 'e', 'f'};
		return bs;
	}
	
	public void sendArrayToJava(String[] array, int[] data)
	{
		String str = array[0] + "#" + array[1];
		
		String datastr = Integer.toString(data[0]) + "#" + Integer.toString(data[1]);
		Toast.makeText(this, "获取数组: " + str + ", " + datastr, Toast.LENGTH_SHORT).show();
	}
}
