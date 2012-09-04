using System.Xml;
using Elmah.Assertions;

namespace Graphite
{
    public sealed class AssertionFactory
    {
        public static IAssertion statsd_log(XmlElement config)
        {
            return new StatsDLogAssertion();
        }
    }
}
