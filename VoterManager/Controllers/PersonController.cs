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
            return View(dataManager.Persons.GetAll());
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
            var model = new PersonViewModel
            {
                Person = obj,
                District = dataManager.Districts.Get((int?)obj.DistrictId ?? 0),
                Nationality = dataManager.Nationalities.Get((int?)obj.NationalityId ?? 0),
                Education = dataManager.Educations.Get((int?)obj.EducationId ?? 0),
                Locality = dataManager.Localities.Get((int?)obj.LocalityId ?? 0),
                Street = dataManager.Streets.Get((int?)obj.StreetId ?? 0),
                House = dataManager.Houses.Get((int?)obj.HouseId ?? 0)
            };
            return View(model);
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
        public ActionResult Create(int? districtId, int? nationalityId,
            int? educationId, int? localityId, int? streetId, int? houseId)
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
        public ActionResult CreateBase(string returnUrl, string processName = "<unknownProcess>", bool isSearched = false, string forCreate = "", int? districtId = null, int? nationalityId = null,
            int? educationId = null, int? localityId = null, int? streetId = null, int? houseId = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
                throw new ArgumentNullException("returnUrl", "Обратный Url не передан!");
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ProcessName = processName;

            if (!isSearched)
                return RedirectToAction("SearchPerson", new { returnUrl = returnUrl, processName = processName });

            var model = new Person();
            if (!string.IsNullOrEmpty(forCreate))
                model.FullName = forCreate;

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
            if (houseId.HasValue)
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

            return View(model);
        }
        [HttpPost]
        public ActionResult CreateBase(Person obj, FormCollection collection)
        {
            var returnUrl = collection["returnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
                throw new ArgumentNullException("returnUrl", "Обратный Url не передан!");
            if (ModelState.IsValid)
            {
                {
                    dataManager.Persons.Save(obj);
                    return Redirect(returnUrl + "?personId=" + obj.Id);
                }
            }
            return View(obj);
        }

        public ActionResult SearchPerson(string returnUrl, string processName)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.ProcessName = processName;
            return View();
        }

        [HttpPost]
        public ActionResult SearchPerson(FormCollection collection)
        {
            if (Request.IsAjaxRequest())
            {
                string term = collection["term"];
                var persons = dataManager.Persons.GetAll();
                var source = new List<KeyValuePair<string, string>>
                    (persons.Select(x =>
                        new KeyValuePair<string, string>(x.Id.ToString(), x.FullName)));
                var result = source.Where(s => s.Value.ToLower().Contains(term.ToLower())).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            var searchPerson = collection["SearchPerson"];
            var userAction = collection["UserAction"];
            var returnUrl = collection["ReturnUrl"];
            if (userAction == "create")
            {
                var fullName = "";
                var names = searchPerson.Trim().Split(' ');
                for (int i = 0; i < names.Length && i < 3; i++)
                {
                    names[i] = names[i].Trim();
                }
                fullName = string.Join("+", names.Take(3));
                return RedirectToAction("CreateBase", new { returnUrl = returnUrl, processName = collection["ProcessName"], isSearched = true, forCreate = fullName });
            }
            else if (userAction == "select")
                return Redirect(returnUrl + "?personId=" + int.Parse(collection["PersonId"]));
            return View();
        }

        [HttpPost]
        public JsonResult SelectedPersonDetail(int Id)
        {
            var obj = dataManager.Persons.Get(Id);
            return Json(new
            {
                Id = obj.Id,
                FullName = obj.FullName,
                BirthDate = obj.BirthDate.HasValue ? obj.BirthDate.Value.ToString("d") : "--.--.----"
            }, JsonRequestBehavior.AllowGet);
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
