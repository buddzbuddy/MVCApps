using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface ICandidatePrecinctRelationRepository
    {
        IEnumerable<CandidatePrecinctRelation> GetAll();
        CandidatePrecinctRelation Get(int Id);
        void Save(CandidatePrecinctRelation obj);
        void Delete(int Id);
    }
}
