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
    public class RegistrationController : Controller
    {
        private DataManager dataManager;
        public RegistrationController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index(int personId)
        {
            var person = dataManager.Persons.Get(personId);
            if (person == null)
                return HttpNotFound();

            return RedirectToAction("Show", "Person", new { Id = personId });
        }


        [HttpGet]
        public ActionResult Input(int personId, int? districtId, int? localityId, int? streetId, int? houseId)
        {
            var person = dataManager.Persons.Get(personId);
            if (person == null)
            {
                ViewBag.Message = "Гражданин для прописки не найден!";
                return View("Error");
            }
            if(dataManager.Registrations.GetAll().Any(r => r.PersonId == personId))
            {
                ViewBag.Message = "Гражданин уже прописан!";
                return View("Error");
            }
            ViewBag.Person = person;
            var model = new Registration
            {
                PersonId = personId
            };
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
            ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                select new SelectListItem
                                {
                                    Text = d.Name,
                                    Value = d.Id.ToString(),
                                    Selected = districtId.HasValue ? districtId.Value == d.Id : false
                                };
            return View(model);
        }

        [HttpPost]
        public ActionResult Input(Registration obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Registrations.GetAll().Any(o => o.PersonId == obj.PersonId))
                {
                    dataManager.Registrations.Save(obj);
                    return RedirectToAction("Show", "Person", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("PersonId",
                        "Регистрация для данного гражданина уже существует!");
                    var person = dataManager.Persons.Get((int?)obj.PersonId ?? 0);
                    if (person == null)
                        return HttpNotFound();
                    ViewBag.Person = person;
                    if (obj.HouseId.HasValue)
                    {
                        var house = dataManager.Houses.Get(obj.HouseId.Value);
                        obj.HouseId = house.Id;
                        obj.StreetId = house.StreetId;
                    }
                    if (obj.StreetId.HasValue)
                    {
                        var street = dataManager.Streets.Get(obj.StreetId.Value);
                        obj.StreetId = street.Id;
                        obj.LocalityId = street.LocalityId;
                        obj.DistrictId = street.DistrictId;
                        ViewBag.Street = street;
                    }
                    if (obj.LocalityId.HasValue)
                    {
                        var locality = dataManager.Localities.Get(obj.LocalityId.Value);
                        obj.LocalityId = locality.Id;
                        obj.DistrictId = locality.DistrictId;
                        ViewBag.Locality = locality;
                    }
                    ViewBag.Districts = from d in dataManager.Districts.GetAll()
                                        select new SelectListItem
                                        {
                                            Text = d.Name,
                                            Value = d.Id.ToString(),
                                            Selected = obj.DistrictId.HasValue ? obj.DistrictId.Value == d.Id : false
                                        };
                    return View(obj);
                }
            }
            return View(obj);
        }

        public ActionResult SetRegistrationFromPerson(int personId)
        {
            var person = dataManager.Persons.Get(personId);
            if (person == null)
                return HttpNotFound();

            var registration = new Registration
            {
                PersonId = personId,
                DistrictId = person.DistrictId,
                LocalityId = person.LocalityId,
                StreetId = person.StreetId,
                HouseId = person.HouseId,
                Apartment = person.Apartment
            };

            dataManager.Registrations.Save(registration);

            return RedirectToAction("Index", new { personId = personId });
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            return View(dataManager.Registrations.Get(id));
        }

        [HttpPost]
        public ActionResult Edit(Registration obj)
        {
            if (ModelState.IsValid)
            {
                if (!dataManager.Registrations.GetAll()
                    .Any(o =>
                        o.PersonId == obj.PersonId &&
                        o.Id != obj.Id))
                {
                    var objFromDb = dataManager.Registrations.Get(obj.Id);
                    objFromDb.Apartment = obj.Apartment;
                    objFromDb.DistrictId = obj.DistrictId;
                    objFromDb.HouseId = obj.HouseId;
                    objFromDb.LocalityId = obj.LocalityId;
                    objFromDb.PassportDate = obj.PassportDate;
                    objFromDb.PassportNo = obj.PassportNo;
                    objFromDb.PassportOrg = obj.PassportOrg;
                    objFromDb.PassportSeries = obj.PassportSeries;
                    objFromDb.PersonId = obj.PersonId;
                    objFromDb.StreetId = obj.StreetId;
                    dataManager.Registrations.Save(objFromDb);
                    return RedirectToAction("Show", new { Id = obj.Id });
                }
                else
                {
                    ModelState.AddModelError("PersonId",
                        "Регистрация для данного гражданина уже существует!");
                    return View(obj);
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var obj = dataManager.Registrations.Get(Id);
            return View(obj);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteCinfirmed(int Id)
        {
            dataManager.Registrations.Delete(Id);
            return RedirectToAction("Index");
        }
    }
}
