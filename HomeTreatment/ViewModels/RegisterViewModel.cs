using System.ComponentModel.DataAnnotations;

namespace HomeTreatment.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please fill your First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please fill your Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please fill your Email Address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please fill your Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your Password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
