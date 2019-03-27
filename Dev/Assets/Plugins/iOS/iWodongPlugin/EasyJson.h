//
//  EasyJson.h
//  Unity-iPhone
//
//  Created by WP.Chu on 2017/9/9.
//
//

#pragma once

#import <Foundation/Foundation.h>

@interface EasyJson : NSObject

-(instancetype)initWithString:(NSString*)str;

-(void)addKey:(NSString *)key string:(NSString *)value;
-(void)addKey:(NSString *)key int:(int)value;
-(void)addKey:(NSString *)key float:(float)value;
-(NSString*)toStr;

-(void)addString:(NSString*)value;
-(void)addInt:(int)value;
-(void)addFloat:(float)value;

-(id)objectAtIndex:(int)idx;
-(id)objectForKey:(NSString*)key;

@end
