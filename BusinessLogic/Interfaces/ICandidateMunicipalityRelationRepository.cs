using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface ICandidateMunicipalityRelationRepository
    {
        IEnumerable<CandidateMunicipalityRelation> GetAll();
        CandidateMunicipalityRelation Get(int Id);
        void Save(CandidateMunicipalityRelation obj);
        void Delete(int Id);
    }
}
