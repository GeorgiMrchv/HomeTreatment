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
    }
}
