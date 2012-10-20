using Graphite.Configuration;

namespace Graphite
{
    public class StaticMetricsPipeProvider : IMetricsPipeProvider
    {
        private static StaticMetricsPipeProvider instance;

        public static StaticMetricsPipeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StaticMetricsPipeProvider();
                }

                return instance;
            }
        }

        /// <summary>
        /// Returns the current MetricsPipe instance.
        /// </summary>
        /// <value></value>
        public MetricsPipe Current { get; set; }

        /// <summary>
        /// Starts a new MetricsPipe instance.
        /// </summary>
        /// <returns></returns>
        public MetricsPipe Start()
        {
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

                current.Dispose();
            }

            return current;
        }
    }
}
