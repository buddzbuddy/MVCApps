using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Organization
    {
        public int Id { get; set; }

        [Display(Name = "Организация")]
        public string Name { get; set; }

        public virtual List<Person> Persons { get; set; }
    }
}
