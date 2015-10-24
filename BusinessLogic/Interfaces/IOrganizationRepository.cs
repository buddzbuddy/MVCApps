using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IOrganizationRepository
    {
        IEnumerable<Organization> GetAll();
        Organization Get(int Id);
        void Save(Organization obj);
        void Delete(int Id);
    }
}
