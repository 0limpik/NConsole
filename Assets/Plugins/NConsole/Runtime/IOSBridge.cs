using System.Runtime.InteropServices;
using AOT;

namespace Olimpik.NConsole.Runtime
{
    public delegate void DelegateMessage(string command, string argument);

    internal static class IOSBridge
    {
        public static DelegateMessage MessageReceive;

        public static void Register() => framework_setDelegate(framework_MessageReceived);

        public static void SendMessage(string message) => framework_message(message ?? string.Empty);

        [DllImport("__Internal")]
        private static extern void framework_message(string message);

        [DllImport("__Internal")]
        private static extern void framework_setDelegate(DelegateMessage callback);

        [MonoPInvokeCallback(typeof(DelegateMessage))]
        private static void framework_MessageReceived(string command, string argument) => MessageReceive?.Invoke(command, argument);
    }
}
