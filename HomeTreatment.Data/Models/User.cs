using System.ComponentModel.DataAnnotations.Schema;
using HomeTreatment.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeTreatment.Data
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }       

        public virtual Patient Patient { get; set; }

        public virtual Doctor Doctor { get; set; }
    }
}
