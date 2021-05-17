using System.Collections.Generic;

namespace HomeTreatment.ViewModels {

    public class PatiensListViewModel
    {
        public IEnumerable<PatientViewModel> Patients { get; set; }

        public IEnumerable<MessageDetailsViewModel> MessageDetails { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string SearchTerm { get; set; }

        public string DoctorId { get; set; }
    }
}