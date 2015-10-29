using System.ComponentModel.DataAnnotations;
namespace Domain.Entities
{
    public class VoterPartyRelation
    {
        public int Id { get; set; }
        [Display(Name = "Избиратель")]
        public int? VoterId { get; set; }
        [Display(Name = "Партия")]
        public int? PartyId { get; set; }
    }
}
