using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Класс "Работник штаба"
    /// </summary>
    public class Worker
    {
        public int Id { get; set; }

        [Display(Name = "Физ. лицо")]
        public int? PersonId { get; set; }
    }
}
