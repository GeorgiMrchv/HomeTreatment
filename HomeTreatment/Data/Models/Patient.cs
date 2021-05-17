using System.Collections.Generic;

namespace HomeTreatment.Data.Models
{
    public class Patient
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Notes { get; set; }

        public bool AttentionLevel { get; set; }

        public string DoctorId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<DoctorPatientMessage> DoctorPatientMessages { get; set; } = new HashSet<DoctorPatientMessage>();
    }
}
