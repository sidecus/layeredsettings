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
    public static class LayeredSettingsHost
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
        public static IHostBuilder CreateHostBuilder(string[] args, IEnumerable<HostEnvironment> environments = null)
        {
            LayeredSettingsHost.ValidateEnvironments(environments);
            
            var builder = Host.CreateDefaultBuilder();
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                
                // Add json file settings based on the environments
                LayeredSettingsHost.AddJsonFiles(hostingContext, config, environments);

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
        private static void ValidateEnvironments(IEnumerable<HostEnvironment> environments)
        {
            if (environments == null || environments.Any())
            {
                return;
            }

            var envDict = new Dictionary<string, HostEnvironment>(StringComparer.OrdinalIgnoreCase);
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
        /// Add json file settings based on current environment and the specified
        /// environment inheritance layers.
        /// If environments param is null, only appsettings.json will be added.
        /// </summary>
        /// <param name="hostingContext">hosting context</param>
        /// <param name="config">config builder</param>
        /// <param name="environments">array of defined environments</param>
        private static void AddJsonFiles(
            HostBuilderContext hostingContext,
            IConfigurationBuilder config,
            IEnumerable<HostEnvironment> environments)
        {
            var reloadOnChange = hostingContext.Configuration.GetValue("hostBuilder:reloadConfigOnChange", defaultValue: true);
            var currentEnvName = hostingContext.HostingEnvironment.EnvironmentName ?? Environments.Production;

            // Find the layering hierarchy (from children to parent), then reverse it.
            // If the current environment cannot be found from the environments list, we fall back to
            // the .net core behavior by creating a temp environment using current environment's name.
            var layers = new List<HostEnvironment>();
            var env = environments?.FirstOrDefault(e => string.Equals(e.Name, currentEnvName, StringComparison.OrdinalIgnoreCase));
            env ??= new HostEnvironment(currentEnvName);
            while (env != null)
            {
                layers.Add(env);
                env = env.Parent;
            }
            
            layers.Reverse();

            // Add json files based on parent to child layering. Note appsettings.json is always included
            config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: reloadOnChange);
            foreach (var environment in layers)
            {
                config.AddJsonFile($"appsettings.{environment.Name}.json", optional: true, reloadOnChange: reloadOnChange);
            }
        }
    }
}
