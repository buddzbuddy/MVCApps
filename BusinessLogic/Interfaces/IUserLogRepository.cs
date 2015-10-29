using Domain.Entities;
using System.Collections.Generic;

namespace BusinessLogic.Interfaces
{
    public interface IUserLogRepository
    {
        IEnumerable<UserLog> GetAll();
        UserLog Get(int Id);
        void Save(UserLog obj);
        void Delete(int Id);
    }
}
