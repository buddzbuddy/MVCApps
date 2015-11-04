using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Implementations
{
    public class CandidatePrecinctRelationRepository : ICandidatePrecinctRelationRepository
    {
        private EFDbContext context;
        public CandidatePrecinctRelationRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<CandidatePrecinctRelation> GetAll()
        {
            return context.CandidatePrecinctRelations;
        }
        public CandidatePrecinctRelation Get(int Id)
        {
            return context.CandidatePrecinctRelations.Find(Id);
        }
        public void Save(CandidatePrecinctRelation obj)
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
            if (obj != null)
            {
                context.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                context.SaveChanges();
            }
            else
                throw new ApplicationException("Объект связи \"Кандидат -> УИК\" для удаления не найден!");
        }
    }
}
