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
    public class PartySupporterRepository : IPartySupporterRepository
    {
        private EFDbContext context;

        public PartySupporterRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<PartySupporter> GetAll()
        {
            return context.PartySupporters;
        }

        public PartySupporter Get(int Id)
        {
            return context.PartySupporters.Find(Id);
        }

        public void Save(PartySupporter obj)
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
