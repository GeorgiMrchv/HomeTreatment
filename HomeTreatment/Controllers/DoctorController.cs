using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HomeTreatment.Web.ViewModels;
using HomeTreatment.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using HomeTreatment.Web.BusinessLayer;
using HomeTreatment.Data.Repository;
using HomeTreatment.Data.Models;

namespace HomeTreatment.Web.Controllers
{
    [Authorize]
    public class DoctorController : Controller
    {
        private readonly IRepository _repository;

        // private IOptions<LoadHistory> _counter;

        private readonly LoadHistory _counter;

        public DoctorController(IRepository repository, IOptions<LoadHistory> appSettings)
        {
            _repository = repository;
            _counter = appSettings.Value;

        }

        public IActionResult DisplayPatients(PatiensListViewModel search)
        {
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _repository.Set<User>().FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;
            if (_repository.Set<Doctor>().Any(an => an.Id == loggedUserId))
            {
                var doctorId = loggedUserId;

                var patients = _repository.Set<Patient>().Select(p => new PatientViewModel
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
            _counter.Counter = 7;
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _repository.Set<User>().FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;
            if (_repository.Set<Doctor>().Any(an => an.Id == loggedUserId))
            {
                var patientMessages = BuildPatientMessages(id, 7);

                return View("Messages", patientMessages);

            }
            return Redirect("~/Authentication/Login");
        }

        [HttpPost]
        public IActionResult Messages(PatientMessagesViewModel patientMessages)
        {
            var doctorId = _repository.Set<DoctorPatientMessage>().FirstOrDefault(fr => fr.PatientId == patientMessages.Patient.Id).DoctorId;

            var messages = _repository.Set<DoctorPatientMessage>().Where(wr => wr.DoctorId == doctorId && wr.PatientId == patientMessages.Patient.Id).ToList();

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
                patientMessages.Patient.Name = _repository.Set<Patient>().Single(sl => sl.Id == patientMessages.Patient.Id).Name;
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

            _repository.Add(patientResponse);


            var seenMessages = messages.Where(sl => sl.IsRead == false).ToList();

            foreach (var item in seenMessages)
            {
                item.IsRead = true;
                _repository.Update(item);
            }


            return RedirectToAction(nameof(Messages), new { id = patientMessages.Patient.Id });

        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            var user = _repository.Set<Patient>().FirstOrDefault(fr => fr.Id == Id);

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
            var edit = _repository.Set<Patient>().FirstOrDefault(fr => fr.Id == model.Id);

            edit.Notes = model.Notes;
            edit.AttentionLevel = model.SelectedItem == "High" ? model.AttentionLevel = true : model.AttentionLevel = false;

            _repository.Update(edit);

            return RedirectToAction(nameof(DisplayPatients));
        }

        [HttpPost]
        public IActionResult LoadMoreMessages(PatientMessagesViewModel model)
        {
            _counter.Counter = _counter.Counter + 7;
            var loadedMessagesCount = _counter.Counter;
            ModelState.Clear();
            return View("Messages", BuildPatientMessages(model.Patient.Id, loadedMessagesCount));
        }

        public PatientMessagesViewModel BuildPatientMessages(string patientId, int loadedMessagesCount)
        {
            var patient =_repository.Set<Patient>().FirstOrDefault(pt => pt.Id == patientId);

            var messages = _repository.Set<DoctorPatientMessage>().Where(msg => msg.PatientId == patientId).ToList();

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
