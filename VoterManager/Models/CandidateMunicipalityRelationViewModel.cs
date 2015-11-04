using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class CandidateMunicipalityRelationViewModel
    {
        public CandidateMunicipalityRelation CandidateMunicipalityRelation { get; set; }
        public Candidate Candidate { get; set; }
        public Municipality Municipality { get; set; }
    }
}