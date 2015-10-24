using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IHouseRepository
    {
        IEnumerable<House> GetAll();
        House Get(int Id);
        void Save(House obj);
        void Delete(int Id);
    }
}
