using Graphite.Configuration;

namespace Graphite
{
    /// <summary>
    /// <see cref="IMetricsPipeProvider"/> with static lifetime for usage in self-hosted applications (e.g. console application).
    /// </summary>
    public class StaticMetricsPipeProvider : IMetricsPipeProvider
    {
        private static StaticMetricsPipeProvider instance;

        /// <summary>
        /// Static instance of this provider (singleton).
        /// </summary>
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
            var result = new MetricsPipe(GraphiteConfigurationProvider.Get(), this, StopwatchWrapper.StartNew);
            Current = result;

            return result;
        }
        
        /// <summary>
        /// Starts a new MetricsPipe instance.
        /// </summary>
        /// <param name="configurationContainer"></param>
        /// <returns></returns>
        public MetricsPipe Start(IConfigurationContainer configurationContainer)
        {
            var result = new MetricsPipe(configurationContainer, this, StopwatchWrapper.StartNew);
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
