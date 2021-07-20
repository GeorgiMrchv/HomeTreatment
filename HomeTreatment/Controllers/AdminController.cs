using System.Linq;
using HomeTreatment.Data;
using HomeTreatment.Data.Models;
using HomeTreatment.Web.Infrastructure;
using HomeTreatment.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HomeTreatment.Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using HomeTreatment.Web.Sample_test;

namespace HomeTreatment.Web.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IRepository _repository;

        public readonly OfficeEquipmentConfig _officeEquipmentConfig; // remove after tests 

        public AdminController(IRepository repository, IOptions<OfficeEquipmentConfig> officeEquipmentConfig)
        {
            _repository = repository;
            _officeEquipmentConfig = officeEquipmentConfig.Value; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AllUsers()
        {
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _repository.Set<User>().FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;

            if (_repository.Set<IdentityUserRole<string>>().Any(fr => fr.UserId == loggedUserId && fr.RoleId == ((int)UserRole.Admin).ToString()))
            {
                var allUsers = _repository.Set<User>().Select(sl => new UserViewModel
                {
                    Id = sl.Id,
                    FirstName = sl.FirstName,
                    LastName = sl.LastName,
                    EmailAddress = sl.Email

                }).ToList();

                foreach (var item in allUsers)
                {
                    if (_repository.Set<Doctor>().Any(an => an.Id == item.Id))
                    {
                        item.Status = "1";
                    }
                    else if (_repository.Set<Patient>().Any(an => an.Id == item.Id))
                    {
                        item.Status = "2";
                    }
                    else if (!_repository.Set<Doctor>().Any(an => an.Id == item.Id) && !_repository.Set<Patient>().Any(an => an.Id == item.Id))
                    {
                        if (_repository.Set<IdentityUserRole<string>>().Any(fr => fr.UserId == item.Id && fr.RoleId == ((int)UserRole.Admin).ToString()))
                        {
                            item.Status = "4";
                        }
                        else
                        {
                            item.Status = "3";
                        }
                    }
                }

                return View(new UserViewModel
                {
                    Users = allUsers
                });
            }

            return Redirect("~/Authentication/Login");
        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
            var timi = _officeEquipmentConfig.DeskConfig.Height; 
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _repository.Set<User>().FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;
            if (_repository.Set<IdentityUserRole<string>>().Any(fr => fr.UserId == loggedUserId && fr.RoleId == ((int)UserRole.Admin).ToString()))
            {
                string statusId = "";
                if (_repository.Set<Patient>().Any(an => an.Id == Id))
                {
                    statusId = "2";
                }
                else if (_repository.Set<Doctor>().Any(an => an.Id == Id))
                {
                    statusId = "1";
                }

                var user = _repository.Set<User>().FirstOrDefault(fr => fr.Id == Id);

                var userModel = new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.Email,
                    Status = statusId == "" ? "New User" : statusId
                };


                return View(userModel);

            }
            return Redirect("~/Authentication/Login");
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel user)
        {
            switch (user.Status)
            {
                case "Patient":
                    if (_repository.Set<Doctor>().Any(an => an.Id == user.Id))
                    {
                        if (_repository.Set<DoctorPatientMessage>().Any(an => an.DoctorId == user.Id))
                        {
                            var exDoctorMessages = _repository.Set<DoctorPatientMessage>().First(fr => fr.DoctorId == user.Id);

                            _repository.Remove(exDoctorMessages);

                        }

                        var exDoctor = _repository.Set<Doctor>().FirstOrDefault(fr => fr.Id == user.Id);

                        _repository.Remove(exDoctor);
                    }

                    var patient = new Patient
                    {
                        Id = user.Id,
                        Name = user.FirstName + " " + user.LastName,
                        EmailAddress = user.EmailAddress
                    };

                    if (_repository.Set<Patient>().Any(an => an.Id == patient.Id))
                    {
                        _repository.Update(patient);
                    }
                    else
                    {
                        _repository.Add(patient);
                    }

                    return RedirectToAction(nameof(AllUsers));

                case "Doctor":
                    if (_repository.Set<Patient>().Any(an => an.Id == user.Id))
                    {
                        if (_repository.Set<DoctorPatientMessage>().Any(an => an.PatientId == user.Id))
                        {
                            var exPatientMessages = _repository.Set<DoctorPatientMessage>().FirstOrDefault(fr => fr.PatientId == user.Id);

                            _repository.Remove(exPatientMessages);
                        }

                        var exPatient = _repository.Set<Patient>().First(fr => fr.Id == user.Id);

                        _repository.Remove(exPatient);

                    }
                    var doctor = new Doctor
                    {
                        Id = user.Id,
                        Name = user.FirstName + " " + user.LastName,
                        Email = user.EmailAddress
                    };
                    if (_repository.Set<Doctor>().Any(an => an.Id == doctor.Id))
                    {
                        _repository.Update(doctor);
                    }
                    else
                    {
                        _repository.Remove(doctor);
                    }

                    return RedirectToAction(nameof(AllUsers));

                case "New User":
                    return RedirectToAction(nameof(AllUsers));

                case "Admin":
                default:
                    break;
            }
           
            return View();
        }        
    }
}

//localhost/Adminstration/EditAdministrator/3
