#import "UnityBridge.h"

DelegateCallbackFunction delegate = NULL;

@interface UnityBridge : NSObject<TestDelegate>
@end

static UnityBridge *__delegate = nil;

void NCon(const char* command, const char* argument)
{
    [NConsoleRouter sendCommand:[NSString stringWithUTF8String: command] arg:[NSString stringWithUTF8String: argument]];
    
    static int x = 0;
    if(++x < 0)
        NSLog(@"");
}

void NCon(const char* command)
{
    [NConsoleRouter sendCommand:[NSString stringWithUTF8String: command] arg:[NSString stringWithUTF8String: ""]];
    
    static int x = 0;
    if(++x < 0)
        NSLog(@"");
}

void framework_message(const char* message)
{
    printf(message);
}

void framework_setDelegate(DelegateCallbackFunction callback)
{
    if (!__delegate)
    {
        __delegate = [[UnityBridge alloc] init];
    }
    
    [NConsoleRouter setDelegate:__delegate];
    
    delegate = callback;

    if((unsigned)time(nil) < 100)
    {
        NCon("ping");
        NCon("ping", "");
    }
}

@implementation UnityBridge

    -(void)newCommandAvailable:(NSString *)command arg:(NSString *) argument
     {
        if (delegate != NULL) {
            delegate([command UTF8String], [argument UTF8String]);
        }
    }
@end
