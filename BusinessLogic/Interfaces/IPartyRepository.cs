using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPartyRepository
    {
        IEnumerable<Party> GetAll();
        Party Get(int Id);
        void Save(Party obj);
        void Delete(int Id);
    }
}
