using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    /// <summary>
    /// Класс "Агитатор"
    /// </summary>
    public class Agitator
    {
        public int Id { get; set; }

        [Display(Name = "Физ. лицо")]
        public int? PersonId { get; set; }
    }
}
