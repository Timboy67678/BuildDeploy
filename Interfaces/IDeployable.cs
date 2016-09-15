using System;
using System.Net;

namespace BuildDeploy
{
    public interface IDeployable
    {
        Uri RequestURI { get; set; }
        NetworkCredential LoginInfo { get; set; }

        /// <summary>
        /// Run the action on the List.ForEach 
        /// </summary>
        /// <param name="filePath">The current file (usually a FQP)</param>
        void RunForFile( string filePath );

        /// <summary>
        /// Called before we process deployable files
        /// </summary>
        /// <param name="files">the array of files to process</param>
        void PreRun( string[] files );

        /// <summary>
        /// Called after we deploy the files
        /// </summary>
        void PostRun( );
    }
}
