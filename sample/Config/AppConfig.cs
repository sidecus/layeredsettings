namespace sample
{
    /// <summary>
    /// Sample AppConfig to bind to settings
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// Gets or sets the string option value 
        /// </summary>
        public string StringOption { get; set; }

        /// <summary>
        /// Gets or sets a string option which shows environment inheritance from appsettings.json
        /// </summary>
        public string FromRoot { get; set; }

        /// <summary>
        /// Gets or sets a string option which shows environment inheritance from appsettings.Production.json
        /// </summary>
        public string FromProduction { get; set; }

        /// <summary>
        /// Gets or sets a string option which shows environment inheritance from appsettings.PPE.json
        /// </summary>
        public string FromPPE { get; set; }

        /// <summary>
        /// Gets or sets a string option which shows environment inheritance from appsettings.Development.json
        /// </summary>
        public string FromDevelopment { get; set; }
    }
}