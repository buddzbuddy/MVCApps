using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IVoterRepository
    {
        IEnumerable<Voter> GetAll();
        Voter Get(int Id);
        void Save(Voter obj);
        void Delete(int Id);
    }
}
