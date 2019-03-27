#import <UIKit/UIKit.h>
#import <MediaPlayer/MediaPlayer.h>
#import <AVFoundation/AVFoundation.h>
#import <AVKit/AVKit.h>

#include "UnityAppController.h"
#include "UI/UnityView.h"
#include "UI/UnityViewControllerBase.h"
#include "UI/OrientationSupport.h"
#include "UI/UnityAppController+ViewHandling.h"
#include "Unity/ObjCRuntime.h"
#include "Unity/VideoPlayer.h"
#include "PluginBase/UnityViewControllerListener.h"


typedef void (^OnTouchBlock)();

@interface CancelOnTouchView : UIView
{
	OnTouchBlock	onTouch;
}
- (id)initWithOnTouchBlock:(CGRect)boundSize touchFunc:(OnTouchBlock)onTouch;
@end

#if UNITY_IOS
@interface MPVideoPlayback : NSObject<UnityViewControllerListener>
{
	MPMoviePlayerController*	moviePlayer;
    UIButton*                   closeButton;
	CancelOnTouchView*			cancelOnTouchView;

	UIColor*					bgColor;
	MPMovieControlStyle			controlMode;
	MPMovieScalingMode			scalingMode;
	BOOL						cancelOnTouch;
}

- (id)initAndPlay:(NSURL*)url bgColor:(UIColor*)color controls:(MPMovieControlStyle)control scaling:(MPMovieScalingMode)scaling cancelOnTouch:(BOOL)cot;
- (void)actuallyStartTheMovie:(NSURL*)url;
- (void)moviePlayBackDidFinish:(NSNotification*)notification;
- (void)finish;
@end
#endif

@interface AVKitVideoPlayback : NSObject<VideoPlayerDelegate,UIViewControllerTransitioningDelegate>
{
	AVPlayerViewController*		videoViewController;
    VideoPlayer*				videoPlayer;
    UIButton*                   closeButton;
	CancelOnTouchView*			cancelOnTouchView;

	UIColor*					bgColor;
	const NSString*				videoGravity;
	BOOL						showControls;
	BOOL						cancelOnTouch;
}

+ (BOOL)IsSupported;

- (void)onPlayerReady;
- (void)onPlayerDidFinishPlayingVideo;

- (id)initAndPlay:(NSURL*)url bgColor:(UIColor*)color showControls:(BOOL)controls videoGravity:(const NSString*)scaling cancelOnTouch:(BOOL)cot;
- (void)actuallyStartTheMovie:(NSURL*)url;
- (void)finish;
@end

#if UNITY_IOS
static MPVideoPlayback*		_MPVideoPlayback	= nil;
#endif
static AVKitVideoPlayback*	_AVKitVideoPlayback	= nil;


