using System;
using System.Web;

namespace Graphite.Web
{
    public class MetricsPipeStartupModule : IHttpModule
    {
        public static class Settings
        {
            static Settings()
            {
                ReportRequestTime = false;
                FixedRequestTimeKey = null;
                RequestTimePrefix = null;
            }

            public static bool ReportRequestTime { get; set; }

            public static string FixedRequestTimeKey { get; set; }

            public static string RequestTimePrefix { get; set; }
        }


        public virtual void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) =>
            {
                WebMetricsPipeProvider.Instance.Start();
            };

            context.EndRequest += (sender, e) =>
            {
                var profiler = WebMetricsPipeProvider.Instance.Stop();

                if (profiler != null && Settings.ReportRequestTime)
                {
                    profiler.ReportTiming(
                        Settings.FixedRequestTimeKey ?? this.ParseMetricKey(HttpContext.Current),
                        profiler.ElapsedMilliseconds);
                }

                if (profiler != null)
                {
                    profiler.Dispose();
                }
            };
        }

        public void Dispose()
        {
        }

        private string ParseMetricKey(HttpContext context)
        {
            Uri uri = context.Request.Url;

            string key = uri.ToMetricKey();

            if (string.IsNullOrWhiteSpace(Settings.RequestTimePrefix))
                return key;

            return Settings.RequestTimePrefix + "." + key;
        }
    }
}