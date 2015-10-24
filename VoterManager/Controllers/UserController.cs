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
                            Worker = dataManager.Workers.Get(u.WorkerId ?? 0)
                        });
        }

        [HttpGet]
        public ActionResult Create()
        {
            var existWorkers = dataManager.Users.GetAll().Where(u => u.WorkerId.HasValue).Select(u => u.WorkerId.Value);
            var workers = new List<SelectListItem> { new SelectListItem() };
            workers.AddRange(from w in dataManager.Workers.GetAll()
                             where !existWorkers.Contains(w.Id)
                             select new SelectListItem
                             {
                                 Text = w.FullName,
                                 Value = w.Id.ToString()
                             });
            ViewBag.Workers = workers;
            return View();
        }

        [HttpPost]
        public ActionResult Create(UserProfile obj)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    dataManager.Users.CreateUserAndAccount(obj.UserName, obj.Password, new { Password = obj.Password, WorkerId = obj.WorkerId });
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
            if (obj == null)
                return HttpNotFound();
            var existWorkers = dataManager.Users.GetAll().Where(u => u.WorkerId.HasValue).Select(u => u.WorkerId.Value).ToList();
            if (obj.WorkerId.HasValue)
                existWorkers.Remove(obj.WorkerId.Value);
            var workers = new List<SelectListItem> { new SelectListItem() };
            workers.AddRange(from w in dataManager.Workers.GetAll()
                             where !existWorkers.Contains(w.Id)
                             select new SelectListItem
                             {
                                 Text = w.FullName,
                                 Value = w.Id.ToString(),
                                 Selected = w.Id == obj.WorkerId
                             });
            ViewBag.Workers = workers;
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(UserProfile obj)
        {
            if (ModelState.IsValid)
            {
                var obj2 = dataManager.Users.Get(obj.UserName);
                if(!(obj2 != null && obj2.UserId != obj.UserId))
                {
                    var objFromDB = dataManager.Users.Get(obj.UserId);
                    if (dataManager.Users.ChangePassword(objFromDB.UserName, objFromDB.Password, obj.Password))
                    {
                        objFromDB.UserName = obj.UserName;
                        objFromDB.Password = obj.Password;
                        objFromDB.WorkerId = obj.WorkerId;
                        dataManager.Users.Save(objFromDB);
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(MembershipCreateStatus.DuplicateUserName));
                }
            }
            var existWorkers = dataManager.Users.GetAll().Where(u => u.WorkerId.HasValue).Select(u => u.WorkerId.Value).ToList();
            if (obj.WorkerId.HasValue)
                existWorkers.Remove(obj.WorkerId.Value);
            var workers = new List<SelectListItem> { new SelectListItem() };
            workers.AddRange(from w in dataManager.Workers.GetAll()
                             where !existWorkers.Contains(w.Id)
                             select new SelectListItem
                             {
                                 Text = w.FullName,
                                 Value = w.Id.ToString(),
                                 Selected = w.Id == obj.WorkerId
                             });
            ViewBag.Workers = workers;
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
