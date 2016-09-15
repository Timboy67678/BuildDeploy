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

        private DeployMethod DeployWith;
        private IDeployable Deployment;
        private string DeployURIPath;
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
                         break;
                     case "sftp":
                         Deployment = new DeploySFTP();
                         DeployWith = DeployMethod.SFTP;
                         break;
                     case "http":
                         Deployment = new DeployHTTP();
                         DeployWith = DeployMethod.HTTP;
                         break;
                     default:
                         Deployment = null;
                         DeployWith = DeployMethod.NONE;
                         break;
                 }
             } );

            HasRequiredOption( "dir|directory=", "Deploy from directory", dir => FileOrWorkingDirectory = dir );
            HasRequiredOption( "remote=", "Target deploying remote location", remote => DeployURIPath = remote );
            HasOption( "ext|extension=", "Targeted files extension", ext => FilesExtension = ext );
        }

        public override int Run( string[] remainingArguments )
        {
            FileAttributes attr = 0;
            var file_names = new List<string>();

            if ( DeployWith == DeployMethod.NONE || Deployment == null)
            {
                Console.WriteLine( "Deployment mode unknown! Current working modes are FTP, SFTP and HTTP" );
                return 1;
            }

            if ( string.IsNullOrEmpty( FilesExtension ) )
                FilesExtension = Program.DefaultExtensionSearch;

            Deployment.RequestURI = DeployURIPath;

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

            if ( file_names.Count == 0 )
            {
                Console.WriteLine( "No files found in \"{0}\"", FileOrWorkingDirectory );
                return 1;
            }

            Deployment.PreRun( file_names.ToArray() );
            file_names.ForEach( Deployment.RunForFile );
            Deployment.PostRun();

            return 0;
        }
    }
}
