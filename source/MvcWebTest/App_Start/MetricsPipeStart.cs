using Graphite.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MvcWebTest.App_Start.MetricsPipeStart), "PreStart")]

namespace MvcWebTest.App_Start
{
    public class MetricsPipeStart
    {
        public static void PreStart()
        {
            // Make sure MetricsPipe handles BeginRequest and EndRequest
            DynamicModuleUtility.RegisterModule(typeof(MetricsPipeStartupModule));

            MetricsPipeStartupModule.Settings.ReportRequestTime = true;
            MetricsPipeStartupModule.Settings.RequestTimePrefix = "request.time";
        }
    }
}