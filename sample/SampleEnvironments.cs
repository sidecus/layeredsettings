namespace sample
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Hosting;
    using LayeredSettings;
    
    /// <summary>
    /// Environments definition for our app
    /// </summary>
    public static class SampleEnvironments
    {
        /// <summary>
        /// Environment list
        /// </summary>
        public static List<HostEnvironment> HostEnvironmentList;

        /// <summary>
        /// Initializes the app environments
        /// </summary>
        static SampleEnvironments()
        {
            var prodEnv = new HostEnvironment(Environments.Production);
            var ppeEnv = new HostEnvironment("PPE");
            var devEnv = new HostEnvironment(Environments.Development, parent: ppeEnv);

            HostEnvironmentList = new List<HostEnvironment>() { prodEnv, devEnv, ppeEnv };
        }
    }
}
