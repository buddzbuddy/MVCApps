using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class MunicipalityViewModel
    {
        public Person Manager { get; set; }
        public Municipality Municipality { get; set; }
        public District District { get; set; }
    }
}