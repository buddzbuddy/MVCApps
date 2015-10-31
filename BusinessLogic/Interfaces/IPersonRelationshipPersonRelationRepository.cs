using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IPersonRelationshipPersonRelationRepository
    {
        IEnumerable<PersonRelationshipPersonRelation> GetAll();
        PersonRelationshipPersonRelation Get(int Id);
        void Save(PersonRelationshipPersonRelation obj);
        void Delete(int Id);
    }
}
