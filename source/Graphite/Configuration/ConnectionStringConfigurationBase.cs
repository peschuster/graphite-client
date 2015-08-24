using System.Collections.Generic;
using System.Linq;

namespace Graphite.Configuration
{
    /// <summary>
    /// Base class for configurations from connection strings.
    /// </summary>
    public abstract class ConnectionStringConfigurationBase
    {
        /// <summary>
        /// Gets the host address.
        /// </summary>        
        public string Address { get; private set; }

        /// <summary>
        /// Gets the port number.
        /// </summary>        
        public int Port { get; private set; }

        /// <summary>
        /// Gets the common prefix key.
        /// </summary>        
        public string PrefixKey { get; private set; }

        /// <summary>
        /// Parses connection string to properties.
        /// </summary>
        /// <param name="connectionString"></param>
        protected virtual IDictionary<string, string> Parse(string connectionString)
        {
            IDictionary<string, string> values = null;

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                values = connectionString
                    .Split(';')
                    .Select(x => x.Split(new[] { '=' }, 2))
                    .ToDictionary(
                        x => x.First().ToLowerInvariant(), 
                        x => x.Skip(1).FirstOrDefault());

                string value;
                if (values.TryGetValue("address", out value))
                {
                    this.Address = value;
                }

                if (values.TryGetValue("port", out value))
                {
                    int temp;
                    if (int.TryParse(value, out temp))
                    {
                        this.Port = temp;
                    }
                }

                if (values.TryGetValue("prefixkey", out value))
                {
                    this.PrefixKey = value;
                }
            }

            return values;
        }
    }
}
