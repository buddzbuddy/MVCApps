using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class PersonRelation
    {
        public int Id { get; set; }
        [Display(Name = "Связанное физ. лицо")]
        public int? Person1Id { get; set; }
        [Display(Name = "Кем является?")]
        public int? Relationship1Id { get; set; }
        [Display(Name = "Связанное физ. лицо")]
        public int? Person2Id { get; set; }
        [Display(Name = "Кем является?")]
        public int? Relationship2Id { get; set; }
    }
}
