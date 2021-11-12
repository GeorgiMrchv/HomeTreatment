using System.ComponentModel.DataAnnotations;

namespace HomeTreatment.Business.ViewModels
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
        //[StringLength(20,ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 9)]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your Password")]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }

    }
}
