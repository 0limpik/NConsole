#import "NConsoleRouter.h"

extern "C" {
    void framework_message(const char* message);

    typedef void (*DelegateCallbackFunction)(const char* command, const char* argument);
    void framework_setDelegate(DelegateCallbackFunction callback);
    void framework_sendMessage(const char* message);
}

extern void NCon(const char* command, const char* argument);
extern void NCon(const char* command);
