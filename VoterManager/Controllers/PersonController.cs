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
    public class PersonController : Controller
    {
        private DataManager dataManager;
        public PersonController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(from m in dataManager.Persons.GetAll()
                        select new PersonViewModel
                        {
                            Person = m,
                            Party = dataManager.Parties.Get(m.PartyId ?? 0),
                            Organization = dataManager.Organizations.Get(m.OrganizationId ?? 0)
                        });
        }

        public ActionResult MunicipalityManagers()
        {
            var managers = dataManager.Municipalities.GetAll().Where(m => m.ManagerId.HasValue);
            Person personObj;
            return View(from m in managers
                        select new
                        {
                            Person = personObj = dataManager.Persons.Get(m.ManagerId.Value),
                            District = dataManager.Districts.Get((int?)personObj.DistrictId ?? 0),
                            Municipality = m
                        }.ToSafeDynamic());
        }
        public ActionResult HouseManagers()
        {
            var managers = dataManager.Houses.GetAll().Where(h => h.ManagerId.HasValue).Select(h => h.ManagerId.Value).Distinct();
            Person personObj;
            House houseObj;
            return View(from m in managers
                        select new
                        {
                            Person = personObj = dataManager.Persons.Get(m),
                            House = houseObj = GetHouseByManagerId(personObj.Id),
                            Street = houseObj != null ? dataManager.Streets.Get((int?)houseObj.StreetId ?? 0) : null
                        }.ToSafeDynamic());
        }

        private House GetHouseByManagerId(int Id)
        {
            var houses = dataManager.Houses.GetAll().Where(h => h.ManagerId == Id);
            if (houses.Count() > 1) return null;
            return houses.First();
        }

        public ActionResult Show(int Id)
        {
            var obj = dataManager.Persons.Get(Id);
            var recommendedPersons = dataManager.Persons.GetAll().Where(p => p.RefererId == obj.Id);
            if (recommendedPersons.Count() > 0)
                ViewBag.RecommendedPersons = recommendedPersons;
            if (obj.RefererId.HasValue)
            {
                var referer = dataManager.Persons.Get(obj.RefererId.Value);
                ViewBag.Referer = referer;
            }
            var model = new PersonViewModel
            {
                Person = obj,
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0),
                Nationality = dataManager.Nationalities.Get((int?)obj.NationalityId ?? 0),
                Education = dataManager.Educations.Get((int?)obj.EducationId ?? 0),
                Locality = dataManager.Localities.Get((int?)obj.LocalityId ?? 0),
                Street = dataManager.Streets.Get((int?)obj.StreetId ?? 0),
                House = dataManager.Houses.Get((int?)obj.HouseId ?? 0),
                Organization = dataManager.Organizations.Get((int?)obj.OrganizationId ?? 0),
                Party = dataManager.Parties.Get(obj.PartyId ?? 0),
                PoliticalViews = (from pp in dataManager.PersonPartyRelations.GetAll()
                                 where pp.PersonId == Id
                                 select new PersonPartyRelationViewModel
                                 {
                                     PersonPartyRelation = pp,
                                     Person = obj,
                                     Party = dataManager.Parties.Get(pp.PartyId ?? 0)
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
            int? educationId, int? partyId, int? localityId, int? streetId, int? houseId/*, int? precinctId*/)
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
            /*if (precinctId.HasValue)
            {
                var precinct = dataManager.Precincts.Get(precinctId.Value);
                model.PrecinctId = precinct.Id;
                districtId = precinct.DistrictId;
                ViewBag.Precinct = precinct;
            }*/
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
            return View(new PersonPartyRelation
            {
                PersonId = personId
            });
        }
        [HttpPost]
        public ActionResult AddParty(PersonPartyRelation obj)
        {
            if(ModelState.IsValid)
            {
                dataManager.PersonPartyRelations.Save(obj);
                return RedirectToAction("Show", new { Id = obj.PersonId });
            }
            return View(obj);
        }
        public ActionResult RemovePartyRelation(int relationId)
        {
            var rel = dataManager.PersonPartyRelations.Get(relationId);
            if(rel != null)
            {
                dataManager.PersonPartyRelations.Delete(rel.Id);
            }
            return RedirectToAction("Show", new { Id = rel.PersonId });
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
                                       Value = n.Id.ToString(),
                                       Selected = obj.OrganizationId == n.Id
                                   });
            ViewBag.Organizations = organizations;
            var parties = new List<SelectListItem> { new SelectListItem() };
            parties.AddRange(from p in dataManager.Parties.GetAll()
                             select new SelectListItem
                             {
                                 Text = p.Name,
                                 Value = p.Id.ToString(),
                                 Selected = p.Id == obj.PartyId
                             });
            ViewBag.Parties = parties;

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
            /*if (obj.PrecinctId.HasValue)
            {
                var precinct = dataManager.Precincts.Get(obj.PrecinctId.Value);
                obj.DistrictId = precinct.DistrictId;
                ViewBag.Precinct = precinct;
            }*/
            var districts = new List<SelectListItem> { new SelectListItem() };
            districts.AddRange(from d in dataManager.Districts.GetAll()
                               select new SelectListItem
                               {
                                   Text = d.Name,
                                   Value = d.Id.ToString(),
                                   Selected = obj.DistrictId == d.Id
                               });
            ViewBag.Districts = districts;
            var referers = new List<SelectListItem> { new SelectListItem() };
            referers.AddRange(from p in dataManager.Persons.GetAll()
                              select new SelectListItem
                              {
                                  Text = p.LastName + " " + p.FirstName + " " + p.MiddleName,
                                  Value = p.Id.ToString(),
                                  Selected = p.RefererId == obj.RefererId
                              });
            ViewBag.Referers = referers;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Person obj)
        {
            if (ModelState.IsValid)
            {
                //if (!dataManager.Persons.GetAll()
                //    .Where(m => m.DistrictId == obj.DistrictId)
                //    .Any(o => (o.LastName == obj.LastName) && (o.FirstName == obj.FirstName)))
                {
                    /*var objFromDb = dataManager.Persons.Get(obj.Id);
                    objFromDb.LastName = obj.LastName;
                    objFromDb.Apartment = obj.Apartment;
                    objFromDb.BirthDate = obj.BirthDate;
                    objFromDb.DistrictId = obj.DistrictId;
                    objFromDb.EducationId = obj.EducationId;
                    objFromDb.Email = obj.Email;
                    objFromDb.FirstName = obj.FirstName;
                    objFromDb.GaveBiometricData = obj.GaveBiometricData;
                    objFromDb.HouseId = obj.HouseId;
                    objFromDb.OrganizationId = obj.OrganizationId;
                    objFromDb.JobTitle = obj.JobTitle;
                    objFromDb.LastName = obj.LastName;
                    objFromDb.LocalityId = obj.LocalityId;
                    objFromDb.MiddleName = obj.MiddleName;
                    objFromDb.NationalityId = obj.NationalityId;
                    objFromDb.Phone = obj.Phone;
                    objFromDb.StreetId = obj.StreetId;
                    objFromDb.RefererId = obj.RefererId;
                    objFromDb.PartyId = obj.PartyId;
                    */

                    dataManager.Persons.Save(obj/*FromDb*/);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                //else
                //{
                //    ModelState.AddModelError("LastName",
                //        "Гражданин с фамилией и именем \"" + obj.LastName + "\" \"" + obj.FirstName + "\" уже существует!");
                //}
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
                House = dataManager.Houses.Get((int?)obj.HouseId ?? 0),
                Organization = dataManager.Organizations.Get((int?)obj.OrganizationId ?? 0)
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
