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
    public class PartyMemberController : Controller
    {
        private DataManager dataManager;
        public PartyMemberController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(dataManager.PartyMembers.GetAll());
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.PartyMembers.Get(Id);
            return View(obj);
        }

        [HttpGet]
        public ActionResult Create(int? personId)
        {
            if (!personId.HasValue)
                return RedirectToAction("CreateBase", "Person", new { returnUrl = Request.Url.ToString(), processName = "Создание члена партии" });
            return View(new PartyMember { PersonId = personId });
        }

        [HttpPost]
        public ActionResult Create(PartyMember obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.PartyMembers.Save(obj);
                return RedirectToAction("Show", new { Id = obj.Id });
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = dataManager.PartyMembers.Get(id);
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(PartyMember obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.PartyMembers.Save(obj);
                return RedirectToAction("Show", new { Id = obj.Id });
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.PartyMembers.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.PartyMembers.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
