using System;
using System.Security.Claims;
using HomeTreatment.Business.Services;
using HomeTreatment.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeTreatment.Web.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IUsersService _userService;
        public DashboardController(IUsersService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var getUser = _userService.GetLogginUser(userId);

            if (getUser.Patients != null)
            {
                var doctor = new PatientViewModel
                {
                    // to carry on
                    Name = getUser.Patients[0].Name
                };

                return View("/Views/Dashboard/DoctorDashboard.cshtml", doctor);
            }
            else if (getUser.Doctor != null)
            {
                var patient = new PatientDashboardViewModel
                {
                    // to carry on
                    DoctorName = getUser.Doctor.Doctors[0].Name
                };

                return View("/Views/Dashboard/PatientDashboard.cshtml", patient);
            }
            else
                throw new Exception("Trying to load Dashboard, but is not Doctor or Patiant");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessageToDoctor(PatientDashboardViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = _userService.GetPatient(userId);

            // To think about ModelState place
            if (!ModelState.IsValid)
                return View("/Views/Dashboard/PatientDashboard.cshtml", _userService.BuildPatiantDashboardViewModel(user.Patient));

            if (user.Patient == null)
                throw new Exception("Should be patient in order to message a doctor.");

            if (user.Patient.DoctorId == null)
            {
                if (string.IsNullOrEmpty(model.DoctorId))
                {
                    ModelState.AddModelError(nameof(model.DoctorId), "The field is required.");

                    return View(model);
                }
                else if (!_userService.IsDoctor(model.DoctorId))
                {
                    ModelState.AddModelError(nameof(model.DoctorId), "There is no doctor with the provided Id.");

                    return View(model);
                }

                user.Patient.DoctorId = model.DoctorId;
            }

            _userService.AddMessage(user, model.Message);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult SetMessageAsRead(string Id)
        {
            _userService.SetMessageAsRead(Id);

            return RedirectToAction(nameof(Index));
        }

    }
}

#region Original Logic
/* Original Logic */

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using HomeTreatment.Data;
//using HomeTreatment.Data.Models;
//using HomeTreatment.Data.Repository;
//using HomeTreatment.Web.ViewModels;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;

//namespace HomeTreatment.Web.Controllers
//{
//    [Authorize]
//    public class DashboardController : Controller
//    {

//        private readonly IRepository _repository;

//        public DashboardController(IRepository repository)
//        {
//            _repository = repository;
//        }

//        public IActionResult Index()
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var user = _repository.Set<User>()
//                .Include(u => u.Patient.DoctorPatientMessages)
//                .Include(u => u.Patient.Doctor)
//                .Include(u => u.Doctor.Patients)
//                .Single(u => u.Id == userId);

//            if (user.Doctor != null)
//            {
//                var patiants = FindPatientsWithUnreadMessages(user.Doctor.Id);

//                return View("/Views/Dashboard/DoctorDashboard.cshtml", patiants);
//            }
//            else if (user.Patient != null)
//            {
//                var patientDashboardModel = BuildPatiantDashboardViewModel(user.Patient);

//                return View("/Views/Dashboard/PatientDashboard.cshtml", patientDashboardModel);
//            }
//            else
//            {
//                throw new Exception("Trying to load Dashboard, but is not Doctor or Patiant");
//            }
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult SendMessageToDoctor(PatientDashboardViewModel model)
//        {
//            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var user = _repository.Set<User>()
//                .Include(u => u.Patient.DoctorPatientMessages)
//                .Single(u => u.Id == userId);

//            if (!ModelState.IsValid)
//            {
//                return View("/Views/Dashboard/PatientDashboard.cshtml", BuildPatiantDashboardViewModel(user.Patient));
//            }

//            if (user.Patient == null)
//                throw new Exception("Should be patient in order to message a doctor.");

//            if (user.Patient.DoctorId == null)
//            {
//                if (string.IsNullOrEmpty(model.DoctorId))
//                {
//                    ModelState.AddModelError(nameof(model.DoctorId), "The field is required.");

//                    return View(model);
//                }
//                else if (!_repository.Set<Doctor>().Any(d => d.Id == model.DoctorId))
//                {
//                    ModelState.AddModelError(nameof(model.DoctorId), "There is no doctor with the provided Id.");

//                    return View(model);
//                }

//                user.Patient.DoctorId = model.DoctorId;
//            }

//           _repository.Add(new DoctorPatientMessage
//            {
//                DoctorId = user.Patient.DoctorId,
//                PatientId = user.Patient.Id,
//                Text = model.Message,
//                IsWrittenByPatient = true,
//                Timestamp = DateTime.Now
//            });

//            return RedirectToAction(nameof(Index));
//        }

//        public IActionResult SetMessageAsRead(string Id)
//        {
//            var message = _repository.Set<DoctorPatientMessage>().Where(fr => fr.PatientId == Id).ToList();

//            foreach (var item in message)
//            {
//                item.IsRead = true;
//                _repository.Update(item);
//            }

//            return RedirectToAction(nameof(Index));
//        }

//        [NonAction]
//        List<PatientViewModel> FindPatientsWithUnreadMessages(string doctorId)
//        {
//            return _repository.Set<Patient>()
//                    .Where(p => p.DoctorId == doctorId && p.DoctorPatientMessages.Any(m => m.IsWrittenByPatient && m.IsRead == false))
//                    .Select(p => new PatientViewModel
//                    {
//                        Id = p.Id,
//                        Name = p.Name,
//                        EmailAddress = p.EmailAddress,
//                        Messages = new List<DoctorPatientMessageViewModel>
//                        {
//                            p.DoctorPatientMessages
//                                .Where(m => m.IsWrittenByPatient && m.IsRead == false)
//                                .OrderByDescending(m => m.Timestamp)
//                                .Select(m => new DoctorPatientMessageViewModel{
//                                    Id = m.Id,
//                                    Text = m.Text,
//                                    Timestamp = m.Timestamp
//                                })
//                                .First()
//                        },

//                        UnreadMessagesCount = p.DoctorPatientMessages.Count(m => m.IsRead == false)
//                    })
//                    .ToList()
//                    .OrderByDescending(p => p.Messages.Single().Timestamp)
//                    .ToList();
//        }

//        [NonAction]
//        PatientDashboardViewModel BuildPatiantDashboardViewModel(Patient patient)
//        {
//            var model = new PatientDashboardViewModel();

//            var messages = patient.DoctorPatientMessages
//                .Where(m => m.DoctorId == patient.DoctorId)
//                .OrderBy(m => m.Timestamp)
//                .ToList();

//            model.Doctors = _repository.Set<Doctor>()
//                .Select(d => new DoctorViewModel
//                {
//                    Id = d.Id,
//                    EmailAddress = d.Email,
//                    Name = d.Name
//                })
//                 .OrderBy(d => d.Name)
//                 .ToList();

//            if (messages.Count != 0)
//            {
//                model.Messages = messages
//                    .Select(m => new DoctorPatientMessageViewModel
//                    {
//                        Id = m.Id,
//                        Text = m.Text,
//                        Timestamp = m.Timestamp,
//                        IsRead = m.IsRead,
//                        IsWrittenByPatient = m.IsWrittenByPatient
//                    })
//                    .ToList();
//            }


//            return model;
//        }
//    }
//}

#endregion