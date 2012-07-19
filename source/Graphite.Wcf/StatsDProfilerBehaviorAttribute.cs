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
        private readonly MetricSetting reportRequestTime;

        private readonly MetricSetting reportHitCount;

        public StatsDProfilerBehaviorAttribute(MetricSetting reportRequestTime = null, MetricSetting reportHitCount = null)
        {
            this.reportRequestTime = reportRequestTime;
            this.reportHitCount = reportHitCount;
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
                            this.reportHitCount));
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }
    }
}