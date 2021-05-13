using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTreatment.Data.Models
{
    public class Doctor
    {
        [Key, ForeignKey("User")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string UserId { get; set; }
    }
}
