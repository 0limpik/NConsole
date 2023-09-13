using System;

namespace Olimpik.NConsole.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NConsoleMethodAttribute : Attribute
    {
        public readonly string Name;
        public readonly string Description;

        public NConsoleMethodAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }
    }
}