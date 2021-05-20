using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeTreatment.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeTreatment
{
    public class HomeController : Controller
    {
        private readonly SignInManager<User> _signInManager;

        public HomeController(SignInManager<User> signInManager)
        {
            
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            _signInManager.SignOutAsync();
            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        



    }
}
