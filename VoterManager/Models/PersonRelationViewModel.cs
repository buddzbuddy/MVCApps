using Domain.Entities;
namespace VoterManager.Models
{
    public class PersonRelationViewModel
    {
        public PersonRelation PersonRelation { get; set; }
        public Person Person { get; set; }
        public Relationship Relationship { get; set; }
    }
}