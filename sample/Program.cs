namespace sample
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using LayeredSettings;
    
    /// <summary>
    /// Main program class
    /// </summary>
    public class Program
    {
        /// <summary>
        /// main entry point
        /// </summary>
        /// <param name="args">command line arguments</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create host builder using the LayeredSettingsHost
        /// </summary>
        /// <param name="args">command line arguments</param>
        /// <returns>host builder</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            LayeredSettingsHost
                .CreateHostBuilder(args, SampleEnvironments.HostEnvironmentList)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
