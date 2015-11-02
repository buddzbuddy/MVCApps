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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Display(Name = "Логин"), Required(ErrorMessage = "Задайте логин")]
        public string UserName { get; set; }

        [Display(Name = "Пароль"), Required(ErrorMessage = "Задайте пароль")]
        public string Password { get; set; }

        [Display(Name = "Физ. лицо")]
        public int? PersonId { get; set; }
    }
}
