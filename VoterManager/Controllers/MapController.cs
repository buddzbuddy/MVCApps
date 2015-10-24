using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        }

        public JsonResult GetData()
        {
            var model = new List<CustomData>();

            var parties = dataManager.Parties.GetAll();
            var houses = dataManager.Houses.GetAll();
            var persons = dataManager.Persons.GetAll();
            var precincts = dataManager.Precincts.GetAll().Where(p => p.Latitude.HasValue && p.Longitude.HasValue);

            foreach(var precinct in precincts)
            {
                var itemHouses = houses.Where(h => h.PrecinctId == precinct.Id);
                var voters = persons.Where(p => itemHouses.Select(ih => ih.Id).Contains(p.HouseId ?? 0));
                var worker = persons.FirstOrDefault(p => p.Id == (precinct.WorkerId ?? 0));

                var item = new CustomData();
                
                item.Longitude = precinct.Longitude.Value;
                item.Latitude = precinct.Latitude.Value;

                item.Name = precinct.Name;

                item.Id = precinct.Id;

                item.IconPath = precinct.IconPath;

                item.Houses = itemHouses.Select(h => h.Name).ToArray();

                item.Parties = new List<KeyValuePair<string, int>>();

                foreach(var v in voters.GroupBy(v => v.PartyId))
                {
                    var party = parties.FirstOrDefault(p => p.Id == (v.Key ?? 0));
                    item.Parties.Add(new KeyValuePair<string, int>(party != null ? party.Name : "не указано", v.Count()));
                }

                item.VoterCount = item.Parties.Sum(x => x.Value);

                item.Worker = worker != null ? worker.FullName : "Не указано";

                model.Add(item);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}
