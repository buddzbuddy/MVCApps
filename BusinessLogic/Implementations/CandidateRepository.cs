using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Implementations
{
    public class CandidateRepository : ICandidateRepository
    {
        private EFDbContext context;

        public CandidateRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Candidate> GetAll()
        {
            return context.Candidates;
        }

        public Candidate Get(int Id)
        {
            return context.Candidates.Find(Id);
        }

        public void Save(Candidate obj)
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
