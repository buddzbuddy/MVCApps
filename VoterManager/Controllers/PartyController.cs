using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoterManager.Controllers
{
    public class PartyController : Controller
    {
        private DataManager dataManager;
        public PartyController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(dataManager.Parties.GetAll());
        }

        public ActionResult Show(int Id)
        {
            return View(dataManager.Parties.Get(Id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Party obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Parties.GetAll().Any(o => o.Name == obj.Name))
                {
                    dataManager.Parties.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Партия с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(dataManager.Parties.Get(id));
        }

        [HttpGet]
        public ActionResult CreatePartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreatePartial(Party obj)
        {
            if(Request.IsAjaxRequest())
            {
                dataManager.Parties.Save(obj);

                return Json(new { Name = obj.Name, Id = obj.Id, Key = "PartyId" }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Party obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Parties.GetAll()
                    .Any(o =>
                        o.Name == obj.Name &&
                        o.Id != obj.Id))
                {
                    var objFromDb = dataManager.Parties.Get(obj.Id);
                    objFromDb.Name = obj.Name;
                    dataManager.Parties.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Партия с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Parties.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Parties.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
