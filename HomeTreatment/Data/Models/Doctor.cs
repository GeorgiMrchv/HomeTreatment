using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTreatment.Data.Models
{
    public class Doctor
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        //public virtual User User { get; set; }
    }
}
