namespace BuildDeploy
{
    public interface IDeployable
    {
        string HostName { get; set; }
        int TargetPort { get; set; }

        /// <summary>
        /// Run the action on the List.ForEach 
        /// </summary>
        /// <param name="filePath">The current file (usually a FQP)</param>
        void RunForFile( string filePath );
    }
}
