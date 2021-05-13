﻿using System;
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
            var patientId = 122; // Here I get the Id from the login user

            var patientMessages = _context.DoctorPatientMessages.Where(sl => sl.PatientId == patientId).ToList(); // get message           

            if (patientMessages.Count == 0)
            {
                return View(new PatientMessagesViewModel
                {
                    IsFirstVisit = true
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
            var patientId = _context.Patients.FirstOrDefault(fr=>fr.UserId == id);
            var patientResponse = new DoctorPatientMessage
            {
                Text = patientMessages.Message,
                PatientId = patientId.Id,
                DoctorId = patientMessages.SelectedItem,
                Timestamp = DateTime.Now,
                IsRead = false,
                IsWrittenByPatient = true
            };

            _context.DoctorPatientMessages.Add(patientResponse);
            _context.SaveChanges();

            return RedirectToAction(nameof(Communication));
        }
    }
}