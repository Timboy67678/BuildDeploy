using System;
using System.IO;
using System.Net;

namespace BuildDeploy
{
    class DeployFTP : IDeployable
    {
        public Uri RequestURI { get; set; }
        public NetworkCredential LoginInfo { get; set; }

        public DeployFTP()
        {
            LoginInfo = new NetworkCredential( "anonymous", "" );
        }

        public void RunForFile( string filePath )
        {
            FileInfo fileInfo = new FileInfo( filePath );
            
            FtpWebRequest ftpRequest = (FtpWebRequest) WebRequest.Create( RequestURI.AbsoluteUri + fileInfo.Name );
            ftpRequest.Credentials = LoginInfo;
            ftpRequest.UseBinary = true;
            ftpRequest.ContentLength = fileInfo.Length;
            ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;

            byte[] fileBuffer = new byte[ fileInfo.Length ];

            try
            {
                using ( FileStream fileStream = fileInfo.OpenRead() )
                using ( Stream ftpStream = ftpRequest.GetRequestStream() )
                {
                    int numRead = fileStream.Read( fileBuffer, 0, fileBuffer.Length );
                    ftpStream.Write( fileBuffer, 0, numRead );
                }
            } catch( Exception e ) {
                Console.WriteLine( "Failed to upload \"{0}\" - {1}", fileInfo.Name, e.Message );
                return;
            }

            FtpWebResponse response = (FtpWebResponse) ftpRequest.GetResponse();

            if ( response.StatusCode != FtpStatusCode.ClosingData )
                Console.WriteLine( "Failed to upload \"{0}\" - {1}", fileInfo.Name, response.StatusDescription );
            else
                Console.WriteLine( "Uploaded \"{0}\" succesfully.", fileInfo.Name );
        }

        public bool PreRun( string[] files )
        {
            return true;
        }

        public void PostRun( )
        {
            //TODO:
        }
    }
}
