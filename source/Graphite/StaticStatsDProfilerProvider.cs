using Graphite.Configuration;

namespace Graphite
{
    public class StaticStatsDProfilerProvider : IStatsDProfilerProvider
    {
        private static StaticStatsDProfilerProvider instance;

        public static StaticStatsDProfilerProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StaticStatsDProfilerProvider();
                }

                return instance;
            }
        }

        /// <summary>
        /// Returns the current StatsDProfiler instance.
        /// </summary>
        /// <value></value>
        public StatsDProfiler Current { get; set; }

        /// <summary>
        /// Starts a new StatsDProfiler instance.
        /// </summary>
        /// <returns></returns>
        public StatsDProfiler Start()
        {
            var result = new StatsDProfiler(GraphiteConfiguration.Instance, this, StopwatchWrapper.StartNew);
            Current = result;

            return result;
        }

        /// <summary>
        /// Stops the current StatsDProfiler instance.
        /// </summary>
        /// <returns></returns>
        public StatsDProfiler Stop()
        {
            StatsDProfiler current = Current;

            if (current != null)
            {
                current.Stop();

                current.Dispose();
            }

            return current;
        }
    }
}
