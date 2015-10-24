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
    public class LocalityController : Controller
    {
        private DataManager dataManager;
        public LocalityController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(from m in dataManager.Localities.GetAll()
                        select new LocalityViewModel
                        {
                            Locality = m,
                            District = dataManager.Districts.Get((int?)m.DistrictId ?? 0)
                        });
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Localities.Get(Id);
            var model = new LocalityViewModel
            {
                Locality = obj,
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0)
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? districtId)
        {
            ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = districtId.HasValue ? districtId.Value == d.Id : false
                                };
            return View();
        }

        [HttpPost]
        public ActionResult Create(Locality obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Localities.GetAll()
                    .Where(m => m.DistrictId == obj.DistrictId)
                    .Any(o => o.Name == obj.Name))
                {
                    dataManager.Localities.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                    ModelState.AddModelError("Name",
                        "Нас. пункт с названием \"" + obj.Name + "\" уже существует!");
            }
            ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = obj.DistrictId.HasValue ? obj.DistrictId.Value == d.Id : false
                                };
            return View(obj);
        }

        [HttpGet]
        public ActionResult CreatePartial(int? districtId)
        {
            if (districtId.HasValue)
                ViewBag.DistrictName = dataManager.Districts.Get(districtId.Value).Name;

            return PartialView(new Locality
                {
                    DistrictId = districtId
                });
        }

        [HttpPost]
        public ActionResult CreatePartial(Locality obj)
        {
            if (Request.IsAjaxRequest())
            {
                dataManager.Localities.Save(obj);

                return Json(new { Name = obj.Name, Id = obj.Id, Key = "LocalityId2" }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChoicePartial(int? districtId, int? Id)
        {
            if (districtId.HasValue)
            {
                ViewBag.District = dataManager.Districts.Get(districtId.Value);
            }

            ViewBag.Localities = from l in dataManager.Localities.GetAll()
                                 where (districtId.HasValue ? l.DistrictId == districtId : true)
                                 select new SelectListItem
                                 {
                                     Text = l.Name,
                                     Value = l.Id.ToString(),
                                     Selected = Id.HasValue ? Id.Value == l.Id : false
                                 };

            return PartialView(new Locality
            {
                DistrictId = districtId
            });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = dataManager.Localities.Get(id);
            ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = obj.DistrictId == d.Id
                                };
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Locality obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Localities.GetAll()
                    .Where(m => m.DistrictId == obj.DistrictId)
                    .Any(o => o.Name == obj.Name))
                {
                    var objFromDb = dataManager.Localities.Get(obj.Id);
                    objFromDb.Name = obj.Name;
                    objFromDb.DistrictId = obj.DistrictId;
                    dataManager.Localities.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Нас. пункт с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Localities.Get(Id);
            var model = new LocalityViewModel
            {
                Locality = obj,
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0)
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Localities.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
