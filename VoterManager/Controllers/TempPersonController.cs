using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain;
using Domain.Entities;
using BusinessLogic;

namespace VoterManager.Controllers
{
    public class TempPersonController : Controller
    {
        private DataManager dataManager;
        public TempPersonController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        // GET: TempPersons
        public ActionResult Index()
        {
            return View(dataManager.TempPersons.GetAll().Take(100).ToList());
        }
    }
}
