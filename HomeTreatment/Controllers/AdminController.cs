using System.Linq;
using HomeTreatment.Data;
//using HomeTreatment.Data.Models;
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

            var allUsers =
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
                Status = p.RoleId
            }).ToList();


            return View(new UserViewModel
            {
                Users = allUsers
            });

        }

        [HttpGet]
        public IActionResult Edit(string Id)
        {
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
                Status = p.RoleId
            }).FirstOrDefault(fr => fr.Id == Id);

            return View(new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmailAddress = user.EmailAddress,
                Status = user.Status

            });
        }

        [HttpPost]
        public IActionResult Edit(UserViewModel user)
        {
            switch (user.Status)
            {
                case "Patient":
                    //var patient = new Patient
                    //{
                    //    Id = 1,
                    //    Name = user.FirstName + " " + user.LastName, 
                    //    EmailAddress = user.EmailAddress
                    //};

                    //var patientRole = _context.UserRoles.FirstOrDefault(fr => fr.UserId == "[[ID]]");
                    //patientRole.RoleId = "2";
                    //_context.UserRoles.Update(patientRole);
                    //_context.Patients.Add(patient);
                    //_context.SaveChanges();
                    break;

                case "Doctor":
                    //var doctor = new Doctor
                    //{
                    //    //Id = user.Id,
                    //    Name = user.FirstName + " " + user.LastName,
                    //    Email = user.EmailAddress
                    //};
                    //var doctorRole = _context.UserRoles.FirstOrDefault(fr => fr.UserId == doctor.Id);
                    //_context.UserRoles.Remove(doctorRole);
                    //_context.SaveChanges();
                    //doctorRole.RoleId = "1";
                    //_context.UserRoles.Add(doctorRole);
                    //_context.SaveChanges();
                    //_context.Doctors.Add(doctor);
                    //_context.SaveChanges();

                    return View(new UserViewModel
                    {
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailAddress = user.EmailAddress,
                        Status = user.Status
                    });

                case "Admin":
                default:
                    break;
            }
            return View();           
        }
    }
}
