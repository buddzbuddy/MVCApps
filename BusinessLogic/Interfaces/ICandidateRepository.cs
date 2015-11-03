using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface ICandidateRepository
    {
        IEnumerable<Candidate> GetAll();
        Candidate Get(int Id);
        void Save(Candidate obj);
        void Delete(int Id);
    }
}
