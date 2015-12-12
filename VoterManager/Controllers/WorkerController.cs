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
    public class WorkerController : Controller
    {
        private DataManager dataManager;
        public WorkerController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {

            return View(dataManager.Workers.GetAll());
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Workers.Get(Id);
            var workerHouseRelations = from wh in dataManager.WorkerHouseRelations.GetAll()
                                       where wh.WorkerId == Id
                                       select wh;
            var model = new WorkerViewModel
            {
                Worker = obj,
                Person = dataManager.Persons.Get(obj.PersonId ?? 0),
                RelatedHouses = new List<WorkerHouseRelationViewModel>(from wh in workerHouseRelations
                                                                       select new WorkerHouseRelationViewModel
                                                                       {
                                                                           WorkerHouseRelation = wh,
                                                                           House = dataManager.Houses.Get(wh.HouseId ?? 0)
                                                                       })
            };
            return View(model);
        }

        public ActionResult ViewInMap_old(int Id)
        {
            var workerHouses = dataManager.WorkerHouseRelations.GetAll().Where(wh => wh.WorkerId == Id && wh.HouseId.HasValue).Select(wh => wh.HouseId.Value).ToList();

            var houses = dataManager.Houses.GetAll().Where(h => workerHouses.Contains(h.Id) && h.Latitude.HasValue && h.Longitude.HasValue).ToList();
            var persons = dataManager.Persons.GetAll().Where(p => houses.Select(x => x.Id).Contains(p.HouseId ?? 0)).ToList();
            var voters = dataManager.Voters.GetAll().Where(v => persons.Select(x => x.Id).Contains(v.PersonId ?? 0)).ToList();
            var parties = dataManager.VoterPartyRelations.GetAll().Where(vp => voters.Select(x => x.Id).Contains(vp.VoterId ?? 0))
                .Select(x => dataManager.Parties.Get(x.PartyId ?? 0)).ToList();
            ViewBag.HouseCount = workerHouses.Count;
            ViewBag.VoterCount = voters.Count;
            ViewBag.PolitViewCount = parties.Count;
            return View(new WorkerViewModel
            {
                Worker = dataManager.Workers.Get(Id)
            });
        }
        public ActionResult ViewInMap(int Id)
        {
            var workerHouses = dataManager.WorkerHouseRelations.GetAll().Where(wh => wh.WorkerId == Id && wh.HouseId.HasValue).Select(wh => wh.HouseId.Value).ToList();

            var houses = dataManager.Houses.GetAll().Where(h => workerHouses.Contains(h.Id) && h.Latitude.HasValue && h.Longitude.HasValue).ToList();
            var persons = dataManager.Persons.GetAll().Where(p => houses.Select(x => x.Id).Contains(p.HouseId ?? 0)).ToList();
            var voters = dataManager.Voters.GetAll().Where(v => persons.Select(x => x.Id).Contains(v.PersonId ?? 0)).ToList();
            var parties = dataManager.VoterPartyRelations.GetAll().Where(vp => voters.Select(x => x.Id).Contains(vp.VoterId ?? 0))
                .Select(x => dataManager.Parties.Get(x.PartyId ?? 0)).ToList();
            ViewBag.HouseCount = workerHouses.Count;
            ViewBag.VoterCount = voters.Count;
            ViewBag.PolitViewCount = parties.Count;
            return View(new WorkerViewModel
            {
                Worker = dataManager.Workers.Get(Id)
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
            var workerHouses = dataManager.WorkerHouseRelations.GetAll().Where(wh => wh.WorkerId == Id && wh.HouseId.HasValue).Select(wh => wh.HouseId.Value).ToList();
            
            var model = new List<MapData>();

            var houses = dataManager.Houses.GetAll().Where(h => workerHouses.Contains(h.Id) && h.Latitude.HasValue && h.Longitude.HasValue).ToList();
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

        [HttpGet]
        public ActionResult Create(int? personId)
        {
            if (!personId.HasValue)
                return RedirectToAction("CreateBase", "Person", new { returnUrl = Request.Url.ToString(), processName = "Создать работника штаба" });
            return View(new Worker { PersonId = personId });
        }

        [HttpPost]
        public ActionResult Create(Worker obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.Workers.Save(obj);
                return RedirectToAction("Show", new { Id = obj.Id });
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = dataManager.Workers.Get(id);
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.FullName,
                                  Value = p.Id.ToString(),
                                  Selected = p.Id == obj.PersonId
                              };
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Worker obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.Workers.Save(obj);
                return RedirectToAction("Show", new { Id = obj.Id });
            }
            ViewBag.Persons = from p in dataManager.Persons.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.FullName,
                                  Value = p.Id.ToString(),
                                  Selected = p.Id == obj.PersonId
                              };
            //ModelState.AddModelError("PersonId", "Работник уже существует!");
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Workers.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Workers.Delete(Id);
            return RedirectToAction("Index");
        }

        public ActionResult AddRelatedHouse(int workerId)
        {
            ViewBag.Houses = from h in dataManager.Houses.GetAll()
                             select new SelectListItem
                             {
                                 Text = h.Name,
                                 Value = h.Id.ToString()
                             };
            return View(new WorkerHouseRelation { WorkerId = workerId });
        }

        [HttpPost]
        public ActionResult AddRelatedHouse(WorkerHouseRelation obj)
        {
            if(ModelState.IsValid)
            {
                dataManager.WorkerHouseRelations.Save(obj);
                return RedirectToAction("Show", new { Id = obj.WorkerId });
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
            var rel = dataManager.WorkerHouseRelations.Get(relationId);
            if (rel != null)
            {
                dataManager.WorkerHouseRelations.Delete(rel.Id);
            }
            return RedirectToAction("Show", new { Id = rel.WorkerId });
        }
    }
}
