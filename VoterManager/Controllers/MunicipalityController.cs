using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using VoterManager.Models;

namespace VoterManager.Controllers
{
    public class MunicipalityController : Controller
    {
        private DataManager dataManager;
        public MunicipalityController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(from m in dataManager.Municipalities.GetAll()
                        select new MunicipalityViewModel
                        {
                            Municipality = m,
                            District = dataManager.Districts.Get((int?)m.DistrictId ?? 0),
                            Manager = dataManager.Persons.Get((int?)m.ManagerId ?? 0)
                        });
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Municipalities.Get(Id);
            var model = new MunicipalityViewModel
            {
                Municipality = obj,
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0),
                Manager = dataManager.Persons.Get((int?)obj.ManagerId ?? 0)
            };

            var relationHouses = dataManager.MunicipalityHouseRelations.GetAll().Where(x => x.MunicipalityId == Id);
            if (relationHouses.Count() > 0)
            {
                Street streetObj;
                var houses = from h in dataManager.Houses.GetAll().Where(x => relationHouses.Select(a => a.HouseId.Value).Contains(x.Id))
                             select new
                             {
                                 House = h,
                                 Street = streetObj = dataManager.Streets.Get(h.StreetId.HasValue ? h.StreetId.Value : 0),
                                 District = streetObj != null ? dataManager.Districts.Get(streetObj.DistrictId.HasValue ? streetObj.DistrictId.Value : 0) : null
                             }.ToSafeDynamic();
                ViewBag.Houses = houses;
            }

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
        public ActionResult Create(Municipality obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Municipalities.GetAll()
                    .Where(m => m.DistrictId == obj.DistrictId)
                    .Any(o => o.Name == obj.Name))
                {
                    dataManager.Municipalities.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                    ModelState.AddModelError("Name",
                        "МТУ с названием \"" + obj.Name + "\" уже существует!");
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
        public ActionResult Edit(int id)
        {
            var obj = dataManager.Municipalities.Get(id);
            ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = obj.DistrictId == d.Id
                                };
            var managers = dataManager.Municipalities.GetAll().Where(m => m.ManagerId.HasValue).Select(m => m.ManagerId.Value).ToList();
            managers.AddRange(dataManager.Houses.GetAll().Where(h => h.ManagerId.HasValue).Select(h => h.ManagerId.Value).Distinct());
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
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Municipality obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Municipalities.GetAll()
                    .Where(m => m.DistrictId == obj.DistrictId)
                    .Any(o => o.Id != obj.Id && o.Name == obj.Name))
                {
                    var objFromDb = dataManager.Municipalities.Get(obj.Id);
                    objFromDb.Name = obj.Name;
                    objFromDb.DistrictId = obj.DistrictId;
                    objFromDb.ManagerId = obj.ManagerId;
                    dataManager.Municipalities.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                        select new SelectListItem
                                        {
                                            Text = d.Name,
                                            Value = d.Id.ToString(),
                                            Selected = obj.DistrictId == d.Id
                                        };
                    var managers = dataManager.Municipalities.GetAll().Where(m => m.ManagerId.HasValue).Select(m => m.ManagerId.Value).ToList();
                    managers.AddRange(dataManager.Houses.GetAll().Where(h => h.ManagerId.HasValue).Select(h => h.ManagerId.Value).Distinct());
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
                    ModelState.AddModelError("Name",
                        "МТУ с названием \"" + obj.Name + "\" уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Municipalities.Get(Id);
            var model = new MunicipalityViewModel
            {
                Municipality = obj,
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0)
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Municipalities.Delete(Id);
            return RedirectToAction("Index");
        }
    }
    public static class SO7429957
    {
        public static dynamic ToSafeDynamic(this object obj)
        {
            //would be nice to restrict to anonymous types - but alas no.
            IDictionary<string, object> toReturn = new ExpandoObject();

            foreach (var prop in obj.GetType().GetProperties(
              BindingFlags.Public | BindingFlags.Instance)
              .Where(p => p.CanRead))
            {
                toReturn[prop.Name] = prop.GetValue(obj, null);
            }

            return toReturn;
        }
    }
}
