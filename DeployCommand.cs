using ManyConsole;

namespace BuildDeploy
{
    public class DeployCommand : ConsoleCommand
    {
        private string WorkingDir, DeployAddress, FilesExtension;

        public DeployCommand()
        {
            IsCommand("deploy", "Deploy applications from directory.");

            HasRequiredOption("dir|directory=", "Application deploy directory", dir => WorkingDir = dir);
            HasRequiredOption("ext|extension=", "Target files extension", ext => FilesExtension = ext);
            HasRequiredOption("addr|address=", "Target deploying address", addr => DeployAddress = addr);
        }

        public override int Run(string[] remainingArguments)
        {
            
            return 0;
        }
    }
}
