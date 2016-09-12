using ManyConsole;

namespace BuildDeploy
{
    class Program
    {
        private static int Main(string[] args)
        {
            var items = ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
            return ConsoleCommandDispatcher.DispatchCommand(items, args, System.Console.Out);
        }
    }
}
