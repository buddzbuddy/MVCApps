using BusinessLogic.Interfaces;
using Domain;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebMatrix.WebData;

namespace BusinessLogic.Implementations
{
    public class UserRepository : IUserRepository
    {
        private EFDbContext context;
        public UserRepository(EFDbContext context)
        {
            this.context = context;
        }
        public string CreateUserAndAccount(string userName, string password, object propertyValues = null, bool requireConfirmationToken = false)
        {
            return WebSecurity.CreateUserAndAccount(userName, password, propertyValues, requireConfirmationToken);
        }

        public bool Login(string userName, string password, bool persistCookie = false)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }

        public bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(userName, currentPassword, newPassword);
        }

        public string CreateAccount(string userName, string password, bool requireConfirmationToken = false)
        {
            return WebSecurity.CreateAccount(userName, password, requireConfirmationToken);
        }

        public void Logout()
        {
            WebSecurity.Logout();
        }

        public IEnumerable<UserProfile> GetAll()
        {
            return context.UserProfiles;
        }

        public UserProfile Get(int userId)
        {
            return context.UserProfiles.Find(userId);
        }

        public UserProfile Get(string userName)
        {
            return Get(GetUserId(userName));
        }

        public void Save(UserProfile obj)
        {
            context.Entry(obj).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
        }
    }
}
