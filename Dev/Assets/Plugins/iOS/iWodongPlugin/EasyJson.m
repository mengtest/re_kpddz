//
//  EasyJson.m
//  Unity-iPhone
//
//  Created by WoDongMac1 on 2017/9/9.
//
//

#import "EasyJson.h"

@implementation EasyJson{
    // for create
    NSMutableDictionary* _mdict;
    NSMutableArray* _marr;
    
    // for parse
    NSArray* _arr;
    NSDictionary* _dict;
}

-(instancetype)init
{
    if (self = [super init])
    {
        _mdict = [[NSMutableDictionary alloc] init];
    }
    
    return self;
}

-(instancetype)initWithString:(NSString*)str
{
    if (self = [super init])
    {
        _arr = nil;
        _dict = nil;
        
        NSError *e = nil;
        NSData* data = [str dataUsingEncoding:NSUTF8StringEncoding];
        id jsonObj = [NSJSONSerialization JSONObjectWithData: data options: NSJSONReadingMutableContainers error: &e];
        
        if ([jsonObj isKindOfClass:[NSArray class]])
            _arr = (NSArray*)jsonObj;
        else
            _dict = (NSDictionary*)jsonObj;
    }
    
    return self;
}

-(void)addKey: (NSString*)key string:(NSString*)value
{
    [_mdict setObject: value forKey: key];
}

-(void)addKey:(NSString *)key int:(int)value
{
    NSString* s = [NSString stringWithFormat:@"%d", value];
    [_mdict setObject: s forKey: key];
}


-(void)addKey:(NSString *)key float:(float)value
{
    NSString* s = [NSString stringWithFormat:@"%f", value];
    [_mdict setObject: s forKey: key];
}

-(NSString*)toStr
{
    NSError *error;
    NSDictionary* dict =  [NSDictionary dictionaryWithDictionary:_mdict];
    NSData* json = [NSJSONSerialization dataWithJSONObject:dict options:kNilOptions error:&error];  //kNilOptions
    
    if (json != nil)
        return [[NSString alloc] initWithData:json encoding:NSUTF8StringEncoding];
    else
        return nil;
}


-(void)addString:(NSString*)value
{
    
}

-(void)addInt:(int)value
{
    
}

-(void)addFloat:(float)value
{
    
}


-(id)objectAtIndex:(int)idx
{
    if (_arr == nil)
        return nil;
    
    if (idx < _arr.count)
        return _arr[idx];
    else
        return nil;
    }

-(id)objectForKey:(NSString*)key
{
    if (_dict == nil)
        return nil;
    
    return [_dict objectForKey:key];
}

@end
