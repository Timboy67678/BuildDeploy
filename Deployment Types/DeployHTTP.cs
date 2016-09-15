using System.Net;

namespace BuildDeploy
{
    class DeployHTTP : IDeployable
    {
        public string RequestURI { get; set; }
        public NetworkCredential LoginInfo { get; set; }

        public void RunForFile( string filePath )
        {
            //TODO:
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
