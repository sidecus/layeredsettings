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
        /// Environment list
        /// </summary>
        public static List<Environment> EnvironmentList;

        /// <summary>
        /// Initializes the app environments
        /// </summary>
        static AppEnvironments()
        {
            var prodEnv = new Environment(Environments.Production);
            var ppeEnv = new Environment("PPE");
            var devEnv = new Environment(Environments.Development, parent: ppeEnv);

            EnvironmentList = new List<Environment>() { prodEnv, devEnv, ppeEnv };
        }
    }
}
