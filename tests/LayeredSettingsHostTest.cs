namespace tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;
    using LayeredSettings;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    public class LayeredSettingsHostTest
    {
        [Fact]
        public void CreateHostBuilder_DoesntThrowOnNullEnvironments()
        {
            var builder = LayeredSettingsHost.CreateHostBuilder(null, null);
            builder.Build();
        }

        [Fact]
        public void CreateHostBuilder_ObserveEnvironmentOverrideFromAppSettingsJson()
        {
            var reloadFlagConfig = new Dictionary<string, string>() {{ "hostbuilder:reloadConfigOnChange", "false" }};
            var guid = Guid.NewGuid().ToString();
            var devEnvName = $"Development{guid}";
            var rootFileName = "appsettings.json";
            var devFileName = $"appsettings.{devEnvName}.json";
            var tempPath = Path.GetTempPath();
            var appSettingsPath = Path.Combine(tempPath, rootFileName);
            var appSettingsDEVPath = Path.Combine(tempPath, devFileName);

            SaveConfig(appSettingsPath, rootFileName);
            var builder = LayeredSettingsHost.CreateHostBuilder(null, null).UseContentRoot(tempPath);
            var config = builder.Build().Services.GetRequiredService<IConfiguration>();
            Assert.Equal(rootFileName, config["FileName"]);

            SaveConfig(appSettingsDEVPath, devEnvName);
            builder = LayeredSettingsHost.CreateHostBuilder(null, new[] {new LayeredSettings.Environment(devEnvName)}).UseContentRoot(tempPath).UseEnvironment(devEnvName);
            config = builder.Build().Services.GetRequiredService<IConfiguration>();
            Assert.Equal(devEnvName, config["FileName"]);
        }

        [Fact]
        public void CreateHostBuilder_ObserveEnvironmentLayering()
        {
            var reloadFlagConfig = new Dictionary<string, string>() {{ "hostbuilder:reloadConfigOnChange", "false" }};
            var guid = Guid.NewGuid().ToString();
            var ppeEnvName = $"PPE{guid}";
            var devEnvName = $"Development{guid}";
            var ppeFileName = $"appsettings.{ppeEnvName}.json";
            var devFileName = $"appsettings.{devEnvName}.json";
            var tempPath = Path.GetTempPath();
            var appSettingsPPEPath = Path.Combine(tempPath, ppeFileName);
            var appSettingsDEVPath = Path.Combine(tempPath, devFileName);

            var ppeEnv = new LayeredSettings.Environment(ppeEnvName);
            var devEnv = new LayeredSettings.Environment(devEnvName, ppeEnv);

            SaveConfig(appSettingsPPEPath, ppeEnvName);
            var builder = LayeredSettingsHost.CreateHostBuilder(null, new[] {ppeEnv}).UseContentRoot(tempPath).UseEnvironment(ppeEnvName);
            var config = builder.Build().Services.GetRequiredService<IConfiguration>();
            Assert.Equal(ppeEnvName, config["FileName"]);

            SaveConfig(appSettingsDEVPath, devEnvName);
            builder = LayeredSettingsHost.CreateHostBuilder(null, new[] {devEnv}).UseContentRoot(tempPath).UseEnvironment(devEnvName);
            config = builder.Build().Services.GetRequiredService<IConfiguration>();
            Assert.Equal(devEnvName, config["FileName"]);
        }

        private void SaveConfig(string path, string value)
        {
            File.WriteAllText(path, $"{{ \"FileName\": \"{value}\" }}");
        }
    }
}
