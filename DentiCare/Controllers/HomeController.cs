using DentiCare.Models.Admin;
using DentiCare.Models.Schedules;
using DentiCare.Models.UserSettings;
using DentiCare.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DentiCare.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IUserInformationsRepository _IUserInformationRepository;
        private readonly IAddressesRepository _IAddressesRepository;
        private readonly ISchedulesRepository _ISchedulesRepository;

        public HomeController(ISchedulesRepository ISchedulesRepository,
                              IUserInformationsRepository IUserInformationsRepository,
                              IAddressesRepository IAddressesRepository) :base()
        {
            _IUserInformationRepository = IUserInformationsRepository;
            _IAddressesRepository = IAddressesRepository;
            _ISchedulesRepository = ISchedulesRepository;
        }

        #region Index action

        [HttpGet]
        public ActionResult Index()
        {
            if (IsAdmin)
            {
                return RedirectToAction("Index", "Admin");
            }
            else if (IsDoctor)
            {
                var twoWeeksLater = DateTime.Today.AddDays(14);
                if (twoWeeksLater.DayOfWeek == DayOfWeek.Sunday)
                {
                    twoWeeksLater = twoWeeksLater.AddDays(1);
                }

                var twoWeeksLaterSchedule = _ISchedulesRepository.Schedules
                                            .FirstOrDefault(x => x.doctor_id == CurrentUserInfo.id &&
                                                                 x.date == twoWeeksLater);
                if (twoWeeksLaterSchedule == null)
                {
                    return RedirectToAction("EnterSchedule", "Schedules");
                }

                return RedirectToAction("TodayVisits", "Appointments");
            }
            else if (IsAssistant)
            {
                return RedirectToAction("TodayVisitsForAssistant", "Appointments");
            }
            else if (LoggedOn)
            {
                return RedirectToAction("Index", "Appointments");
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        #endregion

        #region Doctors, Contact, RefreshAlerts actions

        [HttpGet]
        public ActionResult Doctors()
        {
            var doctors = GetDoctorsInformationsQuery();

            return View(doctors);
        }

        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        
        [HttpGet]
        public PartialViewResult RefreshAlerts()
        {
            return PartialView("_Alerts");
        }

        #endregion

        #region AllUsers (for Assistant)

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public ActionResult AllUsersForAssistant()
        {
            var model = new DisplayUsersViewModel
            {
                UserViews = GetUsersWithRoles(),
                Details = new DisplayDetailsViewModel { PersonalDetails = new PersonalDetails { PersonalID = 0 } }
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public PartialViewResult ShowUserDetails(string Id)
        {
            var model = GetUserDetails(Id);

            return PartialView("_UserDetails", model);
        }

        #endregion
        
        #region Helper methods

        private List<DoctorInfo> GetDoctorsInformationsQuery()
        {
            var doctors = from doctor in UserManager.Users
                          where doctor.medical_exp_id.HasValue
                          select new DoctorInfo
                          {
                              Id = doctor.medical_exp_id.Value,
                              FullName = "lek. stom. " + doctor.UserInfo.first_name +
                                         " " + doctor.UserInfo.last_name,
                              Specialization = doctor.MedicalExperience.specialization,
                              University = doctor.MedicalExperience.university,
                              AboutMyself = doctor.MedicalExperience.about_myself,
                              GraduationDate = doctor.MedicalExperience.graduation_date,
                              Photo = doctor.UserInfo.Photo.url
                          };

            return doctors.ToList();
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
        private DisplayDetailsViewModel GetUserDetails(string Id)
        {
            var user = (from u in UserManager.Users
                        where u.Id == Id
                        select u)
                        .FirstOrDefault();

            var info = _IUserInformationRepository.UserInformations.FirstOrDefault(x => x.id == user.user_info_id);

            var user_address = _IAddressesRepository.Addresses.FirstOrDefault(x => x.id == info.address_id);

            AccountDetails acc = new AccountDetails
            {
                ID = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            AddressDetails address = new AddressDetails
            {
                AddressID = user_address.id,
                City = user_address.city,
                Street = user_address.street,
                HouseNumber = user_address.house_number,
                ApartmentNumber = user_address.apartment_number,
                PostalCode = user_address.postal_code,
                PostalCity = user_address.postal_city
            };
            PersonalDetails personal = new PersonalDetails
            {
                PersonalID = user.UserInfo.id,
                FirstName = user.UserInfo.first_name,
                LastName = user.UserInfo.last_name,
                Gender = user.UserInfo.gender,
                RegisterDate = user.UserInfo.register_date
            };

            string photo = Url.Content("/Images/Photos/profile-photo.jpg");

            if (user.UserInfo.photo_id.HasValue)
            {
                photo = user.UserInfo.Photo.url;
            }
            else if (user.UserInfo.gender == "Kobieta")
            {
                photo = Url.Content("/Images/Photos/profile-photo-female.jpg");
            }

            var details = new DisplayDetailsViewModel
            {
                AccountDetails = acc,
                AddressDetails = address,
                PersonalDetails = personal,
                Photo = photo
            };

            return details;
        }

        #endregion
    }
}