using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [Table("UserProfile")]
    public class UserProfile
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Display(Name = "Логин"), Required(ErrorMessage = "Задайте логин")]
        public string UserName { get; set; }

        [Display(Name = "Пароль"), Required(ErrorMessage = "Задайте пароль")]
        public string Password { get; set; }

        [Display(Name = "ФИО работника")]
        public int? WorkerId { get; set; }
    }
}
