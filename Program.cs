using ManyConsole;

namespace BuildDeploy
{
    class Program
    {
        public const string DefaultExtensionSearch = "*";
        public const string DefaultFTPPort = "21", DefaultSFTPPort = "22", DefaultHTTPPort = "80";

        private static int Main(string[] args)
        {
            var items = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
            return ConsoleCommandDispatcher.DispatchCommand(items, args, System.Console.Out);
        }
    }
}
