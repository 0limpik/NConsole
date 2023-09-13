using System;
using System.Linq;
using System.Text;
using Olimpik.NConsole.Commands;
using UnityEngine;

namespace Olimpik.NConsole.Runtime
{
    public class CommandsRegistration
    {
        private static readonly string[] searchAssemblies = new string[]
        {
            "NConsole",
            "Assembly-CSharp",
            "Assembly-CSharp-firstpass",
        };

        public CommandsContainer Container { get; private set; }

        public CommandsRegistration(ICommandBehaviour behaviour, ILocationProvider provider)
        {
            Container = new CommandsContainer(behaviour, provider);
        }

        public void Register()
        {
            IOSBridge.SendMessage("Register delegate");
            RegisterDelegate();

            IOSBridge.SendMessage("Register commands");
            RegisterCommands();

            var sb = new StringBuilder();
            foreach (var command in Container.Commands)
            {
                sb.AppendLine().Append(command.Name);
            }

            IOSBridge.SendMessage($"Registered {Container.Commands.Count()} commands:\n{sb}");
        }

        public void RegisterCommands()
        {
            Container.Register(searchAssemblies);
            var baseCommands = new BaseCommands(Container.Commands);
            Container.Register(baseCommands);
        }

        private void RegisterDelegate()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                IOSBridge.Register();
            }
            else
            {
                Debug.LogException(new PlatformNotSupportedException($"Supported only {RuntimePlatform.IPhonePlayer}"));
            }

            IOSBridge.MessageReceive += OnMessageReceived;
        }

        private void OnMessageReceived(string name, string argument)
        {
            IOSBridge.SendMessage($"command: {name} argument: {argument}");
            if (name == "ping")
            {
                IOSBridge.SendMessage($"ok");
                return;
            }

            Container.Execute(name, argument);
        }
    }
}
