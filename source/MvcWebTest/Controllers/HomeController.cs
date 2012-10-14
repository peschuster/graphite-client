using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Graphite;

namespace MvcWebTest.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            MetricsPipe.Current.Count("home.index");

            using (MetricsPipe.Current.Step("home.index.view"))
            {
                return View();
            }
        }
        
        public ActionResult About()
        {
            MetricsPipe.Current.Count("home.about");

            using (MetricsPipe.Current.Step("home.about.view"))
            {
                return View();
            }
        }

        public ActionResult Exception()
        {
            throw new NotImplementedException();
        }
    }
}
