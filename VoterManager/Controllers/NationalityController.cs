using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoterManager.Controllers
{
    public class NationalityController : Controller
    {
        private DataManager dataManager;
        public NationalityController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(dataManager.Nationalities.GetAll());
        }

        public ActionResult Show(int Id)
        {
            return View(dataManager.Nationalities.Get(Id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Nationality obj, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Nationalities.GetAll().Any(o => o.Name == obj.Name))
                {
                    dataManager.Nationalities.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Национальность с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(dataManager.Nationalities.Get(id));
        }

        [HttpGet]
        public ActionResult CreatePartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreatePartial(Nationality obj)
        {
            if(Request.IsAjaxRequest())
            {
                dataManager.Nationalities.Save(obj);

                return Json(new { Name = obj.Name, Id = obj.Id, Key = "NationalityId" }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Nationality obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Nationalities.GetAll()
                    .Any(o =>
                        o.Name == obj.Name &&
                        o.Id != obj.Id))
                {
                    var objFromDb = dataManager.Nationalities.Get(obj.Id);
                    objFromDb.Name = obj.Name;
                    dataManager.Nationalities.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Национальность с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Nationalities.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Nationalities.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
