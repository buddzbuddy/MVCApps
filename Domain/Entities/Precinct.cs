using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Base;

namespace Domain.Entities
{
    public class Precinct : EntityBase
    {
        [Display(Name = "УИК")]
        public string Name { get; set; }

        [Display(Name = "Район")]
        public int? DistrictId { get; set; }

        [Display(Name = "Ответственный с партии")]
        public int? WorkerId { get; set; }

        #region Map info

        [Display(Name = "Широта")]
        public double? Latitude { get; set; }

        [Display(Name = "Долгота")]
        public double? Longitude { get; set; }

        public int? Zoom { get; set; }

        public string IconPath { get; set; }
        #endregion
    }
}
