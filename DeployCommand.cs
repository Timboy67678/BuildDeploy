using ManyConsole;
using System;
using System.Collections.Generic;
using System.IO;

namespace BuildDeploy
{
    public class DeployCommand : ConsoleCommand
    {
        enum DeployMethod
        {
            NONE = -1,
            SFTP,
            FTP,
            HTTP,
        };

        DeployMethod DeployWith;

        private string DeployHostname, DeployTargetPort;
        private string FileOrWorkingDirectory, FilesExtension;

        public DeployCommand()
        {
            IsCommand("deploy", "Deploy applications from directory.");

            HasRequiredOption("mode=", "Specified mode to transfer the files (FTP, SFTP or HTTP only atm)", mode => 
            {
                switch (mode.ToLower())
                {
                    case "sftp":
                        DeployWith = DeployMethod.SFTP;
                        break;
                    case "ftp":
                        DeployWith = DeployMethod.FTP;
                        break;
                    case "http":
                        DeployWith = DeployMethod.HTTP;
                        break;
                    default:
                        DeployWith = DeployMethod.NONE;
                        break;
                }
            } );

            HasRequiredOption("dir|directory=", "Deploy from directory", dir => FileOrWorkingDirectory = dir);
            HasRequiredOption("host=", "Target deploying host", host => DeployHostname = host);
            HasOption("ext|extension=", "Targeted files extension", ext => FilesExtension = ext);
            HasOption("port=", "Target deploying port number", port => DeployTargetPort = port);
        }

        public override int Run(string[] remainingArguments)
        {
            FileAttributes attr;

            if (DeployWith == DeployMethod.NONE)
            {
                Console.WriteLine("Deployment mode unknown! Current working modes are SFTP, FTP and HTTP");
                return 1;
            }

            if (string.IsNullOrEmpty(FilesExtension))
                FilesExtension = Program.DefaultExtensionSearch;

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
