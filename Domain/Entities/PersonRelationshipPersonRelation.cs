using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class PersonRelationshipPersonRelation
    {
        public int Id { get; set; }
        [Display(Name = "Физ. лицо")]
        public int? PersonId { get; set; }
        [Display(Name = "Связанное физ. лицо")]
        public int? RelatedPersonId { get; set; }
        [Display(Name = "Тип взаимоотношения")]
        public int? RelationshipId { get; set; }
    }
}
