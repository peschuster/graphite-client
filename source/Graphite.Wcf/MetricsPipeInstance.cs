using System;
using System.ServiceModel;

namespace Graphite.Wcf
{
    public class MetricsPipeInstance : IExtension<OperationContext>
    {
        private readonly MetricsPipe profiler;

        public MetricsPipeInstance(MetricsPipe profiler)
        {
            if (profiler == null)
                throw new ArgumentNullException("profiler");

            this.profiler = profiler;
        }

        public static MetricsPipe Current
        {
            get
            {
                var context = OperationContext.Current;

                if (context == null)
                    return null;

                var instance = context.Extensions.Find<MetricsPipeInstance>();

                return instance == null ? null : instance.profiler;
            }

            set
            {
                var context = OperationContext.Current;

                if (context == null)
                    return;

                var instance = context.Extensions.Find<MetricsPipeInstance>();

                if (instance != null)
                {
                    context.Extensions.Remove(instance);
                }

                if (value != null)
                {
                    instance = new MetricsPipeInstance(value);

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
