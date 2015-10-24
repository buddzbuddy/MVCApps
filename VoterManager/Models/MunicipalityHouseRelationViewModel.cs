using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class MunicipalityHouseRelationViewModel
    {
        public MunicipalityHouseRelation MunicipalityHouseRelation { get; set; }
        public House House { get; set; }
        public Municipality Municipality { get; set; }
    }
}