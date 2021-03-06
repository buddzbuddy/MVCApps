﻿using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Implementations
{
    public class MunicipalityRepository : IMunicipalityRepository
    {
        private EFDbContext context;

        public MunicipalityRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<Municipality> GetAll()
        {
            return context.Municipalities;
        }

        public Municipality Get(int Id)
        {
            return context.Municipalities.Find(Id);
        }

        public void Save(Municipality obj)
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
