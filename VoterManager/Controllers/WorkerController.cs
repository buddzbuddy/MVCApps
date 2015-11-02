using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoterManager.Models;

namespace VoterManager.Controllers
{
    public class WorkerController : Controller
    {
        private DataManager dataManager;
        public WorkerController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {

            return View(from d in dataManager.Workers.GetAll()
                        select new WorkerViewModel
                        {
                            Worker = d,
                            Person = dataManager.Persons.Get(d.PersonId ?? 0)
                        });
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Workers.Get(Id);
            var workerHouseRelations = from wh in dataManager.WorkerHouseRelations.GetAll()
                                       where wh.WorkerId == Id
                                       select wh;
            var model = new WorkerViewModel
            {
                Worker = obj,
                Person = dataManager.Persons.Get(obj.PersonId ?? 0),
                RelatedHouses = new List<WorkerHouseRelationViewModel>(from wh in workerHouseRelations
                                                                       select new WorkerHouseRelationViewModel
                                                                       {
                                                                           WorkerHouseRelation = wh,
                                                                           House = dataManager.Houses.Get(wh.HouseId ?? 0)
                                                                       })
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? personId)
        {
            if (!personId.HasValue)
                return RedirectToAction("CreateBase", "Person", new { returnUrl = Request.Url.ToString(), processName = "Создать работника штаба" });
            return View(new Worker { PersonId = personId });
        }

        [HttpPost]
        public ActionResult Create(Worker obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.Workers.Save(obj);
                return RedirectToAction("Show", new { Id = obj.Id });
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = dataManager.Workers.Get(id);
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.FullName,
                                  Value = p.Id.ToString(),
                                  Selected = p.Id == obj.PersonId
                              };
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Worker obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.Workers.Save(obj);
                return RedirectToAction("Show", new { Id = obj.Id });
            }
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.FullName,
                                  Value = p.Id.ToString(),
                                  Selected = p.Id == obj.PersonId
                              };
            //ModelState.AddModelError("PersonId", "Работник уже существует!");
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Workers.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Workers.Delete(Id);
            return RedirectToAction("Index");
        }

        public ActionResult AddRelatedHouse(int workerId)
        {
            ViewBag.Houses = from h in dataManager.Houses.GetAll()
                             select new SelectListItem
                             {
                                 Text = h.Name,
                                 Value = h.Id.ToString()
                             };
            return View(new WorkerHouseRelation { WorkerId = workerId });
        }

        [HttpPost]
        public ActionResult AddRelatedHouse(WorkerHouseRelation obj)
        {
            if(ModelState.IsValid)
            {
                dataManager.WorkerHouseRelations.Save(obj);
                return RedirectToAction("Show", new { Id = obj.WorkerId });
            }

            ViewBag.Houses = from h in dataManager.Houses.GetAll()
                             select new SelectListItem
                             {
                                 Text = h.Name,
                                 Value = h.Id.ToString()
                             };
            return View(obj);
        }

        public ActionResult RemoveRelatedHouse(int relationId)
        {
            var rel = dataManager.WorkerHouseRelations.Get(relationId);
            if (rel != null)
            {
                dataManager.WorkerHouseRelations.Delete(rel.Id);
            }
            return RedirectToAction("Show", new { Id = rel.WorkerId });
        }
    }
}
