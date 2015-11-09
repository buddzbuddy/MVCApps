using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ITempPersonRepository
    {
        IEnumerable<TempPerson> GetAll();
        TempPerson Get(int Id);
        void Save(TempPerson obj);
        void Delete(int Id);
    }
}
