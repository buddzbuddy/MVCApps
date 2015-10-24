using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementations
{
    public class PartyRepository : IPartyRepository
    {
        private EFDbContext context;

        public PartyRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Party> GetAll()
        {
            return context.Parties;
        }

        public Party Get(int Id)
        {
            return context.Parties.Find(Id);
        }

        public void Save(Party obj)
        {
            if (obj.Id == 0)
                context.Entry(obj).State = System.Data.Entity.EntityState.Added;
            else
                context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }

        public void Delete(int Id)
        {
            var obj = Get(Id);
            context.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
            context.SaveChanges();
        }
    }
}
