using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class PersonPartyRelation
    {
        public int Id { get; set; }
        [Display(Name = "Гражданин")]
        public int? PersonId { get; set; }
        [Display(Name = "Партия")]
        public int? PartyId { get; set; }
    }
}
