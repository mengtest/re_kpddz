//
//  InAppWebView.h
//
//  Created by Mugunth Kumar on 17-Oct-09.
//  Copyright 2009 Mugunth Kumar. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <WebKit/WebKit.h>


@interface InAppWebView : NSObject
{
    WKWebView*		webView;
    UIImageView*   titleBar;
    CGFloat        toolbarHeight;
    
}
+ (InAppWebView*) shareClass;
- (void)onClickClose:(UIButton *)sender;
- (void)showTitleBar;
- (void)showWeb:(NSURL*)url;
- (void)finish;
@end
