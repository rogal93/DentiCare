using DentiCare.Models.Admin;
using DentiCare.Models.UserSettings;
using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DentiCare.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IUserInformationsRepository _IUserInformationRepository;
        private readonly IAddressesRepository _IAddressesRepository;
        private readonly IMedicalExperienceRepository _IMedicalExperienceRepository;

        private readonly ITreatmentsRepository _ITreatmentsRepository;

        public AdminController(IUserInformationsRepository IUserInformationRepository,
                               IAddressesRepository IAddressesRepository,
                               IMedicalExperienceRepository IMedicalExperienceRepository,
                               ITreatmentsRepository ITreatmentsRepository) :base()
        {
            _IUserInformationRepository = IUserInformationRepository;
            _IAddressesRepository = IAddressesRepository;
            _IMedicalExperienceRepository = IMedicalExperienceRepository;
            _ITreatmentsRepository = ITreatmentsRepository;
        }

        #region Users

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var model = new DisplayUsersViewModel
            {
                UserViews = GetUsersWithRoles(),
                Details = new DisplayDetailsViewModel { PersonalDetails = new PersonalDetails { PersonalID = 0 } }
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public PartialViewResult ShowUserDetails(string Id, string Role)
        {
            ViewBag.Role = Role;

            var model = GetUserDetails(Id);

            return PartialView("_UserDetails", model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<PartialViewResult> SetAssistantRole(string Id, string Role)
        {
            await UserManager.RemoveFromRoleAsync(Id, Role);
            if (Role == "Stomatolog")
            {
                var user = UserManager.Users.FirstOrDefault(x => x.Id == Id);
                var medicalID = user.medical_exp_id.Value;
                user.medical_exp_id = null;
                await UserManager.UpdateAsync(user);
                _IMedicalExperienceRepository.DeleteMedicalExperience(medicalID);
            }
            await UserManager.AddToRoleAsync(Id, "Asystentka");

            TempData["Success"] = "Pomyślnie edytowano rolę.";

            var model = GetUsersWithRoles();

            return PartialView("_UsersList", model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<PartialViewResult> SetDoctorRole(string Id, string Role)
        {
            await UserManager.RemoveFromRoleAsync(Id, Role);
            await UserManager.AddToRoleAsync(Id, "Stomatolog");

            var user = UserManager.Users.FirstOrDefault(x => x.Id == Id);
            user.MedicalExperience = new MedicalExperience
            {
                graduation_date = DateTime.Today.AddDays(-1),
                university = "",
                specialization = "",
                about_myself = ""
            };
            await UserManager.UpdateAsync(user);

            TempData["Success"] = "Pomyślnie edytowano rolę.";

            var model = GetUsersWithRoles();

            return PartialView("_UsersList", model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<PartialViewResult> SetPatientRole(string Id, string Role)
        {
            await UserManager.RemoveFromRoleAsync(Id, Role);
            if (Role == "Stomatolog")
            {
                var user = UserManager.Users.FirstOrDefault(x => x.Id == Id);
                var medicalID = user.medical_exp_id.Value;
                user.medical_exp_id = null;
                await UserManager.UpdateAsync(user);
                _IMedicalExperienceRepository.DeleteMedicalExperience(medicalID);
            }
            await UserManager.AddToRoleAsync(Id, "Pacjent");

            TempData["Success"] = "Pomyślnie edytowano rolę.";

            var model = GetUsersWithRoles();

            return PartialView("_UsersList", model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public PartialViewResult RefreshUserDetails()
        {
            var model = new DisplayDetailsViewModel { PersonalDetails = new PersonalDetails { PersonalID = 0 } };

            return PartialView("_UserDetails", model);
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditAddress(AddressDetails model)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(model.PostalCity))
                {
                    model.PostalCity = model.City;
                }

                _IAddressesRepository.EditAddress(new Address
                {
                    id = model.AddressID,
                    city = model.City,
                    house_number = model.HouseNumber,
                    apartment_number = model.ApartmentNumber,
                    street = model.Street,
                    postal_city = model.PostalCity,
                    postal_code = model.PostalCode
                });
                TempData["Success"] = "Pomyślnie edytowano adres.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd. Spróbuj jeszcze raz.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditPersonalDetails(PersonalDetails model)
        {
            if (ModelState.IsValid)
            {
                _IUserInformationRepository.EditUserInformation(new UserInformation
                {
                    id = model.PersonalID,
                    first_name = model.FirstName,
                    last_name = model.LastName,
                    gender = model.Gender,
                    register_date = model.RegisterDate
                });

                TempData["Success"] = "Pomyślnie edytowano dane personalne.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd. Spróbuj jeszcze raz.";
            }

            return RedirectToAction("Index");
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteUser(string Id, string Role)
        {
            ViewBag.MyID = CurrentUser.Id;
            
            var user = UserManager.Users.FirstOrDefault(x => x.Id == Id);
            var user_info = _IUserInformationRepository.UserInformations.FirstOrDefault(x => x.id == user.user_info_id);
            var address = _IAddressesRepository.Addresses.FirstOrDefault(x => x.id == user_info.address_id);
            
            await UserManager.RemoveFromRoleAsync(Id, Role);
            await UserManager.DeleteAsync(user);
            _IUserInformationRepository.DeleteUserInformation(user_info.id);
            _IAddressesRepository.DeleteAddress(address.id);
            
            return RedirectToAction("Index");
        }

        #endregion
        
        #region Treatments

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public ActionResult Treatments()
        {
            var treatments = GetTreatments();

            var model = new DisplayTreatmentsViewModel
            {
                TreatmentViews = treatments,
                Details = new TreatmentDetailsModel(),
                Empty = new TreatmentDetailsModel()
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public PartialViewResult ShowTreatmentDetails(int Id)
        {
            var model = GetTreatmentDetails(Id);

            return PartialView("_EditTreatmentModalPartial", model);
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public PartialViewResult AddTreatment(TreatmentDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                var category_id = _ITreatmentsRepository.TreatmentCategories
                                  .FirstOrDefault(x => x.name == model.Category)
                                  .id;

                _ITreatmentsRepository.CreateTreatment(new Treatment
                {
                    name = model.Name,
                    price = model.Price,
                    category_id = category_id
                });
                TempData["Success"] = "Pomyślnie dodano zabieg.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd przy dodawaniu zabiegu. Spróbuj jeszcze raz.";
            }

            var treatments = GetTreatments();

            return PartialView("_TreatmentsList", treatments);
        }
        
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditTreatment(TreatmentDetailsModel model)
        {
            if (ModelState.IsValid)
            {
                var category_id = _ITreatmentsRepository.TreatmentCategories
                                  .FirstOrDefault(x => x.name == model.Category)
                                  .id;

                _ITreatmentsRepository.EditTreatment(new Treatment
                {
                    id = model.TreatmentID,
                    name = model.Name,
                    price = model.Price,
                    category_id = category_id
                });

                TempData["Success"] = "Pomyślnie edytowano zabieg.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd przy edycji zabiegu. Spróbuj jeszcze raz.";
            }
            
            var treatments = GetTreatments();
            return PartialView("_TreatmentsList", treatments);
        }
        
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public PartialViewResult DeleteTreatment(int id)
        {
            _ITreatmentsRepository.DeleteTreatment(id);
            var empty = new TreatmentDetailsModel { Name = "", Category = "", Price = 0 };

            TempData["Success"] = "Pomyślnie usunięto zabieg.";

            var treatments = GetTreatments();

            return PartialView("_TreatmentsList", treatments);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public PartialViewResult RefreshTreatmentsList()
        {
            var treatments = GetTreatments();

            return PartialView("_TreatmentsList", treatments);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public PartialViewResult ResetAddModal()
        {
            var empty = new TreatmentDetailsModel();
            return PartialView("_AddTreatmentModalPartial", empty);
        }

        #endregion

        #region Helper methods Users

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
            else if(user.UserInfo.gender == "Kobieta")
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
        
        private void EditPhoneNumber(AccountDetails details)
        {
            var token = UserManager.GenerateChangePhoneNumberTokenAsync(details.ID, details.PhoneNumber).Result;
            UserManager.ChangePhoneNumberAsync(details.ID, details.PhoneNumber, token);
        }
        private void EditPersonal(PersonalDetails details)
        {
            var old = _IUserInformationRepository.UserInformations.FirstOrDefault(x => x.id == details.PersonalID);

            _IUserInformationRepository.EditUserInformation(new UserInformation
            {
                id = details.PersonalID,
                address_id = old.id,
                first_name = details.FirstName,
                last_name = details.LastName,
                gender = details.Gender,
                register_date = old.register_date
            });
        }
        private void EditMedical(MedicalDetails details)
        {
            _IMedicalExperienceRepository.EditMedicalExperience(new MedicalExperience
            {
                id = details.MedicalID,
                university = details.University,
                specialization = details.Specialization,
                graduation_date = details.GraduationDate,
                about_myself = details.AboutMyself
            });
        }

        #endregion

        #region Helper methods Treatments

        private List<TreatmentModel> GetTreatments()
        {
            var treatments = from t in _ITreatmentsRepository.Treatments
                             join c in _ITreatmentsRepository.TreatmentCategories
                             on t.category_id equals c.id
                             orderby c.name, t.name
                             select new TreatmentModel
                             {
                                 ID = t.id,
                                 Name = t.name,
                                 Category = c.name,
                                 Price = t.price
                             };

            return treatments.ToList();
        }
        private TreatmentDetailsModel GetTreatmentDetails(int id)
        {
            var details = (from t in _ITreatmentsRepository.Treatments
                           where t.id == id
                           join c in _ITreatmentsRepository.TreatmentCategories
                           on t.category_id equals c.id
                           select new TreatmentDetailsModel
                           {
                               TreatmentID = t.id,
                               Name = t.name,
                               Price = t.price,
                               Category = c.name
                           })
                           .FirstOrDefault();

            return details;
        }

        #endregion
    }
}