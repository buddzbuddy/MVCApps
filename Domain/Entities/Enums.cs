using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Типы сущностей
    /// </summary>
    public enum VoterManagerEntityTypes
    {
        District,
        Locality,
        Street,
        House,
        Municipality,
        MunicipalityHouseRelation,
        Nationality,
        Education,
        Organization,
        Registration,
        Person,
        Party,
        Precinct
    }
}
