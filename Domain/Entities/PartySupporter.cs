using Domain.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// Класс "Сторонник партии"
    /// </summary>
    public class PartySupporter : EntityBase
    {
        [Display(Name = "Физ. лицо")]
        public int? PersonId { get; set; }
    }
}
