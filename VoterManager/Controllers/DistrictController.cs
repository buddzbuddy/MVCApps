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
    public class DistrictController : Controller
    {
        private DataManager dataManager;
        public DistrictController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(from d in dataManager.Districts.GetAll()
                        select new DistrictViewModel
                        {
                            District = d,
                            Manager = dataManager.Persons.Get((int?)d.ManagerId ?? 0)
                        });
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Districts.Get(Id);
            var model = new DistrictViewModel
            {
                District = obj,
                Manager = dataManager.Persons.Get((int?)obj.ManagerId ?? 0)
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(District obj, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Districts.GetAll().Any(o => o.Name == obj.Name))
                {
                    dataManager.Districts.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Район/город с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = dataManager.Districts.Get(id);
            var managers = dataManager.Municipalities.GetAll().Where(m => m.ManagerId.HasValue).Select(m => m.ManagerId.Value).ToList();
            managers.AddRange(dataManager.Houses.GetAll().Where(h => h.ManagerId.HasValue).Select(h => h.ManagerId.Value).Distinct());
            managers.AddRange(dataManager.Districts.GetAll().Where(d => d.ManagerId.HasValue).Select(d => d.ManagerId.Value));
            managers.Remove((int?)obj.ManagerId ?? 0);
            ViewBag.Persons = from p in dataManager.Persons.GetAll().Where(p => !managers.Contains(p.Id))
                              select new SelectListItem
                              {
                                  Text = p.LastName + " " + p.FirstName + " " + p.MiddleName,
                                  Value = p.Id.ToString(),
                                  Selected = p.Id == obj.ManagerId
                              };
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(District obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Districts.GetAll()
                    .Any(o =>
                        o.Name == obj.Name &&
                        o.Id != obj.Id))
                {
                    var objFromDb = dataManager.Districts.Get(obj.Id);
                    objFromDb.Name = obj.Name;
                    objFromDb.ManagerId = obj.ManagerId;
                    dataManager.Districts.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    var managers = dataManager.Municipalities.GetAll().Where(m => m.ManagerId.HasValue).Select(m => m.ManagerId.Value).ToList();
                    managers.AddRange(dataManager.Houses.GetAll().Where(h => h.ManagerId.HasValue).Select(h => h.ManagerId.Value).Distinct());
                    managers.AddRange(dataManager.Districts.GetAll().Where(d => d.ManagerId.HasValue).Select(d => d.ManagerId.Value));
                    managers.Remove((int?)obj.ManagerId ?? 0);
                    ViewBag.Persons = from p in dataManager.Persons.GetAll().Where(p => !managers.Contains(p.Id))
                                      select new SelectListItem
                                      {
                                          Text = p.LastName + " " + p.FirstName + " " + p.MiddleName,
                                          Value = p.Id.ToString(),
                                          Selected = p.Id == obj.ManagerId
                                      };
                    ModelState.AddModelError("Name",
                        "Район/город с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult CreatePartial()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreatePartial(District obj)
        {
            if(Request.IsAjaxRequest())
            {
                dataManager.Districts.Save(obj);

                return Json(new { Name = obj.Name, Id = obj.Id, Key = "DistrictId" }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Districts.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Districts.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
