using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;

namespace Graphite.Configuration
{
    /// <summary>
    /// Graphite configuration parsed from a connection string with name "graphite".
    /// </summary>
    public class GraphiteConnectionStringConfiguration : ConnectionStringConfigurationBase, IGraphiteConfiguration
    {
        /// <summary>
        /// Creates a new instance of <see cref="GraphiteConnectionStringConfiguration"/> and parses data from the configuration file.
        /// </summary>
        public GraphiteConnectionStringConfiguration()
        {
            var c = ConfigurationManager.ConnectionStrings["graphite"];

            if (c != null)
            {
                this.Parse(c.ConnectionString);
            }
        }

        /// <summary>
        /// Gets the transport protocol.
        /// </summary>        
        public TransportType Transport { get; private set; }

        /// <summary>
        /// Parses connection string to properties.
        /// </summary>
        /// <param name="connectionString"></param>
        protected override IDictionary<string, string> Parse(string connectionString)
        {
            var values = base.Parse(connectionString);

            if (values != null)
            {
                string value;
                if (values.TryGetValue("transport", out value))
                {
                    TransportType temp;
                    if (Enum.TryParse(value, out temp))
                    {
                        this.Transport = temp;
                    }
                }

                if (this.Transport == 0)
                {
                    this.Transport = TransportType.Tcp;
                }
            }

            return values;
        }
    }
}
