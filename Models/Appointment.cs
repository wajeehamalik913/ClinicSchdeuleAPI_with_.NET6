using System;
using System.Collections.Generic;

namespace Clinic.Models
{
    public partial class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int SlotId { get; set; }
        public int TotalSlots { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public DateOnly BookingDate { get; set; }
        public string Status { get; set; } = null!;
    }
}
