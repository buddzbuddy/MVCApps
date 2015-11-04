using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class AgitatorPrecinctRelationViewModel
    {
        public AgitatorPrecinctRelation AgitatorPrecinctRelation { get; set; }
        public Agitator Agitator { get; set; }
        public Precinct Precinct { get; set; }
    }
}