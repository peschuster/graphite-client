using System.Configuration;

namespace Graphite.Configuration
{
    public class GraphiteConfiguration : ConfigurationSection
    {
        /// <summary>
        /// The XML name of the GraphiteConfigurationSectionName Configuration Section.
        /// </summary>        
        internal const string GraphiteConfigurationSectionName = "graphite";

        /// <summary>
        /// The XML name of the <see cref="Xmlns"/> property.
        /// </summary>        
        internal const string XmlnsPropertyName = "xmlns";

        /// <summary>
        /// The XML name of the <see cref="Graphite"/> property.
        /// </summary>        
        internal const string GraphitePropertyName = "graphite";

        /// <summary>
        /// The XML name of the <see cref="StatsD"/> property.
        /// </summary>        
        internal const string StatsDPropertyName = "statsd";

        /// <summary>
        /// Gets the W3CReadersConfiguration instance.
        /// </summary>        
        public static GraphiteConfiguration Instance
        {
            get { return (GraphiteConfiguration)ConfigurationManager.GetSection(GraphiteConfigurationSectionName); }
        }

        /// <summary>
        /// Gets the XML namespace of this Configuration Section.
        /// </summary>
        /// <remarks>
        /// This property makes sure that if the configuration file contains the XML namespace,
        /// the parser doesn't throw an exception because it encounters the unknown "xmlns" attribute.
        /// </remarks>        
        [ConfigurationPropertyAttribute(XmlnsPropertyName)]
        public string Xmlns
        {
            get { return (string)this[XmlnsPropertyName]; }
        }

        /// <summary>
        /// Gets or sets the Graphite configuration.
        /// </summary>
        [ConfigurationPropertyAttribute(GraphitePropertyName, IsRequired = false)]
        public GraphiteElement Graphite
        {
            get { return (GraphiteElement)this[GraphitePropertyName]; }
            set { base[GraphitePropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the StatsD configuration.
        /// </summary>
        [ConfigurationPropertyAttribute(StatsDPropertyName, IsRequired = false)]
        public StatsDElement StatsD
        {
            get { return (StatsDElement)this[StatsDPropertyName]; }
            set { base[StatsDPropertyName] = value; }
        }
    }
}
