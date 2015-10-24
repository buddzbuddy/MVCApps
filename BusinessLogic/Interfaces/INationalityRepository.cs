using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface INationalityRepository
    {
        IEnumerable<Nationality> GetAll();
        Nationality Get(int Id);
        void Save(Nationality obj);
        void Delete(int Id);
    }
}
