using System.Threading.Tasks;
using HomeTreatment.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HomeTreatment.Web.Infrastructure;
using HomeTreatment.Data;
using HomeTreatment.Data.Repository;
using HomeTreatment.Data.Models;

namespace HomeTreatment.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly IRepository _repository;

        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager, IRepository repository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            var logout = LogOut();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel userViewModel)
        {
            //validation -- dobra praktika e na publi4nitemetodi da si slagam validation
            if (string.IsNullOrEmpty(userViewModel.EmailAddress))
            {
                ModelState.AddModelError(nameof(UserViewModel.EmailAddress), "Enter a name");
            }

            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            //data access
            var user = await _userManager.FindByEmailAsync(userViewModel.EmailAddress);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(userViewModel.EmailAddress, userViewModel.Password, false, false);

                if (signInResult.Succeeded)
                {
                    string userId = user.Id;

                    if (_repository.Set<Doctor>().Any(an => an.Id == userId))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }

                    else if (_repository.Set<Patient>().Any(an => an.Id == userId))
                    {
                        return RedirectToAction("Index","Dashboard");

                    }

                    else if (!_repository.Set<Patient>().Any(an => an.Id == userId) && !_repository.Set<Doctor>().Any(an => an.Id == userId))
                    {
                        //if (_context.UserRoles.Any(fr => fr.UserId == userId && fr.RoleId == ((int)UserRole.Admin).ToString()))
                        if (_repository.Set<IdentityUserRole<string>>().Any(fr => fr.UserId == userId && fr.RoleId == ((int)UserRole.Admin).ToString()))
                        {
                            return RedirectToAction("AllUsers", "Admin");
                        }
                        var logout = LogOut();
                        return Content("You still don't have a permission");
                    }
                }
            }
            return Content("Invalid user name or password");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            var user = new User 
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, registerViewModel.Password, true, false);

                if (signInResult.Succeeded)
                {
                    var logout = LogOut();
                    return View("Thanks");
                }
            }
            
            return Content("You failed register");
        }


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/Authentication/Login");
        }
    }
}
