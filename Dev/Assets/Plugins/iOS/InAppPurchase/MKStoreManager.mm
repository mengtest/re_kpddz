//
//  MKStoreManager.m
//
//  Created by Mugunth Kumar on 17-Oct-09.
//  Copyright 2009 Mugunth Kumar. All rights reserved.
//  mugunthkumar.com
//

#import "MKStoreManager.h"
#import "cexternAppPurchase.h"

@implementation MKStoreManager

@synthesize purchasableObjects;
@synthesize storeObserver;
@synthesize delegate;

// all your features should be managed one and only by StoreManager
static NSString *featureAId = @"iWodong_IOS_GAME_God_testItem1";
static NSString *featureBId = @"iwodong_IOS_GAME_God_testItem2";

BOOL featureAPurchased;
BOOL featureBPurchased;

static MKStoreManager* _sharedStoreManager; // self

//- (void)dealloc {
	
//	[_sharedStoreManager release];
//	[storeObserver release];
//	[super dealloc];
//}

+ (BOOL) featureAPurchased {
	
	return featureAPurchased;
}

+ (BOOL) featureBPurchased {
	
	return featureBPurchased;
}

+ (MKStoreManager*)sharedManager
{
	@synchronized(self) {
		
        if (_sharedStoreManager == nil) {
			
            id rlt = [[self alloc] init]; // assignment not done here
            if (rlt != nil) {
                _sharedStoreManager.purchasableObjects = [[NSMutableArray alloc] init];
			
                [MKStoreManager loadPurchases];
                _sharedStoreManager.storeObserver = [[MKStoreObserver alloc] init];
                [[SKPaymentQueue defaultQueue] addTransactionObserver:_sharedStoreManager.storeObserver];
            }
        }
    }
    return _sharedStoreManager;
}


#pragma mark Singleton Methods

+ (id)allocWithZone:(NSZone *)zone

{	
    @synchronized(self) {
		
        if (_sharedStoreManager == nil) {
			
            _sharedStoreManager = [super allocWithZone:zone];			
            return _sharedStoreManager;  // assignment and return on first allocation
        }
    }
	
    return nil; //on subsequent allocation attempts return nil	
}


- (id)copyWithZone:(NSZone *)zone
{
    return self;	
}


- (void) requestProductData: (NSSet *)productIdentifiers
{
	SKProductsRequest *request= [[SKProductsRequest alloc] initWithProductIdentifiers:productIdentifiers]; //add any other product here
    
    //[NSSet setWithObjects: @"iWodong_IOS_GAME_God_testItem1", @"iwodong_IOS_GAME_God_testItem2", nil]
	request.delegate = self;
	[request start];
    
    //[productIdentifiers autorelease];
}


- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response
{
	[purchasableObjects addObjectsFromArray:response.products];
	// populate your UI Controls here
    
        
	for(int i=0;i<[purchasableObjects count];i++)
	{
		SKProduct *product = [purchasableObjects objectAtIndex:i];
		//NSLog(@"Feature: %@, Cost: %f, ID: %@",[product localizedTitle],[[product price] doubleValue], [product productIdentifier]);
	}
    
    int nCount = [purchasableObjects count];
    if (nCount > 0)
    {
        onRequestItem(0);
    }
    else
    {
        onRequestItem(1);
    }
    
}

- (void) buyFeatureA
{
	[self buyFeature:featureAId];
}

- (void) buyFeature:(NSString*) featureId
{
	if ([SKPaymentQueue canMakePayments])
	{
        SKProduct *product = nil;
        for(int i=0;i<[purchasableObjects count];i++)
        {
            product = [purchasableObjects objectAtIndex:i];
			NSString *tempIdentifier = [product productIdentifier];
            if ([tempIdentifier isEqualToString:featureId])
            {
                
                SKPayment *payment = [SKPayment paymentWithProduct:product];
                [[SKPaymentQueue defaultQueue] addPayment:payment];
                return;
            }
        }
	}
	else
	{
		UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"MyApp" message:@"You are not authorized to purchase from AppStore"
													   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
		[alert show];
	}
}

- (void) buyFeatureB
{
	[self buyFeature:featureBId];
}

-(void) paymentCanceled: (int)tranState productId:(NSString*)productIdentifier
{
    std::string strID = [productIdentifier UTF8String];
    onBuyItem(tranState, strID.c_str(), "", "");
}

- (void) failedTransaction: (SKPaymentTransaction *)transaction
{
	if([delegate respondsToSelector:@selector(failed)])
		[delegate failed];
	
	NSString *messageToBeShown = [NSString stringWithFormat:@"Reason: %@, You can try: %@", [transaction.error localizedFailureReason], [transaction.error localizedRecoverySuggestion]];
	UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"Unable to complete your purchase" message:messageToBeShown
												   delegate:self cancelButtonTitle:@"OK" otherButtonTitles: nil];
	[alert show];
}

-(void) provideContent: (NSString*) productIdentifier  productReceipt:(NSString*)productReceipt transactionState:(int)tranState transationID:(NSString*)identifier
{
    //transfer data to Script
    std::string strID = [productIdentifier UTF8String];
    std::string strReceipt = [productReceipt UTF8String];
    std::string strTransaction = [identifier UTF8String];
    onBuyItem(tranState, strID.c_str(), strReceipt.c_str(), strTransaction.c_str());
    
	[MKStoreManager updatePurchases];
}


+(void) loadPurchases 
{
	NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];	
	featureAPurchased = [userDefaults boolForKey:featureAId]; 
	featureBPurchased = [userDefaults boolForKey:featureBId]; 	
}


+(void) updatePurchases
{
	NSUserDefaults *userDefaults = [NSUserDefaults standardUserDefaults];
	[userDefaults setBool:featureAPurchased forKey:featureAId];
	[userDefaults setBool:featureBPurchased forKey:featureBId];
}
@end
