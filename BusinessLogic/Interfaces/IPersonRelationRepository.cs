using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IPersonRelationRepository
    {
        IEnumerable<PersonRelation> GetAll();
        PersonRelation Get(int Id);
        void Save(PersonRelation obj);
        void Delete(int Id);
    }
}
