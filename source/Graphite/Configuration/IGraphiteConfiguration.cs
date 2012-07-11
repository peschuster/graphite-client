using System.Net;

namespace Graphite.Configuration
{
    public interface IGraphiteConfiguration
    {
        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        int Port { get; }

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        string Address { get; }

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        TransportType Transport { get; }

        /// <summary>
        /// Gets or sets the prefix key.
        /// </summary>        
        string PrefixKey { get; }
    }
}