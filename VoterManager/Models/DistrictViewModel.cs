using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class DistrictViewModel
    {
        public District District { get; set; }
        public Person Manager { get; set; }
    }
}