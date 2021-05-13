using System.Threading.Tasks;
using HomeTreatment.Data;
using HomeTreatment.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HomeTreatment.Infrastructure.Enum;

namespace HomeTreatment.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;

        private readonly HomeTreatmentDbContext _context;


        public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager,  HomeTreatmentDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserViewModel userViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            var user = await _userManager.FindByEmailAsync(userViewModel.EmailAddress);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(userViewModel.EmailAddress, userViewModel.Password, false, false);

                if (signInResult.Succeeded)
                {
                    string userId = user.Id;

                    var role = _context.UserRoles.FirstOrDefault(fr => fr.UserId == userId).RoleId;
                    
                    if (role == ((int)UserRole.Doctor).ToString())
                    {
                        return RedirectToAction("DisplayPatients", "Patient");
                    }

                    else if (role == ((int)UserRole.Patient).ToString())
                    {
                        return RedirectToAction("Communication", "Doctor", new { id = userId });

                    }

                    else if (role == ((int)UserRole.Admin).ToString())
                    {
                        return RedirectToAction("AllUsers", "Admin");
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
            var user = new User // tuka probvah User
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                UserName = registerViewModel.Email,
                Email = registerViewModel.Email
            };

            var result = await _userManager.CreateAsync(user, registerViewModel.Password);

            if (result.Succeeded)
            {
                //sign in
                var signInResult = await _signInManager.PasswordSignInAsync(user, registerViewModel.Password, true, false);

                if (signInResult.Succeeded)
                {
                    //return RedirectToAction("DisplayPatients", "Patient");
                    return View("Thanks");
                }
            }
            return Content("You failed register");
        }


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
