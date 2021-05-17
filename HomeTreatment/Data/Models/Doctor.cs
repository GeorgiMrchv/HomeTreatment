using System.Collections.Generic;

namespace HomeTreatment.Data.Models
{
    public class Doctor
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<DoctorPatientMessage> DoctorPainetMessages { get; set; } = new HashSet<DoctorPatientMessage>();
    }
}
