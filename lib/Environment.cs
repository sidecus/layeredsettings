namespace LayeredSettings
{
    using System;

    /// <summary>
    /// Environment class which supports Environment inheritance
    /// </summary>
    public class Environment
    {
        /// <summary>
        /// Gets the environment name which is set in IHostEnvironment
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the parent environment which we want to inherit settings from
        /// </summary>
        public Environment Parent { get; } = null;

        /// <summary>
        /// Initializes a new Environment object
        /// </summary>
        /// <param name="name">environment name which is set in IHostEnvironment</param>
        /// <param name="parent">Parent environment, can be null and defaults to null</param>
        public Environment(string name, Environment parent = null)
        {
            this.Name = name ?? throw new ArgumentNullException(nameof(name));
            this.Parent = parent;
        }
    }
}