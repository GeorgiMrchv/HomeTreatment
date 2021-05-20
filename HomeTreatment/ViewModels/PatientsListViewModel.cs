using System.Collections.Generic;

namespace HomeTreatment.ViewModels {

    public class PatiensListViewModel
    {
        public IEnumerable<PatientViewModel> Patients { get; set; }

        public PagingInfo PagingInfo { get; set; }

        public string SearchTerm { get; set; }
    }
}