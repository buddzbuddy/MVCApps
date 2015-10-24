using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPersonRepository
    {
        IEnumerable<Person> GetAll();
        Person Get(int Id);
        void Save(Person obj);
        void Delete(int Id);
    }
}
