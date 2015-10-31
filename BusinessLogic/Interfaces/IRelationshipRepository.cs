using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IRelationshipRepository
    {
        IEnumerable<Relationship> GetAll();
        Relationship Get(int Id);
        void Save(Relationship obj);
        void Delete(int Id);
    }
}
