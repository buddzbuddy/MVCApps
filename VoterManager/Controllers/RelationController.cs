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
    public class RelationController : Controller
    {
        private DataManager dataManager;
        public RelationController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            ViewBag.MunicipalityHouseRelations = from o in dataManager.MunicipalityHouseRelations.GetAll()
                        select new MunicipalityHouseRelationViewModel
                        {
                            MunicipalityHouseRelation = o,
                            Municipality = dataManager.Municipalities.Get(o.MunicipalityId.HasValue ? o.MunicipalityId.Value : 0),
                            House = dataManager.Houses.Get(o.HouseId.HasValue ? o.HouseId.Value : 0)
                        };
            ViewBag.PersonPartyRelations = from pp in dataManager.VoterPartyRelations.GetAll()
                                           select new VoterPartyRelationViewModel
                                           {
                                               VoterPartyRelation = pp,
                                               Voter = dataManager.Voters.Get(pp.VoterId ?? 0),
                                               Party = dataManager.Parties.Get(pp.PartyId ?? 0)
                                           };
            return View();
        }

        [HttpGet]
        public ActionResult AddHouseToMunicipality(int? houseId, int? municipalityId)
        {
            var streets = dataManager.Streets.GetAll();
            ViewBag.Houses = from h in dataManager.Houses.GetAll()
                             select new SelectListItem
                             {
                                 Text = streets.First(x => x.Id == h.StreetId).Name + "/" + h.Name,
                                 Value = h.Id.ToString(),
                                 Selected = houseId.HasValue ? houseId.Value == h.Id : false
                             };
            ViewBag.Municipalities = from m in dataManager.Municipalities.GetAll()
                             select new SelectListItem
                             {
                                 Text = m.Name,
                                 Value = m.Id.ToString(),
                                 Selected = municipalityId.HasValue ? municipalityId.Value == m.Id : false
                             };
            return View();
        }

        [HttpPost]
        public ActionResult AddHouseToMunicipality(MunicipalityHouseRelation obj)
        {
            if(ModelState.IsValid)
            {
                dataManager.MunicipalityHouseRelations.Save(obj);
                return RedirectToAction("Index");
            }

            return View(obj);
        }
    }
}
