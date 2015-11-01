using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IAgitatorRepository
    {
        IEnumerable<Agitator> GetAll();
        Agitator Get(int Id);
        void Save(Agitator obj);
        void Delete(int Id);
    }
}
