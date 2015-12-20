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
    public class PartySupporterController : Controller
    {
        private DataManager dataManager;
        public PartySupporterController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(dataManager.PartySupporters.GetAll());
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.PartySupporters.Get(Id);
            return View(obj);
        }

        [HttpGet]
        public ActionResult Create(int? personId)
        {
            if (!personId.HasValue)
                return RedirectToAction("CreateBase", "Person", new { returnUrl = Request.Url.ToString(), processName = "Создание сторонника партии" });
            return View(new PartySupporter { PersonId = personId });
        }

        [HttpPost]
        public ActionResult Create(PartySupporter obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.PartySupporters.Save(obj);
                return RedirectToAction("Show", new { Id = obj.Id });
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = dataManager.PartySupporters.Get(id);
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(PartySupporter obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.PartySupporters.Save(obj);
                return RedirectToAction("Show", new { Id = obj.Id });
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.PartySupporters.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.PartySupporters.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
