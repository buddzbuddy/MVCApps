using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VoterManager.Models
{
    public class UserViewModel
    {
        public UserProfile UserProfile { get; set; }
        public Person Person { get; set; }
    }
}