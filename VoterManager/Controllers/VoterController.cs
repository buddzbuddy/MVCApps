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
    public class VoterController : Controller
    {
        private DataManager dataManager;
        public VoterController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(dataManager.Voters.GetAll());
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Voters.Get(Id);
            var person = dataManager.Persons.Get(obj.PersonId ?? 0);
            var model = new VoterViewModel
            {
                PersonView = new PersonViewModel
                {
                    Person = person,
                    District = dataManager.Districts.Get((int?)person.DistrictId ?? 0),
                    Nationality = dataManager.Nationalities.Get((int?)person.NationalityId ?? 0),
                    Education = dataManager.Educations.Get((int?)person.EducationId ?? 0),
                    Locality = dataManager.Localities.Get((int?)person.LocalityId ?? 0),
                    Street = dataManager.Streets.Get((int?)person.StreetId ?? 0),
                    House = dataManager.Houses.Get((int?)person.HouseId ?? 0)
                },
                PoliticalViews = (from vp in dataManager.VoterPartyRelations.GetAll()
                                  where vp.VoterId == Id
                                  select new VoterPartyRelationViewModel
                                  {
                                      VoterPartyRelation = vp,
                                      Voter = obj,
                                      Party = dataManager.Parties.Get(vp.PartyId ?? 0)
                                  }).ToList()
            };
            return View(model);
        }

        public ActionResult AsyncChange(int Id)
        {
            var task = Execute(Id);
            return RedirectToAction("Show", new { Id = Id });
        }

        private async System.Threading.Tasks.Task Execute(int id)
        {
            await System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                System.Threading.Thread.Sleep(15000);
                var obj = dataManager.Persons.Get(id);
                obj.FirstName = obj.FirstName + " changed by async";
                dataManager.Persons.Save(obj);
            });
        }

        public ActionResult ViewStateServiceInfo(int personId)
        {
            var hasData = false;
            var obj = dataManager.Persons.Get(personId);
            Street streetObj;
            var houses = from h in dataManager.Houses.GetAll()
                         where h.ManagerId == obj.Id
                         select new
                         {
                             House = h,
                             Street = streetObj = dataManager.Streets.Get((int?)h.StreetId ?? 0),
                             District = streetObj != null ? dataManager.Districts.Get((int?)streetObj.DistrictId ?? 0) : null
                         }.ToSafeDynamic();
            if (houses.Count() > 0)
            {
                ViewBag.Houses = houses;
                hasData = true;
            }
            var municipalities = from m in dataManager.Municipalities.GetAll()
                                 where m.ManagerId == obj.Id
                                 select new
                                 {
                                     Municipality = m,
                                     District = dataManager.Districts.Get((int?)m.DistrictId ?? 0)
                                 }.ToSafeDynamic();
            if (municipalities.Count() > 0)
            {
                ViewBag.Municipalities = municipalities;
                hasData = true;
            }
            var districts = from d in dataManager.Districts.GetAll()
                            where d.ManagerId == obj.Id
                            select d;
            if (districts.Count() > 0)
            {
                ViewBag.Districts = districts;
                hasData = true;
            }
            ViewBag.HasData = hasData;
            return PartialView();
        }

        public ActionResult ViewRegistration(int personId)
        {
            var person = dataManager.Persons.Get(personId);
            var obj = dataManager.Registrations.GetAll().FirstOrDefault(r => r.PersonId == personId);
            if (obj != null)
            {
                var model = new RegistrationViewModel
                {
                    Registration = obj,
                    Person = person,
                    District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0),
                    Locality = dataManager.Localities.Get((int?)obj.LocalityId ?? 0),
                    Street = dataManager.Streets.Get((int?)obj.StreetId ?? 0),
                    House = dataManager.Houses.Get((int?)obj.HouseId ?? 0)
                };
                return PartialView(model);
            }
            return PartialView("NoRegistration");
        }

        public ActionResult ViewAddressManagers(int? districtId, int? houseId)
        {
            ViewBag.AddressType = "месту проживания";
            var hasData = false;
            var district = dataManager.Districts.Get((int?)districtId ?? 0);

            //Аким
            if (district != null)
            {
                ViewBag.DistrictManager = dataManager.Persons.Get((int?)district.ManagerId ?? 0);
                ViewBag.District = district;
                ViewBag.DistrictManagementInfo =
                    new
                    {
                        Manager = dataManager.Persons.Get((int?)district.ManagerId ?? 0),
                        District = district
                    }.ToSafeDynamic();
                hasData = true;
            }

            var house = dataManager.Houses.Get((int?)houseId ?? 0);

            //Руководитель МТУ
            MunicipalityHouseRelation relationMunicipality = house != null
                ? dataManager.MunicipalityHouseRelations.GetAll().FirstOrDefault(x => x.HouseId == house.Id) : null;
            if (relationMunicipality != null)
            {
                var municipality = dataManager.Municipalities.Get((int?)relationMunicipality.MunicipalityId ?? 0);
                if (municipality != null)
                    ViewBag.MunicipalityManagementInfo =
                        new
                        {
                            Manager = dataManager.Persons.Get((int?)municipality.ManagerId ?? 0),
                            Municipality = municipality
                        }.ToSafeDynamic();
                hasData = true;
            }

            //Домком или квартальный
            if (house != null)
            {
                var houseManager = dataManager.Persons.Get((int?)house.ManagerId ?? 0);
                string houseManagerTypeName = "неизвестный";
                if (houseManager != null) houseManagerTypeName = dataManager.Houses.GetAll().Count(h => h.ManagerId == houseManager.Id) > 1 ? "Квартальная" : "Домком";

                ViewBag.HouseManagementInfo =
                        new
                        {
                            Manager = houseManager,
                            ManagerTypeName = houseManagerTypeName,
                            House = house
                        }.ToSafeDynamic();
                hasData = true;
            }
            ViewBag.HasData = hasData;
            return PartialView();
        }

        public ActionResult ViewOfficialAddressManagers(int personId)
        {
            var registration = dataManager.Registrations.GetAll().FirstOrDefault(r => r.PersonId == personId);

            ViewBag.AddressType = "прописке";
            var hasData = false;
            if (registration != null)
            {
                var district = dataManager.Districts.Get((int?)registration.DistrictId ?? 0);

                //Аким
                if (district != null)
                {
                    ViewBag.DistrictManager = dataManager.Persons.Get((int?)district.ManagerId ?? 0);
                    ViewBag.District = district;
                    ViewBag.DistrictManagementInfo =
                        new
                        {
                            Manager = dataManager.Persons.Get((int?)district.ManagerId ?? 0),
                            District = district
                        }.ToSafeDynamic();
                    hasData = true;
                }

                var house = dataManager.Houses.Get((int?)registration.HouseId ?? 0);

                //Руководитель МТУ
                MunicipalityHouseRelation relationMunicipality = house != null
                    ? dataManager.MunicipalityHouseRelations.GetAll().FirstOrDefault(x => x.HouseId == house.Id) : null;
                if (relationMunicipality != null)
                {
                    var municipality = dataManager.Municipalities.Get((int?)relationMunicipality.MunicipalityId ?? 0);
                    if (municipality != null)
                        ViewBag.MunicipalityManagementInfo =
                            new
                            {
                                Manager = dataManager.Persons.Get((int?)municipality.ManagerId ?? 0),
                                Municipality = municipality
                            }.ToSafeDynamic();
                    hasData = true;
                }

                //Домком или квартальный
                if (house != null)
                {
                    var houseManager = dataManager.Persons.Get((int?)house.ManagerId ?? 0);
                    string houseManagerTypeName = "неизвестный";
                    if (houseManager != null) houseManagerTypeName = dataManager.Houses.GetAll().Count(h => h.ManagerId == houseManager.Id) > 1 ? "Квартальная" : "Домком";

                    ViewBag.HouseManagementInfo =
                            new
                            {
                                Manager = houseManager,
                                ManagerTypeName = houseManagerTypeName,
                                House = house
                            }.ToSafeDynamic();
                    hasData = true;
                }
            }
            ViewBag.HasData = hasData;
            return PartialView("ViewAddressManagers");
        }

        [HttpGet]
        public ActionResult Create(int? organizationId, int? districtId, int? nationalityId,
            int? educationId, int? partyId, int? localityId, int? streetId, int? houseId)
        {
            var nationalities = new List<SelectListItem> { new SelectListItem() };
            nationalities.AddRange(from n in dataManager.Nationalities.GetAll()
                                   select new SelectListItem
                                   {
                                       Text = n.Name,
                                       Value = n.Id.ToString(),
                                       Selected = nationalityId == n.Id
                                   });
            ViewBag.Nationalities = nationalities;
            var educations = new List<SelectListItem> { new SelectListItem() };
            educations.AddRange(from n in dataManager.Educations.GetAll()
                                select new SelectListItem
                                {
                                    Text = n.Name,
                                    Value = n.Id.ToString(),
                                    Selected = educationId == n.Id
                                });
            ViewBag.Educations = educations;
            var organizations = new List<SelectListItem> { new SelectListItem() };
            organizations.AddRange(from n in dataManager.Organizations.GetAll()
                                   select new SelectListItem
                                   {
                                       Text = n.Name,
                                       Value = n.Id.ToString(),
                                       Selected = organizationId == n.Id
                                   });
            ViewBag.Organizations = organizations;
            var parties = new List<SelectListItem> { new SelectListItem() };
            parties.AddRange(from p in dataManager.Parties.GetAll()
                             select new SelectListItem
                             {
                                 Text = p.Name,
                                 Value = p.Id.ToString(),
                                 Selected = p.Id == partyId
                             });
            ViewBag.Parties = parties;
            var model = new Person();
            if(houseId.HasValue)
            {
                var house = dataManager.Houses.Get(houseId.Value);
                model.HouseId = house.Id;
                streetId = house.StreetId;
            }
            if (streetId.HasValue)
            {
                var street = dataManager.Streets.Get(streetId.Value);
                model.StreetId = street.Id;
                localityId = street.LocalityId;
                districtId = street.DistrictId;
                ViewBag.Street = street;
            }
            if (localityId.HasValue)
            {
                var locality = dataManager.Localities.Get(localityId.Value);
                model.LocalityId = locality.Id;
                districtId = locality.DistrictId;
                ViewBag.Locality = locality;
            }
            var districts = new List<SelectListItem> { new SelectListItem() };
            districts.AddRange(from d in dataManager.Districts.GetAll()
                               select new SelectListItem
                               {
                                   Text = d.Name,
                                   Value = d.Id.ToString(),
                                   Selected = districtId == d.Id
                               });
            ViewBag.Districts = districts;
            var referers = new List<SelectListItem> { new SelectListItem() };
            referers.AddRange(from p in dataManager.Persons.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.LastName + " " + p.FirstName + " " + p.MiddleName,
                                  Value = p.Id.ToString()
                              });
            ViewBag.Referers = referers;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(Person obj)
        {
            if (ModelState.IsValid)
            {
                {
                    dataManager.Persons.Save(obj);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult AddParty(int personId)
        {
            ViewBag.Parties = from p in dataManager.Parties.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.Name,
                                  Value = p.Id.ToString()
                              };
            return View(new VoterPartyRelation
            {
                VoterId = personId
            });
        }
        [HttpPost]
        public ActionResult AddParty(VoterPartyRelation obj)
        {
            if(ModelState.IsValid)
            {
                dataManager.VoterPartyRelations.Save(obj);
                return RedirectToAction("Show", new { Id = obj.VoterId });
            }
            return View(obj);
        }
        public ActionResult RemovePartyRelation(int relationId)
        {
            var rel = dataManager.VoterPartyRelations.Get(relationId);
            if(rel != null)
            {
                dataManager.VoterPartyRelations.Delete(rel.Id);
            }
            return RedirectToAction("Show", new { Id = rel.VoterId });
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
    }
}
