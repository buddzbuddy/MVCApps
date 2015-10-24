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
    public class OrganizationRepository : IOrganizationRepository
    {
        private EFDbContext context;

        public OrganizationRepository(EFDbContext context)
        {
            this.context = context;
            //context.Regions.Include(r => r.Organizations);
        }
        public IEnumerable<Organization> GetAll()
        {
            return context.Organizations;
        }

        public Organization Get(int Id)
        {
            return context.Organizations.Find(Id);
        }

        public void Save(Organization obj)
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
