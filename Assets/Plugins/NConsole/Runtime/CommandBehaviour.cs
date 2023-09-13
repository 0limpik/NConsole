using System;
using System.Reflection;

namespace Olimpik.NConsole.Runtime
{
    public interface ICommandBehaviour
    {
        void NotFound();
        void Result(string result);
        void Success();
        void Exception(Exception exception);
    }

    public interface ILocationProvider
    {
        string GetMethodLog(MethodInfo method);
    }

    public class CommandBehaviour : ICommandBehaviour, ILocationProvider
    {
        public void NotFound() => IOSBridge.SendMessage($"Not found");

        public void Success() => IOSBridge.SendMessage($"Success");

        public void Result(string result) => IOSBridge.SendMessage($"Result:\n{result}");

        public void Exception(Exception exception)
            => IOSBridge.SendMessage($"An error occurred while executing:\n{exception}");

        public string GetMethodLog(MethodInfo method) => $"{method.DeclaringType.FullName}:{method.Name}";
    }
}
