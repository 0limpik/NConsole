using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Olimpik.NConsole.Attributes;

namespace Olimpik.NConsole.Runtime
{
    internal class CommandFabric
    {
        public Requires TryCreate(MethodInfo method, out Command command)
        {
            var requires = Requires.All;
            command = null;

            var methAttr = CheckMethod(method, ref requires);
            var argsAttrs = CheckArgs(method, ref requires);
            var retAttr = CheckSignature(method, ref requires);

            if (requires == Requires.All)
            {
                var commandInfo = new CommandInfo(methAttr.Name, methAttr.Description, retAttr?.Description,
                    argsAttrs.Select(argAttr => (argAttr.Name, argAttr.Description)).ToArray());

                command = new Command(commandInfo, method);
            }

            return requires;
        }

        private NConsoleMethodAttribute CheckMethod(MethodInfo method, ref Requires requires)
        {
            var attr = method.GetCustomAttribute<NConsoleMethodAttribute>();

            if (attr == null)
            {
                requires ^= Requires.Attribute;
            }

            return attr;
        }

        private IEnumerable<NConsoleArgAttribute> CheckArgs(MethodInfo method, ref Requires requires)
        {
            var attrs = method.GetCustomAttributes<NConsoleArgAttribute>();

            var param = method.GetParameters();

            if (attrs.Count() > 0 && param.Length == 0)
            {
                requires ^= Requires.ParametersCount;

                if (param.FirstOrDefault()?.ParameterType != typeof(string))
                {
                    requires ^= Requires.ParametersType;
                }
            }

            return attrs;
        }

        private NConsoleReturnAttribute CheckSignature(MethodInfo method, ref Requires requires)
        {
            var attr = method.GetCustomAttribute<NConsoleReturnAttribute>();

            if (attr != null && method.ReturnType != typeof(string))
            {
                requires ^= Requires.ReturnType;
            }

            if (attr == null && method.ReturnType != typeof(void))
            {
                requires ^= Requires.ReturnTypeVoid;
            }

            var asyncAttr = method.GetCustomAttribute<AsyncStateMachineAttribute>();

            if (asyncAttr != null)
            {
                requires ^= Requires.AsyncMethod;
            }

            return attr;
        }

        [Flags]
        public enum Requires
        {
            All = 0b_0,
            Attribute = 0b_1,
            ParametersCount = 0b_100,
            ParametersType = 0b_1000,
            ReturnType = 0b_1000_0,
            ReturnTypeVoid = 0b_1000_00,
            AsyncMethod = 0b_1000_000,
        }
    }
}
