using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Implementations
{
    public class AgitatorPrecinctRelationRepository : IAgitatorPrecinctRelationRepository
    {
        private EFDbContext context;
        public AgitatorPrecinctRelationRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<AgitatorPrecinctRelation> GetAll()
        {
            return context.AgitatorPrecinctRelations;
        }
        public AgitatorPrecinctRelation Get(int Id)
        {
            return context.AgitatorPrecinctRelations.Find(Id);
        }
        public void Save(AgitatorPrecinctRelation obj)
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
                throw new ApplicationException("Объект связи \"Агитатор -> УИК\" для удаления не найден!");
        }
    }
}
