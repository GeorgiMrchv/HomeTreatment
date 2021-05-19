using System;
using System.Collections.Generic;
using System.Linq;
using HomeTreatment.Data;
using HomeTreatment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeTreatment.Controllers
{
    public class DashboardController : Controller
    {

        private readonly HomeTreatmentDbContext _context;

        public DashboardController(HomeTreatmentDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _context.Users.FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;

            if (_context.Doctors.Any(an => an.Id == loggedUserId))
            {
                var doctorId = loggedUserId;
                var currDoctorPatients =
                     (
                     from c in _context.Patients
                     join p in _context.DoctorPatientMessages on c.Id equals p.PatientId into ps
                     from p in ps.DefaultIfEmpty()
                     orderby p.Timestamp descending
                     select new MessageDetailsViewModel
                     {
                         Id = p.Id,
                         Name = c.Name,
                         EmailAddress = c.EmailAddress,
                         Notes = p.Text,
                         IsRead = p.IsRead,
                         Timestamp = p.Timestamp,
                         DoctorId = p.DoctorId,
                         IsWrittenByPatient = p.IsWrittenByPatient,
                         PatientId = p.PatientId


                     }).Where(wr => wr.DoctorId == doctorId && wr.IsRead == false && wr.IsWrittenByPatient == true).ToList();

                Dictionary<string, List<MessageDetailsViewModel>> myDictionary = currDoctorPatients.GroupBy(o => o.PatientId).ToDictionary(g => g.Key, g => g.ToList());

                return View("Views/Patient/Index.cshtml", new PatiensListViewModel
                {
                    MessagessAll = myDictionary
                });
            }

            else if (_context.Patients.Any(an => an.Id == loggedUserId))
            {
                var patientId = loggedUserId;

                var patientMessages = _context.DoctorPatientMessages.Where(sl => sl.PatientId == patientId).ToList(); // get message           

                if (patientMessages.Count == 0)
                {
                    var allDoctors = _context.Doctors.Select(sl => new DoctorViewModel
                    {
                        Id = sl.Id,
                        Name = sl.Name,
                        EmailAddress = sl.Email
                    }).ToList();

                    return View("Views/Doctor/Index.cshtml", new PatientMessagesViewModel
                    {
                        IsFirstVisit = true,
                        Doctors = allDoctors
                    });
                }
                else
                {
                    List<DoctorPatientMessageViewModel> allMessages = new List<DoctorPatientMessageViewModel>();

                    foreach (var message in patientMessages)
                    {
                        var messageViewModel = new DoctorPatientMessageViewModel
                        {
                            Id = message.Id,
                            DoctorId = message.DoctorId,
                            PatientId = message.PatientId,
                            Text = message.Text,
                            IsRead = message.IsRead,
                            Timestamp = message.Timestamp,
                            IsWrittenByPatient = message.IsWrittenByPatient

                        };

                        allMessages.Add(messageViewModel);
                    }


                    return View("Views/Doctor/Index.cshtml", new PatientMessagesViewModel
                    {
                        IsFirstVisit = false,
                        Messages = allMessages
                    });
                }

            }
            return View();
        }

        [HttpPost]
        public IActionResult SendMessageToDoctor(PatientMessagesViewModel patientMessages, string id)
        {
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _context.Users.FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;

            var patient = _context.Patients.FirstOrDefault(fr => fr.Id == loggedUserId);
            var patientResponse = new DoctorPatientMessage
            {
                Text = patientMessages.Message,
                PatientId = patient.Id,
                DoctorId = patientMessages.SelectedItem == null ? _context.DoctorPatientMessages.FirstOrDefault(fr => fr.PatientId == loggedUserId).DoctorId : patientMessages.SelectedItem,
                Timestamp = DateTime.Now,
                IsRead = false,
                IsWrittenByPatient = true
            };

            _context.DoctorPatientMessages.Add(patientResponse);
            _context.SaveChanges();
            if (patientMessages.SelectedItem != null)
            {
                patient.DoctorId = patientMessages == null ? _context.DoctorPatientMessages.FirstOrDefault(fr => fr.PatientId == loggedUserId).DoctorId : patientMessages.SelectedItem;
                patient.Notes = patientMessages.Message;

                _context.Patients.Update(patient);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult SetMessageAsRead(string Id)
        {
            var message = _context.DoctorPatientMessages.Where(fr => fr.PatientId == Id).ToList();

            foreach (var item in message)
            {
                item.IsRead = true;
                _context.DoctorPatientMessages.Update(item);                
            }
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }

    }
}
