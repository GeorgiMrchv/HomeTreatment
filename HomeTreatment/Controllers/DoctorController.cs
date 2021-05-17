using System;
using System.Collections.Generic;
using System.Linq;
using HomeTreatment.Data;
using HomeTreatment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeTreatment.Controllers
{
    public class DoctorController : Controller
    {

        private readonly HomeTreatmentDbContext _context;

        public DoctorController(HomeTreatmentDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Communication(string id)
        {
            var patientId = id; // Here I get the Id from the login user

            var patientMessages = _context.DoctorPatientMessages.Where(sl => sl.PatientId == patientId).ToList(); // get message           

            if (patientMessages.Count == 0)
            {
                var allDoctors = _context.Doctors.Select(sl => new DoctorViewModel
                {
                    Id = sl.Id,
                    Name = sl.Name,
                    EmailAddress = sl.Email
                }).ToList();

                return View(new PatientMessagesViewModel
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

                
                return View(new PatientMessagesViewModel
                {
                    IsFirstVisit = false,
                    Messages = allMessages                    
                });
            }

        }

        [HttpPost]
        public IActionResult Communication(PatientMessagesViewModel patientMessages, string id)
        {
            var patient = _context.Patients.FirstOrDefault(fr => fr.Id == id);
            var patientResponse = new DoctorPatientMessage
            {
                Text = patientMessages.Message,
                PatientId = patient.Id,
                DoctorId = patientMessages.SelectedItem,
                Timestamp = DateTime.Now,
                IsRead = false,
                IsWrittenByPatient = true
            };

            _context.DoctorPatientMessages.Add(patientResponse);
            _context.SaveChanges();

            patient.DoctorId = patientMessages.SelectedItem;
            patient.Notes = patientMessages.Message;

            _context.Patients.Update(patient);
            _context.SaveChanges();

            return RedirectToAction(nameof(Communication));
        }
    }
}
