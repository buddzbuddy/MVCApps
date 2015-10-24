using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoterManager.Controllers
{
    public class OrganizationController : Controller
    {
        private DataManager dataManager;
        public OrganizationController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(dataManager.Organizations.GetAll());
        }

        public ActionResult Show(int Id)
        {
            return View(dataManager.Organizations.Get(Id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Organization obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Organizations.GetAll().Any(o => o.Name == obj.Name))
                {
                    dataManager.Organizations.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Организация с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(dataManager.Organizations.Get(id));
        }

        [HttpGet]
        public ActionResult CreatePartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreatePartial(Organization obj)
        {
            if(Request.IsAjaxRequest())
            {
                dataManager.Organizations.Save(obj);

                return Json(new { Name = obj.Name, Id = obj.Id, Key = "OrganizationId" }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Organization obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Organizations.GetAll()
                    .Any(o =>
                        o.Name == obj.Name &&
                        o.Id != obj.Id))
                {
                    var objFromDb = dataManager.Organizations.Get(obj.Id);
                    objFromDb.Name = obj.Name;
                    dataManager.Organizations.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Организация с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Organizations.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Organizations.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
