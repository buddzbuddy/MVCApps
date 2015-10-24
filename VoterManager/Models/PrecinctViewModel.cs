using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class PrecinctViewModel
    {
        public Precinct Precinct { get; set; }
        public District District { get; set; }
        public Worker Worker { get; set; }
    }
}