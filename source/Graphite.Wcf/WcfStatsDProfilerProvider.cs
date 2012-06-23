using System.ServiceModel;
using Graphite.Configuration;

namespace Graphite.Wcf
{
    public class WcfStatsDProfilerProvider : IStatsDProfilerProvider
    {
        private static WcfStatsDProfilerProvider instance;

        public static WcfStatsDProfilerProvider Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new WcfStatsDProfilerProvider();
                }

                return instance;
            }
        }

        public StatsDProfiler Current
        {
            get
            {
                return StatsDProfilerInstance.Current;
            }

            set
            {
                StatsDProfilerInstance.Current = value;
            }
        }

        public StatsDProfiler Start()
        {
            var context = OperationContext.Current;

            if (context == null)
                return null;

            var result = new StatsDProfiler(GraphiteConfiguration.Instance, this, StopwatchWrapper.StartNew);
            Current = result;

            return result;
        }

        public void Stop()
        {
            StatsDProfiler current = Current;

            if (current != null)
            {
                current.Stop();

                current.Dispose();
            }
        }
    }
}