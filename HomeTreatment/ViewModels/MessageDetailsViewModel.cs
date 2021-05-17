using System;

namespace HomeTreatment.ViewModels
{
    public class MessageDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string Notes { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsRead { get; set; }

        public string DoctorId { get; set; }
    }
}
