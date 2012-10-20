using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Graphite.Wcf
{
    [Obsolete("Use Graphite.Wcf.MetricsPipeBehaviorAttribute", false)]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class StatsDProfilerBehaviorAttribute : Attribute, IServiceBehavior
    {
        private readonly MetricsPipeBehaviorAttribute inner;


        public StatsDProfilerBehaviorAttribute(bool reportRequestTime, bool reportHitCount, string fixedRequestTimeKey = null, string requestTimePrefix = null, string fixedHitCountKey = null, string hitCountPrefix = null)
        {
            this.inner = new MetricsPipeBehaviorAttribute(reportRequestTime, reportHitCount, fixedRequestTimeKey, requestTimePrefix, fixedHitCountKey, hitCountPrefix);
        }

        public void AddBindingParameters(
            ServiceDescription serviceDescription, 
            ServiceHostBase serviceHostBase, 
            Collection<ServiceEndpoint> endpoints, 
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            this.inner.ApplyDispatchBehavior(serviceDescription, serviceHostBase);
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}