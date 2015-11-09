using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TempPerson
    {
        public int Id { get; set; }
        [Display(Name = "ПИН")]
        public string pin { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Display(Name = "Отчество")]
        public string MiddleName { get; set; }

        [Display(Name = "Д.р.")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Тип документа")]
        public string DocumentType { get; set; }
        [Display(Name = "Паспорт №,серия")]
        public string PassportNo { get; set; }
        [Display(Name = "Дата выдачи")]
        public DateTime? PassportDate { get; set; }
        [Display(Name = "Выд. орган")]
        public string PassportOrg { get; set; }
        [Display(Name = "Район")]
        public string District { get; set; }
        [Display(Name = "Улица")]
        public string Street { get; set; }
        [Display(Name = "Дом")]
        public string House { get; set; }
        [Display(Name = "Кв.")]
        public string Apartment { get; set; }
        [Display(Name = "Категория гр.")]
        public string Category { get; set; }
        [Display(Name = "Выплата")]
        public string PaymentType { get; set; }
    }
}
