using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Olimpik.NConsole.Attributes;
using UnityEngine;

namespace Olimpik.NConsole.Runtime
{
    public class CommandsContainer
    {
        private const BindingFlags allVisibility = BindingFlags.Public | BindingFlags.NonPublic;

        private static readonly Dictionary<Assembly, HashSet<MethodInfo>> cachedMethods = new();

        private readonly ICommandBehaviour behaviour;
        private readonly ILocationProvider provider;

        private readonly Dictionary<string, Command> commands = new();
        private readonly CommandFabric fabric = new();
        private readonly Dictionary<Command, object> registerdObject = new();

        public IEnumerable<Command> Commands => commands.Values;

        public CommandsContainer(ICommandBehaviour behaviour, ILocationProvider provider)
        {
            this.behaviour = behaviour;
            this.provider = provider;
        }

        public void Register(object obj)
        {
            var type = obj.GetType();
            foreach (var method in type.GetMethods(allVisibility | BindingFlags.Instance))
            {
                var command = TryRegister(method);
                if (command != null)
                {
                    registerdObject.Add(command, obj);
                }
            }
        }

        public void Register(string[] searchAssemblies)
        {
            var cached = cachedMethods.Keys
                .Where(EqualName)
                .ToList();

            foreach (var method in AppDomain.CurrentDomain.GetAssemblies()
                .Except(cached)
                .Where(EqualName)
                .SelectMany(x => x.GetTypes())
                .SelectMany(x => x.GetMethods(allVisibility | BindingFlags.Static))
                .Concat(cached.SelectMany(x => cachedMethods[x])))
            {
                if (TryRegister(method) != null)
                {
                    var assembly = method.DeclaringType.Assembly;
                    if (!cachedMethods.TryGetValue(assembly, out var methods))
                    {
                        methods = new HashSet<MethodInfo>();
                        cachedMethods.Add(assembly, methods);
                    }

                    methods.Add(method);
                }
            }

            bool EqualName(Assembly assembly)
            {
                var name = assembly.GetName().Name;
                return searchAssemblies.Any(y => y == name);
            }
        }

        private Command TryRegister(MethodInfo method)
        {
            if (method.GetCustomAttribute<NConsoleMethodAttribute>() == null)
            {
                return null;
            }

            if (commands.Values.Any(x => x.Method == method))
            {
                return null;
            }

            var requires = fabric.TryCreate(method, out var command);
            if (requires != CommandFabric.Requires.All)
            {
                Debug.LogError($"Wrong NConsole method: {provider.GetMethodLog(method)}\n{requires}");
                return null;
            }

            if (commands.TryGetValue(command.Name, out var existCommand))
            {
                var existCommandLog = provider.GetMethodLog(existCommand.Method);
                Debug.LogError($"Command: {provider.GetMethodLog(method)} already exist in {existCommandLog}");
                return null;
            }

            commands.Add(command.Name, command);
            return command;
        }

        public void Execute(string name, string argument)
        {
            if (!commands.TryGetValue(name, out var command))
            {
                behaviour.NotFound();
                return;
            }

            var result = Execute(command, argument, out var exception);

            if (exception != null)
            {
                behaviour.Exception(exception);
                return;
            }

            if (command.HasReturn)
            {
                behaviour.Result(result);
            }
            else
            {
                behaviour.Success();
            }
        }

        private string Execute(Command command, string argument, out Exception exception)
        {
            exception = null;
            try
            {
                object result;
                registerdObject.TryGetValue(command, out var obj);
                if (command.HasArgument)
                {
                    result = command.Method.Invoke(obj, new object[] { argument });
                }
                else
                {
                    result = command.Method.Invoke(obj, Array.Empty<object>());
                }

                if (command.HasReturn)
                {
                    return result.ToString();
                }

                return string.Empty;

            }
            catch (Exception ex)
            {
                exception = ex;
                return null;
            }
        }
    }
}
