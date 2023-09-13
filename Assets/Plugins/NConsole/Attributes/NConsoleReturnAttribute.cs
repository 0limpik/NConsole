using System;

namespace Olimpik.NConsole.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class NConsoleReturnAttribute : Attribute
    {
        public readonly string Description;

        public NConsoleReturnAttribute(string description)
        {
            this.Description = description;
        }
    }
}