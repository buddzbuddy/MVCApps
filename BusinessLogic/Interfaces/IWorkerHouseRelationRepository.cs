using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IWorkerHouseRelationRepository
    {
        IEnumerable<WorkerHouseRelation> GetAll();
        WorkerHouseRelation Get(int Id);
        void Save(WorkerHouseRelation obj);
        void Delete(int Id);
    }
}
