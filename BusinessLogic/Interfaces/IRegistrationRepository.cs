using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IRegistrationRepository
    {
        IEnumerable<Registration> GetAll();
        Registration Get(int Id);
        void Save(Registration obj);
        void Delete(int Id);
    }
}
