using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class House
    {
        public int Id { get; set; }

        [Display(Name = "Дом")]
        public string Name { get; set; }

        [Display(Name = "Улица")]
        public int? StreetId { get; set; }

        [Display(Name = "Руководитель дома***")]
        public int? ManagerId { get; set; }

        [Display(Name = "Ответственный от партии за этот дом")]
        public int? WorkerId { get; set; }

        [Display(Name = "УИК")]
        public int? PrecinctId { get; set; }

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
