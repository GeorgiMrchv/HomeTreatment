using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeTreatment.Data.Models
{
    public class Patient
    {
        [Key, ForeignKey("User")]
        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Notes { get; set; }

        public bool AttentionLevel { get; set; }

        public string UserId { get; set; }

        public string DoctorId { get; set; }

        public IEnumerable<DoctorPatientMessage> DoctorPatientMessages { get; set; }
    }
}
