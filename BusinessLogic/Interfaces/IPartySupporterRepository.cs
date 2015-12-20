using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPartySupporterRepository
    {
        IEnumerable<PartySupporter> GetAll();
        PartySupporter Get(int Id);
        void Save(PartySupporter obj);
        void Delete(int Id);
    }
}
