using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using VoterManager.Models;

namespace VoterManager.Controllers
{
    public class UserLogController : Controller
    {
        //
        // GET: /Chart/
        private DataManager dataManager;
        public UserLogController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(dataManager.UserLogs.GetAll().OrderByDescending(x => x.LoginDate));
        }
    }
}
