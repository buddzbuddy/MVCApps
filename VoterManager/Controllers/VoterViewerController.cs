﻿using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoterManager.Models;

namespace VoterManager.Controllers
{
    [Authorize]
    public class VoterViewerController : Controller
    {
        private DataManager dataManager;
        public VoterViewerController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Search()
        {
            var districts = new List<SelectListItem> { new SelectListItem() };
            districts.AddRange(from d in dataManager.Districts.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString()
                                });
            ViewBag.Districts = districts;
            var parties = new List<SelectListItem> { new SelectListItem() };
            parties.AddRange(from p in dataManager.Parties.GetAll()
                             select new SelectListItem
                             {
                                 Text = p.Name,
                                 Value = p.Id.ToString()
                             });
            ViewBag.Parties = parties;
            var precincts = new List<SelectListItem> { new SelectListItem() };
            precincts.AddRange(from p in dataManager.Precincts.GetAll().OrderBy(x => x.Name)
                               select new SelectListItem
                               {
                                   Text = p.Name,
                                   Value  = p.Id.ToString()
                               });
            ViewBag.Precincts = precincts;
            return View(new SearchFormViewModel());
        }

        [HttpPost]
        public ActionResult Search(SearchFormViewModel obj)
        {
            var ageFilteredValues = GetAgeFilteredValuesByExpression(obj.AgeExpression, obj.Age);
            var housesByPrecinct = obj.PrecinctId.HasValue ? GetHousesByPrecinctId(obj.PrecinctId.Value) : new List<int>();
            var voterPartyRelations = dataManager.VoterPartyRelations.GetAll().Where(x => x.PartyId == obj.PartyId);
            var result = from v in dataManager.Voters.GetAll().Select(x => new VoterViewModel
            {
                Voter = x,
                Person = dataManager.Persons.Get(x.PersonId ?? 0)
            })
                         where (v.Person.LastName ?? "").ToLowerInvariant().StartsWith((obj.LastName ?? "").ToLowerInvariant())
                         && (v.Person.FirstName ?? "").ToLowerInvariant().StartsWith((obj.FirstName ?? "").ToLowerInvariant())
                         && (v.Person.MiddleName ?? "").ToLowerInvariant().StartsWith((obj.MiddleName ?? "").ToLowerInvariant())
                         && (obj.GaveBiometricData == SearchFormViewModel.GaveBiometricDataType.yes ? v.Voter.GaveBiometricData : (obj.GaveBiometricData == SearchFormViewModel.GaveBiometricDataType.no ? !v.Voter.GaveBiometricData : true))
                             //filtering by living address
                         && (obj.DistrictId.HasValue ? v.Person.DistrictId == obj.DistrictId : true)
                         && (obj.LocalityId.HasValue ? obj.LocalityId.Value == v.Person.LocalityId : true)
                         && (obj.StreetId.HasValue ? obj.StreetId.Value == v.Person.StreetId : true)
                         && (obj.HouseId.HasValue ? obj.HouseId.Value == v.Person.HouseId : true)
                             //filtering by party
                         && (obj.PartyId.HasValue ? voterPartyRelations.Select(x => x.VoterId).Contains(v.Voter != null ? v.Voter.Id : 0) : true)
                             //filtering by age
                         && (obj.Age.HasValue ? (ageFilteredValues.Contains(v.Person.Years ?? 0)) : true)
                             //filtering by precinct point in house
                         && (obj.PrecinctId.HasValue ? (housesByPrecinct.Contains(v.Person.HouseId ?? 0)) : true)
                         select v;
            var districts = new List<SelectListItem> { new SelectListItem() };
            districts.AddRange(from d in dataManager.Districts.GetAll()
                               select new SelectListItem
                               {
                                   Text = d.Name,
                                   Value = d.Id.ToString()
                               });
            ViewBag.Districts = districts;

            ViewBag.Locality = dataManager.Localities.Get(obj.LocalityId ?? 0);

            ViewBag.Street = dataManager.Streets.Get(obj.StreetId ?? 0);

            ViewBag.House = dataManager.Houses.Get(obj.HouseId ?? 0);

            var parties = new List<SelectListItem> { new SelectListItem() };
            parties.AddRange(from p in dataManager.Parties.GetAll()
                             select new SelectListItem
                             {
                                 Text = p.Name,
                                 Value = p.Id.ToString()
                             });
            ViewBag.Parties = parties;

            var precincts = new List<SelectListItem> { new SelectListItem() };
            precincts.AddRange(from p in dataManager.Precincts.GetAll().OrderBy(x => x.Name)
                               select new SelectListItem
                               {
                                   Text = p.Name,
                                   Value = p.Id.ToString()
                               });
            ViewBag.Precincts = precincts;

            obj.SearchResult = result.ToList();
            return View(obj);
        }

        private IEnumerable<int> GetAgeFilteredValuesByExpression(SearchFormViewModel.AgeExpressionType aExpType, int? age)
        {
            var ageList = new List<int>();
            for(int i = 1; i <= 150; i++)
            {
                ageList.Add(i);
            }
            if (aExpType == SearchFormViewModel.AgeExpressionType.Equal) return ageList.Where(a => a == age);
            if (aExpType == SearchFormViewModel.AgeExpressionType.GreatEqual) return ageList.Where(a => a >= age);
            if (aExpType == SearchFormViewModel.AgeExpressionType.GreatThen) return ageList.Where(a => a > age);
            if (aExpType == SearchFormViewModel.AgeExpressionType.LessEqual) return ageList.Where(a => a <= age);
            if (aExpType == SearchFormViewModel.AgeExpressionType.LessThen) return ageList.Where(a => a < age);
            return ageList;
        }
        public string GetNameByEntityId(int Id, VoterManagerEntityTypes eType)
        {
            if (eType == VoterManagerEntityTypes.District)
                return dataManager.Districts.Get(Id).Name;
            if (eType == VoterManagerEntityTypes.Locality)
                return dataManager.Localities.Get(Id).Name;
            if (eType == VoterManagerEntityTypes.Street)
                return dataManager.Streets.Get(Id).Name;
            if (eType == VoterManagerEntityTypes.House)
                return dataManager.Houses.Get(Id).Name;
            if (eType == VoterManagerEntityTypes.Party)
                return string.Join(",", dataManager.VoterPartyRelations.GetAll().Where(x => x.VoterId == Id && x.PartyId.HasValue).Select(x => dataManager.Parties.Get(x.PartyId.Value).Name));
            if (eType == VoterManagerEntityTypes.Precinct)
                return dataManager.Precincts.Get(Id).Name;
            return "";
        }

        private IEnumerable<int> GetHousesByPrecinctId(int precinctId)
        {
            return from h in dataManager.Houses.GetAll()
                   where h.PrecinctId == precinctId
                   select h.Id;
        }
    }
}
