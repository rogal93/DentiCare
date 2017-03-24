using DentiCare.Models.Appointments;
using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DentiCare.Controllers
{
    public class AppointmentsController : BaseController
    {
        private readonly IUserInformationsRepository _IUserInformationRepository;
        private readonly IMedicalExperienceRepository _IMedicalExperienceRepository;
        private readonly ISchedulesRepository _ISchedulesRepository;
        private readonly IAppointmentsRepository _IAppointmentsRepository;

        public AppointmentsController(IUserInformationsRepository IUserInformationRepository,
                                      IMedicalExperienceRepository IMedicalExperienceRepository,
                                      ISchedulesRepository ISchedulesRepository,
                                      IAppointmentsRepository IAppointmentsRepository) :base()
        {
            _IUserInformationRepository = IUserInformationRepository;
            _IMedicalExperienceRepository = IMedicalExperienceRepository;
            _ISchedulesRepository = ISchedulesRepository;
            _IAppointmentsRepository = IAppointmentsRepository;
        }

        #region BookedTerms (for patient)

        [HttpGet]
        [Authorize(Roles = "Pacjent")]
        public ActionResult Index()
        {
            var model = GetBookedTerms(CurrentUserInfo.id);

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Pacjent")]
        public async Task<PartialViewResult> Cancel(int Id)
        {
            var visit = _IAppointmentsRepository.Appointments.FirstOrDefault(x => x.id == Id);
            var doctorFullName = visit.Doctor.first_name + " " + visit.Doctor.last_name;

            _IAppointmentsRepository.DeleteAppointment(Id);

            var title = "Anulowano wizytę w ProDentiCare!";
            var message = "Anulowanie rezerwacji terminu wizyty stomatologicznej:<br><br> - data: " +
                          visit.date.ToShortDateString() + "<br> - godzina: " +
                          visit.date.ToShortTimeString() + "<br> - stomatolog: lek. stom. " +
                          doctorFullName + "<br><br>Zapraszamy!<br><br>Centrum ProDentiCare";

            await UserManager.SendEmailAsync(CurrentUser.Id, title, message);

            TempData["Success"] = "Wizyta odwołana pomyślnie.";

            var model = GetBookedTerms(CurrentUserInfo.id);

            return PartialView("_BookedTermsList", model);
        }

        #endregion

        #region MakeAppointment (for patient)

        [HttpGet]
        [Authorize(Roles = "Pacjent")]
        public ActionResult MakeAppointment()
        {
            var doctorsIDsNames = GetNamesOfDoctors();
            var details = GetDoctorDetails(doctorsIDsNames.First().Key);
            var freeTerms = GetFreeTerms(doctorsIDsNames.First().Key);

            var viewModel = new MakeAppointmentViewModel
            {
                DoctorsIDsNames = doctorsIDsNames,
                DoctorDetails = details,
                FreeTerms = freeTerms
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Pacjent")]
        public PartialViewResult ShowDoctorDetails(int Id)
        {
            var model = GetDoctorDetails(Id);

            return PartialView("_DoctorDetails", model);
        }

        [HttpGet]
        [Authorize(Roles = "Pacjent")]
        public PartialViewResult ShowFreeTerms(int Id)
        {
            var model = GetFreeTerms(Id);

            return PartialView("_FreeTermsList", model);
        }

        [HttpGet]
        [Authorize(Roles = "Pacjent")]
        public async Task<PartialViewResult> SubmitAppointment(int Id, int Hour)
        {
            var schedule = _ISchedulesRepository.Schedules.FirstOrDefault(x => x.id == Id);

            if (schedule != null)
            {
                _IAppointmentsRepository.CreateAppointment(new Appointment
                {
                    doctor_id = schedule.doctor_id,
                    patient_id = CurrentUserInfo.id,
                    date = new DateTime(schedule.date.Year, schedule.date.Month, schedule.date.Day, Hour, 0, 0)
                });

                var title = "Umówiona wizyta w ProDentiCare!";
                var message = "Rezerwacja terminu wizyty stomatologicznej:<br><br> - data: " + 
                              schedule.date.ToShortDateString() + "<br> - godzina: " + 
                              Hour + ":00<br> - stomatolog: lek. stom. " + schedule.Doctor.first_name + " " +
                              schedule.Doctor.last_name + "<br><br>Zapraszamy!<br><br>Centrum ProDentiCare";

                await UserManager.SendEmailAsync(CurrentUser.Id, title, message);

                TempData["Success"] = "Wizyta zarezerowana pomyślnie.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd. Proszę spróbować jeszcze raz.";
            }

            var model = GetFreeTerms(schedule.doctor_id);

            return PartialView("_FreeTermsList", model);
        }

        #endregion

        #region History (for patient)

        [HttpGet]
        [Authorize(Roles = "Pacjent")]
        public ActionResult History()
        {
            var model = GetHistory(CurrentUserInfo.id);

            return View(model);
        }

        #endregion

        #region UndescribedAppointments (for doctor)

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public ActionResult UndescribedAppointments()
        {
            var undescribed = GetUndescribed(CurrentUserInfo.id);
            var empty = new UndescribedDetails { AppointmentID = 0, Comment = "", Price = 0 };

            var model = new UndescribedViewModel
            {
                Undescribed = undescribed,
                Details = empty
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult ShowUndescribedDetails(int Id)
        {
            var model = GetUndescribedDetails(Id);

            return PartialView("_UndescribedDetails", model);
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult RefreshUndescribedDetails()
        {
            var empty = new UndescribedDetails { AppointmentID = 0, Comment = "", Price = 0 };

            return PartialView("_UndescribedDetails", empty);
        }

        [HttpPost]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult DescribeAppointment(UndescribedDetails model)
        {
            if (ModelState.IsValid)
            {
                _IAppointmentsRepository.EditAppointment(new Appointment
                {
                    id = model.AppointmentID,
                    comment = model.Comment,
                    price = model.Price
                });

                TempData["Success"] = "Pomyślnie opisano wizytę.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd przy opisywaniu wizyty. Spróbuj jeszcze raz.";
            }

            var undescribed = GetUndescribed(CurrentUserInfo.id);

            return PartialView("_undescribedList", undescribed);
        }

        #endregion

        #region TodaysVisits (for doctor)

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public ActionResult TodayVisits()
        {
            var model = GetTodaysAppointments(CurrentUserInfo.id);

            return View(model);
        }

        #endregion

        #region ArrangeAppointment (for assistant)

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public ActionResult ArrangeAppointment()
        {
            var doctors = GetNamesOfDoctors();
            var patients = GetNamesOfPatients();
            var freeterms = GetFreeTerms(doctors.First().Key);

            var viewModel = new ArrangeAppointmentViewModel
            {
                NamesOfDoctors = doctors,
                Patients = patients,
                FreeTerms = freeterms
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public PartialViewResult ShowFreeDoctorTerms(int Id)
        {
            var model = GetFreeTerms(Id);

            return PartialView("_FreeDoctorTermsList", model);
        }

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public async Task<PartialViewResult> Arrange(int patientID, int scheduleID, int Hour)
        {
            var schedule = _ISchedulesRepository.Schedules.FirstOrDefault(x => x.id == scheduleID);
            var date = new DateTime(schedule.date.Year, schedule.date.Month, schedule.date.Day, Hour, 0, 0);

            _IAppointmentsRepository.CreateAppointment(new Appointment
            {
                doctor_id = schedule.doctor_id,
                patient_id = patientID,
                date = date
            });

            var title = "Umówiona wizyta w ProDentiCare!";
            var message = "Rezerwacja terminu wizyty stomatologicznej:<br><br> - data: " +
                          schedule.date.ToShortDateString() + "<br> - godzina: " +
                          Hour + ":00<br> - stomatolog: lek. stom. " + schedule.Doctor.first_name + " " +
                          schedule.Doctor.last_name + "<br><br>Zapraszamy!<br><br>Centrum ProDentiCare";

            var user = (from u in UserManager.Users
                        where u.UserInfo.id == patientID
                        select u)
                        .FirstOrDefault();

            await UserManager.SendEmailAsync(user.Id, title, message);

            TempData["Success"] = "Pomyślnie zaaranżowano wizytę dnia " + date.ToShortDateString() +
                " o godz. " + date.ToShortTimeString();

            var model = GetFreeTerms(schedule.doctor_id);

            return PartialView("_FreeDoctorTermsList", model);
        }

        #endregion

        #region CancelAppointment (for assistant)

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public ActionResult CancelAppointment()
        {
            var patients = GetNamesOfPatients();
            var booked = new List<AppointmentModel>();// GetBookedTerms(patients.First().UserInfoID);

            var viewModel = new CancelAppointmentViewModel
            {
                Patients = patients,
                PatientAppointments = booked
            };

            return View(viewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public PartialViewResult ShowBookedTerms(int Id)
        {
            var model = GetBookedTerms(Id);

            return PartialView("_BookedTermsList", model);
        }

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public async Task<PartialViewResult> CancelByAssistant(int Id, int PatientID)
        {
            var visit = _IAppointmentsRepository.Appointments.FirstOrDefault(x => x.id == Id);
            var doctorFullName = visit.Doctor.first_name + " " + visit.Doctor.last_name;

            _IAppointmentsRepository.DeleteAppointment(Id);

            var title = "Anulowano wizytę w ProDentiCare!";
            var message = "Anulowanie rezerwacji terminu wizyty stomatologicznej:<br><br> - data: " +
                          visit.date.ToShortDateString() + "<br> - godzina: " +
                          visit.date.ToShortTimeString() + "<br> - stomatolog: lek. stom. " +
                          doctorFullName + "<br><br>Zapraszamy!<br><br>Centrum ProDentiCare";

            await UserManager.SendEmailAsync(CurrentUser.Id, title, message);

            TempData["Success"] = "Wizyta odwołana pomyślnie.";

            var model = GetBookedTerms(PatientID);

            return PartialView("_BookedTermsList", model);
        }

        #endregion
        
        #region TodaysVisits (for assistant)

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public ActionResult TodayVisitsForAssistant()
        {
            var doctors = GetNamesOfDoctors();
            var appointments = GetTodaysAppointments(doctors.First().Key);

            var model = new TodayVisitsViewModel
            {
                DoctorsIDsNames = doctors,
                Appointments = appointments
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public PartialViewResult RefreshTodayVisitsList(int Id)
        {
            var model = GetTodaysAppointments(Id);

            return PartialView("_TodayVisitsList", model);
        }

        #endregion

        #region Helper methods

        private List<AppointmentModel> GetTodaysAppointments(int doctorID)
        {
            var appointments = from a in _IAppointmentsRepository.Appointments
                               where a.doctor_id == doctorID &&
                                     a.date.Day == DateTime.Today.Day
                               orderby a.date
                               select new AppointmentModel
                               {
                                   ID = a.id,
                                   Date = a.date,
                                   PatientFullName = a.Patient.first_name + " " + a.Patient.last_name
                               };

            return appointments.ToList();
        }

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

        private List<AppointmentModel> GetBookedTerms(int patientID)
        {
            var now = DateTime.Now;

            var booked = from a in _IAppointmentsRepository.Appointments
                         where a.patient_id == patientID &&
                               a.date > now
                         orderby a.date
                         select new AppointmentModel
                         {
                             ID = a.id,
                             Date = a.date,
                             Comment = a.comment,
                             Price = a.price,
                             DoctorFullName = "lek. stom. " + a.Doctor.first_name + " " + a.Doctor.last_name,
                             PatientFullName = a.Patient.first_name + " " + a.Patient.last_name
                         };

            return booked.ToList();
        }
        
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
        private DoctorDetails GetDoctorDetails(int id)
        {
            var doctor = UserManager.Users.FirstOrDefault(x => x.user_info_id == id);

            var details = new DoctorDetails
            {
                DoctorID = doctor.user_info_id,
                Photo = doctor.UserInfo.Photo.url,
                University = doctor.MedicalExperience.university,
                Specialization = doctor.MedicalExperience.specialization,
                GraduationDate = doctor.MedicalExperience.graduation_date,
                AboutMyself = doctor.MedicalExperience.about_myself
            };

            return details;
        }
        private List<FreeTerm> GetFreeTerms(int doctorID)
        {
            var schedules = (from schedule in _ISchedulesRepository.Schedules
                             where schedule.doctor_id == doctorID &&
                                   schedule.day_off == false &&
                                   schedule.date >= DateTime.Today
                             orderby schedule.date
                             select schedule)
                             .ToList();

            var freeTerms = new List<FreeTerm>();

            foreach (var schedule in schedules)
            {
                int startingHour = schedule.hour_from;
                int numberOfHours = schedule.hour_to - schedule.hour_from;
                
                if (schedule.date == DateTime.Today && DateTime.Now.Hour + 1 > startingHour)
                {
                    startingHour = DateTime.Now.Hour + 1;
                    numberOfHours = schedule.hour_to - startingHour;
                }

                if (numberOfHours > 0)
                {
                    for (int i = 0; i < numberOfHours; i++)
                    {
                        var date = new DateTime(schedule.date.Year, schedule.date.Month, schedule.date.Day, startingHour + i, 0, 0);

                        var term = _IAppointmentsRepository.Appointments
                                   .FirstOrDefault(x => x.doctor_id == schedule.doctor_id &&
                                                        x.date == date);
                        if (term == null)
                        {
                            freeTerms.Add(new FreeTerm
                            {
                                ScheduleID = schedule.id,
                                Hour = startingHour + i,
                                Date = schedule.date,
                                DayOfWeek = schedule.date.DayOfWeek
                            });
                        }
                    }
                }
            }

            return freeTerms;                     
        }

        private List<AppointmentModel> GetHistory(int patientID)
        {
            var now = DateTime.Now;

            var history = from a in _IAppointmentsRepository.Appointments
                          where a.patient_id == patientID &&
                                a.date < now
                          orderby a.date descending
                          select new AppointmentModel
                          {
                              ID = a.id,
                              Date = a.date,
                              Comment = a.comment,
                              Price = a.price,
                              DoctorFullName = "lek. stom. " + a.Doctor.first_name + " " + a.Doctor.last_name,
                              PatientFullName = a.Patient.first_name + " " + a.Patient.last_name
                          };

            return history.ToList();
        }

        private List<AppointmentModel> GetUndescribed(int doctorID)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var now = DateTime.Now;

            var undescribed = from a in _IAppointmentsRepository.Appointments
                              where a.doctor_id == doctorID &&
                                    a.date > yesterday &&
                                    a.date <= now &&
                                    a.comment == null &&
                                    a.price == 0
                              orderby a.date
                              select new AppointmentModel
                              {
                                  ID = a.id,
                                  Date = a.date,
                                  DoctorFullName = "lek. stom. " + a.Doctor.first_name + " " + a.Doctor.last_name,
                                  PatientFullName = a.Patient.first_name + " " + a.Patient.last_name,
                                  Comment = a.comment,
                                  Price = a.price
                              };

            return undescribed.ToList();
        }
        private UndescribedDetails GetUndescribedDetails(int id)
        {
            var undescribed = (from a in _IAppointmentsRepository.Appointments
                               where a.id == id
                               select new UndescribedDetails
                               {
                                   AppointmentID = a.id,
                                   Comment = a.comment,
                                   Price = a.price
                               })
                               .FirstOrDefault();

            return undescribed;
        }

        #endregion
    }
}