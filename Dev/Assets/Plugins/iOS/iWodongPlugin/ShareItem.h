//
//  Header.h
//  Unity-iPhone
//
//  Created by 瑞趣 on 2018/2/4.
//

#ifndef Share_Item_Header_h
#define Share_Item_Header_h

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
@interface SharedItem : NSObject<UIActivityItemSource>
-(instancetype)initWithData:(UIImage*)img andFile:(NSURL*)file;
@property (nonatomic, strong) UIImage *img;
@property (nonatomic, strong) NSURL *path;
@end

#endif /* Header_h */
