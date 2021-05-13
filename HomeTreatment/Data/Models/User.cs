using System.ComponentModel.DataAnnotations.Schema;
using HomeTreatment.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace HomeTreatment.Data
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }       

        [ForeignKey("Patient")]
        public int? PatientId { get; set; }

        public virtual Patient Patient { get; set; }

        [ForeignKey("Doctor")]
        public int? DoctorId { get; set; }

        public virtual Doctor Doctor { get; set; }

        
    }
}
