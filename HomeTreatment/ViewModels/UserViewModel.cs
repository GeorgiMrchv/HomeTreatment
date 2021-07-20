using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeTreatment.Web.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string Password { get; set; }

        public string Status { get; set; }

        public IEnumerable<UserViewModel> Users { get; set; }

    }
}
