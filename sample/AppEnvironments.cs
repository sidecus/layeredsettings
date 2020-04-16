namespace sample
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Hosting;
    using LayeredSettings;
    
    /// <summary>
    /// Environments definition for our app
    /// </summary>
    public static class AppEnvironments
    {
        /// <summary>
        /// Environments
        /// </summary>
        public static List<Environment> EnvironmentList = new List<Environment>();

        /// <summary>
        /// Initializes the app environments
        /// </summary>
        static AppEnvironments()
        {
            var prodEnv = new Environment(Environments.Production);
            var ppeEnv = new Environment("PPE");

            // Dev is inherited from PPE
            var devEnv = new Environment(Environments.Development, ppeEnv);

            EnvironmentList.Add(prodEnv);
            EnvironmentList.Add(devEnv);
            EnvironmentList.Add(ppeEnv);
        }
    }
}
