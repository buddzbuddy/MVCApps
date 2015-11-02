using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Implementations
{
    public class WorkerHouseRelationRepository : IWorkerHouseRelationRepository
    {
        private EFDbContext context;
        public WorkerHouseRelationRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<WorkerHouseRelation> GetAll()
        {
            return context.WorkerHouseRelations;
        }
        public WorkerHouseRelation Get(int Id)
        {
            return context.WorkerHouseRelations.Find(Id);
        }
        public void Save(WorkerHouseRelation obj)
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
                throw new ApplicationException("Объект связи \"Работник штаба -> Дом\" для удаления не найден!");
        }
    }
}
