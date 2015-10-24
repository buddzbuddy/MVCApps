using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IDistrictRepository
    {
        IEnumerable<District> GetAll();
        District Get(int Id);
        void Save(District obj);
        void Delete(int Id);
    }
}
