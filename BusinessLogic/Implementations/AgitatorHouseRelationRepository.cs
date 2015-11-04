using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Implementations
{
    public class AgitatorHouseRelationRepository : IAgitatorHouseRelationRepository
    {
        private EFDbContext context;
        public AgitatorHouseRelationRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<AgitatorHouseRelation> GetAll()
        {
            return context.AgitatorHouseRelations;
        }
        public AgitatorHouseRelation Get(int Id)
        {
            return context.AgitatorHouseRelations.Find(Id);
        }
        public void Save(AgitatorHouseRelation obj)
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
                throw new ApplicationException("Объект связи \"Агитатор -> Дом\" для удаления не найден!");
        }
    }
}
