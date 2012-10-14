using System;
using System.ServiceModel;
using Graphite.Wcf;

namespace WcfTestService
{
    [ServiceBehavior]
    [MetricsPipeBehaviorAttribute(true, true, fixedRequestTimeKey: null, requestTimePrefix: "service.example", fixedHitCountKey: null, hitCountPrefix: "service.example")]
    public class ExampleGraphiteService
    {
        public void Test()
        {
        }
    }
}
