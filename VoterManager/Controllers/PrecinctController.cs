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
    public class PrecinctController : Controller
    {
        private DataManager dataManager;
        public PrecinctController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(from m in dataManager.Precincts.GetAll()
                        select new PrecinctViewModel
                        {
                            Precinct = m,
                            District = dataManager.Districts.Get((int?)m.DistrictId ?? 0),
                            Worker = dataManager.Workers.Get(m.WorkerId ?? 0)
                        });
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Precincts.Get(Id);
            var model = new PrecinctViewModel
            {
                Precinct = obj,
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0),
                Worker = dataManager.Workers.Get(obj.WorkerId ?? 0)
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? districtId, int? workerId)
        {
            ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = districtId.HasValue ? districtId.Value == d.Id : false
                                };
            var workers = new List<SelectListItem>
            {
                new SelectListItem()
            };
            workers.AddRange(from w in dataManager.Workers.GetAll()
                             select new SelectListItem
                             {
                                 Text = dataManager.Persons.Get(w.PersonId ?? 0).FullName,
                                 Value = w.Id.ToString(),
                                 Selected = w.Id == workerId
                             });
            ViewBag.Workers = workers;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Precinct obj, FormCollection collection)
        {
            //if (ModelState.IsValid)
            //{
            //    if (!dataManager.Precincts.GetAll()
            //        .Where(m => m.DistrictId == obj.DistrictId)
            //        .Any(o => o.Name == obj.Name))
            //    {
            //        dataManager.Precincts.Save(obj);
            //        return RedirectToAction("Show", new { Id = obj.Id });
            //    }
            //    else
            //        ModelState.AddModelError("Name",
            //            "УИК с названием \"" + obj.Name + "\" уже существует!");
            //}
            //ViewBag.Districts = from d in dataManager.Districts.GetAll()
            //                    select new SelectListItem
            //                    {
            //                        Text = d.Name,
            //                        Value = d.Id.ToString(),
            //                        Selected = obj.DistrictId == d.Id
            //                    };
            //var workers = new List<SelectListItem>
            //{
            //    new SelectListItem()
            //};
            //workers.AddRange(from w in dataManager.Workers.GetAll()
            //                 select new SelectListItem
            //                 {
            //                     Text = dataManager.Persons.Get(w.PersonId ?? 0).FullName,
            //                     Value = w.Id.ToString(),
            //                     Selected = w.Id == obj.WorkerId
            //                 });
            //ViewBag.Workers = workers;
            //return View(obj);
            string latitude = collection["Latitude"];
            string longitude = collection["Longitude"];
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                obj.Latitude = double.Parse(latitude);
                obj.Longitude = double.Parse(longitude);
            }
            dataManager.Precincts.Save(obj);
            return RedirectToAction("Show", new { Id = obj.Id });
        }

        [HttpGet]
        public ActionResult CreatePartial(int? districtId)
        {
            if (districtId.HasValue)
                ViewBag.DistrictName = dataManager.Districts.Get(districtId.Value).Name;

            return PartialView(new Precinct
                {
                    DistrictId = districtId
                });
        }

        [HttpPost]
        public ActionResult CreatePartial(Precinct obj)
        {
            if (Request.IsAjaxRequest())
            {
                dataManager.Precincts.Save(obj);

                return Json(new { Name = obj.Name, Id = obj.Id, Key = "PrecinctId2" }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChoicePartial(int? districtId, int? Id)
        {
            if (districtId.HasValue)
            {
                ViewBag.District = dataManager.Districts.Get(districtId.Value);
            }

            ViewBag.Precincts = from l in dataManager.Precincts.GetAll()
                                 where (districtId.HasValue ? l.DistrictId == districtId : true)
                                 select new SelectListItem
                                 {
                                     Text = l.Name,
                                     Value = l.Id.ToString(),
                                     Selected = Id.HasValue ? Id.Value == l.Id : false
                                 };

            return PartialView(new Precinct
            {
                DistrictId = districtId
            });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = dataManager.Precincts.Get(id);
            ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = obj.DistrictId == d.Id
                                };
            var workers = new List<SelectListItem>
            {
                new SelectListItem()
            };
            workers.AddRange(from w in dataManager.Workers.GetAll()
                             select new SelectListItem
                             {
                                 Text = dataManager.Persons.Get(w.PersonId ?? 0).FullName,
                                 Value = w.Id.ToString(),
                                 Selected = w.Id == obj.WorkerId
                             });
            ViewBag.Workers = workers;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Precinct obj, FormCollection collection)
        {
            string latitude = collection["Latitude"];
            string longitude = collection["Longitude"];
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                obj.Latitude = double.Parse(latitude.Replace('.', ','));
                obj.Longitude = double.Parse(longitude.Replace('.', ','));
            }
            dataManager.Precincts.Save(obj);
            return RedirectToAction("Show", new { Id = obj.Id });
        }

        [HttpGet]
        public ActionResult SetMapPosition(int Id)
        {
            return View("Error");
            /*var obj = dataManager.Precincts.Get(Id);
            return View(obj);*/
        }

        [HttpPost]
        public ActionResult SetMapPosition(Precinct obj/*FormCollection collection*/)
        {
            /*string latitude = collection["Latitude"];
            string longitude = collection["Longitude"];
            string zoom = collection["Zoom"];
            string iconPath = collection["IconPath"];
            int id = int.Parse(collection["Id"]);
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                var objFromDb = dataManager.Precincts.Get(id);
                objFromDb.Latitude = double.Parse(latitude.Replace('.', ','));
                objFromDb.Longitude = double.Parse(longitude.Replace('.', ','));
                objFromDb.Zoom = int.Parse(zoom);
                objFromDb.IconPath = iconPath;
                dataManager.Precincts.Save(objFromDb);
            }*/
            return View("Error");
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Precincts.Get(Id);
            var model = new PrecinctViewModel
            {
                Precinct = obj,
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0),
                Worker = dataManager.Workers.Get(obj.WorkerId ?? 0)
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Precincts.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
