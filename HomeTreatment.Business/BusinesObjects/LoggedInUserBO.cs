using HomeTreatment.Data.Models;
using System.Collections.Generic;

namespace HomeTreatment.Business.BusinesObjects
{
    public class LoggedInUserBO
    {
        public Patient Patient { get; set; }

        public PatientDashboardBO Doctor { get; set; }

        public List<PatientBO> Patients { get; set; }
    }
}
