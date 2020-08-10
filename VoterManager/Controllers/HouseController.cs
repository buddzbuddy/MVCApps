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
    public class HouseController : Controller
    {
        private DataManager dataManager;
        public HouseController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(from m in dataManager.Houses.GetAll()
                        select new HouseViewModel
                        {
                            House = m,
                            Street = dataManager.Streets.Get((int?)m.StreetId ?? 0),
                            Manager = dataManager.Persons.Get((int?)m.ManagerId ?? 0),
                            Precinct = dataManager.Precincts.Get(m.PrecinctId ?? 0)
                        });
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Houses.Get(Id);
            var model = new HouseViewModel
            {
                House = obj,
                Street = dataManager.Streets.Get((int?)obj.StreetId ?? 0),
                Manager = dataManager.Persons.Get((int?)obj.ManagerId ?? 0),
                Precinct = dataManager.Precincts.Get(obj.PrecinctId ?? 0)
            };
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              where p.HouseId == obj.Id
                              select p;
            var relationMunicipality = dataManager.MunicipalityHouseRelations.GetAll().FirstOrDefault(x => x.HouseId == Id);
            if(relationMunicipality != null)
            {
                ViewBag.Municipality = dataManager.Municipalities.Get(relationMunicipality.MunicipalityId.HasValue ? relationMunicipality.MunicipalityId.Value : 0);
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? streetId)
        {
            ViewBag.Streets = from s in dataManager.Streets.GetAll()
                                select new SelectListItem
                                {
                                    Text = s.Name,
                                    Value = s.Id.ToString(),
                                    Selected = streetId.HasValue ? streetId.Value == s.Id : false
                                };
            var precincts = new List<SelectListItem> { new SelectListItem() };
            precincts.AddRange(from p in dataManager.Precincts.GetAll()
                               select new SelectListItem
                               {
                                   Text = p.Name,
                                   Value = p.Id.ToString()
                               });
            ViewBag.Precincts = precincts;
            var managers = dataManager.Municipalities.GetAll().Where(m => m.ManagerId.HasValue).Select(m => m.ManagerId.Value).ToList();
            managers.AddRange(dataManager.Districts.GetAll().Where(d => d.ManagerId.HasValue).Select(d => d.ManagerId.Value));
            var persons = new List<SelectListItem> { new SelectListItem() };
            persons.AddRange(from p in dataManager.Persons.GetAll()
                             where !managers.Contains(p.Id)
                             select new SelectListItem
                             {
                                 Text = p.FullName,
                                 Value = p.Id.ToString()
                             });
            ViewBag.Persons = persons;
            return View();
        }

        [HttpPost]
        public ActionResult Create(House obj, FormCollection collection)
        {
            /*if (ModelState.IsValid)
            {
                if (!dataManager.Houses.GetAll()
                    .Where(m => m.StreetId == obj.StreetId)
                    .Any(o => o.Name == obj.Name))
                {
                    dataManager.Houses.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                    ModelState.AddModelError("Name",
                        "Дом с названием \"" + obj.Name + "\" уже существует!");
            }
            ViewBag.Streets = from d in dataManager.Streets.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = obj.StreetId.HasValue ? obj.StreetId.Value == d.Id : false
                                };
            var precincts = new List<SelectListItem> { new SelectListItem() };
            precincts.AddRange(from p in dataManager.Precincts.GetAll()
                               select new SelectListItem
                               {
                                   Text = p.Name,
                                   Value = p.Id.ToString(),
                                   Selected = p.Id == obj.PrecinctId
                               });
            ViewBag.Precincts = precincts;
            return View(obj);*/
            string latitude = collection["Latitude"];
            string longitude = collection["Longitude"];
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                obj.Latitude = double.Parse(latitude);
                obj.Longitude = double.Parse(longitude);
            }
            dataManager.Houses.Save(obj);
            return RedirectToAction("Show", new { Id = obj.Id });
        }

        public ActionResult CreateByChain(int Id)
        {
            var obj = dataManager.Houses.Get(Id);
            ViewBag.Streets = from d in dataManager.Streets.GetAll()
                              select new SelectListItem
                              {
                                  Text = d.Name,
                                  Value = d.Id.ToString(),
                                  Selected = obj.StreetId == d.Id
                              };
            var managers = dataManager.Municipalities.GetAll().Where(m => m.ManagerId.HasValue).Select(m => m.ManagerId.Value).ToList();
            //managers.AddRange(dataManager.Houses.GetAll().Where(h => h.ManagerId.HasValue).Select(h => h.ManagerId.Value).Distinct());
            managers.AddRange(dataManager.Districts.GetAll().Where(d => d.ManagerId.HasValue).Select(d => d.ManagerId.Value));
            managers.Remove((int?)obj.ManagerId ?? 0);
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              where !managers.Contains(p.Id)
                              select new SelectListItem
                              {
                                  Text = p.LastName + " " + p.FirstName + " " + p.MiddleName,
                                  Value = p.Id.ToString(),
                                  Selected = p.Id == obj.ManagerId
                              };
            var precincts = new List<SelectListItem> { new SelectListItem() };
            precincts.AddRange(from p in dataManager.Precincts.GetAll()
                               select new SelectListItem
                               {
                                   Text = p.Name,
                                   Value = p.Id.ToString(),
                                   Selected = p.Id == obj.PrecinctId
                               });
            ViewBag.Precincts = precincts;
            return View("Create", new House
            {
                IconPath = obj.IconPath,
                Latitude = obj.Latitude,
                Longitude = obj.Longitude,
                ManagerId = obj.ManagerId,
                PrecinctId = obj.PrecinctId,
                StreetId = obj.StreetId,
                WorkerId = obj.WorkerId,
                Zoom = obj.Zoom
            });
        }
        [HttpGet]
        public ActionResult CreatePartial(int? streetId)
        {
            if (streetId.HasValue)
                ViewBag.StreetName = dataManager.Streets.Get(streetId.Value).Name;

            return PartialView(new House
                {
                    StreetId = streetId
                });
        }

        [HttpPost]
        public ActionResult CreatePartial(House obj)
        {
            if (Request.IsAjaxRequest())
            {
                dataManager.Houses.Save(obj);

                return Json(new { Name = obj.Name, Id = obj.Id, Key = "HouseId2" }, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChoicePartial(int? streetId, int? Id)
        {
            if (streetId.HasValue)
            {
                ViewBag.Street = dataManager.Streets.Get(streetId.Value);
            }

            ViewBag.Houses = from l in dataManager.Houses.GetAll()
                                 where l.StreetId == streetId
                                 select new SelectListItem
                                 {
                                     Text = l.Name,
                                     Value = l.Id.ToString(),
                                     Selected = Id.HasValue ? Id.Value == l.Id : false
                                 };

            return PartialView(new House
            {
                StreetId = streetId
            });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = dataManager.Houses.Get(id);
            ViewBag.Streets = from d in dataManager.Streets.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = obj.StreetId == d.Id
                                };
            var managers = dataManager.Municipalities.GetAll().Where(m => m.ManagerId.HasValue).Select(m => m.ManagerId.Value).ToList();
            //managers.AddRange(dataManager.Houses.GetAll().Where(h => h.ManagerId.HasValue).Select(h => h.ManagerId.Value).Distinct());
            managers.AddRange(dataManager.Districts.GetAll().Where(d => d.ManagerId.HasValue).Select(d => d.ManagerId.Value));
            managers.Remove((int?)obj.ManagerId ?? 0);
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              where !managers.Contains(p.Id)
                              select new SelectListItem
                              {
                                  Text = p.LastName + " " + p.FirstName + " " + p.MiddleName,
                                  Value = p.Id.ToString(),
                                  Selected = p.Id == obj.ManagerId
                              };
            var precincts = new List<SelectListItem> { new SelectListItem() };
            precincts.AddRange(from p in dataManager.Precincts.GetAll()
                               select new SelectListItem
                               {
                                   Text = p.Name,
                                   Value = p.Id.ToString(),
                                   Selected = p.Id == obj.PrecinctId
                               });
            ViewBag.Precincts = precincts;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(House obj, FormCollection collection)
        {
            /*if (ModelState.IsValid)
            {
                if (!dataManager.Houses.GetAll()
                    .Where(m => m.StreetId == obj.StreetId)
                    .Any(o => o.Id != obj.Id && o.Name == obj.Name))
                {
                    var objFromDb = dataManager.Houses.Get(obj.Id);
                    objFromDb.Name = obj.Name;
                    objFromDb.StreetId = obj.StreetId;
                    objFromDb.ManagerId = obj.ManagerId;
                    objFromDb.PrecinctId = obj.PrecinctId;
                    dataManager.Houses.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                    ModelState.AddModelError("Name",
                        "Дом с названием \"" + obj.Name + "\" уже существует!");
            }
            ViewBag.Streets = from d in dataManager.Streets.GetAll()
                              select new SelectListItem
                              {
                                  Text = d.Name,
                                  Value = d.Id.ToString(),
                                  Selected = obj.StreetId == d.Id
                              };
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.LastName + " " + p.FirstName + " " + p.MiddleName,
                                  Value = p.Id.ToString(),
                                  Selected = p.Id == obj.ManagerId
                              };
            var precincts = new List<SelectListItem> { new SelectListItem() };
            precincts.AddRange(from p in dataManager.Precincts.GetAll()
                               select new SelectListItem
                               {
                                   Text = p.Name,
                                   Value = p.Id.ToString(),
                                   Selected = p.Id == obj.PrecinctId
                               });
            ViewBag.Precincts = precincts;
            return View(obj);*/
            string latitude = collection["Latitude"];
            string longitude = collection["Longitude"];
            if (!string.IsNullOrEmpty(latitude) && !string.IsNullOrEmpty(longitude))
            {
                obj.Latitude = double.Parse(latitude);
                obj.Longitude = double.Parse(longitude);
            }
            dataManager.Houses.Save(obj);
            return RedirectToAction("Show", new { Id = obj.Id });
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Houses.Get(Id);
            var model = new HouseViewModel
            {
                House = obj,
                Street = dataManager.Streets.Get((int?)obj.StreetId ?? 0)
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Houses.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
