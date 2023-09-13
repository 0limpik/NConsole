using System.Collections.Generic;
using Olimpik.NConsole.Runtime;

namespace Olimpik.NConsole.EditorN
{
    internal class CommandsRegistrationEditor
    {
        private static readonly string[] searchAssemblies = new string[]
        {
            "NConsole.Editor",
            "Assembly-CSharp-Editor",
            "Assembly-CSharp-Editor-firstpass",
        };

        private readonly CommandsRegistration registration;
        public IEnumerable<Command> Commands => registration.Container.Commands;

        public CommandsRegistrationEditor(CommandsRegistration registration)
        {
            this.registration = registration;
        }

        public CommandsRegistrationEditor Register()
        {
            registration.RegisterCommands();
            registration.Container.Register(searchAssemblies);
            return this;
        }

        public void Execute(string name, string argument) => registration.Container.Execute(name, argument);
    }
}
