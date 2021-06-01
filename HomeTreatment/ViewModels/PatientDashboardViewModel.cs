using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeTreatment.ViewModels
{
    public class PatientDashboardViewModel
    {
        public List<DoctorPatientMessageViewModel> Messages { get; set; }

        [Required]
        [MaxLength(2048)]
        public string Message { get; set; }

        public string DoctorId { get; set; }

        public string DoctorName { get; set; }

        public List<DoctorViewModel> Doctors { get; set; }
    }
}
