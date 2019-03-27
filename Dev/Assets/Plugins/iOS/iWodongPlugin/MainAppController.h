//
//  MainAppController.h
//  Unity-iPhone
//
//  Created by WoDongMac1 on 2017/9/9.
//
//

#pragma once

#import "UnityAppController.h"
#include "WXApi.h"

@interface MainAppController : UnityAppController<WXApiDelegate>
@end

inline MainAppController*	GetCustomerUnityAppController()
{
    return (MainAppController*)[UIApplication sharedApplication].delegate;
}

IMPL_APP_CONTROLLER_SUBCLASS(MainAppController);


