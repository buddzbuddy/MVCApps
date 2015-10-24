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
    public class HouseRepository : IHouseRepository
    {
        private EFDbContext context;

        public HouseRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<House> GetAll()
        {
            return context.Houses;
        }

        public House Get(int Id)
        {
            return context.Houses.Find(Id);
        }

        public void Save(House obj)
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
