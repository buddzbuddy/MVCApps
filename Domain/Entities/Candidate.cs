using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    /// <summary>
    /// Класс "Кандидат"
    /// </summary>
    public class Candidate
    {
        public int Id { get; set; }

        [Display(Name = "Физ. лицо")]
        public int? PersonId { get; set; }
    }
}
