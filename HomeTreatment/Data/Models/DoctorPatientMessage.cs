using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomeTreatment.Data.Models;

namespace HomeTreatment.Data
{
    public class DoctorPatientMessage
    {
        [Key]
        public int Id { get; set; }

        public string DoctorId { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsRead { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patients { get; set; }

        public bool IsWrittenByPatient { get; set; }

    }
}
