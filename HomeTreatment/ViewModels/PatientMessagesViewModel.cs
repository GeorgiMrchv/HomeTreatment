using System.Collections.Generic;

namespace HomeTreatment.ViewModels
{
    public class PatientMessagesViewModel
    {
        public PatientViewModel Patient { get; set; }

        public string Message { get; set; }

        public bool IsFirstVisit { get; set; }

        public string SelectedItem { get; set; }

        public List<DoctorPatientMessageViewModel> Messages { get; set; }
    }
}
