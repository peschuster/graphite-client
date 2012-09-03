using System.Configuration;

namespace Graphite.System.Configuration
{
    [ConfigurationCollectionAttribute(typeof(EventlogListenerElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class EventlogListenerElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// Gets the type of the <see cref="ConfigurationElementCollection"/>.
        /// </summary>
        /// <returns>The <see cref="ConfigurationElementCollectionType"/> of this collection.</returns>        
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        /// <summary>
        /// Gets the element key for the specified configuration element.
        /// </summary>
        /// <param name="element">The <see cref="ConfigurationElement"/> to return the key for.</param>
        /// <returns>
        /// An <see cref="object"/> that acts as the key for the specified <see cref="ConfigurationElement"/>.
        /// </returns>        
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EventlogListenerElement)element).Key;
        }

        /// <summary>
        /// Creates a new <see cref="EventlogListenerElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="EventlogListenerElement"/>.
        /// </returns>        
        protected override ConfigurationElement CreateNewElement()
        {
            return new EventlogListenerElement();
        }

        /// <summary>
        /// Gets the <see cref="EventlogListenerElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="EventlogListenerElement"/> to retrieve.</param>        
        public EventlogListenerElement this[int index]
        {
            get { return (EventlogListenerElement)this.BaseGet(index); }
        }

        /// <summary>
        /// Gets the <see cref="EventlogListenerElement"/> with the specified key.
        /// </summary>
        /// <param name="name">The key of the <see cref="EventlogListenerElement"/> to retrieve.</param>        
        public EventlogListenerElement this[object name]
        {
            get { return (EventlogListenerElement)this.BaseGet(name); }
        }

        /// <summary>
        /// Gets the <see cref="EventlogListenerElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="EventlogListenerElement"/> to retrieve.</param>        
        public EventlogListenerElement GetItemAt(int index)
        {
            return (EventlogListenerElement)base.BaseGet(index);
        }

        /// <summary>
        /// Gets the <see cref="EventlogListenerElement"/> with the specified key.
        /// </summary>
        /// <param name="name">The key of the <see cref="EventlogListenerElement"/> to retrieve.</param>        
        public EventlogListenerElement GetItemByKey(string name)
        {
            return (EventlogListenerElement)base.BaseGet((object)(name));
        }
    }
}