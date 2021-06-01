using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HomeTreatment.ViewModels;
using HomeTreatment.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using HomeTreatment.BusinessLayer;

namespace HomeTreatment.Controllers
{
    [Authorize]
    public class DoctorController : Controller
    {

        private readonly HomeTreatmentDbContext _context;

        private IOptions<LoadHistory> _counter;

        public DoctorController(HomeTreatmentDbContext context, IOptions<LoadHistory> appSettings)
        {
            _context = context;
            _counter = appSettings;

        }

        public IActionResult DisplayPatients(PatiensListViewModel search)
        {
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _context.Users.FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;
            if (_context.Doctors.Any(an => an.Id == loggedUserId))
            {
                var doctorId = loggedUserId;

                var patients = _context.Patients.Select(p => new PatientViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    AttentionLevel = p.AttentionLevel,
                    EmailAddress = p.EmailAddress,
                    Notes = p.Notes,
                    DoctorId = p.DoctorId
                });

                if (search.SearchTerm == null)
                {
                    var allPatientsOfCurrentDoctor = patients.Where(wr => wr.DoctorId == loggedUserId).ToList();

                    return View(new PatiensListViewModel
                    {
                        Patients = allPatientsOfCurrentDoctor,

                    });
                }

                var filterPatients = patients.Where(e => e.Name.Contains(search.SearchTerm) || e.EmailAddress.Contains(search.SearchTerm)).ToList();

                if (search.SearchTerm.Contains("high") && search.SearchTerm.Length == 4)
                {
                    filterPatients = patients.Where(e => e.Name.Contains(search.SearchTerm) || e.AttentionLevel == true).ToList();
                }

                return View(new PatiensListViewModel
                {
                    Patients = filterPatients,

                });

            }
            return Redirect("~/Authentication/Login");
        }

        [HttpGet]
        public IActionResult Messages(string id)
        {
            _counter.Value.counter = 7;
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _context.Users.FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;
            if (_context.Doctors.Any(an => an.Id == loggedUserId))
            {
                var patientMessages = BuildPatientMessages(id, 7);

                return View("Messages", patientMessages);

            }
            return Redirect("~/Authentication/Login");
        }

        [HttpPost]
        public IActionResult Messages(PatientMessagesViewModel patientMessages)
        {
            var doctorId = _context.DoctorPatientMessages.FirstOrDefault(fr => fr.PatientId == patientMessages.Patient.Id).DoctorId;

            var messages = _context.DoctorPatientMessages.Where(wr => wr.DoctorId == doctorId && wr.PatientId == patientMessages.Patient.Id).ToList();

            if (!ModelState.IsValid)
            {

                patientMessages.Messages = messages
                    .Select(m => new DoctorPatientMessageViewModel
                    {
                        Id = m.Id,
                        Text = m.Text,
                        Timestamp = m.Timestamp,
                        IsRead = m.IsRead,
                        IsWrittenByPatient = m.IsWrittenByPatient
                    }).OrderByDescending(or => or.Timestamp).Take(7)
                    .ToList();
                patientMessages.Patient.Name = _context.Patients.Single(sl => sl.Id == patientMessages.Patient.Id).Name;
                return View("/Views/Doctor/Messages.cshtml", patientMessages);
            }

            var patientResponse = new DoctorPatientMessage
            {
                Text = patientMessages.Message,
                PatientId = patientMessages.Patient.Id,
                DoctorId = doctorId,
                Timestamp = DateTime.Now,
                IsRead = true
            };

            _context.DoctorPatientMessages.Add(patientResponse);
            _context.SaveChanges();


            var seenMessages = messages.Where(sl => sl.IsRead == false).ToList();

            foreach (var item in seenMessages)
            {
                item.IsRead = true;
                _context.DoctorPatientMessages.Update(item);
            }
            _context.SaveChanges();


            return RedirectToAction(nameof(Messages), new { id = patientMessages.Patient.Id });

        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            var user = _context.Patients.FirstOrDefault(fr => fr.Id == Id);

            var model = new PatientViewModel
            {
                Name = user.Name,
                AttentionLevel = user.AttentionLevel,
                Notes = user.Notes
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(PatientViewModel model)
        {
            var edit = _context.Patients.FirstOrDefault(fr => fr.Id == model.Id);

            edit.Notes = model.Notes;
            edit.AttentionLevel = model.SelectedItem == "High" ? model.AttentionLevel = true : model.AttentionLevel = false;

            _context.Patients.Update(edit);
            _context.SaveChanges();

            return RedirectToAction(nameof(DisplayPatients));
        }

        [HttpPost]
        public IActionResult LoadMoreMessages(PatientMessagesViewModel model)
        {
            _counter.Value.counter = _counter.Value.counter + 7;
            var loadedMessagesCount = _counter.Value.counter;
            ModelState.Clear();
            return View("Messages", BuildPatientMessages(model.Patient.Id, loadedMessagesCount));
        }

        public PatientMessagesViewModel BuildPatientMessages(string patientId, int loadedMessagesCount)
        {
            var patient = _context.Patients.FirstOrDefault(pt => pt.Id == patientId);

            var messages = _context.DoctorPatientMessages.Where(msg => msg.PatientId == patientId).ToList();

            var patientViewModel = new PatientViewModel
            {
                Id = patient.Id,
                Name = patient.Name,
                AttentionLevel = patient.AttentionLevel,
                EmailAddress = patient.EmailAddress,
                Notes = patient.Notes
            };

            List<DoctorPatientMessageViewModel> allMessages = new List<DoctorPatientMessageViewModel>();

            foreach (var message in messages)
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


            allMessages = allMessages.OrderByDescending(ord => ord.Timestamp).Take(loadedMessagesCount).ToList();



            var patientMessages = new PatientMessagesViewModel
            {
                Patient = patientViewModel,
                Messages = allMessages,

            };

            return patientMessages;
        }
    }
}
