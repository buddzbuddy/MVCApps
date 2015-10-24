using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class HouseViewModel
    {
        public House House { get; set; }
        public Street Street { get; set; }
        public Person Manager { get; set; }
        public Precinct Precinct { get; set; }
    }
}