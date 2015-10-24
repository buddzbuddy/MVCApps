using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class WorkerViewModel
    {
        public Worker Worker { get; set; }
        public Person Person { get; set; }
    }
}