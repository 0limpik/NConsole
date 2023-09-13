using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Olimpik.NConsole.Attributes;
using Olimpik.NConsole.Runtime;
using UnityEngine;

namespace Olimpik.NConsole.Commands
{
    public class BaseCommands
    {
        private static readonly string divier = $"{new string('-', 32)}";
        private static readonly string divierNewLine = $"\n{divier}\n";

        private readonly IEnumerable<Command> commands;

        public BaseCommands(IEnumerable<Command> commands)
        {
            this.commands = commands;
        }

        [NConsoleMethod("help", "Request help")]
        [NConsoleArg("command name", "display command info")]
        [NConsoleReturn("help and list command info")]
        public string Help(string argument)
        {
            if (!string.IsNullOrEmpty(argument))
            {
                var command = commands.FirstOrDefault(x => x.Name == argument);

                if (command == null)
                {
                    return "Command not found, try search with \"list\"";
                }

                return $"{command.Info}";
            }

            var helpCommand = commands.First(x => x.Name == "help");
            var listCommand = commands.First(x => x.Name == "list");
            return $"{helpCommand.Info}{divierNewLine}{listCommand.Info}";
        }

        [NConsoleMethod("list", "Display all awalible commands")]
        [NConsoleArg("display all commands")]
        [NConsoleArg("help", "display commands table")]
        [NConsoleArg("\"search pattern\"", "display commands list")]
        [NConsoleReturn("commands list or table")]
        public string List(string argument)
        {
            var allCommands = commands
                .OrderBy(x => x.Name)
                .Where(x => x.Name is "list" or "help");

            if (argument == "help")
            {
                return $"{divier}\n{string.Join(divierNewLine, allCommands.Select(x => x))}\n{divier}";
            }

            if (!string.IsNullOrEmpty(argument))
            {
                allCommands = allCommands.Where(x => x.Name.Contains(argument));
            }

            return $"{string.Join("\n", allCommands.Select(x => x.Name))}";
        }

        [NConsoleMethod("clear_cache", "Clear application cache folder ak \"Application.persistentDataPath\" or \"po NSHomeDirectory ()\"")]
        [NConsoleReturn("deleted files count")]
        public static string ClearCache()
        {
            var appDirectory = new DirectoryInfo(Application.persistentDataPath);

            var count = appDirectory.EnumerateFiles("*", SearchOption.AllDirectories).Count();

            appDirectory.Delete(true);

            return $"{count}";
        }

        [NConsoleMethod("get", "Http GET request")]
        [NConsoleArg("url")]
        [NConsoleReturn("response body")]
        public static string Get(string argument)
        {
            if (string.IsNullOrEmpty(argument))
            {
                argument = "https://api.ipify.org";
            }

            var response = new WebClient().DownloadString(argument);
            return response;
        }
    }
}
