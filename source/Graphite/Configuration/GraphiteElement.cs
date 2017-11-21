using System;
using System.Configuration;
using System.Net;

namespace Graphite.Configuration
{
    /// <summary>
    /// Configuration for graphite backend.
    /// </summary>
    public class GraphiteElement : ConfigurationElement, IGraphiteConfiguration
    {
        /// <summary>
        /// The XML name of the <see cref="Address"/> property.
        /// </summary>        
        internal const string AddressPropertyName = "address";

        /// <summary>
        /// The XML name of the <see cref="Port"/> property.
        /// </summary>        
        internal const string PortPropertyName = "port";

        /// <summary>
        /// The XML name of the <see cref="Transport"/> property.
        /// </summary>        
        internal const string TransportPropertyName = "transport";

        /// <summary>
        /// The XML name of the <see cref="PrefixKey"/> property.
        /// </summary>    
        internal const string PrefixKeyPropertyName = "prefixKey";

        /// <summary>
        /// The XML name of the <see cref="Lifetime"/> property
        /// </summary>
        internal const string LifetimePropertyName = "lifetime";

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        [ConfigurationPropertyAttribute(PortPropertyName, IsRequired = true)]
        public int Port
        {
            get { return (int)base[PortPropertyName]; }
            set { base[PortPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        [ConfigurationPropertyAttribute(AddressPropertyName, IsRequired = true)]
        public string Address
        {
            get { return (string)base[AddressPropertyName]; }
            set { base[AddressPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the port number.
        /// </summary>        
        [ConfigurationProperty(TransportPropertyName, IsRequired = true, DefaultValue = TransportType.Tcp)]
        public TransportType Transport
        {
            get { return (TransportType)base[TransportPropertyName]; }
            set { base[TransportPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the prefix key.
        /// </summary>        
        [ConfigurationProperty(PrefixKeyPropertyName, IsRequired = false)]
        public string PrefixKey
        {
            get { return (string)base[PrefixKeyPropertyName]; }
            set { base[PrefixKeyPropertyName] = value; }
        }

        /// <summary>
        /// When using UDP protocol, the time before renewing the socket
        /// </summary>        
        [ConfigurationProperty(LifetimePropertyName, IsRequired = false)]
        public TimeSpan Lifetime
        {
            get { return (TimeSpan)base[LifetimePropertyName]; }
            set { base[LifetimePropertyName] = value; }
        }
    }
}
