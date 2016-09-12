using ManyConsole;
using System;
using System.Collections.Generic;
using System.IO;

namespace BuildDeploy
{
    public class DeployCommand : ConsoleCommand
    {
        private string FileOrWorkingDirectory, DeployHost, DeployPort, FilesExtension;

        public DeployCommand()
        {
            IsCommand("deploy", "Deploy applications from directory.");

            HasRequiredOption("dir|directory=", "Deploy from directory", dir => FileOrWorkingDirectory = dir);
            HasRequiredOption("host=", "Target deploying host", host => DeployHost = host);
            HasOption("ext|extension=", "Targeted files extension", ext => FilesExtension = ext);
            HasOption("port=", "Target deploying port number", port => DeployPort = port);
        }

        public override int Run(string[] remainingArguments)
        {
            if (string.IsNullOrEmpty(FilesExtension))
                FilesExtension = Program.DefaultExtensionSearch;

            if (string.IsNullOrEmpty(DeployPort))
                DeployPort = Program.DefaultDeployPort;

            FileAttributes attr;
            List<string> file_names = new List<string>();

            try {
                attr = File.GetAttributes(FileOrWorkingDirectory);
            } catch(IOException e)  {
                Console.WriteLine(string.Format("Error: {0}", e.Message));
                return 1;
            }

            if (attr.HasFlag(FileAttributes.Directory))
                file_names.AddRange(Directory.GetFiles(FileOrWorkingDirectory, string.Format("*.{0}", FilesExtension)));
            else
                file_names.Add(FileOrWorkingDirectory);
            
            foreach(var file in file_names)
            {
                Console.WriteLine(file);
            }
            
            return 0;
        }
    }
}
