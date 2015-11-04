using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;

namespace BusinessLogic.Implementations
{
    public class CandidateMunicipalityRelationRepository : ICandidateMunicipalityRelationRepository
    {
        private EFDbContext context;
        public CandidateMunicipalityRelationRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<CandidateMunicipalityRelation> GetAll()
        {
            return context.CandidateMunicipalityRelations;
        }
        public CandidateMunicipalityRelation Get(int Id)
        {
            return context.CandidateMunicipalityRelations.Find(Id);
        }
        public void Save(CandidateMunicipalityRelation obj)
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
                throw new ApplicationException("Объект связи \"Кандидат -> МТУ\" для удаления не найден!");
        }
    }
}
