using ManyConsole;
using System;
using System.Collections.Generic;
using System.IO;

namespace BuildDeploy
{
    public class DeployCommand : ConsoleCommand
    {
        private IDeployable Deployment;
        private Uri DeployFullURI;
        private string FileOrWorkingDirectory, FilesExtension;

        public DeployCommand( )
        {
            IsCommand( "deploy", "Deploy applications from directory" );

            HasRequiredOption( "remote=", "The remote upload location URI (only ftp:// and sftp:// are supported as of now)", remote =>
            {
                try {
                    DeployFullURI = new Uri( remote );
                    if ( !DeployFullURI.AbsoluteUri.EndsWith( "/" ) )
                        DeployFullURI = new Uri( string.Format( "{0}/", DeployFullURI.AbsoluteUri ) );
                } catch ( UriFormatException e ) {
                    Console.WriteLine( "Error parsing the remote url - {0}", e.Message );
                    Environment.Exit( 1 );
                }
                
                switch ( DeployFullURI.Scheme.ToLower() )
                {
                    case "ftp":
                        Deployment = new DeployFTP();
                        break;
                    case "sftp":
                        Deployment = new DeploySFTP();
                        break;
                    default:
                        Deployment = null;
                        break;
                }
            } );

            HasRequiredOption( "dir|directory=", "Deploy from specified directory", dir => FileOrWorkingDirectory = dir );
            HasOption( "ext|extension=", "Targeted files extension in specified directory", ext => FilesExtension = ext );

            HasOption( "user|username=", "Login username", username => Deployment.LoginInfo.UserName = username );
            HasOption( "pass|password=", "Login password", pass => Deployment.LoginInfo.Password = pass );
            HasOption( "privatekey=", "Private key for authentication (used only for SFTP for now)", privatekey_path => 
            {
                if ( Deployment.GetType() == typeof( DeploySFTP ) )
                    ( (DeploySFTP) Deployment ).PrivateKeyPath = privatekey_path;
            } );

            HasOption( "privatekeypass=", "Private key passphrase (if there is one)", privatekey_pass => 
            {
                if ( Deployment.GetType() == typeof( DeploySFTP ) )
                    ( (DeploySFTP) Deployment ).PrivateKeyPassphrase = privatekey_pass;
            } );
        }

        public override int Run( string[] remainingArguments )
        {
            FileAttributes attr = 0;
            List<string> file_names = new List<string>();

            if ( Deployment == null)
            {
                Console.WriteLine( "Deployment scheme \"{0}\" unsupported! Current working schemes are FTP and SFTP", DeployFullURI.Scheme.ToUpper() );
                return 1;
            }

            if ( string.IsNullOrEmpty( FilesExtension ) )
                FilesExtension = Program.DefaultExtensionSearch;

            Deployment.RequestURI = DeployFullURI;

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

            if ( Deployment.PreRun( file_names.ToArray() ) )
            {
                file_names.ForEach( Deployment.RunForFile );
                Deployment.PostRun();
            }

            return 0;
        }
    }
}
