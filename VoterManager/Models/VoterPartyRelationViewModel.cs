using Domain.Entities;
namespace VoterManager.Models
{
    public class VoterPartyRelationViewModel
    {
        public VoterPartyRelation VoterPartyRelation { get; set; }
        public Voter Voter { get; set; }
        public Party Party { get; set; }
    }
}