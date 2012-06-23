using System.Web;

namespace Graphite.Web
{
    public class StatsDStartupModule : IHttpModule
    {
        private readonly bool reportRequestTime;

        private readonly string constantRequestTimeKey;

        public StatsDStartupModule(bool reportRequestTime, string constantRequestTimeKey = null)
        {
            this.constantRequestTimeKey = constantRequestTimeKey;
            this.reportRequestTime = reportRequestTime;
        }

        public virtual void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) =>
            {
                WebStatsDProfilerProvider.Instance.Start();
            };

            context.EndRequest += (sender, e) =>
            {
                var profiler = WebStatsDProfilerProvider.Instance.Stop();

                if (profiler != null && reportRequestTime)
                {
                    this.ReportRequestTime(profiler, HttpContext.Current);
                }
            };
        }

        public void Dispose()
        {
        }

        private void ReportRequestTime(StatsDProfiler profiler, HttpContext context)
        {
            profiler.ReportTiming(
                constantRequestTimeKey ?? this.ParseMetricKey(context), 
                profiler.ElapsedMilliseconds);
        }

        private string ParseMetricKey(HttpContext context)
        {
            var uri = context.Request.Url;

            return uri.AbsolutePath
                .Replace('/', '.')
                .Replace('\\', '.');
        }
    }
}
