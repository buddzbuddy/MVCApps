using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoterManager.Controllers
{
    public class MonitorController : Controller
    {
        // GET: Monitor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Map()
        {
            return View();
        }
    }
}