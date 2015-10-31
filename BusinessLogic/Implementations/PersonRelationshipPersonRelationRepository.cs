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
    public class PersonRelationshipPersonRelationRepository : IPersonRelationshipPersonRelationRepository
    {
        private EFDbContext context;
        public PersonRelationshipPersonRelationRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<PersonRelationshipPersonRelation> GetAll()
        {
            return context.PersonRelationshipPersonRelations;
        }
        public PersonRelationshipPersonRelation Get(int Id)
        {
            return context.PersonRelationshipPersonRelations.Find(Id);
        }
        public void Save(PersonRelationshipPersonRelation obj)
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
