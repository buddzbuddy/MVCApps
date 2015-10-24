using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("Persons")]
    public class Person
    {
        public int Id { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "ФИО")]
        public virtual string FullName
        {
            get
            {
                return LastName + " " + FirstName + " " + MiddleName;
            }
        }

        public virtual int? Years
        {
            get
            {
                return BirthDate.HasValue ? (DateTime.Today.Year - BirthDate.Value.Year - (BirthDate < DateTime.Today ? 1 : 0)) : (int?)null;
            }
        }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'.'MM'.'yyyy}", ApplyFormatInEditMode = true)]
        //[RegularExpression(@"^(0[1-9]|1[012])[.](0[1-9]|[12][0-9]|3[01])[.]\d{4}$", ErrorMessage = "End Date should be in MM/dd/yyyy format")]
        public DateTime? BirthDate { get; set; }

        #region Contacts
        [Display(Name = "Телефон"), Phone]
        public string Phone { get; set; }

        [Display(Name = "Email-почта"), EmailAddress]
        public string Email { get; set; }
        #endregion

        #region Address
        [Display(Name = "Район проживания")]
        public int? DistrictId { get; set; }

        [Display(Name = "Населенный пункт проживания")]
        public int? LocalityId { get; set; }

        [Display(Name = "Улица проживания")]
        public int? StreetId { get; set; }

        [Display(Name = "Дом проживания")]
        public int? HouseId { get; set; }

        [Display(Name = "Квартира проживания")]
        public string Apartment { get; set; }
        #endregion

        [Display(Name = "Национальность")]
        public int? NationalityId { get; set; }

        [Display(Name = "Образование")]
        public int? EducationId { get; set; }

        [Display(Name = "Сдал биометрические данные?")]
        public bool GaveBiometricData { get; set; }

        #region Organization info
        [Display(Name = "Организация")]
        public int? OrganizationId { get; set; }

        [Display(Name = "Должность")]
        public string JobTitle { get; set; }
        #endregion

        [Display(Name = "Кто рекомендовал:")]
        public int? RefererId { get; set; }

        public virtual List<House> Houses { get; set; }

        [Display(Name = "Политический взгляд")]
        public int? PartyId { get; set; }

        /*[Display(Name = "УИК")]
        public int? PrecinctId { get; set; }*/
    }
}
