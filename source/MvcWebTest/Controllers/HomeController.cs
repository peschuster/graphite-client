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
            StatsDProfiler.Current.Count("home.index");

            using (StatsDProfiler.Current.Step("home.index.view"))
            {
                return View();
            }
        }
        
        public ActionResult About()
        {
            StatsDProfiler.Current.Count("home.about");

            using (StatsDProfiler.Current.Step("home.about.view"))
            {
                return View();
            }
        }
    }
}
