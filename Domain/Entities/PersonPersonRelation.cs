using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class PersonRelativeRelation
    {
        public int Id { get; set; }
        [Display(Name = "Гражданин")]
        public int? PersonId { get; set; }
        [Display(Name = "Родственник")]
        public int? RelativeId { get; set; }
    }
}
