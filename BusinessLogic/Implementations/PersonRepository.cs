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
    public class PersonRepository : IPersonRepository
    {
        private EFDbContext context;

        public PersonRepository(EFDbContext context)
        {
            this.context = context;
            //context.Regions.Include(r => r.Organizations);
        }
        public IEnumerable<Person> GetAll()
        {
            return context.Persons;
        }

        public Person Get(int Id)
        {
            return context.Persons.Find(Id);
        }

        public void Save(Person obj)
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
