using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IPersonPartyRelationRepository
    {
        IEnumerable<PersonPartyRelation> GetAll();
        PersonPartyRelation Get(int Id);
        void Save(PersonPartyRelation obj);
        void Delete(int Id);
    }
}
