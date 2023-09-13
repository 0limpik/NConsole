using System.Collections.Generic;
using System.Linq;
using System.Text;
using Olimpik.NConsole.Runtime;
using UnityEditor;
using UnityEngine;

namespace Olimpik.NConsole.EditorN
{
    internal class BootstrapEditor
    {
        public static CommandsRegistrationEditor Instance
            => instance ??= new CommandsRegistrationEditor(new CommandsRegistration(behaviour, behaviour)).Register();
        private static CommandsRegistrationEditor instance;

        private readonly static CommandBehaviourEditor behaviour = new();
        private static IEnumerable<Command> commands;

        [InitializeOnLoadMethod]
        static void EntryPoint()
        {
            var registration = new CommandsRegistration(behaviour, behaviour);
            var editorRegistration = new CommandsRegistrationEditor(registration);
            editorRegistration.Register();

            commands = registration.Container.Commands;
        }

        [MenuItem("Tools/NConsole/CheckCommands")]
        static void CheckCommands()
        {
            Instance.Register();

            var sb = new StringBuilder();
            sb.AppendLine($"Registred {commands.Count()} commands:");
            foreach (var command in commands)
            {
                sb.AppendLine($"{command.Info}");
                sb.AppendLine($"(at {behaviour.GetMethodLog(command.Method)})");
            }
            Debug.Log(sb.ToString());
        }
    }
}
