using System.Linq;
using HomeTreatment.Data;
using HomeTreatment.Data.Models;
using HomeTreatment.Infrastructure.Enum;
using HomeTreatment.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeTreatment.Controllers
{
    public class AdminController : Controller
    {
        private readonly HomeTreatmentDbContext _context;

        public AdminController(HomeTreatmentDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AllUsers()
        {
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _context.Users.FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;
            if (_context.UserRoles.Any(fr => fr.UserId == loggedUserId && fr.RoleId == ((int)UserRole.Admin).ToString()))
            {
                var allUsers = _context.Users.Select(sl => new UserViewModel
                {
                    Id = sl.Id,
                    FirstName = sl.FirstName,
                    LastName = sl.LastName,
                    EmailAddress = sl.Email

                }).ToList();

                foreach (var item in allUsers)
                {
                    if (_context.Doctors.Any(an => an.Id == item.Id))
                    {
                        item.Status = "1";
                    }
                    else if (_context.Patients.Any(an => an.Id == item.Id))
                    {
                        item.Status = "2";
                    }
                    else if (!_context.Doctors.Any(an => an.Id == item.Id) && !_context.Patients.Any(an => an.Id == item.Id))
                    {
                        if (_context.UserRoles.Any(fr => fr.UserId == item.Id && fr.RoleId == ((int)UserRole.Admin).ToString()))
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
            var loggedUserEmail = User.Identity.Name;
            var loggedUserId = _context.Users.FirstOrDefault(fr => fr.Email == loggedUserEmail).Id;
            if (_context.UserRoles.Any(fr => fr.UserId == loggedUserId && fr.RoleId == ((int)UserRole.Admin).ToString()))
            {
                string statusId = "";
                if (_context.Patients.Any(an => an.Id == Id))
                {
                    statusId = "2";
                }
                else if (_context.Doctors.Any(an => an.Id == Id))
                {
                    statusId = "1";
                }
                var user =
                (
                from c in _context.Users
                join p in _context.UserRoles on c.Id equals p.UserId into ps
                from p in ps.DefaultIfEmpty()
                select new UserViewModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    EmailAddress = c.Email,
                    Status = statusId == "" ? "New User" : statusId
                }).FirstOrDefault(fr => fr.Id == Id);


                return View(new UserViewModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    EmailAddress = user.EmailAddress,
                    Status = user.Status

                });

            }
            return Redirect("~/Authentication/Login");
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel user)
        {
            switch (user.Status)
            {
                case "Patient":
                    var patient = new Patient
                    {
                        Id = user.Id,
                        Name = user.FirstName + " " + user.LastName,
                        EmailAddress = user.EmailAddress
                    };

                    _context.Patients.Add(patient);
                    _context.SaveChanges();

                    return RedirectToAction(nameof(AllUsers));
                case "Doctor":
                    var doctor = new Doctor
                    {
                        Id = user.Id,
                        Name = user.FirstName + " " + user.LastName,
                        Email = user.EmailAddress
                    };
                    _context.Doctors.Add(doctor);
                    _context.SaveChanges();
                   
                    return RedirectToAction(nameof(AllUsers));

                case "New User":
                    break;

                case "Admin":
                default:
                    break;
            }
            return View();
        }
    }
}
