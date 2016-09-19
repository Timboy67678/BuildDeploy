using System;
using System.IO;
using System.Net;
using Tamir.SharpSsh;
using Tamir.SharpSsh.jsch;

namespace BuildDeploy
{
    class DeploySFTP : IDeployable
    {
        public Uri RequestURI { get; set; }
        public NetworkCredential LoginInfo { get; set; }
        public string PrivateKeyPath, PrivateKeyPassphrase;
        private Sftp SftpRequest;

        public DeploySFTP()
        {
            PrivateKeyPath = PrivateKeyPassphrase = string.Empty;
            LoginInfo = new NetworkCredential( "anonymous", string.Empty );
        }

        public void RunForFile( string filePath )
        {
            FileInfo fileInfo = new FileInfo( filePath );
            
            try {
                SftpRequest.Put( filePath, RequestURI.AbsolutePath );
            } catch( SftpException e ) {
                Console.WriteLine( "Failed to upload \"{0}\" - {1} ({2})", fileInfo.Name, e.message, e.id );
                return;
            }

            Console.WriteLine( "Uploaded \"{0}\" succesfully.", fileInfo.Name );
        }

        public bool PreRun( string[] files )
        {
            try
            {
                SftpRequest = new Sftp( RequestURI.Host, LoginInfo.UserName, LoginInfo.Password );
                if ( !string.IsNullOrEmpty( PrivateKeyPath ) )
                    SftpRequest.AddIdentityFile( PrivateKeyPath, PrivateKeyPassphrase );
                SftpRequest.Connect( RequestURI.Port == -1 ? 22 : RequestURI.Port );
            } catch( Exception e ) {
                Console.WriteLine( "Failed to conect to remote sftp server - {0}", e.Message );
                return false;
            }

            return true;
        }

        public void PostRun( )
        {
            if ( SftpRequest != null )
                SftpRequest.Close();
        }
    }
}
