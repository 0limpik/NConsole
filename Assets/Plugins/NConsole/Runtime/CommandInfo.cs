using System.Text;

namespace Olimpik.NConsole.Runtime
{
    public class CommandInfo
    {
        public readonly string Name;
        public readonly string Description;
        public readonly (string Name, string Description)[] Arguments;
        public readonly string Return;

        private StringBuilder sb;

        public CommandInfo(string name, string description, string @return, (string, string)[] arguments)
        {
            this.Name = name;
            this.Description = description;
            this.Return = @return;
            this.Arguments = arguments;
        }

        public override string ToString()
        {
            if (sb == null)
            {
                sb = new StringBuilder();
                AddSignature(sb);
                AddArguments(sb);
                sb.AppendLine();
                AddReturn(sb);
            }

            return sb.ToString();
        }

        private void AddSignature(StringBuilder sb) => sb.Append("\"").Append(Name).Append("\": ").Append(Description);

        private void AddArguments(StringBuilder sb)
        {
            if (Arguments == null)
            {
                return;
            }

            foreach (var argument in Arguments)
            {
                sb.AppendLine();

                if (argument.Name == null)
                {

                    sb.Append("    [no arg] ").Append(argument.Description);
                }
                else
                {
                    sb.Append("    [arg] \"").Append(argument.Name).Append("\": ").Append(argument.Description);
                }
            }
        }

        public void AddReturn(StringBuilder sb)
        {
            if (Return == null)
            {
                return;
            }

            sb.Append("[ret] ").Append(Return);
        }
    }
}
