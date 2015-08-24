using System.Configuration;

namespace Graphite.Configuration
{
    /// <summary>
    /// StatsD configuration parsed from a connection string with name "statsd".
    /// </summary>
    public class StatsDConnectionStringConfiguration : ConnectionStringConfigurationBase, IStatsDConfiguration
    {
        /// <summary>
        /// Creates a new instance of <see cref="StatsDConnectionStringConfiguration"/> and parses data from the configuration file.
        /// </summary>
        public StatsDConnectionStringConfiguration()
        {
            var c = ConfigurationManager.ConnectionStrings["statsd"];

            if (c != null)
            {
                this.Parse(c.ConnectionString);
            }
        }
    }
}
