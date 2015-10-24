using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPrecinctRepository
    {
        IEnumerable<Precinct> GetAll();
        Precinct Get(int Id);
        void Save(Precinct obj);
        void Delete(int Id);
    }
}
