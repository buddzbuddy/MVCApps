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
    public class StreetController : Controller
    {
        private DataManager dataManager;
        public StreetController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(from s in dataManager.Streets.GetAll()
                        select new StreetViewModel
                        {
                            Street = s,
                            District = dataManager.Districts.Get((int?)s.DistrictId ?? 0),
                            Locality = dataManager.Localities.Get((int?)s.LocalityId ?? 0)
                        });
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Streets.Get(Id);
            var model = new StreetViewModel
            {
                Street = obj,
                Locality = dataManager.Localities.Get((int?)obj.LocalityId ?? 0),
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0)
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? districtId, int? localityId)
        {
            var model = new Street();
            if (localityId.HasValue)
            {
                var locality = dataManager.Localities.Get(localityId.Value);
                model.LocalityId = locality.Id;
                districtId = locality.DistrictId;
                ViewBag.Locality = locality;
            }
            ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = districtId.HasValue ? districtId.Value == d.Id : false
                                };
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Street obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Streets.GetAll()
                    .Where(m => m.DistrictId == obj.DistrictId)
                    .Any(o => o.Name == obj.Name))
                {
                    dataManager.Streets.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                    ModelState.AddModelError("Name",
                        "Улица с названием \"" + obj.Name + "\" уже существует!");
            }
            ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = obj.DistrictId.HasValue ? obj.DistrictId.Value == d.Id : false
                                };
            if (obj.LocalityId.HasValue)
            {
                var locality = dataManager.Localities.Get(obj.LocalityId.Value);
                ViewBag.Locality = locality;
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult CreatePartial(int? districtId, int? localityId)
        {
            if (districtId.HasValue)
                ViewBag.DistrictName = dataManager.Districts.Get(districtId.Value).Name;
            if (localityId.HasValue)
                ViewBag.LocalityName = dataManager.Localities.Get(localityId.Value).Name;

            return PartialView(new Street
                {
                    DistrictId = districtId,
                    LocalityId = localityId
                });
        }

        [HttpPost]
        public ActionResult CreatePartial(Street obj)
        {
            if (Request.IsAjaxRequest())
            {
                dataManager.Streets.Save(obj);

                return Json(new { Name = obj.Name, Id = obj.Id, Key = "StreetId2" }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChoicePartial(int? districtId, int? localityId, int? Id)
        {
            if (districtId.HasValue)
            {
                ViewBag.District = dataManager.Districts.Get(districtId.Value);
            }
            if (localityId.HasValue)
                ViewBag.Locality = dataManager.Localities.Get(localityId.Value);
            var streets = dataManager.Streets.GetAll().Where(s => districtId.HasValue ? districtId.Value == s.DistrictId : true);
            if (localityId.HasValue) streets = streets.Where(s => s.LocalityId == localityId);
            ViewBag.Streets = from s in streets
                                 select new SelectListItem
                                 {
                                     Text = s.Name,
                                     Value = s.Id.ToString(),
                                     Selected = Id.HasValue ? Id.Value == s.Id : false
                                 };

            return PartialView(new Street
            {
                DistrictId = districtId,
                LocalityId = localityId
            });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = dataManager.Streets.Get(id);
            if (obj.LocalityId.HasValue)
            {
                var locality = dataManager.Localities.Get(obj.LocalityId.Value);
                obj.LocalityId = locality.Id;
                obj.DistrictId = locality.DistrictId;
                ViewBag.Locality = locality;
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

        [HttpPost]
        public ActionResult Edit(Street obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Streets.GetAll()
                    .Where(m => m.DistrictId == obj.DistrictId)
                    .Any(o => o.Name == obj.Name))
                {
                    var objFromDb = dataManager.Streets.Get(obj.Id);
                    objFromDb.Name = obj.Name;
                    objFromDb.DistrictId = obj.DistrictId;
                    objFromDb.LocalityId = obj.LocalityId;
                    dataManager.Streets.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("Name",
                        "Улица с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Streets.Get(Id);
            var model = new StreetViewModel
            {
                Street = obj,
                Locality = dataManager.Localities.Get(obj.LocalityId ?? 0),
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0)
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Streets.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
