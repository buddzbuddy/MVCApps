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
    public class RegistrationRepository : IRegistrationRepository
    {
        private EFDbContext context;

        public RegistrationRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Registration> GetAll()
        {
            return context.Registrations;
        }

        public Registration Get(int Id)
        {
            return context.Registrations.Find(Id);
        }

        public void Save(Registration obj)
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
