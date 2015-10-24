using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class District
    {
        public int Id { get; set; }

        [Required, Display(Name = "Район")]
        public string Name { get; set; }

        [Display(Name = "Аким")]
        public int? ManagerId { get; set; }
        public virtual List<Locality> Localities { get; set; }
        public virtual List<Municipality> Municipalities { get; set; }
        public virtual List<Person> Persons { get; set; }
    }
}