#if UNITY_IOS
@implementation MPVideoPlayback
- (id)initAndPlay:(NSURL*)url bgColor:(UIColor*)color controls:(MPMovieControlStyle)control scaling:(MPMovieScalingMode)scaling cancelOnTouch:(BOOL)cot
{
	if( (self = [super init]) )
	{
		UnityPause(1);

		bgColor			= color;
		controlMode		= control;
		scalingMode		= scaling;
		cancelOnTouch	= cot;

		UnityRegisterViewControllerListener((id<UnityViewControllerListener>)self);
		[self performSelector:@selector(actuallyStartTheMovie:) withObject:url afterDelay:0];
	}
	return self;
}
- (void)dealloc
{
	[self finish];
}
- (void)actuallyStartTheMovie:(NSURL*)url
{
	@autoreleasepool
	{
		moviePlayer = [[MPMoviePlayerController alloc] initWithContentURL:url];
		if (moviePlayer == nil)
			return;

		UIView* bgView = [moviePlayer backgroundView];
		bgView.backgroundColor = bgColor;

		[moviePlayer prepareToPlay];
		moviePlayer.controlStyle = controlMode;
		moviePlayer.scalingMode = scalingMode;

		[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(moviePlayBackDidFinish:) name:MPMoviePlayerPlaybackDidFinishNotification object:moviePlayer];
		[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(moviePlayBackDidFinish:) name:MPMoviePlayerDidExitFullscreenNotification object:moviePlayer];
		[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(moviePlayBackSourceTypeAvailable:) name:MPMovieSourceTypeAvailableNotification object:moviePlayer];
		[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(moviePlayBackMediaTypesAvailable:) name:MPMovieMediaTypesAvailableNotification object:moviePlayer];
		[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(moviePlayBackNaturalSizeAvailable:) name:MPMovieNaturalSizeAvailableNotification object:moviePlayer];
		[[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(audioRouteChanged:) name:AVAudioSessionRouteChangeNotification object:nil];

		moviePlayer.view.frame = GetAppController().rootView.bounds;

		UIView* videoView = moviePlayer.view;
		if(cancelOnTouch)
		{
            UIImageView* image = [[UIImageView alloc] initWithImage:[UIImage imageNamed:@"movie_stop_icon"]];
            if (image != nil) {
                
                CGFloat pos_x = CGRectGetWidth(GetAppController().rootView.bounds) - image.image.size.width;
                CGFloat pos_y = 0;
                CGRect touchRect = CGRectMake(pos_x, pos_y, image.image.size.width, image.image.size.height);
                CGSize screenSize = GetAppController().rootView.bounds.size;
                if (screenSize.width/screenSize.height <= 1.4) {//ipad
                    touchRect = CGRectMake(pos_x  - image.image.size.width, pos_y * 2, image.image.size.width * 2, image.image.size.height * 2);
                    [image setContentScaleFactor:2];
                }
                image.frame = touchRect;
                [GetAppController().rootView addSubview:image];
                
                videoView = cancelOnTouchView = [[CancelOnTouchView alloc]initWithOnTouchBlock:touchRect touchFunc:^(){
                    if(_MPVideoPlayback)
                        [_MPVideoPlayback finish];
                }];
                [cancelOnTouchView addSubview:moviePlayer.view];
            }
		}
		[GetAppController().rootView addSubview:videoView];
	}
}
- (void)moviePlayBackDidFinish:(NSNotification*)notification
{
	[self finish];
}
- (void)moviePlayBackSourceTypeAvailable:(NSNotification*)notification
{
	if (moviePlayer.movieSourceType == MPMovieSourceTypeUnknown)
	{
		[self finish];
	}
}
- (void)moviePlayBackMediaTypesAvailable:(NSNotification*)notification
{
	if (moviePlayer.movieMediaTypes == MPMovieMediaTypeMaskNone)
	{
		[self finish];
	}
}
- (void)moviePlayBackNaturalSizeAvailable:(NSNotification*)notification
{
	CGSize screenSize = GetAppController().rootView.bounds.size;
	BOOL ret = [VideoPlayer CheckScalingModeAspectFill:moviePlayer.naturalSize screenSize:screenSize];

	if (ret == YES && moviePlayer.scalingMode == MPMovieScalingModeAspectFit)
	{
		moviePlayer.scalingMode = MPMovieScalingModeAspectFill;
	}
}
- (void)audioRouteChanged:(NSNotification*)notification
{
	// not really cool:
	// it might happen that due to audio route changing ios can pause playback
	// alas at this point playbackRate might be not yet changed, so we just resume always
	if(moviePlayer)
		[moviePlayer play];
}
- (void)viewDidLayoutSubviews:(NSNotification *)notification {
	moviePlayer.view.frame = GetAppController().rootView.bounds;
}
- (void)finish
{
	@synchronized(self)
	{
		if(moviePlayer)
		{
		// remove notifications right away to avoid recursively calling finish from callback
			[[NSNotificationCenter defaultCenter] removeObserver:self];
		}
        if (closeButton != nil) {
            [closeButton removeFromSuperview];
            closeButton = nil;
        }
        
        if (cancelOnTouchView != nil) {
            [cancelOnTouchView removeFromSuperview];
            cancelOnTouchView = nil;
        }

		[moviePlayer.view removeFromSuperview];

		[moviePlayer pause];
		[moviePlayer stop];
		moviePlayer = nil;

		_MPVideoPlayback	= nil;

		UnityUnregisterViewControllerListener((id<UnityViewControllerListener>)self);

		if(UnityIsPaused())
			UnityPause(0);
	}
}
@end
#endif

@implementation AVKitVideoPlayback
static Class _AVPlayerViewControllerClass = nil;

#if !UNITY_TVOS
static NSUInteger supportedInterfaceOrientations_DefaultImpl(id self_, SEL _cmd)
{
	return GetAppController().rootViewController.supportedInterfaceOrientations;
}
#endif

+ (void)InitClass
{
	// TODO: dispatch_once
	if(_AVPlayerViewControllerClass == nil)
	{
		NSBundle* avKitBundle = [NSBundle bundleWithPath:@"/System/Library/Frameworks/AVKit.framework"];
		if(avKitBundle)
		{
			if( (_AVPlayerViewControllerClass = [avKitBundle classNamed:@"AVPlayerViewController"]) == nil )
			{
				[avKitBundle unload];
				return;
			}
#if !UNITY_TVOS
			ObjCSetKnownInstanceMethod(_AVPlayerViewControllerClass, @selector(supportedInterfaceOrientations), (IMP)&supportedInterfaceOrientations_DefaultImpl);
#endif
		}
	}
}
+ (BOOL)IsSupported
{
	[AVKitVideoPlayback InitClass];
	return _AVPlayerViewControllerClass != nil;
}
- (id)initAndPlay:(NSURL*)url bgColor:(UIColor*)color showControls:(BOOL)controls videoGravity:(const NSString*)scaling cancelOnTouch:(BOOL)cot
{
	if( (self = [super init]) )
	{
		UnityPause(1);

		showControls	= controls;
		videoGravity	= scaling;
		bgColor			= color;
		cancelOnTouch	= cot;

		[self performSelector:@selector(actuallyStartTheMovie:) withObject:url afterDelay:0];
	}
	return self;
}
- (void)dealloc
{
	[self finish];
}
- (void)actuallyStartTheMovie:(NSURL*)url
{
	@autoreleasepool
	{
		[AVKitVideoPlayback InitClass];
		videoViewController = [[_AVPlayerViewControllerClass alloc] init];

		videoViewController.showsPlaybackControls = showControls;
		videoViewController.view.backgroundColor = bgColor;
		videoViewController.videoGravity = (NSString *) videoGravity;
		videoViewController.transitioningDelegate = self;

#if UNITY_TVOS
		// In tvOS clicking Menu button while video is playing will exit the app. So when
		// app disables exiting to menu behavior, we need to catch the click and ignore it.
		if (!UnityGetAppleTVRemoteAllowExitToMenu())
		{
			UITapGestureRecognizer *tapRecognizer = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(handleTap:)];
			tapRecognizer.allowedPressTypes = @[@(UIPressTypeMenu)];
			[videoViewController.view addGestureRecognizer:tapRecognizer];
		}
#endif

		if(cancelOnTouch)
        {

             UIImageView* image = [[UIImageView alloc] initWithImage:[UIImage imageNamed:@"movie_stop_icon"]];
             if (image != nil) {
                 
                 CGFloat pos_x = CGRectGetWidth(GetAppController().rootView.bounds) - image.image.size.width;
                 CGFloat pos_y = 0;
                 CGRect touchRect = CGRectMake(pos_x, pos_y, image.image.size.width, image.image.size.height);
                 CGSize screenSize = GetAppController().rootView.bounds.size;
                 if (screenSize.width/screenSize.height <= 1.4) {//ipad
                     touchRect = CGRectMake(pos_x  - image.image.size.width, pos_y * 2, image.image.size.width * 2, image.image.size.height * 2);
                     [image setContentScaleFactor:2];
                 }
                 image.frame = touchRect;
                [videoViewController.view addSubview:image];
                
                cancelOnTouchView = [[CancelOnTouchView alloc] initWithOnTouchBlock:touchRect touchFunc:^(){
                    if(_AVKitVideoPlayback)
                        [_AVKitVideoPlayback finish];
                }];
                [videoViewController.view addSubview:cancelOnTouchView];
            }
		}

		videoPlayer = [[VideoPlayer alloc] init];
		videoPlayer.delegate = self;
		[videoPlayer loadVideo:url];
	}
}
- (void)handleTap:(UITapGestureRecognizer*)sender
{
	if (cancelOnTouch && (sender.state == UIGestureRecognizerStateEnded))
		[self finish];
}
- (void)onPlayerReady
{
	videoViewController.player = videoPlayer.player;

	CGSize screenSize = GetAppController().rootView.bounds.size;
	BOOL ret = [VideoPlayer CheckScalingModeAspectFill:videoPlayer.videoSize screenSize:screenSize];
	if (ret == YES && [videoViewController.videoGravity isEqualToString:AVLayerVideoGravityResizeAspect] == YES)
	{
		videoViewController.videoGravity = AVLayerVideoGravityResizeAspectFill;
	}

	[videoPlayer playVideoPlayer];
#if UNITY_TVOS
	GetAppController().window.rootViewController = videoViewController;
#else
	UIViewController *viewController = [GetAppController() topMostController];
	if ([viewController isEqual:videoViewController] == NO && [videoViewController isBeingPresented] == NO)
	{
		[viewController presentViewController:videoViewController animated:NO completion:nil];
	}
#endif
}
- (void)onPlayerDidFinishPlayingVideo
{
	[self finish];
}
- (void)onPlayerError:(NSError*)error
{
	[self finish];
}
- (id <UIViewControllerAnimatedTransitioning>)animationControllerForDismissedController:(UIViewController *)dismissed {
	if ([dismissed isEqual:videoViewController] == YES)
	{
		[self finish];
	}

	return nil;
}
- (void)finish
{
	@synchronized(self)
	{
#if UNITY_TVOS
		GetAppController().window.rootViewController = GetAppController().rootViewController;
#else
		UIViewController *viewController = [GetAppController() topMostController];
		if ([viewController isEqual:videoViewController] == YES && [viewController isBeingDismissed] == NO)
		{
			[viewController dismissViewControllerAnimated:NO completion:nil];
		}
#endif
        if (closeButton != nil) {
            [closeButton removeFromSuperview];
            closeButton = nil;
        }
        if (cancelOnTouchView != nil) {
            [cancelOnTouchView removeFromSuperview];
            cancelOnTouchView = nil;
        }
		[videoPlayer unloadPlayer];

		videoPlayer = nil;
		videoViewController = nil;

		_AVKitVideoPlayback	= nil;

#if UNITY_TVOS
		UnityCancelTouches();
#endif

		if(UnityIsPaused())
			UnityPause(0);
	}
}
@end

