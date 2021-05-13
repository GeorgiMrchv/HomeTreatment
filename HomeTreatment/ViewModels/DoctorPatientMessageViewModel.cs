﻿using System;

namespace HomeTreatment.ViewModels
{
    public class DoctorPatientMessageViewModel
    {
        public int Id { get; set; }

        public string DoctorId { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsRead { get; set; }

        public int PatientId { get; set; }

        public bool IsWrittenByPatient { get; set; }
    }
}