using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Locality
    {
        public int Id { get; set; }

        [Display(Name = "Район")]
        public int? DistrictId { get; set; }
        
        [Display(Name = "Населенный пункт")]
        public string Name { get; set; }

        public virtual List<Person> Persons { get; set; }
        public virtual List<Street> Streets { get; set; }
    }
}
