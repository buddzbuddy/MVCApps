using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Display(Name = "Возраст")]
        public virtual int? Years
        {
            get
            {
                return BirthDate.HasValue ? (DateTime.Today.Year - BirthDate.Value.Year - (BirthDate < DateTime.Today ? 1 : 0)) : (int?)null;
            }
        }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{yyyy'-'MM'-'0:dd}", ApplyFormatInEditMode = true)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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
    }
}
