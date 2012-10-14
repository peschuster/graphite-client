using System.Web;
using Graphite.Configuration;

namespace Graphite.Web
{
    public class WebMetricsPipeProvider : IMetricsPipeProvider
    {
        private const string CacheKey = "MetricsPipe.Instance";

        private static WebMetricsPipeProvider instance;

        public static WebMetricsPipeProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WebMetricsPipeProvider();
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
                var context = HttpContext.Current;

                if (context == null)
                    return null;

                return context.Items[CacheKey] as MetricsPipe;
            }

            set
            {
                var context = HttpContext.Current;

                if (context == null)
                    return;

                context.Items[CacheKey] = value;
            }
        }

        /// <summary>
        /// Starts a new MetricsPipe instance.
        /// </summary>
        /// <returns></returns>
        public MetricsPipe Start()
        {
            var context = HttpContext.Current;

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