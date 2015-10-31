using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoterManager.Controllers
{
    public class RelationshipController : Controller
    {
        private DataManager dataManager;
        public RelationshipController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(dataManager.Relationships.GetAll());
        }

        public ActionResult Show(int Id)
        {
            return View(dataManager.Relationships.Get(Id));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Relationship obj, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Relationships.GetAll().Any(o => o.Type == obj.Type))
                {
                    dataManager.Relationships.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Тип взаимоотношений с названием \"" + obj.Type + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(dataManager.Relationships.Get(id));
        }

        [HttpGet]
        public ActionResult CreatePartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreatePartial(Relationship obj)
        {
            if(Request.IsAjaxRequest())
            {
                dataManager.Relationships.Save(obj);

                return Json(new { Name = obj.Type, Id = obj.Id, Key = "RelationshipId" }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(Relationship obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Relationships.GetAll()
                    .Any(o =>
                        o.Type == obj.Type &&
                        o.Id != obj.Id))
                {
                    var objFromDb = dataManager.Relationships.Get(obj.Id);
                    objFromDb.Type = obj.Type;
                    dataManager.Relationships.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Национальность с названием \"" + obj.Type + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Relationships.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Relationships.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
