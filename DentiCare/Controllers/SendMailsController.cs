using DentiCare.Models.Admin;
using DentiCare.Models.Appointments;
using DentiCare.Models.SendMails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DentiCare.Controllers
{
    [Authorize]
    public class SendMailsController : BaseController
    {
        [HttpGet]
        [Authorize]
        public ActionResult Index()
        {
            if (IsAdmin || IsDoctor || IsAssistant)
            {
                return RedirectToAction("SendMail");
            }
            else
            {
                return RedirectToAction("SendMailForPatient");
            }
        }

        #region SendMail (for patient)

        [HttpGet]
        [Authorize(Roles = "Pacjent")]
        public ActionResult SendMailForPatient()
        {
            var doctorsIDsNames = GetNamesOfDoctors();

            var model = new MailFromPatientViewModel
            {
                NamesOfDoctors = doctorsIDsNames
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Pacjent")]
        public async Task<ActionResult> SendMailForPatient(MailFromPatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                var receiverID = UserManager.Users.FirstOrDefault(x => x.user_info_id == model.Receiver).Id;
                string title = "Wiadomość od: " + CurrentUserName + ". Tytuł: " + model.Title;
                await UserManager.SendEmailAsync(receiverID, title, model.Body);

                TempData["Success"] = "Wysłano wiadomość.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd podczas wysyłania maila. Proszę sprobować jeszcze raz.";
            }

            return RedirectToAction("SendMailForPatient");
        }

        #endregion

        #region SendMail

        [HttpGet]
        [Authorize(Roles = "Stomatolog, Administrator, Asystentka")]
        public ActionResult SendMail()
        {
            var model = new MailViewModel
            {
                UserViews = GetUsersWithRoles(),
                Message = new Mail()
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog, Administrator, Asystentka")]
        public async Task<PartialViewResult> Send(string ReceiverID, string Title, string Body)
        {
            string title = "Wiadomość od: " + CurrentUserName + ". Tytuł: " + Title;
            await UserManager.SendEmailAsync(ReceiverID, title, Body);

            TempData["Success"] = "Wysłano wiadomość.";
            
            var model = new Mail();

            return PartialView("_Message", model);
        }

        #endregion

        #region Helper methods

        private Dictionary<int, string> GetNamesOfDoctors()
        {
            var names = from user in UserManager.Users
                        where user.medical_exp_id.HasValue
                        orderby user.UserInfo.last_name
                        select new UserNameWithIDs
                        {
                            UserInfoID = user.user_info_id,
                            FullName = user.UserInfo.first_name + " " + user.UserInfo.last_name
                        };

            return names.ToDictionary(x => x.UserInfoID, x => x.FullName);
        }

        private List<UserModel> GetUsersWithRoles()
        {
            var users = (from user in UserManager.Users
                         select user)
                         .ToList();

            var roles = (from role in RoleManager.Roles
                         select role)
                         .ToList();

            var userWithRoles = from user in users
                                where user.Id != CurrentUser.Id
                                join role in roles
                                on user.Roles.FirstOrDefault().RoleId equals role.Id
                                orderby role.Name, user.UserInfo.last_name
                                select new UserModel
                                {
                                    ID = user.Id,
                                    FirstName = user.UserInfo.first_name,
                                    LastName = user.UserInfo.last_name,
                                    Role = role.Name
                                };

            return userWithRoles.ToList();
        }

        #endregion
    }
}