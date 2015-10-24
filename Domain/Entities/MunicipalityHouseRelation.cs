using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MunicipalityHouseRelation
    {
        public int Id { get; set; }
        
        [Display(Name = "Дом")]
        public int? HouseId { get; set; }

        [Display(Name = "МТУ")]
        public int? MunicipalityId { get; set; }
    }
}
