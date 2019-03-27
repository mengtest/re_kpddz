//
//  cexternAppPurchase.h
//  InAppPurchasesExample
//
//  Created by iWodong on 13-9-5.
//
//

#ifndef InAppPurchasesExample_cexternAppPurchase_h
#define InAppPurchasesExample_cexternAppPurchase_h

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
    /**
     * Register script function response to the item request
     */
    void registerScriptOnResponseRequest(const char*  callbackFunc);
    
    /**
     * Register observer function of transactions
     */
    void registerScriptObserver( const char*  completeCallbackFunc, const char*  failedCallbackFunc);
    
    
    /**
     * requestItemInfo
     */
    void requestItemInfo(const char* strItemList);
    
    /**
     * Request item call back
     */
    //+(void) onRequestItem:(int) nRlt;
    void onRequestItem(int nRlt);

    /**
     * Buy item call back
     */
    //+(void) onBuyItem:(int) nRlt: (const char*) identifier: (const char*) receipt;
    void onBuyItem(int nRlt,const char* identifier,const char* receipt,const char* transationID);

    /**
     * Buy item by identifier
     */
    void buyItem(char* identifier);
    
    ////////////////////////////////////////////////////////////
    // http post
    //+(void) postData:(const char*) info;
    
    /**
     * Execute script functions by handler have registered.
     */
     //+(void) executeScritpHandler:(int) nScriptHandler: (int) agrc;


    
#ifdef __cplusplus
}
#endif

#endif
