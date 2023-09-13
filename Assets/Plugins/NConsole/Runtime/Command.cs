using System.Reflection;

namespace Olimpik.NConsole.Runtime
{
    public class Command
    {
        public readonly CommandInfo Info;
        public readonly MethodInfo Method;
        public string Location => $"{Method.DeclaringType.FullName}:{Method.Name}";

        public string Name => Info.Name;

        public bool HasArgument => Method.GetParameters().Length > 0;
        public bool HasReturn => Method.ReturnType != typeof(void);

        internal Command(CommandInfo info, MethodInfo method)
        {
            this.Info = info;
            this.Method = method;
        }

        public override string ToString() => $"{Info}\n{Location}";
    }
}
