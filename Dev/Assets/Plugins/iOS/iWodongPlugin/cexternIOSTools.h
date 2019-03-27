//
//  cexternAppPurchase.h
//  InAppPurchasesExample
//
//  Created by iWodong on 13-9-5.
//
//

#ifndef Cextern_IOS_Tools_h
#define Cextern_IOS_Tools_h

	#include <iostream>
	#include <string>
	#include <vector>

	using namespace std;

	/**
	 * Bridge between Script and Object-C
	 */

	#ifdef __cplusplus
	extern "C" {
	#endif
    //执行#C#函数
    void CallUnityFunction(NSString *callbackFun, NSString *args);
	//注册回调函数的对象
    void registerCallbackGameObject(const char*  name);
    //打开网页
	void openURL(char* identifier);
    //Copy到粘贴面板
    void copyToPasteBoard(char* identifier);
    //取设备唯一ID
    void getUUID();
	//取广告标识符
    void getIDFA();
	//处理IPV6地址
    const char* getIPv6(const char *mHost,const char *mPort);
    //打开内嵌网页
    void OpenWebView(char* c_url);
	#ifdef __cplusplus
	}
	#endif

#endif
