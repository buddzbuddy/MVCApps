using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Registration
    {
        public int Id { get; set; }

        [Display(Name = "Гражданин")]
        public int? PersonId { get; set; }

        #region Address
        [Display(Name = "Район прописки")]
        public int? DistrictId { get; set; }

        [Display(Name = "Населенный пункт прописки")]
        public int? LocalityId { get; set; }

        [Display(Name = "Улица прописки")]
        public int? StreetId { get; set; }

        [Display(Name = "Дом прописки")]
        public int? HouseId { get; set; }

        [Display(Name = "Квартира прописки")]
        public string Apartment { get; set; }
        #endregion

        #region Passport
        [Display(Name = "Серия паспорта")]
        public string PassportSeries { get; set;}

        [Display(Name = "Номер паспорта")]
        public string PassportNo { get; set; }

        [Display(Name = "Дата выдачи")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? PassportDate { get; set; }

        [Display(Name = "Выдавший орган")]
        public string PassportOrg { get; set; }
        #endregion
    }
}
