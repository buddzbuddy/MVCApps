using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Implementations
{
    public class UserLogRepository : IUserLogRepository
    {
        private EFDbContext context;

        public UserLogRepository(EFDbContext context)
        {
            this.context = context;
        }
        public IEnumerable<UserLog> GetAll()
        {
            return context.UserLogs;
        }

        public UserLog Get(int Id)
        {
            return context.UserLogs.Find(Id);
        }

        public void Save(UserLog obj)
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
