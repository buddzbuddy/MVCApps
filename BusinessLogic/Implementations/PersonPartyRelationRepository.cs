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
    public class PersonPartyRelationRepository : IPersonPartyRelationRepository
    {
        private EFDbContext context;
        public PersonPartyRelationRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<PersonPartyRelation> GetAll()
        {
            return context.PersonPartyRelations;
        }
        public PersonPartyRelation Get(int Id)
        {
            return context.PersonPartyRelations.Find(Id);
        }
        public void Save(PersonPartyRelation obj)
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
                throw new ApplicationException("Объект связи для удаления \"Гражданин->Партия\" не найден!");
        }
    }
}
