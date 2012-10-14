using System.ServiceModel;
using Graphite.Configuration;

namespace Graphite.Wcf
{
    public class WcfMetricsPipeProvider : IMetricsPipeProvider
    {
        private static WcfMetricsPipeProvider instance;

        public static WcfMetricsPipeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WcfMetricsPipeProvider();
                }

                return instance;
            }
        }

        /// <summary>
        /// Returns the current MetricsPipe instance.
        /// </summary>
        /// <value></value>
        public MetricsPipe Current
        {
            get
            {
                return MetricsPipeInstance.Current;
            }

            set
            {
                MetricsPipeInstance.Current = value;
            }
        }

        /// <summary>
        /// Starts a new MetricsPipe instance.
        /// </summary>
        /// <returns></returns>
        public MetricsPipe Start()
        {
            var context = OperationContext.Current;

            if (context == null)
                return null;

            var result = new MetricsPipe(GraphiteConfiguration.Instance, this, StopwatchWrapper.StartNew);
            Current = result;

            return result;
        }

        /// <summary>
        /// Stops the current MetricsPipe instance.
        /// </summary>
        /// <returns></returns>
        public MetricsPipe Stop()
        {
            MetricsPipe current = Current;

            if (current != null)
            {
                current.Stop();
            }

            return current;
        }
    }
}