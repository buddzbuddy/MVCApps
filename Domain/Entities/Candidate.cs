using System.ComponentModel.DataAnnotations;
using Domain.Base;
namespace Domain.Entities
{
    /// <summary>
    /// Класс "Кандидат"
    /// </summary>
    public class Candidate : EntityBase
    {
        [Display(Name = "Физ. лицо")]
        public int? PersonId { get; set; }

        [Display(Name = "Тип маркера на карте"), Required(ErrorMessage = "Укажите маркер")]
        public string MarkerType { get; set; }
    }
}
