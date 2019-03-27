

#include "InAppWebView.h"
#include "UnityAppController.h"


static InAppWebView* _staticWebView = nil;

@implementation InAppWebView

+ (InAppWebView*) shareClass
{
    @synchronized(self)
    {
        if (_staticWebView == nil)
        {
            _staticWebView = [[self alloc] init];
        }
    }
    return _staticWebView;
}
+(id)allocWithZone:(NSZone *)zone{
    
    @synchronized(self){
        
        if (!_staticWebView) {
            
            _staticWebView = [super allocWithZone:zone]; //确保使用同一块内存地址
            
            return _staticWebView;
            
        }
        
    }
    
    return nil;
}
- (id)copyWithZone:(NSZone *)zone;{
    
    return self; //确保copy对象也是唯一
}


- (void)dealloc
{
	//[self finish];
}
- (void)showTitleBar
{
	@autoreleasepool
	{
        titleBar = [[UIImageView alloc] initWithImage:[UIImage imageNamed:@"webview_toolbar"]];
        UIImage* image = [UIImage imageNamed:@"webview_tb_back"];
        if (titleBar != nil && image != nil)
        {
            CGSize screenSize = GetAppController().rootView.bounds.size;
            toolbarHeight = titleBar.image.size.height;
            CGRect touchRect = CGRectMake(-1, 0, screenSize.width+1, toolbarHeight);
            if (screenSize.width/screenSize.height > 1.4) //iphnoe
            {
                toolbarHeight = titleBar.image.size.height * 0.5;
                touchRect = CGRectMake(-1, 0, screenSize.width+1, toolbarHeight);
                [titleBar setContentScaleFactor:0.5];
            }
            titleBar.frame = touchRect;
            titleBar.userInteractionEnabled = true;
            
            CGFloat btnWidth =image.size.width;
            CGFloat btnHeight =image.size.height;
            touchRect = CGRectMake(10, 10, btnWidth, btnHeight);
            if (screenSize.width/screenSize.height > 1.4) //iphone
            {
                btnWidth =image.size.width * 0.5;
                btnHeight =image.size.height * 0.5;
                touchRect = CGRectMake(10, 5, btnWidth, btnHeight);
            }
            
            UIButton* buttonClose = [[UIButton alloc] init];
            [buttonClose setBackgroundImage:image forState:UIControlStateNormal];
            [buttonClose setFrame:touchRect];
            [buttonClose addTarget:self action:@selector(onClickClose:) forControlEvents:UIControlEventTouchUpInside];
            //[buttonClose sendActionsForControlEvents:UIControlEventTouchUpInside];
            
            [titleBar addSubview:buttonClose];
            [GetAppController().rootView addSubview:titleBar];
        }
	}
}

- (void)onClickClose:(UIButton *)sender
{
    if (_staticWebView)
    {
        [_staticWebView finish];
    }
}

- (void)showWeb:(NSURL*)url
{
    @autoreleasepool {
        
        // 1.创建webview，并设置大小，"20"为状态栏高度
        webView = [[WKWebView alloc] initWithFrame:CGRectMake(0, toolbarHeight, GetAppController().rootView.frame.size.width,
                                                                         GetAppController().rootView.frame.size.height - toolbarHeight)];
        // 2.创建请求
        NSMutableURLRequest *request =[NSMutableURLRequest requestWithURL:url];
        // 3.加载网页
        [webView loadRequest:request];
        
        // 最后将webView添加到界面
        [GetAppController().rootView addSubview:webView];
    }
}

- (void)finish
{
	@synchronized(self)
	{
        if (_staticWebView != nil)
        {
            if (webView != nil) {
                [webView removeFromSuperview];
                webView = nil;
            }
        
            if (titleBar != nil) {
                [titleBar removeFromSuperview];
                titleBar = nil;
            }
//            _staticWebView = nil;
        }
//		if(UnityIsPaused())
//			UnityPause(0);
	}
}
@end


//extern "C" void OpenWeb(const char* c_url)
//{
//    _WKWebView = [WKWebViewInApp alloc];
//    NSURL* url = [NSURL URLWithString:[NSString stringWithUTF8String:c_url]];
//    [_WKWebView showTitleBar];
//    [_WKWebView showWeb:url];
//
//}


