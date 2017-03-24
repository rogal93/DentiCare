using DentiCare.Models.Appointments;
using DentiCare.Models.Schedules;
using DentiCare.Repository.Abstract;
using DentiCare.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DentiCare.Controllers
{
    [Authorize]
    public class SchedulesController : BaseController
    {
        private readonly ISchedulesRepository _ISchedulesRepository;
        private readonly IAppointmentsRepository _IAppointmentsRepository;

        public SchedulesController(ISchedulesRepository ISchedulesRepository,
                                   IAppointmentsRepository IAppointmentsRepository) :base()
        {
            _ISchedulesRepository = ISchedulesRepository;
            _IAppointmentsRepository = IAppointmentsRepository;
        }

        #region CurrentSchedule (for doctor)

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("CurrentSchedule");
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public ActionResult CurrentSchedule()
        {
            var schedules = GetSchedules(CurrentUserInfo.id);
            var dayState = new DayState { DayOff = null };
            var hours = new HoursAtWork
            {
                ScheduleID = 0,
                HourFrom = 8,
                HourTo = 16,
                HoursFrom = GetHoursFrom(),
                HoursTo = GetHoursTo()
            };

            var model = new CurrentScheduleViewModel
            {
                Schedules = schedules,
                DayState = dayState,
                HoursAtWork = hours
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult ActivateScheduleEditButtons(int Id)
        {
            bool isDayOff = _ISchedulesRepository.Schedules.FirstOrDefault(p => p.id == Id).day_off;
            
            var model = new DayState
            {
                DayOff = isDayOff
            };

            return PartialView("_ScheduleEditButtons", model);
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public async Task<PartialViewResult> SetDayOff(int Id)
        {
            var schedule = _ISchedulesRepository.Schedules.FirstOrDefault(p => p.id == Id);
            _ISchedulesRepository.EditSchedule(new Schedule
            {
                id = schedule.id,
                date = schedule.date,
                doctor_id = schedule.doctor_id,
                day_off = true,
                hour_from = schedule.hour_from,
                hour_to = schedule.hour_to
            });

            var assistantRoleId = RoleManager.Roles.FirstOrDefault(x => x.Name == "Asystentka").Id;
            var assistantId = (from user in UserManager.Users
                             where user.Roles.Any(r => r.RoleId == assistantRoleId)
                             select user)
                             .FirstOrDefault().Id;

            string title = "Stomatolog " + schedule.Doctor.first_name + " " + schedule.Doctor.last_name + " wziął dzień wolny.";
            string body = "Proszę poinformować pacjentów o zmianie planów, którzy dnia " + schedule.date.ToShortDateString() + "mieli umówione wizyty.";

            await UserManager.SendEmailAsync(assistantId, title, body);

            TempData["Success"] = "Dzień " + schedule.date.ToShortDateString() + " jest wolny.";

            var model = new DayState
            {
                DayOff = null
            };

            return PartialView("_ScheduleEditButtons", model);
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult RefreshScheduleList()
        {
            var schedules = GetSchedules(CurrentUserInfo.id);

            return PartialView("_MyScheduleList", schedules);
        }

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult RefreshAddHoursModal(int Id)
        {
            var hours = new HoursAtWork
            {
                ScheduleID = Id,
                HourFrom = 8,
                HourTo = 16,
                HoursFrom = GetHoursFrom(),
                HoursTo = GetHoursTo()
            };

            return PartialView("_ScheduleAddHoursModal", hours);
        }

        [HttpPost]
        [Authorize(Roles = "Stomatolog")]
        public PartialViewResult AddHours(HoursAtWork model)
        {
            if (ModelState.IsValid)
            {
                _ISchedulesRepository.EditSchedule(new Schedule
                {
                    id = model.ScheduleID,
                    day_off = false,
                    hour_from = model.HourFrom,
                    hour_to = model.HourTo
                });
                TempData["Success"] = "Pomyślnie dodano godziny pracy.";
            }
            else
            {
                TempData["Warning"] = "Wystąpił błąd. Spróbuj jeszcze raz.";
            }

            var schedules = GetSchedules(CurrentUserInfo.id);

            return PartialView("_MyScheduleList", schedules);
        }

        #endregion

        #region EnterSchedule (for doctor)

        [HttpGet]
        [Authorize(Roles = "Stomatolog")]
        public ActionResult EnterSchedule()
        {
            var firstUnsetDay = GetFirstUnsetDay();
            var hoursFrom = GetHoursFrom();
            var hoursTo = GetHoursTo();

            var model = new ScheduleModel
            {
                Date = firstUnsetDay,
                DayOff = false,
                HourFrom = 8,
                HourTo = 16,
                HoursFrom = hoursFrom,
                HoursTo = hoursTo
            };
            
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Stomatolog")]
        public ActionResult EnterSchedule(ScheduleModel viewModel)
        {
            if (ModelState.IsValid && viewModel.HourFrom < viewModel.HourTo)
            {
                _ISchedulesRepository.CreateSchedule(new Schedule
                {
                    doctor_id = CurrentUserInfo.id,
                    date = viewModel.Date,
                    hour_from = viewModel.HourFrom,
                    hour_to = viewModel.HourTo,
                    day_off = viewModel.DayOff
                });

                TempData["Success"] = "Pomyślnie dodano grafik.";
            }
            else
            {
                TempData["Warning"] = "Nie udało się dodać grafiku. Spróbuj jeszcze raz.";
            }

            return RedirectToAction("EnterSchedule");
        }


        #endregion

        #region DoctorSchedules (for assistant)

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public ActionResult DoctorSchedule()
        {
            var doctorsIDsNames = GetNamesOfDoctors();
            var schedules = GetSchedules(doctorsIDsNames.First().Key);

            var model = new DoctorScheduleViewModel
            {
                NamesOfDoctors = doctorsIDsNames,
                Schedules = schedules
            };

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Asystentka")]
        public PartialViewResult ShowDoctorSchedule(int Id)
        {
            var model = GetSchedules(Id);

            return PartialView("_DoctorScheduleList", model);
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

        private List<ScheduleModel> GetSchedules(int doctorID)
        {
            var query = from s in _ISchedulesRepository.Schedules
                        where s.doctor_id == doctorID &&
                              s.date >= DateTime.Today
                        orderby s.date
                        select new ScheduleModel
                        {
                            ID = s.id,
                            Date = s.date,
                            HourFrom = s.hour_from,
                            HourTo = s.hour_to,
                            DayOff = s.day_off
                        };

            return query.ToList();
        }
        private DateTime GetFirstUnsetDay()
        {
            DateTime nextDay = DateTime.Today.AddDays(1);

            var setDays = (from s in _ISchedulesRepository.Schedules
                              where s.doctor_id == CurrentUserInfo.id &&
                                    s.date > DateTime.Today
                              orderby s.date
                              select new ScheduleModel
                              {
                                  ID = s.id,
                                  DayOff = s.day_off,
                                  HourFrom = s.hour_from,
                                  HourTo = s.hour_to,
                                  Date = s.date
                              })
                               .ToList();
            
            if (setDays.Count > 0)
            {
                nextDay = setDays.Last().Date.AddDays(1);
            }

            if (nextDay.DayOfWeek == DayOfWeek.Sunday)
            {
                nextDay = nextDay.AddDays(1);
            }
            
            return nextDay;
        }
        private Dictionary<int, string> GetHoursFrom()
        {
            var hours = new Dictionary<int, string>
            {
                { 8, "8:00" }, { 9, "9:00" }, { 10, "10:00" }, { 11, "11:00" },
                { 12, "12:00" }, { 13, "13:00" }, { 14, "14:00" }, { 15, "15:00" },
                { 16, "16:00" }, { 17, "17:00" }, { 18, "18:00" }, { 19, "19:00" }
            };
            return hours;
        }
        private Dictionary<int, string> GetHoursTo()
        {
            var hours = new Dictionary<int, string>
            {
                { 9, "9:00" }, { 10, "10:00" }, { 11, "11:00" }, { 12, "12:00" },
                { 13, "13:00" }, { 14, "14:00" }, { 15, "15:00" }, { 16, "16:00" },
                { 17, "17:00" }, { 18, "18:00" }, { 19, "19:00" }, { 20, "20:00" },
            };
            return hours;
        }
        
        #endregion
    }
}