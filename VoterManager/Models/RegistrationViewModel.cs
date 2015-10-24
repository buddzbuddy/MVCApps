using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class RegistrationViewModel
    {
        public Registration Registration { get; set; }
        public Person Person { get; set; }
        public District District { get; set; }
        public Locality Locality { get; set; }
        public Street Street { get; set; }
        public House House { get; set; }
    }
}