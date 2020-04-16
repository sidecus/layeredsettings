namespace sample
{
    /// <summary>
    /// Sample AppConfig to bind to settings
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Gets or sets a common option set in appsettings.json
        /// </summary>
        public string CommonOption { get; set; }

        /// <summary>
        /// Gets or sets a string option which is only set in appsettings.Production.json
        /// </summary>
        public string ProductionOnlyOption { get; set; }

        /// <summary>
        /// Gets or sets a string option which is only set in appsettings.PPE.json
        /// </summary>
        public string PPEOnlyOption { get; set; }

        /// <summary>
        /// Gets or sets a string option which is only set in appsettings.Development.json
        /// </summary>
        public string DevelopmentOnlyOption { get; set; }

        /// <summary>
        /// Gets or sets a string option which is set in appsettings.PPE.json and gets overriden in appsettings.Development.json
        /// </summary>
        public string DevOverridePPEOption { get; set; }
    }
}