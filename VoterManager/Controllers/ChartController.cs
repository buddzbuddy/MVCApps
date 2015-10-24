using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

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
            var itemHouses = dataManager.Houses.GetAll().Where(h => h.PrecinctId == Id);
            var voters = dataManager.Persons.GetAll().Where(p => itemHouses.Select(ih => ih.Id).Contains(p.HouseId ?? 0));

            foreach (var v in voters.GroupBy(v => v.PartyId))
            {
                var party = dataManager.Parties.GetAll().FirstOrDefault(p => p.Id == (v.Key ?? 0));
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

    }
}
