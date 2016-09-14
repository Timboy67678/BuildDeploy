using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildDeploy
{
    class DeploySFTP : IDeployable
    {
        public string HostName { get; set; }
        public int TargetPort { get; set; }

        public void RunForFile( string filePath )
        {
            //TODO:
        }
    }
}
