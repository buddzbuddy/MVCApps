using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class AgitatorHouseRelationViewModel
    {
        public AgitatorHouseRelation AgitatorHouseRelation { get; set; }
        public Agitator Agitator { get; set; }
        public House House { get; set; }
    }
}