using System;
using System.Web;

namespace Graphite.Web
{
    public class StatsDStartupModule : IHttpModule
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
                WebStatsDProfilerProvider.Instance.Start();
            };

            context.EndRequest += (sender, e) =>
            {
                var profiler = WebStatsDProfilerProvider.Instance.Stop();

                if (profiler != null && Settings.ReportRequestTime)
                {
                    profiler.ReportTiming(
                        Settings.FixedRequestTimeKey ?? this.ParseMetricKey(HttpContext.Current),
                        profiler.ElapsedMilliseconds);
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