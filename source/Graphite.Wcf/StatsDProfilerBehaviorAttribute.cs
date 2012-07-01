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

        public StatsDProfilerBehaviorAttribute(bool reportRequestTime, string fixedRequestTimeKey = null, string requestTimePrefix = null)
        {
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
                        new StatsDProfilerMessageInspector(this.reportRequestTime, this.fixedRequestTimeKey, this.requestTimePrefix));
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}