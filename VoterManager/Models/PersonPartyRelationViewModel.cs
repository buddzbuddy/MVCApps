using Domain.Entities;
namespace VoterManager.Models
{
    public class PersonPartyRelationViewModel
    {
        public PersonPartyRelation PersonPartyRelation { get; set; }
        public Person Person { get; set; }
        public Party Party { get; set; }
    }
}