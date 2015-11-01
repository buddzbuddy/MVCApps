using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Implementations
{
    public class AgitatorRepository : IAgitatorRepository
    {
        private EFDbContext context;

        public AgitatorRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Agitator> GetAll()
        {
            return context.Agitators;
        }

        public Agitator Get(int Id)
        {
            return context.Agitators.Find(Id);
        }

        public void Save(Agitator obj)
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
