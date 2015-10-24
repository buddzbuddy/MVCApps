using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IEducationRepository
    {
        IEnumerable<Education> GetAll();
        Education Get(int Id);
        void Save(Education obj);
        void Delete(int Id);
    }
}
