using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPartyMemberRepository
    {
        IEnumerable<PartyMember> GetAll();
        PartyMember Get(int Id);
        void Save(PartyMember obj);
        void Delete(int Id);
    }
}
