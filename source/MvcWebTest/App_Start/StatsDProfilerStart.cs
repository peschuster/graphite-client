using System.Web;
using Graphite.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: WebActivator.PreApplicationStartMethod(
    typeof(MvcWebTest.App_Start.StatsDProfilerStart), "PreStart")]

namespace MvcWebTest.App_Start
{
    public class StatsDProfilerStart
    {
        public static void PreStart()
        {
            // Make sure the MiniProfiler handles BeginRequest and EndRequest
            DynamicModuleUtility.RegisterModule(typeof(StatsDProfileStartupModule));
        }
    }

    public class StatsDProfileStartupModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.BeginRequest += (sender, e) =>
            {
                StatsDProfilerProvider.Start();
            };

            context.EndRequest += (sender, e) =>
            {
                StatsDProfilerProvider.Stop();
            };
        }

        public void Dispose()
        {
        }
    }
}