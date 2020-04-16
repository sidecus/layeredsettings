namespace LayeredSettings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Custom web host for webhost builder creation
    /// </summary>
    public static class ZyinHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IHostBuilder"/> class with pre-configured defaults.
        /// </summary>
        /// <remarks>
        /// Following is the same as Host.CreateDefaultBuilder in dotnet core generic host.
        /// https://github.com/dotnet/runtime/blob/96a3bfed4acffbe88403d5f599cee358b880ac5d/src/libraries/Microsoft.Extensions.Hosting/src/Host.cs
        /// We have to repeat the logic since the JsonFile config needs to be done before user secrets, environment variables and command line,
        /// and there is no way for us to just inject it at the begining without repeating the logic.
        /// </remarks>
        /// <param name="args">The command line args.</param>
        /// <param name="environments">The predefined environments. If it's null we'll default to the .net core default behavior</param>
        /// <returns>The initialized <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder CreateLayeredSettingsBuilder(string[] args, IEnumerable<Environment> environments = null)
        {
            ZyinHost.ValidateEnvironments(environments);
            
            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                
                // Add json file settings based on the environments
                ZyinHost.AddJsonFiles(hostingContext, config, environments);

                if (env.IsDevelopment() && !string.IsNullOrEmpty(env.ApplicationName))
                {
                    var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
                    if (appAssembly != null)
                    {
                        config.AddUserSecrets(appAssembly, optional: true);
                    }
                }

                config.AddEnvironmentVariables();

                if (args != null)
                {
                    config.AddCommandLine(args);
                }
            });

            return builder;
        }

        /// <summary>
        /// Validate the environments array to make sure environments are distinct
        /// </summary>
        /// <param name="environments">environment array</param>
        private static void ValidateEnvironments(IEnumerable<Environment> environments)
        {
            if (environments == null || environments.Any())
            {
                return;
            }

            var envDict = new Dictionary<string, Environment>(StringComparer.OrdinalIgnoreCase);
            foreach (var env in environments)
            {
                if (envDict.ContainsKey(env.Name))
                {
                    throw new InvalidOperationException("Environment name is not unique in the given environments array");
                }

                envDict.Add(env.Name, env);
            }
        }

        /// <summary>
        /// Add environment based json file settings
        /// </summary>
        /// <param name="hostingContext">hosting context</param>
        /// <param name="config">config builder</param>
        private static void AddJsonFiles(HostBuilderContext hostingContext, IConfigurationBuilder config, IEnumerable<Environment> environments)
        {
            var reloadOnChange = hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", defaultValue: true);
            var envName = hostingContext.HostingEnvironment.EnvironmentName ?? Environments.Production;

            // Find the inheritance hierarchy (from leaf node to root), then reverse it (root to leaf)
            var envHierarchy = new List<Environment>();
            while (envName != null && environments.Any(e => string.Equals(e.Name, envName, StringComparison.OrdinalIgnoreCase)))
            {
                var env = environments.First(e => string.Equals(e.Name, envName, StringComparison.OrdinalIgnoreCase));
                envHierarchy.Add(env);
                envName = env.Parent?.Name;
            }

            envHierarchy.Reverse();

            // Add json files based on the root to leaf hierarchy. Note appsettings.json is always included
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: reloadOnChange);
            foreach (var env in envHierarchy)
            {
                config.AddJsonFile($"appsettings.{env.Name}.json", optional: true, reloadOnChange: reloadOnChange);
            }
        }
    }
}
