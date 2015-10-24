using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class StreetViewModel
    {
        public Street Street { get; set; }
        public Locality Locality { get; set; }
        public District District { get; set; }
    }
}