using HomeTreatment.Data;
using HomeTreatment.Data.Models;
using HomeTreatment.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using HomeTreatment.Business.BusinesObjects;

namespace HomeTreatment.Business.Services
{
    public class Users : IUsersService
    {
        private readonly IRepository _repository;

        public Users(IRepository repository)
        {
            _repository = repository;
        }

        public LoggedInUserBO GetLogginUser(string userId)
        {
            var user = GetUser()
                .Include(u => u.Patient.Doctor)
                .Include(u => u.Doctor.Patients)
                .Single(u => u.Id == userId);

            if (user.Doctor != null)
            {
                var patiants = FindPatientsWithUnreadMessages(user.Doctor.Id);
                var returned = new LoggedInUserBO
                {
                    Patients = patiants
                };
                return returned;
            }
            else if (user.Patient != null)
            {
                var patientDashboardModel = BuildPatiantDashboardViewModel(user.Patient);
                var returned = new LoggedInUserBO
                {
                    Doctor = patientDashboardModel
                };
                return returned;
            }
            else
            {
                throw new Exception("Trying to load Dashboard, but is not Doctor or Patiant");
            }
        }

        List<PatientBO> FindPatientsWithUnreadMessages(string doctorId)
        {
            return _repository.Set<Patient>()
                    .Where(p => p.DoctorId == doctorId && p.DoctorPatientMessages.Any(m => m.IsWrittenByPatient && m.IsRead == false))
                    .Select(p => new PatientBO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        EmailAddress = p.EmailAddress,
                        Messages = new List<DoctorPatientMessageBO>
                        {
                            p.DoctorPatientMessages
                                .Where(m => m.IsWrittenByPatient && m.IsRead == false)
                                .OrderByDescending(m => m.Timestamp)
                                .Select(m => new DoctorPatientMessageBO{
                                    Id = m.Id,
                                    Text = m.Text,
                                    Timestamp = m.Timestamp
                                })
                                .First()
                        },

                        UnreadMessagesCount = p.DoctorPatientMessages.Count(m => m.IsRead == false)
                    })
                    .ToList()
                    .OrderByDescending(p => p.Messages.Single().Timestamp)
                    .ToList();
        }

        public PatientDashboardBO BuildPatiantDashboardViewModel(Patient patient)
        {
            var model = new PatientDashboardBO();

            var messages = patient.DoctorPatientMessages
                .Where(m => m.DoctorId == patient.DoctorId)
                .OrderBy(m => m.Timestamp)
                .ToList();

            model.Doctors = _repository.Set<Doctor>()
                .Select(d => new Doctor
                {
                    Id = d.Id,
                    Email = d.Email,
                    Name = d.Name
                })
                 .OrderBy(d => d.Name)
                 .ToList();

            if (messages.Count != 0)
            {
                model.Messages = messages
                    .Select(m => new DoctorPatientMessage
                    {
                        Id = m.Id,
                        Text = m.Text,
                        Timestamp = m.Timestamp,
                        IsRead = m.IsRead,
                        IsWrittenByPatient = m.IsWrittenByPatient
                    })
                    .ToList();
            }

            return model;
        }

        public void SetMessageAsRead(string Id)
        {
            var message = _repository.Set<DoctorPatientMessage>().Where(fr => fr.PatientId == Id).ToList();

            foreach (var item in message)
            {
                item.IsRead = true;
                _repository.Update(item);
            }
        }

        public void AddMessage(User user, string text)
        {
            _repository.Add(new DoctorPatientMessage
            {
                DoctorId = user.Patient.DoctorId,
                PatientId = user.Patient.Id,
                Text = text,
                IsWrittenByPatient = true,
                Timestamp = DateTime.Now
            });
        }

        public bool IsDoctor(string doctorId) => _repository.Set<Doctor>().Any(d => d.Id == doctorId);

        public IQueryable<User> GetUser()
        {
            return _repository.Set<User>().Include(u => u.Patient.DoctorPatientMessages);
        }

        public User GetPatient(string userId)
        {
            return GetUser().Single(u => u.Id == userId);
        }
       
    }
}
