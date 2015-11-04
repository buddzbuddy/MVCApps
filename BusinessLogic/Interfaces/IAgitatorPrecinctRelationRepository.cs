using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IAgitatorPrecinctRelationRepository
    {
        IEnumerable<AgitatorPrecinctRelation> GetAll();
        AgitatorPrecinctRelation Get(int Id);
        void Save(AgitatorPrecinctRelation obj);
        void Delete(int Id);
    }
}
