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
            var model = new WorkerViewModel
            {
                Worker = obj,
                Person = dataManager.Persons.Get(obj.PersonId ?? 0)
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.FullName,
                                  Value = p.Id.ToString()
                              };
            return View();
        }

        [HttpPost]
        public ActionResult Create(Worker obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Workers.GetAll().Any(o => o.PersonId == obj.PersonId))
                {
                    var person = dataManager.Persons.Get(obj.PersonId ?? 0);
                    obj.FullName = person != null ? person.FullName : "ФИО не указан";
                    dataManager.Workers.Save(obj);

                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                ModelState.AddModelError("PersonId",
                    "Работник уже существует!");
            }
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.FullName,
                                  Value = p.Id.ToString()
                              };
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
            if (!dataManager.Workers.GetAll()
                    .Any(o =>
                        o.PersonId == obj.PersonId &&
                        o.Id != obj.Id))
            {
                var person = dataManager.Persons.Get(obj.PersonId ?? 0);
                obj.FullName = person != null ? person.FullName : "ФИО не указан";
                var fdb = dataManager.Workers.Get(obj.Id);
                fdb.PersonId = obj.PersonId;
                fdb.FullName = obj.FullName;
                dataManager.Workers.Save(fdb);
                return RedirectToAction("Show", new { Id = obj.Id });
            }
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.FullName,
                                  Value = p.Id.ToString(),
                                  Selected = p.Id == obj.PersonId
                              };
            ModelState.AddModelError("PersonId", "Работник уже существует!");
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
    }
}
