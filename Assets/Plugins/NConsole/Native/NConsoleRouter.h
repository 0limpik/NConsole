#import <Foundation/Foundation.h>

@protocol TestDelegate <NSObject>
    -(void)newCommandAvailable:(NSString*)commnand arg:(NSString*)argument;
@end

@interface NConsoleRouter : NSObject
    +(void)sendCommand:(NSString*)message arg:(NSString*)argument;
    +(void)setDelegate:(id<TestDelegate>)delegate;
@end
