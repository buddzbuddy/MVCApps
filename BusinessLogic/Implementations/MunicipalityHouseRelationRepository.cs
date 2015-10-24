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
    public class MunicipalityHouseRelationRepository : IMunicipalityHouseRelationRepository
    {
        private EFDbContext context;
        public MunicipalityHouseRelationRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<MunicipalityHouseRelation> GetAll()
        {
            return context.MunicipalityHouseRelations;
        }
        public MunicipalityHouseRelation Get(int Id)
        {
            return context.MunicipalityHouseRelations.Find(Id);
        }
        public void Save(MunicipalityHouseRelation obj)
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
                throw new ApplicationException("Объект связи для удаления \"МТУ->ДОМ\" не найден!");
        }
    }
}
