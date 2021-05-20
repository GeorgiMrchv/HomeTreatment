﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using HomeTreatment.ViewModels;
using HomeTreatment.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace HomeTreatment.Controllers
{
    public class DoctorController : Controller
    {

        private readonly HomeTreatmentDbContext _context;

        public int PageSize = 4;
        public DoctorController(HomeTreatmentDbContext context)
        {
            _context = context;
        }

        [Authorize]
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

                int patientPage = 1;

                if (search.SearchTerm == null)
                {
                    var patientsByPage = patients
                       .OrderBy(p => p.Id)
                       .Skip((patientPage - 1) * PageSize)
                       .Take(PageSize)
                       .ToList();

                    var allPatientsOfCurrentDoctor = patientsByPage.Where(wr => wr.DoctorId == loggedUserId).ToList();

                    return View(new PatiensListViewModel
                    {
                        Patients = allPatientsOfCurrentDoctor,
                        PagingInfo = new PagingInfo
                        {
                            CurrentPage = patientPage,
                            ItemsPerPage = PageSize,
                            TotalItems = _context.Patients.Count()
                        }
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
                    PagingInfo = new PagingInfo
                    {
                        CurrentPage = 1,
                        ItemsPerPage = PageSize,
                        TotalItems = _context.Patients.Count()
                    }
                });

            }
            return Redirect("~/Authentication/Login");
        }

        [HttpGet]
        public IActionResult Messages(string id)
        {
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _context.Users.FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;
            if (_context.Doctors.Any(an => an.Id == loggedUserId))
            {
                var patientMessages = BuildPatientMessages(id);

                return View("Messages", patientMessages);

            }
            return Redirect("~/Authentication/Login");
        }

        [HttpPost]
        public IActionResult Messages(PatientMessagesViewModel patientMessages, string id)
        {
            var doctorId = _context.DoctorPatientMessages.FirstOrDefault(fr => fr.PatientId == id).DoctorId;
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

            return RedirectToAction(nameof(Messages), new { id = patientMessages.Patient.Id });

        }

        public PatientMessagesViewModel BuildPatientMessages(string patientId)
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


            var patientMessages = new PatientMessagesViewModel
            {
                Patient = patientViewModel,
                Messages = allMessages

            };

            return patientMessages;
        }
    }
}
