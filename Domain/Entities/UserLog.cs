using System;
using Domain.Base;
namespace Domain.Entities
{
    public class UserLog : EntityBase
    {
        //public int Id { get; set; }
        public string UserName { get; set; }

        private DateTime _LoginDate = DateTime.Now;
        public DateTime LoginDate { get { return _LoginDate; } set { _LoginDate = value; } }
    }
}
