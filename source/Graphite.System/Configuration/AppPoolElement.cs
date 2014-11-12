using System.Configuration;

namespace Graphite.System.Configuration
{
    public class AppPoolElement : ConfigurationElement
    {
        /// <summary>
        /// The XML name of the <see cref="Key"/> property.
        /// </summary>
        internal const string KeyPropertyName = "key";

        /// <summary>
        /// The XML name of the <see cref="AppPoolName"/> property.
        /// </summary>
        internal const string AppPoolPropertyName = "appPoolName";

        /// <summary>
        /// The XML name of the <see cref="WorkingSet"/> property.
        /// </summary>        
        internal const string WorkingSetPropertyName = "workingSet";

        /// <summary>
        /// The XML name of the <see cref="Type"/> property.
        /// </summary>        
        internal const string TypePropertyName = "type";

        /// <summary>
        /// The XML name of the <see cref="Target"/> property.
        /// </summary>        
        internal const string TargetPropertyName = "target";

        /// <summary>
        /// The XML name of the <see cref="Interval"/> property.
        /// </summary>        
        internal const string IntervalPropertyName = "interval";

        /// <summary>
        /// The XML name of the <see cref="Category"/> property.
        /// </summary>
        internal const string CategoryPropertyName = "category";

        /// <summary>
        /// The XML name of the <see cref="Counter"/> property.
        /// </summary>        
        internal const string CounterPropertyName = "counter";

        /// <summary>
        /// Gets or sets the Key.
        /// </summary>
        [ConfigurationPropertyAttribute(KeyPropertyName, IsRequired = true, IsKey = true)]
        public string Key
        {
            get { return (string)base[KeyPropertyName]; }
            set { base[KeyPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the AppPoolName.
        /// </summary>
        [ConfigurationPropertyAttribute(AppPoolPropertyName, IsRequired = true)]
        public string AppPoolName
        {
            get { return (string)base[AppPoolPropertyName]; }
            set { base[AppPoolPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Sampling rate.
        /// </summary>
        [ConfigurationPropertyAttribute(WorkingSetPropertyName, IsRequired = false)]
        public bool WorkingSet
        {
            get { return (bool)base[WorkingSetPropertyName]; }
            set { base[WorkingSetPropertyName] = value; }
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

        /// <summary>
        /// Gets or sets the Interval (in seconds).
        /// </summary>
        [ConfigurationPropertyAttribute(IntervalPropertyName, DefaultValue = (short)30)]
        public short Interval
        {
            get { return (short)base[IntervalPropertyName]; }
            set { base[IntervalPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Category.
        /// </summary>
        [ConfigurationPropertyAttribute(CategoryPropertyName, IsRequired = false)]
        public string Category
        {
            get { return (string)base[CategoryPropertyName]; }
            set { base[CategoryPropertyName] = value; }
        }

        /// <summary>
        /// Gets or sets the Counter.
        /// </summary>
        [ConfigurationPropertyAttribute(CounterPropertyName, IsRequired = false)]
        public string Counter
        {
            get { return (string)base[CounterPropertyName]; }
            set { base[CounterPropertyName] = value; }
        }
    }
}
