using System.Xml;
using Elmah.Assertions;

namespace Graphite
{
    /// <summary>
    /// Factory for creating <see cref="LogAssertion"/> objects.
    /// </summary>
    public sealed class AssertionFactory
    {
        /// <summary>
        /// Creates a new <see cref="LogAssertion"/>.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IAssertion assert_log(XmlElement config)
        {
            return new LogAssertion();
        }
    }
}