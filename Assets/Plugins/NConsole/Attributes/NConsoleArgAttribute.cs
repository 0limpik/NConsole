using System;

namespace Olimpik.NConsole.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class NConsoleArgAttribute : Attribute
    {
        public readonly string Name;
        public readonly string Description;

        public NConsoleArgAttribute(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>Action when argument empty</summary>
        public NConsoleArgAttribute(string description)
        {
            this.Description = description;
        }
    }
}