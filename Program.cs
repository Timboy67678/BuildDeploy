using ManyConsole;

namespace BuildDeploy
{
    class Program
    {
        public const string DefaultExtensionSearch = "exe";
        public const string DefaultDeployPort = "10102";

        private static int Main(string[] args)
        {
            var items = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
            return ConsoleCommandDispatcher.DispatchCommand(items, args, System.Console.Out);
        }
    }
}
