using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface ILocalityRepository
    {
        IEnumerable<Locality> GetAll();
        Locality Get(int Id);
        void Save(Locality obj);
        void Delete(int Id);
    }
}
