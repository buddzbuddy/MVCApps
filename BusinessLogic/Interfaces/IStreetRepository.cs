using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IStreetRepository
    {
        IEnumerable<Street> GetAll();
        Street Get(int Id);
        void Save(Street obj);
        void Delete(int Id);
    }
}
