using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Olimpik.NConsole.Runtime;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Olimpik.NConsole.EditorN
{
    public class NConsoleLinker : IPreprocessBuildWithReport
    {
        private static readonly XmlSerializer serializer = new(typeof(Linker));
        private static string Path => $"{Application.dataPath}/Plugins/NConsole/Editor/link.xml";

        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report) => Link();

        [MenuItem("Tools/NConsole/StartLinker")]
        public static void Link()
        {
            var linker = new Linker { Assemblies = new List<Assembly>() };

            var behaviour = new CommandBehaviour();
            var registration = new CommandsRegistration(behaviour, behaviour);
            registration.RegisterCommands();

            foreach (var command in registration.Container.Commands)
            {
                var commandType = command.Method.DeclaringType;
                var assemblyName = commandType.Assembly.GetName().Name;
                var assembly = linker.Assemblies.SingleOrDefault(x => x.Fullname == assemblyName);
                if (assembly == null)
                {
                    assembly = new Assembly { Fullname = assemblyName, Types = new List<Type>() };
                    linker.Assemblies.Add(assembly);
                }

                var typeName = commandType.FullName.Replace('+', '/');
                var type = assembly.Types.SingleOrDefault(x => x.Fullname == typeName);
                if (type == null)
                {
                    type = new Type { Fullname = typeName, Methods = new List<Method>() };
                    assembly.Types.Add(type);
                }

                var method = type.Methods.SingleOrDefault(x => x.Name == command.Method.Name);
                if (method == null)
                {
                    method = new Method { Name = command.Method.Name };
                    type.Methods.Add(method);
                }
            }

            File.Delete(Path);
            using var writer = new FileStream(Path, FileMode.CreateNew);
            serializer.Serialize(writer, linker);
        }

        [XmlRoot("method")]
        public class Method
        {
            [XmlAttribute("signature")]
            public string Signature { get; set; }

            [XmlAttribute("name")]
            public string Name { get; set; }
        }

        [XmlRoot("type")]
        public class Type
        {
            [XmlAttribute("fullname")]
            public string Fullname { get; set; }

            [XmlAttribute("preserve")]
            public string Preserve { get; set; }

            [XmlElement("method")]
            public List<Method> Methods { get; set; }
        }

        [XmlRoot("assembly")]
        public class Assembly
        {
            [XmlElement("type")]
            public List<Type> Types { get; set; }

            [XmlAttribute("fullname")]
            public string Fullname { get; set; }
        }

        [XmlRoot("linker")]
        public class Linker
        {
            [XmlElement("assembly")]
            public List<Assembly> Assemblies { get; set; }
        }
    }
}
