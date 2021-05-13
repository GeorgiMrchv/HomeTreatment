using System.Collections.Generic;
using HomeTreatment.Data;

namespace HomeTreatment.ViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Notes { get; set; }

        public bool AttentionLevel { get; set; }

    }
}
