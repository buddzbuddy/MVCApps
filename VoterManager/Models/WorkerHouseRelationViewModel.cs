using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class WorkerHouseRelationViewModel
    {
        public WorkerHouseRelation WorkerHouseRelation { get; set; }
        public Worker Worker { get; set; }
        public House House { get; set; }
    }
}