using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoterManager.Controllers
{
    public class EducationController : Controller
    {
        private DataManager dataManager;
        public EducationController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(dataManager.Educations.GetAll());
        }

        public ActionResult Show(int Id)
        {
            return View(dataManager.Educations.Get(Id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Education obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Educations.GetAll().Any(o => o.Name == obj.Name))
                {
                    dataManager.Educations.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Образование с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(dataManager.Educations.Get(id));
        }

        [HttpGet]
        public ActionResult CreatePartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreatePartial(Education obj)
        {
            if(Request.IsAjaxRequest())
            {
                dataManager.Educations.Save(obj);

                return Json(new { Name = obj.Name, Id = obj.Id, Key = "EducationId" }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Education obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Educations.GetAll()
                    .Any(o =>
                        o.Name == obj.Name &&
                        o.Id != obj.Id))
                {
                    var objFromDb = dataManager.Educations.Get(obj.Id);
                    objFromDb.Name = obj.Name;
                    dataManager.Educations.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Образование с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Educations.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Educations.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
