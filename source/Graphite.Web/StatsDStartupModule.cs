using System.Web;

namespace Graphite.Web
{
    public class StatsDStartupModule : IHttpModule
    {
        public virtual void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) =>
            {
                WebStatsDProfilerProvider.Instance.Start();
            };

            context.EndRequest += (sender, e) =>
            {
                WebStatsDProfilerProvider.Instance.Stop();
            };
        }

        public void Dispose()
        {
        }
    }
}
