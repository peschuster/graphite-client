using System.Configuration;

namespace Graphite.System.Configuration
{
    [ConfigurationCollectionAttribute(typeof(ListenerElement), CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class ListenerElementCollection : ConfigurationElementCollection
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
            return ((ListenerElement)element).Key;
        }

        /// <summary>
        /// Creates a new <see cref="ListenerElement"/>.
        /// </summary>
        /// <returns>
        /// A new <see cref="ListenerElement"/>.
        /// </returns>        
        protected override ConfigurationElement CreateNewElement()
        {
            return new ListenerElement();
        }

        /// <summary>
        /// Gets the <see cref="ListenerElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="ListenerElement"/> to retrieve.</param>        
        public ListenerElement this[int index]
        {
            get { return (ListenerElement)this.BaseGet(index); }
        }

        /// <summary>
        /// Gets the <see cref="ListenerElement"/> with the specified key.
        /// </summary>
        /// <param name="name">The key of the <see cref="ListenerElement"/> to retrieve.</param>        
        public ListenerElement this[object name]
        {
            get { return (ListenerElement)this.BaseGet(name); }
        }

        /// <summary>
        /// Gets the <see cref="ListenerElement"/> at the specified index.
        /// </summary>
        /// <param name="index">The index of the <see cref="ListenerElement"/> to retrieve.</param>        
        public ListenerElement GetItemAt(int index)
        {
            return (ListenerElement)base.BaseGet(index);
        }

        /// <summary>
        /// Gets the <see cref="ListenerElement"/> with the specified key.
        /// </summary>
        /// <param name="name">The key of the <see cref="ListenerElement"/> to retrieve.</param>        
        public ListenerElement GetItemByKey(string name)
        {
            return (ListenerElement)base.BaseGet((object)(name));
        }
    }
}