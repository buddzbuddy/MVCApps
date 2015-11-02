using BusinessLogic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using VoterManager.Models;

namespace VoterManager.Controllers
{
    public class UserController : Controller
    {
        private DataManager dataManager;
        public UserController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
        public ActionResult Index()
        {
            return View(from u in dataManager.Users.GetAll()
                        select new UserViewModel
                        {
                            UserProfile = u,
                            Person = dataManager.Persons.Get(u.PersonId ?? 0)
                        });
        }

        [HttpGet]
        public ActionResult Create(int? personId)
        {
            if (!personId.HasValue)
                return RedirectToAction("CreateBase", "Person", new { returnUrl = Request.Url.ToString(), processName = "Создать пользователя системы" });
            return View(new UserProfile { PersonId = personId });
        }

        [HttpPost]
        public ActionResult Create(UserProfile obj)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    dataManager.Users.CreateUserAndAccount(obj.UserName, obj.Password, new { Password = obj.Password, PersonId = obj.PersonId });
                    return RedirectToAction("Index");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var obj = dataManager.Users.Get(Id);
            
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(UserProfile obj, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var obj2 = dataManager.Users.Get(obj.UserName);
                if(!(obj2 != null && obj2.UserId != obj.UserId))
                {
                    var objFromDb = dataManager.Users.Get(obj.UserId);
                    if (dataManager.Users.ChangePassword(obj.UserName, objFromDb.Password, obj.Password))
                    {
                        objFromDb.PersonId = obj.PersonId;
                        objFromDb.UserName = obj.UserName;
                        objFromDb.Password = obj.Password;
                        dataManager.Users.Save(objFromDb);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", ErrorCodeToString(MembershipCreateStatus.DuplicateUserName));
                }
            }
            return View(obj);
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // Полный список кодов состояния см. по адресу http://go.microsoft.com/fwlink/?LinkID=177550
            //.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Имя пользователя уже существует. Введите другое имя пользователя.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Имя пользователя для данного адреса электронной почты уже существует. Введите другой адрес электронной почты.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Указан недопустимый пароль. Введите допустимое значение пароля.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Указан недопустимый адрес электронной почты. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Указан недопустимый ответ на вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "Указан недопустимый вопрос для восстановления пароля. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.InvalidUserName:
                    return "Указано недопустимое имя пользователя. Проверьте значение и повторите попытку.";

                case MembershipCreateStatus.ProviderError:
                    return "Поставщик проверки подлинности вернул ошибку. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                case MembershipCreateStatus.UserRejected:
                    return "Запрос создания пользователя был отменен. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";

                default:
                    return "Произошла неизвестная ошибка. Проверьте введенное значение и повторите попытку. Если проблему устранить не удастся, обратитесь к системному администратору.";
            }
        }
    }
}
