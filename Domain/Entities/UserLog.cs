using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }

        private DateTime _LoginDate = DateTime.Now;
        public DateTime LoginDate { get { return _LoginDate; } set { _LoginDate = value; } }
    }
}
