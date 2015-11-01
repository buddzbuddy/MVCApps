using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Implementations
{
    public class PersonRelationRepository : IPersonRelationRepository
    {
        private EFDbContext context;
        public PersonRelationRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<PersonRelation> GetAll()
        {
            return context.PersonRelations;
        }
        public PersonRelation Get(int Id)
        {
            return context.PersonRelations.Find(Id);
        }
        public void Save(PersonRelation obj)
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
                throw new ApplicationException("Объект связи \"Физ. лицо -> Связанное физ. лицо\" для удаления не найден!");
        }
    }
}
