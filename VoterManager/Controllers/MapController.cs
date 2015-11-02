using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoterManager.Models;

namespace VoterManager.Controllers
{
    public class MapController : Controller
    {
        private DataManager dataManager;
        public MapController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        //
        // GET: /Map/

        public ActionResult Index()
        {
            var precincts = dataManager.Precincts.GetAll().Where(p => p.Latitude.HasValue && p.Longitude.HasValue);

            return View();
        }


        private class CustomData
        {
            public double Longitude;
            public double Latitude;
            public string Name;
            public int Id;
            public string IconPath;
            public string[] Houses;
            public List<KeyValuePair<string, int>> Parties;
            public int VoterCount;
            public string Worker;
            public int WorkerId;
        }

        public JsonResult GetData()
        {
            var model = new List<CustomData>();

            var parties = dataManager.Parties.GetAll();
            var houses = dataManager.Houses.GetAll();
            var voterPartyRelations = dataManager.VoterPartyRelations.GetAll();
            //var persons = dataManager.Persons.GetAll();
            var voters = from v in dataManager.Voters.GetAll()
                         select new VoterViewModel
                         {
                             Person = dataManager.Persons.Get(v.PersonId ?? 0),
                             PoliticalViews = (from vp in voterPartyRelations
                                               where vp.VoterId == v.Id
                                               select new VoterPartyRelationViewModel
                                               {
                                                   VoterPartyRelation = vp,
                                                   Voter = v,
                                                   Party = dataManager.Parties.Get(vp.PartyId ?? 0)
                                               }).ToList()
                         };
            var precincts = dataManager.Precincts.GetAll().Where(p => p.Latitude.HasValue && p.Longitude.HasValue);

            foreach(var precinct in precincts)
            {
                var itemHouses = houses.Where(h => h.PrecinctId == precinct.Id);
                var precinctVoters = voters.Where(v => itemHouses.Select(ih => ih.Id).Contains(v.Person.HouseId ?? 0));
                var worker = dataManager.Workers.Get(precinct.WorkerId ?? 0);//voters.FirstOrDefault(p => p.Person.Id == (precinct.WorkerId ?? 0));

                var item = new CustomData();
                
                item.Longitude = precinct.Longitude.Value;
                item.Latitude = precinct.Latitude.Value;

                item.Name = precinct.Name;

                item.Id = precinct.Id;

                item.IconPath = precinct.IconPath;

                item.Houses = itemHouses.Select(h => h.Name).ToArray();

                item.Parties = new List<KeyValuePair<string, int>>();

                foreach(var v in precinctVoters.SelectMany(x => x.PoliticalViews).GroupBy(v => v.Party != null ? v.Party.Id : 0))
                {
                    var party = parties.FirstOrDefault(p => p.Id == v.Key);
                    item.Parties.Add(new KeyValuePair<string, int>(party != null ? party.Name : "не указано", v.Count()));
                }

                item.VoterCount = precinctVoters != null ? precinctVoters.Count() : 0; //item.Parties.Sum(x => x.Value);

                item.Worker = worker != null ? dataManager.Persons.Get(worker.PersonId ?? 0).FullName : "Не указано";
                item.WorkerId = worker != null ? worker.Id : 0;
                model.Add(item);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
