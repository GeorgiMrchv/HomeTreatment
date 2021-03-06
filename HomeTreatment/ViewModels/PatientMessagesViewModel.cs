using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeTreatment.Web.ViewModels
{
    public class PatientMessagesViewModel
    {
        public PatientViewModel Patient { get; set; }

        [Required(ErrorMessage = "Please add a text message!")] 
        //TODO: To add length attribute here
        public string Message { get; set; }

        public bool IsFirstVisit { get; set; }

        public string SelectedItem { get; set; }

        public List<DoctorPatientMessageViewModel> Messages { get; set; }

        public List<DoctorViewModel> Doctors { get; set; }
    }
}
