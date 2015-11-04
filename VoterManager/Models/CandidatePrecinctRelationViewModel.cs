using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class CandidatePrecinctRelationViewModel
    {
        public CandidatePrecinctRelation CandidatePrecinctRelation { get; set; }
        public Candidate Candidate { get; set; }
        public Precinct Precinct { get; set; }
    }
}