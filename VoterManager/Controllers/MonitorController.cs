using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VoterManager.Controllers
{
    public class MonitorController : Controller
    {
        private DataManager dataManager;
        public MonitorController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowCandidates()
        {
            return View();
        }

        public ActionResult Map()
        {
            return View();
        }
        
        public JsonResult Precincts()
        {
            var precincts = dataManager.Precincts.GetAll().Where(p => p.Latitude.HasValue && p.Longitude.HasValue).ToList();

            return Json(precincts, JsonRequestBehavior.AllowGet);
        }

        class PrecinctCandidate
        {
            public Candidate Candidate { get; set; }
            public Precinct Precinct { get; set; }
            public string CandidateName { get; set; }
        }
        public JsonResult PrecinctsByCandidates()
        {
            var model = new List<PrecinctCandidate>();
            var precincts = dataManager.Precincts.GetAll().Where(p => p.Latitude.HasValue && p.Longitude.HasValue).ToList();

            foreach (var precinct in precincts)
            {
                var item = new PrecinctCandidate { Precinct = precinct };
                var candidatePrecinctRelation = dataManager.CandidatePrecinctRelations.GetAll().FirstOrDefault(x => x.PrecinctId == precinct.Id);
                if (candidatePrecinctRelation != null && candidatePrecinctRelation.CandidateId.HasValue)
                {
                    item.Candidate = dataManager.Candidates.Get(candidatePrecinctRelation.CandidateId.Value);
                    item.CandidateName = dataManager.Persons.Get(item.Candidate.PersonId ?? 0).FullName;
                }
                model.Add(item);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public JsonResult CreateMarker(string lat, string lng)
        {
            var latlng = new LatLng
            {
                Latitude = lat,
                Longitude = lng
            };
            dataManager.GEO.LatLngSave(latlng);
            var marker = new Marker
            {
                LatLngId = latlng.Id
            };
            dataManager.GEO.MarkerSave(marker);

            return Json(marker, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EditMarker(int Id, string lat, string lng)
        {
            var status = 0;
            var marker = dataManager.GEO.MarkerGet(Id);
            if(marker != null)
            {
                var latlng = dataManager.GEO.LatLngGet(marker.LatLngId ?? 0);
                if (latlng != null)
                {
                    latlng.Latitude = lat;
                    latlng.Longitude = lng;
                    dataManager.GEO.LatLngSave(latlng);
                    status = 1;
                }
            }
            return Json(status, JsonRequestBehavior.AllowGet);
        }


        class MarkerView
        {
            public string lat;
            public string lng;
            public int Id;
        }

        public JsonResult Markers()
        {
            return Json(from m in dataManager.GEO.MarkerGetAll()
                        select new MarkerView
                        {
                            Id = m.Id,
                            lat = dataManager.GEO.LatLngGet(m.LatLngId ?? 0).Latitude,
                            lng = dataManager.GEO.LatLngGet(m.LatLngId ?? 0).Longitude
                        }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreatePolygon(string[] lats, string[] lngs)
        {
            var status = 0;
            if (lats.Length > 0 && lngs.Length > 0 && lats.Length == lngs.Length)
            {
                var polygon = new Polygon { Name = "Polygon created:" + DateTime.Now.ToShortTimeString() };
                dataManager.GEO.PolygonSave(polygon);
                for (int i = 0; i < lats.Length; i++)
                {
                    var latlng = new LatLng
                    {
                        Latitude = lats[i],
                        Longitude = lngs[i]
                    };
                    dataManager.GEO.LatLngSave(latlng);
                    var plRelation = new PolygonLatLngRelation
                    {
                        PolygonId = polygon.Id,
                        LatLngId = latlng.Id
                    };
                    dataManager.GEO.PolygonLatLngRelationSave(plRelation);
                }
                return Json(polygon, JsonRequestBehavior.DenyGet);
            }
            return Json(status, JsonRequestBehavior.DenyGet);
        }

        [HttpPost]
        public JsonResult EditPolygon(int Id, string[] lats, string[] lngs)
        {
            string status = "";
            try
            {
                if (lats.Length > 0 && lngs.Length > 0 && lats.Length == lngs.Length)
                {
                    var polygon = dataManager.GEO.PolygonGet(Id);
                    RemoveLatLngsByPolygonId(polygon.Id);
                    for (int i = 0; i < lats.Length; i++)
                    {
                        var latlng = new LatLng
                        {
                            Latitude = lats[i],
                            Longitude = lngs[i]
                        };
                        dataManager.GEO.LatLngSave(latlng);
                        var plRelation = new PolygonLatLngRelation
                        {
                            PolygonId = polygon.Id,
                            LatLngId = latlng.Id
                        };
                        dataManager.GEO.PolygonLatLngRelationSave(plRelation);
                        status = "1";
                    }
                }
            }
            catch (Exception e)
            {
                status = e.TargetSite.Name + ": " + e.Message;
            }
            return Json(status, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult DeletePolygon(int Id)
        {
            string status = "";
            try
            {
                var polygon = dataManager.GEO.PolygonGet(Id);
                RemoveLatLngsByPolygonId(polygon.Id);
                dataManager.GEO.PolygonDelete(Id);
                status = "1";
            }
            catch (Exception e)
            {
                status = e.TargetSite.Name + ": " + e.Message;
            }
            return Json(status, JsonRequestBehavior.DenyGet);
        }


        [HttpPost]
        public JsonResult DeleteMarker(int Id)
        {
            string status = "";
            try
            {
                dataManager.GEO.MarkerDelete(Id);
                status = "1";
            }
            catch (Exception e)
            {
                status = e.TargetSite.Name + ": " + e.Message;
            }
            return Json(status, JsonRequestBehavior.DenyGet);
        }

        void RemoveLatLngsByPolygonId(int polygonId)
        {
            var plRels = (from pl in dataManager.GEO.PolygonLatLngRelationGetAll()
                         where pl.PolygonId == polygonId
                         select pl).ToList();
            foreach (var pl in plRels)
            {
                dataManager.GEO.PolygonLatLngRelationDelete(pl.Id);
                if (pl.LatLngId.HasValue)
                {
                    dataManager.GEO.LatLngDelete(pl.LatLngId.Value);
                }
            }
        }

        class PolygonView
        {
            public int Id;
            public string Name;
            public Array[] LatLngs;
        }
        public JsonResult Polygons()
        {
            return Json(from p in dataManager.GEO.PolygonGetAll()
                        select new PolygonView
                        {
                            Id = p.Id,
                            Name = p.Name,
                            LatLngs = (from pl in dataManager.GEO.PolygonLatLngRelationGetAll()
                                       where pl.PolygonId == p.Id
                                       select new[] { dataManager.GEO.LatLngGet(pl.LatLngId ?? 0).Latitude, dataManager.GEO.LatLngGet(pl.LatLngId ?? 0).Longitude })
                                       .ToArray()
                        }, JsonRequestBehavior.AllowGet);
        }
    }
}