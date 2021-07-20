using System.Collections.Generic;

namespace HomeTreatment.Web.ViewModels
{
    public class PatientViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Notes { get; set; }

        public bool AttentionLevel { get; set; }

        public string DoctorId { get; set; }

        public List<DoctorPatientMessageViewModel> Messages { get; set; }

        public int UnreadMessagesCount { get; set; }

        public string SelectedItem { get; set; }
    }
}
