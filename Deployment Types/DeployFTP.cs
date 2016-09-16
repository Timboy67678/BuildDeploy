using System;
using System.Net;

namespace BuildDeploy
{
    class DeployFTP : IDeployable
    {
        public Uri RequestURI { get; set; }
        public NetworkCredential LoginInfo { get; set; }

        public void RunForFile( string filePath )
        {
            using ( var ftp_client = new WebClient() )
            {
                ftp_client.Credentials = LoginInfo;
                ftp_client.BaseAddress = RequestURI.ToString();

                using ( var write = ftp_client.OpenWrite( RequestURI ) )
                {
                    //TODO
                }
            }
        }

        public void PreRun( string[] files )
        {
            //TODO:
        }

        public void PostRun( )
        {
            //TODO:
        }
    }
}
