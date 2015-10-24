using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Worker
    {
        public int Id { get; set; }

        [Display(Name = "Информация о работнике")]
        public int? PersonId { get; set; }

        public string FullName { get; set; }

        public virtual List<House> Houses { get; set; }
        public virtual List<Precinct> Precincts { get; set; }
    }
}
