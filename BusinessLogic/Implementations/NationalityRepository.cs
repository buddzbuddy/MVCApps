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
    public class NationalityRepository : INationalityRepository
    {
        private EFDbContext context;

        public NationalityRepository(EFDbContext context)
        {
            this.context = context;
            //context.Regions.Include(r => r.Organizations);
        }
        public IEnumerable<Nationality> GetAll()
        {
            return context.Nationalities;
        }

        public Nationality Get(int Id)
        {
            return context.Nationalities.Find(Id);
        }

        public void Save(Nationality obj)
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
