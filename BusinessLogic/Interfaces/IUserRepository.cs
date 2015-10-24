using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interfaces
{
    public interface IUserRepository
    {
        string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false);
        bool Login(string userName, string password, bool persistCookie = false);
        int GetUserId(string userName);
        bool ChangePassword(string userName, string currentPassword, string newPassword);
        string CreateAccount(string userName, string password, bool requireConfirmationToken = false);
        void Logout();

        IEnumerable<UserProfile> GetAll();
        UserProfile Get(int userId);
        UserProfile Get(string userName);
        void Save(UserProfile obj);
    }
}
