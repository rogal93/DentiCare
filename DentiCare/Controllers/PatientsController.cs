using DentiCare.Models.Appointments;
using DentiCare.Models.Patients;
using DentiCare.Models.UserSettings;
using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DentiCare.Controllers
{
    public class PatientsController : BaseController
    {
        private readonly IUserInformationsRepository _IUserInformationRepository;
        private readonly IAddressesRepository _IAddressesRepository;
        private readonly IMedicalDocumentsRepository _IMedicalDocumentsRepository;

        public PatientsController(IUserInformationsRepository IUserInformationRepository,
                                  IAddressesRepository IAddressesRepository,
                                  IMedicalDocumentsRepository IMedicalDocumentsRepository) :base()
        {
            _IUserInformationRepository = IUserInformationRepository;
            _IAddressesRepository = IAddressesRepository;
            _IMedicalDocumentsRepository = IMedicalDocumentsRepository;
        }

        #region AllPatients

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public ActionResult Index()
        {
            var patients = GetNamesOfPatients();

            var model = new AllPatientsViewModel
            {
                Patients = patients,
                Details = new DisplayDetailsViewModel { PersonalDetails = new PersonalDetails { PersonalID = 0 } }
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult ShowPatientDetails(int Id)
        {
            var model = GetUserDetails(Id);

            return PartialView("_PatientDetails", model);
        }

        #endregion

        #region MedicalDocumentation

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public ActionResult MedicalDocumentation()
        {
            var patients = GetNamesOfPatients();

            var model = new MedicalDocumentationViewModel
            {
                Patients = patients,
                PatientDocuments = new PatientDocumentsViewModel
                {
                    Documents = new List<DocumentModel>(),
                    NewDocument = new DocumentModel(),
                    Download = new DownloadModel { DocumentID = 0 }
                }
            };
            
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult ShowPatientDocuments(int Id)
        {
            var patientDocs = GetPatientDocuments(Id);
            var newDoc = new DocumentModel
            {
                PatientID = Id
            };

            var model = new PatientDocumentsViewModel
            {
                Documents = patientDocs,
                NewDocument = newDoc,
                Download = new DownloadModel { DocumentID = 0 }
            };

            return PartialView("_PatientDocuments", model);
        }

        [HttpPost]
        [Authorize(Roles = "Stomatolog")]
        public ActionResult AddDocument(DocumentModel model)
        {
            if (ModelState.IsValid)
            {
                var userinfo = UserManager.Users.FirstOrDefault(x => x.user_info_id == model.PatientID).UserInfo;
                var folderName = userinfo.last_name + userinfo.first_name;
                folderName = normalize(folderName);
                folderName.ToLower();

                var result = UploadDocumentFile(model.DocumentFile, folderName);
                if (result)
                {
                    var doc = new MedicalDocument
                    {
                        description = model.Description,
                        url = Url.Content("/MedicalDocuments/" + folderName + '/' + model.DocumentFile.FileName),
                        upload_date = DateTime.Today,
                        patient_id = model.PatientID
                    };

                    _IMedicalDocumentsRepository.CreateMedicalDocument(doc);

                    TempData["Success"] = "Pomyślnie dodano dokument";
                }
            }
            else
            {
                TempData["Warning"] = "Dokument musi mieć rozszerzenie .doc, .docx, .pdf, .jpg, .jpeg lub .png i rozmiar niewiększy niż 3MB.";
            }
            
            return RedirectToAction("MedicalDocumentation");
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public FileResult DownloadDocument(DownloadModel model)
        {
            if (ModelState.IsValid)
            {
                var url = _IMedicalDocumentsRepository.Documents.FirstOrDefault(x => x.id == model.DocumentID).url;
                var path = Server.MapPath(url);
                var fileName = Path.GetFileName(path);

                byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult DeleteDocument(int Id)
        {
            var url = _IMedicalDocumentsRepository.Documents.FirstOrDefault(x => x.id == Id).url;
            var patientID = _IMedicalDocumentsRepository.Documents.FirstOrDefault(x => x.id == Id).patient_id;
            var path = Server.MapPath(url);
            var result = DeleteDocumentFile(path);
            if (result)
            {
                _IMedicalDocumentsRepository.DeleteMedicalExperience(Id);

                TempData["Success"] = "Usunięto dokument.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd podczas usuwania dokumentu. Spróbuj jeszcze raz.";
            }

            var patientDocs = GetPatientDocuments(patientID);
            var newDoc = new DocumentModel
            {
                PatientID = Id
            };

            var model = new PatientDocumentsViewModel
            {
                Documents = patientDocs,
                NewDocument = newDoc,
                Download = new DownloadModel { DocumentID = 0 }
            };

            return PartialView("_PatientDocuments", model);
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult RefreshDownloadButton(int Id)
        {
            var model = new DownloadModel
            {
                DocumentID = Id
            };

            return PartialView("_Download", model);
        }

        #endregion

        #region Helper methods

        private List<UserNameWithIDs> GetNamesOfPatients()
        {
            var patientRoleId = RoleManager.Roles.FirstOrDefault(x => x.Name == "Pacjent").Id;

            var names = from user in UserManager.Users
                        where user.Roles.Any(r => r.RoleId == patientRoleId)
                        orderby user.UserInfo.last_name
                        select new UserNameWithIDs
                        {
                            UserInfoID = user.user_info_id,
                            FullName = user.UserInfo.first_name + " " + user.UserInfo.last_name
                        };

            return names.ToList();
        }
        private DisplayDetailsViewModel GetUserDetails(int Id)
        {
            var user = (from u in UserManager.Users
                        where u.UserInfo.id == Id
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

        private List<DocumentModel> GetPatientDocuments(int patientID)
        {
            var documents = from doc in _IMedicalDocumentsRepository.Documents
                            where doc.patient_id == patientID
                            select new DocumentModel
                            {
                                ID = doc.id,
                                Description = doc.description,
                                UploadDate = doc.upload_date,
                                PatientID = doc.patient_id,
                                Url = doc.url
                            };

            return documents.ToList();
        }
        private bool UploadDocumentFile(HttpPostedFileBase file, string folderName)
        {
            var path = Server.MapPath("/MedicalDocuments/" + folderName + '/');
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            path += file.FileName;

            if (!System.IO.File.Exists(path))
            {
                try
                {
                    file.SaveAs(path);
                    TempData["Success"] = "Nowy dokument dodany.";
                    return true;
                }
                catch (Exception)
                {
                    TempData["Warning"] = "Wystąpił błąd podczas dodania dokumentu. Spróbuj jeszcze raz.";
                    return false;
                }
            }
            else
            {
                TempData["Warning"] = "Istnieje już plik z taką nazwą.";
                return false;
            }
        }
        private bool DeleteDocumentFile(string fullPath)
        {
            if (System.IO.File.Exists(fullPath))
            {
                try
                {
                    System.IO.File.Delete(fullPath);
                    return true;
                }
                catch (Exception)
                {
                    TempData["Warning"] = "Wystąpił błąd podczas usuwania pliku dokumentu. Spróbuj jeszcze raz.";
                    return false;
                }
            }
            return false;
        }

        private string normalize(string word)
        {
            if (word == null || "".Equals(word))
            {
                return word;
            }
            char[] charArray = word.ToCharArray();
            char[] normalizedArray = new char[charArray.Length];
            for (int i = 0; i < normalizedArray.Length; i++)
            {
                normalizedArray[i] = normalizeChar(charArray[i]);
            }
            return new String(normalizedArray);
        }
        private char normalizeChar(char c)
        {
            switch (c)
            {
                case 'ą': return 'a';
                case 'ć': return 'c';
                case 'ę': return 'e';
                case 'Ł':
                case 'ł': return 'l';
                case 'ń': return 'n';
                case 'Ó':
                case 'ó': return 'o';
                case 'Ś':
                case 'ś': return 's';
                case 'ż':
                case 'Ż':
                case 'Ź':
                case 'ź': return 'z';
            }
            return c;
        }

        #endregion
    }
}