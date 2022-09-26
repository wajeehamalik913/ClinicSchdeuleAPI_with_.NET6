using System;
using System.Collections.Generic;

namespace Clinic.Models
{
    public partial class DoctorSchedule
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int SlotsBooked { get; set; }
        public int SlotsAvailable { get; set; }
        public int TotalAppointments { get; set; }
        public DateOnly Date { get; set; }
    }
}
