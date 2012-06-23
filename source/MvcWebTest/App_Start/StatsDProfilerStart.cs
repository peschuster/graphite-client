using Graphite.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MvcWebTest.App_Start.StatsDProfilerStart), "PreStart")]

namespace MvcWebTest.App_Start
{
    public class StatsDProfilerStart
    {
        public static void PreStart()
        {
            // Make sure StatsDProfiler handles BeginRequest and EndRequest
            DynamicModuleUtility.RegisterModule(typeof(StatsDStartupModule));

            StatsDStartupModule.Settings.ReportRequestTime = true;
            StatsDStartupModule.Settings.RequestTimePrefix = "request.time";
        }
    }
}