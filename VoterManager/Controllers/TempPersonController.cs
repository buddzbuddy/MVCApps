using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain;
using Domain.Entities;
using BusinessLogic;

namespace VoterManager.Controllers
{
    public class TempPersonController : Controller
    {
        private DataManager dataManager;
        private const int PageSize = 10;
        public TempPersonController(DataManager dataManager)
        {
            this.dataManager = dataManager;
            ViewBag.PageSize = PageSize;
        }
        // GET: TempPersons
        public ActionResult Index(int? createdItems, int? noCreatedItems, int? page)
        {
            if (page == null) page = 1;
            ViewBag.CreatedItems = createdItems;
            ViewBag.NoCreatedItems = noCreatedItems;
            var tempPersons = dataManager.TempPersons.GetAll();
            //setting page parameters
            ViewBag.ItemsCount = tempPersons.Count();
            ViewBag.CurrentPage = page;

            return View(tempPersons
                .Skip((page.Value - 1) * PageSize).Take(PageSize)
                .ToList());
        }
        public ActionResult LoadToPersons(int from, int to)
        {
            var tempPersons = dataManager.TempPersons.GetAll().Skip(from).Take(to).ToList();
            int total = tempPersons.Count;
            int createdItems = 0;
            int noCreatedItems = 0;
            foreach (var tempPerson in tempPersons)
            {
                try
                {
                    var person = new Person
                    {
                        FullName = tempPerson.LastName + "+" + tempPerson.FirstName + "+" + tempPerson.MiddleName,
                        BirthDate = tempPerson.BirthDate,
                        Apartment = tempPerson.Apartment
                    };
                    var districtId = tempPerson.District.ToLower() == "ленинский" ? 53 : tempPerson.District.ToLower() == "октябрьский" ? 54 : tempPerson.District.ToLower() == "первомайский" ? 55 : 56;
                    person.DistrictId = districtId;

                    var streetId = GetStreetId(districtId, tempPerson.Street.Trim());
                    person.StreetId = streetId;
                    var houseId = GetHouseId(streetId, tempPerson.House.Trim());
                    person.HouseId = houseId;
                    dataManager.Persons.Save(person);
                    createdItems++;
                }
                catch (Exception e)
                {
                    noCreatedItems++;
                }
                
            }

            return RedirectToAction("Index", new { createdItems = createdItems, noCreatedItems = noCreatedItems });
        }

        int GetStreetId(int districtId, string streetName)
        {
            var streets = dataManager.Streets.GetAll().Where(s => s.DistrictId == districtId).ToList();
            foreach (var street in streets)
            {
                if (street.Name.StartsWith(streetName)) return street.Id;
            }
            var newStreet = new Street
            {
                DistrictId = districtId,
                Name = streetName
            };
            dataManager.Streets.Save(newStreet);
            return newStreet.Id;
        }

        int GetHouseId(int streetId, string houseName)
        {
            var houses = dataManager.Houses.GetAll().Where(h => h.StreetId == streetId).ToList();
            foreach (var house in houses)
            {
                if (house.Name.StartsWith(houseName)) return house.Id;
            }
            var newHouse = new House
            {
                StreetId = streetId,
                Name = houseName
            };
            dataManager.Houses.Save(newHouse);
            return newHouse.Id;
        }
    }
}
