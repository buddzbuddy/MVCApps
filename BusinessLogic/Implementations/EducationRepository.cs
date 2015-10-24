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
    public class EducationRepository : IEducationRepository
    {
        private EFDbContext context;

        public EducationRepository(EFDbContext context)
        {
            this.context = context;
            //context.Regions.Include(r => r.Organizations);
        }
        public IEnumerable<Education> GetAll()
        {
            return context.Educations;
        }

        public Education Get(int Id)
        {
            return context.Educations.Find(Id);
        }

        public void Save(Education obj)
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
