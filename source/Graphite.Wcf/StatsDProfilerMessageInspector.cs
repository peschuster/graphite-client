using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Graphite.Wcf
{
    public class StatsDProfilerMessageInspector : IDispatchMessageInspector
    {
        private readonly MetricSetting reportRequestTime;

        private readonly MetricSetting reportHitCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="StatsDProfilerMessageInspector"/> class.
        /// </summary>
        public StatsDProfilerMessageInspector(MetricSetting reportRequestTime = null, MetricSetting reportHitCount = null) 
            : base()
        {
            this.reportRequestTime = reportRequestTime;
            this.reportHitCount = reportHitCount;
        }

        /// <summary>
        /// Adds an instance of the <see cref="StatsDProfilerInstance"/> class to the current operation context after an inbound message has been received but before the message is dispatched to the intended operation.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="channel">The incoming channel.</param>
        /// <param name="instanceContext">The current service instance.</param>
        /// <returns>The object used to correlate state. This object is passed back in the BeforeSendReply method.</returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            WcfStatsDProfilerProvider.Instance.Start();

            return null;
        }

        /// <summary>
        /// Removes the registered instance of the <see cref="StatsDProfilerInstance"/> class from the current operation context after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is null if the operation is one way.</param>
        /// <param name="correlationState">The correlation object returned from the AfterReceiveRequest method.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            StatsDProfiler profiler = WcfStatsDProfilerProvider.Instance.Stop();

            try
            {
                if (profiler != null)
                {
                    if (this.reportRequestTime != null && this.reportRequestTime.Enable)
                    {
                        if (!string.IsNullOrEmpty(this.reportRequestTime.FixedKey))
                        {
                            profiler.ReportTiming(
                                this.reportRequestTime.FixedKey.ToLowerInvariant(),
                                profiler.ElapsedMilliseconds);
                        }
                        else if (OperationContext.Current != null && OperationContext.Current.IncomingMessageHeaders != null)
                        {
                            profiler.ReportTiming(
                                this.ParseMetricKey(OperationContext.Current.IncomingMessageHeaders, this.reportRequestTime.KeyPrefix).ToLowerInvariant(),
                                profiler.ElapsedMilliseconds);
                        }
                    }

                    if (this.reportHitCount != null && this.reportHitCount.Enable)
                    {
                        if (!string.IsNullOrEmpty(this.reportHitCount.FixedKey))
                        {
                            profiler.ReportCounter(
                                this.reportHitCount.FixedKey.ToLowerInvariant(), 
                                1);
                        }
                        else if (OperationContext.Current != null && OperationContext.Current.IncomingMessageHeaders != null)
                        {
                            profiler.ReportCounter(
                                this.ParseMetricKey(OperationContext.Current.IncomingMessageHeaders, this.reportHitCount.KeyPrefix).ToLowerInvariant(), 
                                1);
                        }
                    }
                }
            }
            catch (SystemException exception)
            {
                Trace.TraceError(exception.Format());
            }

            if (profiler != null)
            {
                profiler.Dispose();
            }
        }

        private string ParseMetricKey(MessageHeaders headers, string prefix)
        {
            if (headers == null)
                throw new ArgumentNullException("headers");

            string key = headers.Action.Split('/', '\\').Last().ToUnderscores();

            if (string.IsNullOrWhiteSpace(prefix))
                return key;

            return prefix + "." + key;
        }
    }
}
