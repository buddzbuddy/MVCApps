using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Implementations
{
    public class VoterRepository : IVoterRepository
    {
        private EFDbContext context;

        public VoterRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Voter> GetAll()
        {
            return context.Voters;
        }

        public Voter Get(int Id)
        {
            return context.Voters.Find(Id);
        }

        public void Save(Voter obj)
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
