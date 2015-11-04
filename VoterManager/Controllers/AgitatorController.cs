using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoterManager.Models;

namespace AgitatorManager.Controllers
{
    public class AgitatorController : Controller
    {
        private DataManager dataManager;
        public AgitatorController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(from v in dataManager.Agitators.GetAll()
                        select new AgitatorViewModel
                        {
                            Agitator = v,
                            Person = dataManager.Persons.Get(v.PersonId ?? 0)
                        });
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Agitators.Get(Id);
            var person = dataManager.Persons.Get(obj.PersonId ?? 0);
            var model = new AgitatorViewModel
            {
                Agitator = obj,
                Person = person,
                RelatedHouses = new List<AgitatorHouseRelationViewModel>(from ah in dataManager.AgitatorHouseRelations.GetAll()
                                                                         where ah.AgitatorId == Id
                                                                         select new AgitatorHouseRelationViewModel
                                                                         {
                                                                             AgitatorHouseRelation = ah,
                                                                             House = dataManager.Houses.Get(ah.HouseId ?? 0)
                                                                         }),
                RelatedPrecincts = new List<AgitatorPrecinctRelationViewModel>(from ap in dataManager.AgitatorPrecinctRelations.GetAll()
                                                                               where ap.AgitatorId == Id
                                                                               select new AgitatorPrecinctRelationViewModel
                                                                               {
                                                                                   AgitatorPrecinctRelation = ap,
                                                                                   Precinct = dataManager.Precincts.Get(ap.PrecinctId ?? 0)
                                                                               })
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? personId)
        {
            if (!personId.HasValue)
                return RedirectToAction("CreateBase", "Person", new { returnUrl = Request.Url.ToString(), processName = "Создать агитатора" });
            return View(new Agitator { PersonId = personId });
        }

        [HttpPost]
        public ActionResult Create(Agitator obj)
        {
            if (ModelState.IsValid)
            {
                {
                    dataManager.Agitators.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
            }
            return View(obj);
        }
        
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = dataManager.Persons.Get(id);
            var nationalities = new List<SelectListItem> { new SelectListItem() };
            nationalities.AddRange(from n in dataManager.Nationalities.GetAll()
                                   select new SelectListItem
                                   {
                                       Text = n.Name,
                                       Value = n.Id.ToString(),
                                       Selected = obj.NationalityId == n.Id
                                   });
            ViewBag.Nationalities = nationalities;
            var educations = new List<SelectListItem> { new SelectListItem() };
            educations.AddRange(from n in dataManager.Educations.GetAll()
                                select new SelectListItem
                                {
                                    Text = n.Name,
                                    Value = n.Id.ToString(),
                                    Selected = obj.EducationId == n.Id
                                });
            ViewBag.Educations = educations;
            var organizations = new List<SelectListItem> { new SelectListItem() };
            organizations.AddRange(from n in dataManager.Organizations.GetAll()
                                   select new SelectListItem
                                   {
                                       Text = n.Name,
                                       Value = n.Id.ToString()
                                   });

            if(obj.HouseId.HasValue)
            {
                var house = dataManager.Houses.Get(obj.HouseId.Value);
                obj.StreetId = house.StreetId;
                ViewBag.House = house;
            }
            if (obj.StreetId.HasValue)
            {
                var street = dataManager.Streets.Get(obj.StreetId.Value);
                obj.LocalityId = street.LocalityId;
                obj.DistrictId = street.DistrictId;
                ViewBag.Street = street;
            }
            if (obj.LocalityId.HasValue)
            {
                var locality = dataManager.Localities.Get(obj.LocalityId.Value);
                obj.DistrictId = locality.DistrictId;
                ViewBag.Locality = locality;
            }
            var districts = new List<SelectListItem> { new SelectListItem() };
            districts.AddRange(from d in dataManager.Districts.GetAll()
                               select new SelectListItem
                               {
                                   Text = d.Name,
                                   Value = d.Id.ToString(),
                                   Selected = obj.DistrictId == d.Id
                               });
            ViewBag.Districts = districts;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Person obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.Persons.Save(obj);
                return RedirectToAction("Show", new { Id = obj.Id });
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Persons.Get(Id);
            var model = new PersonViewModel
            {
                Person = obj,
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0),
                Nationality = dataManager.Nationalities.Get((int?)obj.NationalityId ?? 0),
                Locality = dataManager.Localities.Get((int?)obj.LocalityId ?? 0),
                Street = dataManager.Streets.Get((int?)obj.StreetId ?? 0),
                House = dataManager.Houses.Get((int?)obj.HouseId ?? 0)
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Persons.Delete(Id);
            return RedirectToAction("Index");
        }

        public ActionResult AddRelatedHouse(int agitatorId)
        {
            ViewBag.Houses = from h in dataManager.Houses.GetAll()
                             select new SelectListItem
                             {
                                 Text = h.Name,
                                 Value = h.Id.ToString()
                             };
            return View(new AgitatorHouseRelation { AgitatorId = agitatorId });
        }

        [HttpPost]
        public ActionResult AddRelatedHouse(AgitatorHouseRelation obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.AgitatorHouseRelations.Save(obj);
                return RedirectToAction("Show", new { Id = obj.AgitatorId });
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
            var rel = dataManager.AgitatorHouseRelations.Get(relationId);
            if (rel != null)
            {
                dataManager.AgitatorHouseRelations.Delete(rel.Id);
            }
            return RedirectToAction("Show", new { Id = rel.AgitatorId });
        }

        public ActionResult AddRelatedPrecinct(int agitatorId)
        {
            ViewBag.Precincts = from h in dataManager.Precincts.GetAll()
                                select new SelectListItem
                                {
                                    Text = h.Name,
                                    Value = h.Id.ToString()
                                };
            return View(new AgitatorPrecinctRelation { AgitatorId = agitatorId });
        }

        [HttpPost]
        public ActionResult AddRelatedPrecinct(AgitatorPrecinctRelation obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.AgitatorPrecinctRelations.Save(obj);
                return RedirectToAction("Show", new { Id = obj.AgitatorId });
            }

            ViewBag.Precincts = from h in dataManager.Precincts.GetAll()
                                select new SelectListItem
                                {
                                    Text = h.Name,
                                    Value = h.Id.ToString()
                                };
            return View(obj);
        }

        public ActionResult RemoveRelatedPrecinct(int relationId)
        {
            var rel = dataManager.AgitatorPrecinctRelations.Get(relationId);
            if (rel != null)
            {
                dataManager.AgitatorPrecinctRelations.Delete(rel.Id);
            }
            return RedirectToAction("Show", new { Id = rel.AgitatorId });
        }
    }
}
