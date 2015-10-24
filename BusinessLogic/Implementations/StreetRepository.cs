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
    public class StreetRepository : IStreetRepository
    {
        private EFDbContext context;

        public StreetRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Street> GetAll()
        {
            return context.Streets;
        }

        public Street Get(int Id)
        {
            return context.Streets.Find(Id);
        }

        public void Save(Street obj)
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