@implementation CancelOnTouchView
- (id)initWithOnTouchBlock:(CGRect)boundSize touchFunc:(OnTouchBlock)onTouch_;
{
	if( (self = [super initWithFrame:boundSize]) )
    {
        self.bounds = boundSize;
		self->onTouch = [onTouch_ copy];
	}
	return self;
}
- (void)touchesBegan:(NSSet*)touches withEvent:(UIEvent*)event
{
}
- (void)touchesEnded:(NSSet*)touches withEvent:(UIEvent*)event
{
	onTouch();
}
- (void)touchesCancelled:(NSSet*)touches withEvent:(UIEvent*)event
{
}
- (void)touchesMoved:(NSSet*)touches withEvent:(UIEvent*)event
{
}
- (void)onUnityUpdateViewLayout
{
	CGRect bounds = GetAppController().rootView.bounds;
	self.frame = bounds;
	for(UIView* view in self.subviews)
		view.frame = bounds;
}
@end

extern "C" void UnityPlayFullScreenVideo(const char* path, const float* color, unsigned controls, unsigned scaling)
{
	const BOOL	cancelOnTouch[]	= { NO, NO, YES, YES };
	UIColor* 	bgColor			= [UIColor colorWithRed:color[0] green:color[1] blue:color[2] alpha:color[3]];

	const bool isURL = ::strstr(path, "://") != 0;
	NSURL* url = nil;
	if(isURL)
	{
		url = [NSURL URLWithString:[NSString stringWithUTF8String:path]];
	}
	else
	{
		NSString* relPath	= path[0] == '/' ? [NSString stringWithUTF8String:path] : [NSString stringWithFormat:@"Data/Raw/%s", path];
		NSString* fullPath	= [[NSBundle mainBundle].bundlePath stringByAppendingPathComponent:relPath];
		url = [NSURL fileURLWithPath:fullPath];
	}

	{
		const BOOL		showControls[]	=	{ YES, YES, NO, NO };
		const NSString* videoGravity[]	=
		{
			AVLayerVideoGravityResizeAspectFill,	// ???
			AVLayerVideoGravityResizeAspect,
			AVLayerVideoGravityResizeAspectFill,
			AVLayerVideoGravityResize,
		};

		if([AVKitVideoPlayback IsSupported])
		{
			[_AVKitVideoPlayback finish];
			_AVKitVideoPlayback = [[AVKitVideoPlayback alloc] initAndPlay:url bgColor:bgColor
				showControls:showControls[controls] videoGravity:videoGravity[scaling] cancelOnTouch:cancelOnTouch[controls]
			];
			return;
		}
	}

#if UNITY_IOS
	{
		const MPMovieControlStyle controlMode[] =
		{
			MPMovieControlStyleFullscreen,
			MPMovieControlStyleEmbedded,
			MPMovieControlStyleNone,
			MPMovieControlStyleNone,
		};
		const MPMovieScalingMode scalingMode[] =
		{
			MPMovieScalingModeNone,
			MPMovieScalingModeAspectFit,
			MPMovieScalingModeAspectFill,
			MPMovieScalingModeFill,
		};

		[_MPVideoPlayback finish];
		_MPVideoPlayback = [[MPVideoPlayback alloc] initAndPlay:url bgColor:bgColor
			controls:controlMode[controls] scaling:scalingMode[scaling] cancelOnTouch:cancelOnTouch[controls]
		];
	}
#endif
}

extern "C" int UnityIsFullScreenPlaying()
{
#if UNITY_IOS
	return _MPVideoPlayback || _AVKitVideoPlayback ? 1 : 0;
#else
	return _AVKitVideoPlayback ? 1 : 0;
#endif
}

