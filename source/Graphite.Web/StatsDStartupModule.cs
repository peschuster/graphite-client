using System;
using System.Web;

namespace Graphite.Web
{
    public class StatsDStartupModule : IHttpModule
    {
        private readonly bool reportRequestTime;

        private readonly string constantRequestTimeKey;

        private readonly string requestTimePrefix;

        public StatsDStartupModule(bool reportRequestTime, string constantRequestTimeKey = null, string requestTimePrefix = null)
        {
            this.reportRequestTime = reportRequestTime;
            this.constantRequestTimeKey = constantRequestTimeKey;
            this.requestTimePrefix = requestTimePrefix;
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
            Uri uri = context.Request.Url;

            string key = uri.AbsolutePath
                .Replace('/', '.')
                .Replace('\\', '.')
                .Trim('.');

            if (string.IsNullOrWhiteSpace(this.requestTimePrefix))
                return key;

            return this.requestTimePrefix + "." + key;
        }
    }
}
