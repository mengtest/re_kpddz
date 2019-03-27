//
//  MKStoreObserver.m
//
//  Created by Mugunth Kumar on 17-Oct-09.
//  Copyright 2009 Mugunth Kumar. All rights reserved.
//

#import "MKStoreObserver.h"
#import "MKStoreManager.h"

@implementation MKStoreObserver

- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions
{
	for (SKPaymentTransaction *transaction in transactions)
	{
		switch (transaction.transactionState)
		{
			case SKPaymentTransactionStatePurchased:
				
                [self completeTransaction:transaction];
				
                break;
				
            case SKPaymentTransactionStateFailed:
				
                [self failedTransaction:transaction];
				
                break;
				
            case SKPaymentTransactionStateRestored:
				
                [self restoreTransaction:transaction];
				
            default:
				
                break;
		}			
	}
}

- (void) failedTransaction: (SKPaymentTransaction *)transaction
{	
    if (transaction.error.code != SKErrorPaymentCancelled)		
    {		
        // Optionally, display an error here.		
    }	
	[[MKStoreManager sharedManager] paymentCanceled:SKPaymentTransactionStateFailed productId:transaction.payment.productIdentifier];
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];
}


- (void) completeTransaction: (SKPaymentTransaction *)transaction
{
    NSString* identifier = transaction.transactionIdentifier;
    NSString* receipt = [transaction.transactionReceipt base64Encoding];
//        [[NSString alloc] initWithBytes:transaction.transactionReceipt.bytes
//                                                length:transaction.transactionReceipt.length
//                                                encoding:NSUTF8StringEncoding];
   
    
    [[MKStoreManager sharedManager] provideContent: transaction.payment.productIdentifier
                                    productReceipt:receipt
                                    transactionState:SKPaymentTransactionStatePurchased
                                    transationID:identifier];
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];	
}

- (void) restoreTransaction: (SKPaymentTransaction *)transaction
{
    NSString* identifier = transaction.transactionIdentifier;
    NSString* receipt = [transaction.transactionReceipt base64Encoding];
//    NSString* receipt = [[NSString alloc] initWithBytes:transaction.originalTransaction.transactionReceipt.bytes
//                                                length:transaction.originalTransaction.transactionReceipt.length
//                                                encoding:NSUTF8StringEncoding];
    
    
    
    [[MKStoreManager sharedManager] provideContent: transaction.originalTransaction.payment.productIdentifier
                                    productReceipt:receipt
                                    transactionState:SKPaymentTransactionStateRestored
                                    transationID:identifier];
    [[SKPaymentQueue defaultQueue] finishTransaction: transaction];	
}

@end
