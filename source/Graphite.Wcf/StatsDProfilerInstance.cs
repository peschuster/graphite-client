using System;
using System.ServiceModel;

namespace Graphite.Wcf
{
    public class StatsDProfilerInstance : IExtension<OperationContext>
    {
        private readonly StatsDProfiler profiler;

        public StatsDProfilerInstance(StatsDProfiler profiler)
        {
            if (profiler == null)
                throw new ArgumentNullException("profiler");

            this.profiler = profiler;
        }

        public static StatsDProfiler Current
        {
            get
            {
                var context = OperationContext.Current;

                if (context == null)
                    return null;

                var instance = context.Extensions.Find<StatsDProfilerInstance>();

                return instance == null ? null : instance.profiler;
            }

            set
            {
                var context = OperationContext.Current;

                if (context == null)
                    return;

                var instance = context.Extensions.Find<StatsDProfilerInstance>();

                if (instance != null)
                {
                    context.Extensions.Remove(instance);
                }

                if (value != null)
                {
                    instance = new StatsDProfilerInstance(value);

                    context.Extensions.Add(instance);
                }
            }
        }

        void IExtension<OperationContext>.Attach(OperationContext owner)
        {
        }

        void IExtension<OperationContext>.Detach(OperationContext owner)
        {
        }
    }
}
