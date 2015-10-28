using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IVoterPartyRelationRepository
    {
        IEnumerable<VoterPartyRelation> GetAll();
        VoterPartyRelation Get(int Id);
        void Save(VoterPartyRelation obj);
        void Delete(int Id);
    }
}
