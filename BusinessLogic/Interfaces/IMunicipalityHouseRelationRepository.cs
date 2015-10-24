using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IMunicipalityHouseRelationRepository
    {
        IEnumerable<MunicipalityHouseRelation> GetAll();
        MunicipalityHouseRelation Get(int Id);
        void Save(MunicipalityHouseRelation obj);
        void Delete(int Id);
    }
}
