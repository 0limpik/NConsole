#import "NConsoleRouter.h"

@implementation NConsoleRouter

    id __delegate = nil;

    + (void)sendCommand:(NSString *)command arg:(NSString *) argument
    {
        if (__delegate && [__delegate respondsToSelector:@selector(newCommandAvailable:arg:)])
        {
            [__delegate newCommandAvailable:command arg:argument];
        }
    }

    + (void)setDelegate:(id<TestDelegate>)delegate {
        __delegate = delegate;
    }

@end
