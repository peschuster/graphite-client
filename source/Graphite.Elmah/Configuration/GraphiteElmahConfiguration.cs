using System.Configuration;

namespace Graphite.Configuration
{
    public class GraphiteElmahConfiguration : ConfigurationSection
    {
        /// <summary>
        /// The XML name of the GraphiteElmahConfigurationSectionName Configuration Section.
        /// </summary>        
        internal const string GraphiteElmahConfigurationSectionName = "graphite.elmah";

        /// <summary>
        /// The XML name of the <see cref="Xmlns"/> property.
        /// </summary>        
        internal const string XmlnsPropertyName = "xmlns";

        /// <summary>
        /// The XML name of the <see cref="Key"/> property.
        /// </summary>
        internal const string KeyPropertyName = "key";

        /// <summary>
        /// The XML name of the <see cref="Type"/> property.
        /// </summary>
        internal const string TypePropertyName = "type";

        /// <summary>
        /// The XML name of the <see cref="Target"/> property.
        /// </summary>
        internal const string TargetPropertyName = "target";

        /// <summary>
        /// Gets the W3CReadersConfiguration instance.
        /// </summary>        
        public static GraphiteElmahConfiguration Instance
        {
            get { return (GraphiteElmahConfiguration)ConfigurationManager.GetSection(GraphiteElmahConfigurationSectionName); }
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
        /// Gets or sets the Key.
        /// </summary>
        [ConfigurationPropertyAttribute(KeyPropertyName, IsRequired = true)]
        public string Key
        {
            get { return (string)base[KeyPropertyName]; }
            set { base[KeyPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Type.
        /// </summary>
        [ConfigurationPropertyAttribute(TypePropertyName, IsRequired = true)]
        public string Type
        {
            get { return (string)base[TypePropertyName]; }
            set { base[TypePropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Target.
        /// </summary>
        [ConfigurationPropertyAttribute(TargetPropertyName, IsRequired = true)]
        public string Target
        {
            get { return (string)base[TargetPropertyName]; }
            set { base[TargetPropertyName] = value; }
        }
    }
}
