using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Street
    {
        public int Id { get; set; }

        [Display(Name = "Улица")]
        public string Name { get; set; }

        [Display(Name = "Район")]
        public int? DistrictId { get; set; }

        [Display(Name = "Нас. пункт")]
        public int? LocalityId { get; set; }

        public virtual List<Person> Persons { get; set; }
        public virtual List<House> Houses { get; set; }
    }
}
