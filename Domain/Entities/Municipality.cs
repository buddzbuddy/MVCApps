using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Municipality
    {
        public int Id { get; set; }

        [Required, Display(Name = "МТУ")]
        public string Name { get; set; }

        [Display(Name = "Район")]
        public int? DistrictId { get; set; }

        [Display(Name = "Руководитель")]
        public int? ManagerId { get; set; }
    }
}
