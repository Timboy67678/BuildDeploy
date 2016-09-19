using System;
using System.Net;

namespace BuildDeploy
{
    class DeployHTTP : IDeployable
    {
        public Uri RequestURI { get; set; }
        public NetworkCredential LoginInfo { get; set; }

        public void RunForFile( string filePath )
        {
            //TODO:
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
