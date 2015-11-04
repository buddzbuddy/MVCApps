using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VoterManager.Models;

namespace CandidateManager.Controllers
{
    public class CandidateController : Controller
    {
        private DataManager dataManager;
        public CandidateController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(from v in dataManager.Candidates.GetAll()
                        select new CandidateViewModel
                        {
                            Candidate = v,
                            Person = dataManager.Persons.Get(v.PersonId ?? 0)
                        });
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Candidates.Get(Id);
            var person = dataManager.Persons.Get(obj.PersonId ?? 0);
            var model = new CandidateViewModel
            {
                Candidate = obj,
                Person = person,
                RelatedPrecincts = new List<CandidatePrecinctRelationViewModel>(from cp in dataManager.CandidatePrecinctRelations.GetAll()
                                                                                where cp.CandidateId == Id
                                                                                select new CandidatePrecinctRelationViewModel
                                                                                {
                                                                                    CandidatePrecinctRelation = cp,
                                                                                    Precinct = dataManager.Precincts.Get(cp.PrecinctId ?? 0)
                                                                                }),
                RelatedMunicipalities = new List<CandidateMunicipalityRelationViewModel>(from cp in dataManager.CandidateMunicipalityRelations.GetAll()
                                                                                         where cp.CandidateId == Id
                                                                                         select new CandidateMunicipalityRelationViewModel
                                                                                {
                                                                                    CandidateMunicipalityRelation = cp,
                                                                                    Municipality = dataManager.Municipalities.Get(cp.MunicipalityId ?? 0)
                                                                                })
            };
            return View(model);
        }

        [HttpGet]
        public ActionResult Create(int? personId)
        {
            if (!personId.HasValue)
                return RedirectToAction("CreateBase", "Person", new { returnUrl = Request.Url.ToString(), processName = "Создать кандидата" });
            return View(new Candidate { PersonId = personId });
        }

        [HttpPost]
        public ActionResult Create(Candidate obj)
        {
            if (ModelState.IsValid)
            {
                {
                    dataManager.Candidates.Save(obj);
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

        public ActionResult AddRelatedPrecinct(int candidateId)
        {
            ViewBag.Precincts = from h in dataManager.Precincts.GetAll()
                                select new SelectListItem
                                {
                                    Text = h.Name,
                                    Value = h.Id.ToString()
                                };
            return View(new CandidatePrecinctRelation { CandidateId = candidateId });
        }

        [HttpPost]
        public ActionResult AddRelatedPrecinct(CandidatePrecinctRelation obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.CandidatePrecinctRelations.Save(obj);
                return RedirectToAction("Show", new { Id = obj.CandidateId });
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
            var rel = dataManager.CandidatePrecinctRelations.Get(relationId);
            if (rel != null)
            {
                dataManager.CandidatePrecinctRelations.Delete(rel.Id);
            }
            return RedirectToAction("Show", new { Id = rel.CandidateId });
        }

        public ActionResult AddRelatedMunicipality(int candidateId)
        {
            ViewBag.Municipalities = from h in dataManager.Municipalities.GetAll()
                                     select new SelectListItem
                                     {
                                         Text = h.Name,
                                         Value = h.Id.ToString()
                                     };
            return View(new CandidateMunicipalityRelation { CandidateId = candidateId });
        }

        [HttpPost]
        public ActionResult AddRelatedMunicipality(CandidateMunicipalityRelation obj)
        {
            if (ModelState.IsValid)
            {
                dataManager.CandidateMunicipalityRelations.Save(obj);
                return RedirectToAction("Show", new { Id = obj.CandidateId });
            }

            ViewBag.Municipalities = from h in dataManager.Municipalities.GetAll()
                                     select new SelectListItem
                                     {
                                         Text = h.Name,
                                         Value = h.Id.ToString()
                                     };
            return View(obj);
        }

        public ActionResult RemoveRelatedMunicipality(int relationId)
        {
            var rel = dataManager.CandidateMunicipalityRelations.Get(relationId);
            if (rel != null)
            {
                dataManager.CandidateMunicipalityRelations.Delete(rel.Id);
            }
            return RedirectToAction("Show", new { Id = rel.CandidateId });
        }
    }
}
