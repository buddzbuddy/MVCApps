using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoterManager.Controllers
{
    [Authorize]
    public class EntitiesController : Controller
    {
        //
        // GET: /Catalog/

        public ActionResult Index()
        {
            return View();
        }

    }
}
