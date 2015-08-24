namespace Graphite.Configuration
{
    /// <summary>
    /// Provider for graphite and statsD configuration.
    /// </summary>
    public static class GraphiteConfigurationProvider
    {
        /// <summary>
        /// Returns the available configuration.
        /// </summary>
        /// <returns></returns>
        public static IConfigurationContainer Get()
        {
            IConfigurationContainer config;

            config = GraphiteConfiguration.Instance;

            if (config != null)
                return config;

            return new ConnectionStringContainer();
        }
    }
}
