using System.Xml;
using Elmah.Assertions;

namespace Graphite
{
    public sealed class AssertionFactory
    {
        public static IAssertion assert_log(XmlElement config)
        {
            return new LogAssertion();
        }
    }
}
