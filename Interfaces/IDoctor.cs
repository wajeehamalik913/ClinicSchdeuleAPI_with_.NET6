using Clinic.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApi.Interfaces
{
    public interface IDoctor
    {
        public Task<List<User>> GetDoctors();
        public Task<User> GetDoctor(int id);
        //public Task<IActionResult> GetDoctorSlots(Guid id);
        //public Task<IActionResult> GetDoctorAvailibility();
        //public Task<IActionResult> GetDoctorMaxAppointments();
        //public Task<IActionResult> GetDoctorHours(Guid id);



    }
}
