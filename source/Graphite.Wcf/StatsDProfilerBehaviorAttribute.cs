using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Graphite.Wcf
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class StatsDProfilerBehaviorAttribute : Attribute, IServiceBehavior
    {
        private readonly bool reportRequestTime;

        private readonly string fixedRequestTimeKey;

        private readonly string requestTimePrefix;

        private readonly bool reportHitCount;

        private readonly string hitCountPrefix;

        private readonly string fixedHitCountKey;

        public StatsDProfilerBehaviorAttribute(bool reportRequestTime, bool reportHitCount, string fixedRequestTimeKey = null, string requestTimePrefix = null, string fixedHitCountKey = null, string hitCountPrefix = null)
        {
            this.fixedHitCountKey = fixedHitCountKey;
            this.hitCountPrefix = hitCountPrefix;
            this.reportHitCount = reportHitCount;
            this.reportRequestTime = reportRequestTime;
            this.fixedRequestTimeKey = fixedRequestTimeKey;
            this.requestTimePrefix = requestTimePrefix;
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
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (EndpointDispatcher endpoint in dispatcher.Endpoints)
                {
                    endpoint.DispatchRuntime.MessageInspectors.Add(
                        new StatsDProfilerMessageInspector(
                            this.reportRequestTime, 
                            this.reportHitCount, 
                            this.fixedRequestTimeKey, 
                            this.requestTimePrefix,
                            this.fixedHitCountKey,
                            this.hitCountPrefix));
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}