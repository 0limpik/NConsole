using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Olimpik.NConsole.Runtime;
using UnityEditor;
using UnityEngine;

namespace Olimpik.NConsole.EditorN
{
    internal class CommandBehaviourEditor : ICommandBehaviour, ILocationProvider
    {
        private Dictionary<Type, MonoScript> ScriptsCache => scriptsCache ??=
            AssetDatabase.FindAssets($"t:{nameof(MonoScript)}")
            .Select(x => AssetDatabase.GUIDToAssetPath(x))
            .Select(x => AssetDatabase.LoadAssetAtPath<MonoScript>(x))
            .Select(x => (type: x.GetClass(), script: x))
            .Where(x => x.type != null)
            .GroupBy(x => x.type)
            .Select(x => x.First())
            .ToDictionary(x => x.type, x => x.script);
        private Dictionary<Type, MonoScript> scriptsCache;

        public void NotFound() => Debug.LogError("Command Not Found");

        public void Result(string result) => Debug.Log(result);

        public void Success() => Debug.Log("Success");

        public void Exception(Exception exception) => Debug.LogException(exception);

        public string GetMethodLog(MethodInfo method)
            => $"<a href=\"{AssetDatabase.GetAssetPath(ScriptsCache[method.DeclaringType])}\">{method.DeclaringType}:{method.Name}</a>";
    }
}
