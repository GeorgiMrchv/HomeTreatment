using HomeTreatment.Data;
using HomeTreatment.Data.Models;
using System.Collections.Generic;

namespace HomeTreatment.Business.BusinesObjects
{
    public class PatientBO 
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string DoctorId { get; set; }

        public List<DoctorPatientMessageBO> Messages { get; set; }

        public int UnreadMessagesCount { get; set; }
    }
}
