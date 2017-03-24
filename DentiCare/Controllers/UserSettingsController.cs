using DentiCare.Models.UserSettings;
using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DentiCare.Controllers
{
    [Authorize]
    public class UserSettingsController : BaseController
    {
        private readonly IPhotosRepository _IPhotosRepository;
        private readonly IUserInformationsRepository _IUserInformationsRepository;
        private readonly IAddressesRepository _IAddressesRepository;
        private readonly IMedicalExperienceRepository _IMedicalExperienceRepository;
        
        public UserSettingsController(IAddressesRepository IAddressesRepository,
                                      IUserInformationsRepository IUserInformationsRepository,
                                      IPhotosRepository IPhotosRepository,
                                      IMedicalExperienceRepository IMedicalExperienceRepository) :base()
        {
            _IAddressesRepository = IAddressesRepository;
            _IPhotosRepository = IPhotosRepository;
            _IMedicalExperienceRepository = IMedicalExperienceRepository;
            _IUserInformationsRepository = IUserInformationsRepository;
        }

        #region Index Action

        [HttpGet]
        public ActionResult Index()
        {
            var model = GetDisplayDetailsViewModel();

            return View(model);
        }

        #endregion

        #region ChangeAddress Actions

        [HttpGet]
        public ActionResult ChangeAddress()
        {
            var model = new NewAddressViewModel
            {
                PhoneNumber = CurrentUser.PhoneNumber,

                City = CurrentUserAddress.city,
                Street = CurrentUserAddress.street,
                HouseNumber = CurrentUserAddress.house_number,
                ApartamentNumber = CurrentUserAddress.apartment_number,
                
                PostalCode = CurrentUserAddress.postal_code,
                PostalCity = CurrentUserAddress.postal_city
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeAddress(NewAddressViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (CurrentUser.PhoneNumber != model.PhoneNumber)
                {
                    var token = UserManager.GenerateChangePhoneNumberTokenAsync(CurrentUser.Id, model.PhoneNumber).Result;
                    UserManager.ChangePhoneNumberAsync(CurrentUser.Id, model.PhoneNumber, token);
                }

                var newAddress = new Address
                {
                    id = CurrentUserAddress.id,
                    city = model.City,
                    street = model.Street,
                    house_number = model.HouseNumber,
                    apartment_number = model.ApartamentNumber,
                    postal_code = model.PostalCode,
                    postal_city = model.PostalCity
                };

                _IAddressesRepository.EditAddress(newAddress);
                TempData["Success"] = "Zmieniono dane adresowe.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd. Spróbuj jeszcze raz.";
            }

            return View();
        }

        #endregion

        #region SetPhoto Actions

        [HttpGet]
        public ActionResult SetPhoto()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SetPhoto(SetPhotoModel model)
        {
            if (ModelState.IsValid)
            {
                var result = UploadPhoto(model.PhotoFile);
                if (result)
                {
                    var photo = new Photo { url = Url.Content("/Images/Photos/" + model.PhotoFile.FileName) };

                    if (!CurrentUserInfo.photo_id.HasValue)
                    {
                        _IPhotosRepository.AddPhoto(photo);

                        _IUserInformationsRepository.EditUserInformation(new UserInformation
                        {
                            id = CurrentUserInfo.id,
                            address_id = CurrentUserInfo.address_id,
                            first_name = CurrentUserInfo.first_name,
                            last_name = CurrentUserInfo.last_name,
                            gender = CurrentUserInfo.gender,
                            photo_id = photo.id,
                            register_date = CurrentUserInfo.register_date
                        });
                    }
                    else
                    {
                        var oldPhotoPath = _IPhotosRepository.Photos.FirstOrDefault(x => x.id == CurrentUserInfo.photo_id.Value).url;
                        var fullPath = Server.MapPath(oldPhotoPath);
                        DeletePhoto(fullPath);

                        photo.id = CurrentUserInfo.photo_id.Value;
                        _IPhotosRepository.ChangePhoto(photo);
                    }
                }
            }
            else
            {
                TempData["Warning"] = "Zdjęcie musi mieć rozszerzenie .jpg, .jpeg lub .png i rozmiar niewiększy niż 3MB.";
            }

            return RedirectToAction("SetPhoto");
        }

        #endregion

        #region ChangeMedicalExperience

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public ActionResult ChangeMedicalExperience()
        {
            var model = new MedicalDetails
            {
                MedicalID = CurrentUserMedicalExperience.id,
                University = CurrentUserMedicalExperience.university,
                Specialization = CurrentUserMedicalExperience.specialization,
                AboutMyself = CurrentUserMedicalExperience.about_myself,
                GraduationDate = CurrentUserMedicalExperience.graduation_date
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Stomatolog")]
        public ActionResult ChangeMedicalExperience(MedicalDetails model)
        {
            if (ModelState.IsValid)
            {
                _IMedicalExperienceRepository.EditMedicalExperience(new MedicalExperience
                {
                    id = model.MedicalID,
                    university = model.University,
                    graduation_date = model.GraduationDate,
                    specialization = model.Specialization,
                    about_myself = model.AboutMyself
                });
                
                TempData["Success"] = "Zmieniono dane lekarza.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd. Spróbuj jeszcze raz.";
            }

            return RedirectToAction("ChangeMedicalExperience");
        }

        #endregion

        #region Helper methods

        private DisplayDetailsViewModel GetDisplayDetailsViewModel()
        {
            var accountDetails = new AccountDetails
            {
                Email = CurrentUser.Email,
                PhoneNumber = CurrentUser.PhoneNumber
            };

            var personalDetails = new PersonalDetails
            {
                PersonalID = CurrentUserInfo.id,
                FirstName = CurrentUserInfo.first_name,
                LastName = CurrentUserInfo.last_name,
                Gender = CurrentUserInfo.gender,
                RegisterDate = CurrentUserInfo.register_date,
            };

            var addressDetails = new AddressDetails
            {
                AddressID = CurrentUserAddress.id,
                City = CurrentUserAddress.city,
                Street = CurrentUserAddress.street,
                HouseNumber = CurrentUserAddress.house_number,
                ApartmentNumber = CurrentUserAddress.apartment_number,
                PostalCode = CurrentUserAddress.postal_code,
                PostalCity = CurrentUserAddress.postal_city
            };
            
            MedicalDetails medicalDetails = null;
            if (IsDoctor)
            {
                medicalDetails = new MedicalDetails
                {
                    MedicalID = CurrentUserMedicalExperience.id,
                    University = CurrentUserMedicalExperience.university,
                    Specialization = CurrentUserMedicalExperience.specialization,
                    GraduationDate = CurrentUserMedicalExperience.graduation_date,
                    AboutMyself = CurrentUserMedicalExperience.about_myself
                };
            }

            string photo = Url.Content("/Images/Photos/profile-photo.jpg");

            if (CurrentUserInfo.photo_id.HasValue)
            {
                photo = CurrentUserInfo.Photo.url;
            }
            else if (CurrentUserInfo.gender == "Kobieta")
            {
                photo = Url.Content("/Images/Photos/profile-photo-female.jpg");
            }

            var model = new DisplayDetailsViewModel
            {
                AccountDetails = accountDetails,
                PersonalDetails = personalDetails,
                AddressDetails = addressDetails,
                MedicalDetails = medicalDetails,
                Photo = photo
            };

            return model;
        }

        private bool UploadPhoto(HttpPostedFileBase photoFile)
        {
            var path = Server.MapPath("/Images/Photos/" + photoFile.FileName);
            if (!System.IO.File.Exists(path))
            {
                try
                {
                    photoFile.SaveAs(path);
                    TempData["Success"] = "Nowe zdjęcie dodane.";
                    return true;
                }
                catch (Exception)
                {
                    TempData["Warning"] = "Wystąpił błąd podczas dodania nowego zdjęcia. Spróbuj jeszcze raz.";
                    return false;
                }
            }
            else
            {
                TempData["Warning"] = "Istnieje już plik z taką nazwą.";
                return false;
            }
        }
        private void DeletePhoto(string fullPath)
        {
            if (System.IO.File.Exists(fullPath))
            {
                try
                {
                    System.IO.File.Delete(fullPath);
                }
                catch (Exception)
                {
                    TempData["Warning"] = "Wystąpił błąd podczas usuwania poprzedniego zdjęcia. Spróbuj jeszcze raz.";
                }
            }
        }

        #endregion
    }
}