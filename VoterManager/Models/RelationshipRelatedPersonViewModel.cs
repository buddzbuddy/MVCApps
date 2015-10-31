using Domain.Entities;
namespace VoterManager.Models
{
    public class RelationshipRelatedPersonViewModel
    {
        public PersonRelationshipPersonRelation PersonRelationshipPersonRelation { get; set; }
        public Relationship Relationship { get; set; }
        public Person RelatedPerson { get; set; }
    }
}