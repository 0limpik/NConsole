using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Search;
using UnityEngine;

namespace Olimpik.NConsole.EditorN
{
    internal class NConsoleSearchProvider
    {
        private static readonly Regex Command = new("^\\S*");
        private static readonly Regex Argument = new("(?<= ).+");

        private static CommandsRegistrationEditor Registration => BootstrapEditor.Instance;

        private static SearchContext searchContext;

        [SearchItemProvider]
        private static SearchProvider CreateProvider()
        {
            var searchProvider = new SearchProvider(nameof(NConsole), nameof(NConsole))
            {
                filterId = "nc:",
                fetchItems = GetSearchItems,
                trackSelection = (item, context) => searchContext = context,
                fetchThumbnail = (item, context) => EditorGUIUtility.IconContent("tab_next").image as Texture2D
            };

            var guiContent = new GUIContent("Execute", EditorGUIUtility.IconContent("d_PlayButton On@2x").image as Texture2D);
            var actions = new SearchAction(searchProvider.id, $"{searchProvider.id}_action", guiContent)
            {
                execute = ExecuteCommand
            };

            searchProvider.actions.Add(actions);

            return searchProvider;
        }

        private static void ExecuteCommand(SearchItem[] items)
        {
            var name = items[0].data as string;

            if (!searchContext.searchQuery.TrimStart().StartsWith(name))
            {
                searchContext.searchText = $"{searchContext.filterId} {name} ";
            }

            var argumentStr = Argument.Match(searchContext.searchQuery).Value;

            Registration.Execute(name, argumentStr);
        }

        private static object GetSearchItems(SearchContext context, List<SearchItem> items, SearchProvider provider)
        {
            if (context.filterId == null)
            {
                return null;
            }

            var commandName = Command.Match(context.searchQuery).Value;

            foreach (var command in Registration.Commands.Where(x => x.Name.Contains(commandName)))
            {
                var item = provider.CreateItem($"command_{command.Name}", command.Name,
                    command.Info.Description, null, command.Name);
                items.Add(item);
            }

            return null;
        }
    }
}
