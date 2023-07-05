using ConsoleAppFramework;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Jynx.Cli.Commands
{
    internal class DescriptionAttribute : CommandAttribute
    {
        public DescriptionAttribute(string description, [CallerMemberName] string commandName = "") : base(FormatName(commandName), description)
        {
        }

        private static string FormatName(string name)
        {
            var r = new Regex(@"
                (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            return r.Replace(name, "-").ToLower();
        }
    }
}
