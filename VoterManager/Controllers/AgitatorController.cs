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

        public ActionResult ViewInMap(int Id)
        {
            var agitatorHouses = dataManager.AgitatorHouseRelations.GetAll().Where(wh => wh.AgitatorId == Id && wh.HouseId.HasValue).Select(wh => wh.HouseId.Value).ToList();

            var houses = dataManager.Houses.GetAll().Where(h => agitatorHouses.Contains(h.Id) && h.Latitude.HasValue && h.Longitude.HasValue).ToList();
            var persons = dataManager.Persons.GetAll().Where(p => houses.Select(x => x.Id).Contains(p.HouseId ?? 0)).ToList();
            var voters = dataManager.Voters.GetAll().Where(v => persons.Select(x => x.Id).Contains(v.PersonId ?? 0)).ToList();
            var parties = dataManager.VoterPartyRelations.GetAll().Where(vp => voters.Select(x => x.Id).Contains(vp.VoterId ?? 0))
                .Select(x => dataManager.Parties.Get(x.PartyId ?? 0)).ToList();
            ViewBag.HouseCount = agitatorHouses.Count;
            ViewBag.VoterCount = voters.Count;
            ViewBag.PolitViewCount = parties.Count;
            return View(new AgitatorViewModel
            {
                Agitator = dataManager.Agitators.Get(Id)
            });
        }
        private class MapData
        {
            public double Longitude;
            public double Latitude;
            public string Name;
            public int Id;
            public string IconPath;
            public string[] Houses;
            public List<KeyValuePair<string, int>> Parties;
            public int VoterCount;
        }

        public JsonResult GetData(int Id)
        {
            var agitatorHouses = dataManager.AgitatorHouseRelations.GetAll().Where(ah => ah.AgitatorId == Id && ah.HouseId.HasValue).Select(wh => wh.HouseId.Value).ToList();

            var model = new List<MapData>();

            var houses = dataManager.Houses.GetAll().Where(h => agitatorHouses.Contains(h.Id) && h.Latitude.HasValue && h.Longitude.HasValue).ToList();
            var persons = dataManager.Persons.GetAll().Where(p => houses.Select(x => x.Id).Contains(p.HouseId ?? 0)).ToList();
            var voters = dataManager.Voters.GetAll().Where(v => persons.Select(x => x.Id).Contains(v.PersonId ?? 0)).ToList();
            var voterPartyRelations = dataManager.VoterPartyRelations.GetAll().ToList();
            var parties = voterPartyRelations.Where(vp => voters.Select(x => x.Id).Contains(vp.VoterId ?? 0))
                .Select(x => dataManager.Parties.Get(x.PartyId ?? 0)).ToList();

            foreach (var house in houses)
            {
                var hPersons = persons.Where(x => x.HouseId == house.Id).ToList();
                var hVoters = voters.Where(x => hPersons.Select(x2 => x2.Id).Contains(x.PersonId ?? 0)).ToList();
                var hVoterPartyRelations = voterPartyRelations.Where(x => hVoters.Select(x2 => x2.Id).Contains(x.VoterId ?? 0)).ToList();
                var hParties = parties.Where(x => hVoterPartyRelations.Select(x2 => x2.PartyId).Contains(x.Id)).ToList();
                var item = new MapData();

                item.Longitude = house.Longitude.Value;
                item.Latitude = house.Latitude.Value;

                item.Name = house.Name;

                item.Id = house.Id;

                item.IconPath = house.IconPath;

                item.Parties = new List<KeyValuePair<string, int>>();

                foreach (var hParty in hParties.GroupBy(x => x.Id).Select(x => x.First()))
                {
                    item.Parties.Add(new KeyValuePair<string, int>(hParty.Name, hVoterPartyRelations.Where(x => x.PartyId == hParty.Id).Count()));
                }

                item.VoterCount = hVoters != null ? hVoters.Count() : 0; //item.Parties.Sum(x => x.Value);

                model.Add(item);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewInMap2(int Id)
        {
            var agitatorPrecincts = dataManager.AgitatorPrecinctRelations.GetAll().Where(ap => ap.AgitatorId == Id && ap.PrecinctId.HasValue).Select(ap => ap.PrecinctId.Value).ToList();
            //var agitatorHouses = dataManager.AgitatorHouseRelations.GetAll().Where(wh => wh.AgitatorId == Id && wh.HouseId.HasValue).Select(wh => wh.HouseId.Value).ToList();

            var agitatorHouses = dataManager.Houses.GetAll()
                .Where(ah => ah.PrecinctId.HasValue && agitatorPrecincts.Contains(ah.PrecinctId.Value))
                .Select(ah => ah.Id).ToList();

            var houses = dataManager.Houses.GetAll().Where(h => agitatorHouses.Contains(h.Id) && h.Latitude.HasValue && h.Longitude.HasValue).ToList();
            var persons = dataManager.Persons.GetAll().Where(p => houses.Select(x => x.Id).Contains(p.HouseId ?? 0)).ToList();
            var voters = dataManager.Voters.GetAll().Where(v => persons.Select(x => x.Id).Contains(v.PersonId ?? 0)).ToList();
            var parties = dataManager.VoterPartyRelations.GetAll().Where(vp => voters.Select(x => x.Id).Contains(vp.VoterId ?? 0))
                .Select(x => dataManager.Parties.Get(x.PartyId ?? 0)).ToList();
            ViewBag.HouseCount = agitatorHouses.Count;
            ViewBag.VoterCount = voters.Count;
            ViewBag.PolitViewCount = parties.Count;
            return View(new AgitatorViewModel
            {
                Agitator = dataManager.Agitators.Get(Id)
            });
        }

        public JsonResult GetData2(int Id)
        {
            var agitatorPrecincts = dataManager.AgitatorPrecinctRelations.GetAll().Where(ap => ap.AgitatorId == Id && ap.PrecinctId.HasValue).Select(ap => ap.PrecinctId.Value).ToList();
            //var agitatorHouses = dataManager.AgitatorHouseRelations.GetAll().Where(ah => ah.AgitatorId == Id && ah.HouseId.HasValue).Select(wh => wh.HouseId.Value).ToList();
            var agitatorHouses = dataManager.Houses.GetAll()
                .Where(ah => ah.PrecinctId.HasValue && agitatorPrecincts.Contains(ah.PrecinctId.Value))
                .Select(ah => ah.Id).ToList();
            var model = new List<MapData>();

            var houses = dataManager.Houses.GetAll().Where(h => agitatorHouses.Contains(h.Id) && h.Latitude.HasValue && h.Longitude.HasValue).ToList();
            var persons = dataManager.Persons.GetAll().Where(p => houses.Select(x => x.Id).Contains(p.HouseId ?? 0)).ToList();
            var voters = dataManager.Voters.GetAll().Where(v => persons.Select(x => x.Id).Contains(v.PersonId ?? 0)).ToList();
            var voterPartyRelations = dataManager.VoterPartyRelations.GetAll().ToList();
            var parties = voterPartyRelations.Where(vp => voters.Select(x => x.Id).Contains(vp.VoterId ?? 0))
                .Select(x => dataManager.Parties.Get(x.PartyId ?? 0)).ToList();

            foreach (var precinct in agitatorPrecincts)
            {
                var obj = dataManager.Precincts.Get(precinct);
                var pHouses = houses.Where(h => h.PrecinctId == precinct).Select(h => h.Id).ToList();
                var hPersons = persons.Where(x => pHouses.Contains(x.HouseId ?? 0)).ToList();
                var hVoters = voters.Where(x => hPersons.Select(x2 => x2.Id).Contains(x.PersonId ?? 0)).ToList();
                var hVoterPartyRelations = voterPartyRelations.Where(x => hVoters.Select(x2 => x2.Id).Contains(x.VoterId ?? 0)).ToList();
                var hParties = parties.Where(x => hVoterPartyRelations.Select(x2 => x2.PartyId).Contains(x.Id)).ToList();
                var item = new MapData();

                item.Longitude = obj.Longitude.Value;
                item.Latitude = obj.Latitude.Value;

                item.Name = obj.Name;

                item.Id = obj.Id;

                item.IconPath = obj.IconPath;

                item.Parties = new List<KeyValuePair<string, int>>();

                foreach (var hParty in hParties.GroupBy(x => x.Id).Select(x => x.First()))
                {
                    item.Parties.Add(new KeyValuePair<string, int>(hParty.Name, hVoterPartyRelations.Where(x => x.PartyId == hParty.Id).Count()));
                }

                item.VoterCount = hVoters != null ? hVoters.Count() : 0; //item.Parties.Sum(x => x.Value);

                model.Add(item);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
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
