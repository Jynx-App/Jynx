namespace Jynx.Cli
{
    internal static class ConsoleEx
    {
        public static void WriteLines(IEnumerable<string> values, Func<string?, int, string?>? prefix = null, Func<string?, int, string?>? suffix = null)
        {
            var i = 0;

            foreach (var value in values)
            {
                var p = prefix is not null ? prefix(value, i) : "";

                var s = suffix is not null ? suffix(value, i) : "";

                Console.WriteLine($"{p}{value}{s}");

                i++;
            }
        }
    }
}
