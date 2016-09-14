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
            FTP,
            SFTP,
            HTTP,
        };

        DeployMethod DeployWith;
        IDeployable Deployment;
        private string DeployHostname, DeployTargetPort;
        private string FileOrWorkingDirectory, FilesExtension;

        public DeployCommand( )
        {
            IsCommand( "deploy", "Deploy applications from directory." );

            HasRequiredOption( "mode=", "Specified mode to transfer the files (FTP, SFTP or HTTP only atm)", mode =>
             {
                 switch ( mode.ToLower() )
                 {
                     case "ftp":
                         Deployment = new DeployFTP();
                         DeployWith = DeployMethod.FTP;
                         DeployTargetPort = Program.DefaultFTPPort;
                         break;
                     case "sftp":
                         Deployment = new DeploySFTP();
                         DeployWith = DeployMethod.SFTP;
                         DeployTargetPort = Program.DefaultSFTPPort;
                         break;
                     case "http":
                         Deployment = new DeployHTTP();
                         DeployWith = DeployMethod.HTTP;
                         DeployTargetPort = Program.DefaultHTTPPort;
                         break;
                     default:
                         Deployment = null;
                         DeployWith = DeployMethod.NONE;
                         break;
                 }
             } );

            HasRequiredOption( "dir|directory=", "Deploy from directory", dir => FileOrWorkingDirectory = dir );
            HasRequiredOption( "host=", "Target deploying host", host => DeployHostname = host );
            HasOption( "ext|extension=", "Targeted files extension", ext => FilesExtension = ext );
            HasOption( "port=", "Target deploying port number", port => DeployTargetPort = port );
        }

        public override int Run( string[] remainingArguments )
        {
            FileAttributes attr = 0;
            int port = 0;
            var file_names = new List<string>();

            if ( DeployWith == DeployMethod.NONE || Deployment == null)
            {
                Console.WriteLine( "Deployment mode unknown! Current working modes are FTP, SFTP and HTTP" );
                return 1;
            }

            if ( string.IsNullOrEmpty( FilesExtension ) )
                FilesExtension = Program.DefaultExtensionSearch;

            Deployment.HostName = DeployHostname;
            if ( int.TryParse( DeployTargetPort, out port ) )
                Deployment.TargetPort = port;
            else {
                Console.WriteLine( "Failed to convert port \"{0}\" to integer!", DeployTargetPort );
                return 1;
            }

            try {
                attr = File.GetAttributes( FileOrWorkingDirectory );
            } catch ( IOException e ) {
                Console.WriteLine( string.Format( "Error: {0}", e.Message ) );
                return 1;
            }

            if ( attr.HasFlag( FileAttributes.Directory ) )
                file_names.AddRange( Directory.GetFiles( FileOrWorkingDirectory, string.Format( "*.{0}", FilesExtension ) ) );
            else
                file_names.Add( FileOrWorkingDirectory );

            file_names.ForEach( Deployment.RunForFile );

            return 0;
        }
    }
}
