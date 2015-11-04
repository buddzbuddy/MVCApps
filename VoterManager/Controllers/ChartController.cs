using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using VoterManager.Models;

namespace VoterManager.Controllers
{
    public class ChartController : Controller
    {
        //
        // GET: /Chart/
        private DataManager dataManager;
        public ChartController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChartSaveToCache(string key)
        {
            return View();
        }
        public ActionResult ClearCache()
        {
            return View();
        }

        public ActionResult Precinct(int Id)
        {
            var parties = new List<KeyValuePair<string, int>>();
            var itemHouses = dataManager.Houses.GetAll().Where(h => h.PrecinctId == Id).Select(x => x.Id);
            var voterPartyRelations = dataManager.VoterPartyRelations.GetAll();
            var voters = from v in dataManager.Voters.GetAll()
                         where itemHouses.Contains(dataManager.Persons.Get(v.PersonId ?? 0).HouseId ?? 0)
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

            foreach (var v in voters.SelectMany(x => x.PoliticalViews).GroupBy(v => v.Party != null ? v.Party.Id : 0))
            {
                var party = dataManager.Parties.Get(v.Key);
                parties.Add(new KeyValuePair<string, int>(party != null ? party.Name : "не указано", v.Count()));
            }

            var myChart = new Chart(width: 160, height: 140)
            .AddSeries(
                name: "Employee",
                xValue: parties.Select(p => p.Key).ToArray(),
                yValues: parties.Select(p => p.Value).ToArray(),
                chartType: "Pie");
            //return File(myChart.Write().GetBytes(), "image/jpeg");
            //Image image = Image.FromFile(Path.Combine(Server.MapPath("/Images/Resources"), "img1.png"));
            //var str = new FileStream(Path.Combine(Server.MapPath("/Images/Resources"), "img1.png"), FileMode.Create);
            //str.Write(myChart.GetBytes(), 0, myChart.GetBytes().Length);
            //Image image2 = Image.FromStream(str);

            //using (Graphics g = Graphics.FromImage(image2))
            //{
            //    // do something with the Graphics (eg. write "Hello World!")
            //    string text = "Hello World!";

            //    // Create font and brush.
            //    Font drawFont = new Font("Arial", 10);
            //    SolidBrush drawBrush = new SolidBrush(Color.Black);

            //    // Create point for upper-left corner of drawing.
            //    PointF stringPoint = new PointF(0, 0);

            //    g.DrawString(text, drawFont, drawBrush, stringPoint);
            //}

            //MemoryStream ms = new MemoryStream();

            //image2.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);



            return File(myChart.GetBytes(), "image/jpeg");
        }

        public ActionResult House(int Id)
        {
            var politicalViews = new List<KeyValuePair<string, int>>();
            var hPersons = dataManager.Persons.GetAll().Where(x => x.HouseId == Id);
            var hVoters = dataManager.Voters.GetAll().Where(x => hPersons.Select(x2 => x2.Id).Contains(x.PersonId ?? 0));
            var hVoterPartyRelations = dataManager.VoterPartyRelations.GetAll().Where(x => hVoters.Select(x2 => x2.Id).Contains(x.VoterId ?? 0));
            var hParties = dataManager.Parties.GetAll().Where(x => hVoterPartyRelations.Select(x2 => x2.PartyId).Contains(x.Id));
            foreach (var hParty in hParties.GroupBy(x => x.Id).Select(x => x.First()))
            {
                politicalViews.Add(new KeyValuePair<string, int>(hParty.Name, hVoterPartyRelations.Where(x => x.PartyId == hParty.Id).Count()));
            }

            var chart = new Chart(width: 160, height: 140)
            .AddSeries(
                name: "Employee",
                xValue: politicalViews.Select(p => p.Key).ToArray(),
                yValues: politicalViews.Select(p => p.Value).ToArray(),
                chartType: "Pie");
            return File(chart.GetBytes(), "image/jpeg");
        }
        public ActionResult Worker(int Id)
        {
            var politicalViews = new List<KeyValuePair<string, int>>();
            var workerHouses = dataManager.WorkerHouseRelations.GetAll().Where(wh => wh.WorkerId == Id && wh.HouseId.HasValue).Select(wh => wh.HouseId.Value).ToList();

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
                
                foreach (var hParty in hParties.GroupBy(x => x.Id).Select(x => x.First()))
                {
                    politicalViews.Add(new KeyValuePair<string, int>(hParty.Name, hVoterPartyRelations.Where(x => x.PartyId == hParty.Id).Count()));
                }
            }
            var chart = new Chart(width: 200, height: 200)
            .AddSeries(
                name: "Employee",
                xValue: politicalViews.Select(p => p.Key).ToArray(),
                yValues: politicalViews.Select(p => p.Value).ToArray(),
                chartType: "Pie");
            return File(chart.GetBytes(), "image/jpeg");
        }

    }
}
