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

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsRead { get; set; }

        public bool IsWrittenByPatient { get; set; }

        public string PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }

        public string DoctorId { get; set; }

        [ForeignKey(nameof(DoctorId))]
        public virtual Doctor Doctor { get; set; }       

    }
}
