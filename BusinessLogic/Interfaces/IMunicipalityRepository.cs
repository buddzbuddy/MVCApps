using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMunicipalityRepository
    {
        IEnumerable<Municipality> GetAll();
        Municipality Get(int Id);
        void Save(Municipality obj);
        void Delete(int Id);
    }
}
