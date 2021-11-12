using HomeTreatment.Data;
using HomeTreatment.Data.Models;
using System.Collections.Generic;

namespace HomeTreatment.Business.BusinesObjects
{
    public class PatientDashboardBO 
    {
        public List<Doctor> Doctors { get; set; }

        public List<DoctorPatientMessage> Messages { get; set; }
    }
}
