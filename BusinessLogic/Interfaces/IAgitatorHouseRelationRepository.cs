using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IAgitatorHouseRelationRepository
    {
        IEnumerable<AgitatorHouseRelation> GetAll();
        AgitatorHouseRelation Get(int Id);
        void Save(AgitatorHouseRelation obj);
        void Delete(int Id);
    }
}
