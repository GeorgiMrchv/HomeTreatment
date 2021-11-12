using HomeTreatment.Data;
using System;
using System.Collections.Generic;

namespace HomeTreatment.Business.BusinesObjects
{
    public class DoctorPatientMessageBO
    {
        public int Id { get; set; }

        public string DoctorId { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsRead { get; set; }

        public string PatientId { get; set; }

        public bool IsWrittenByPatient { get; set; }
    }
}
