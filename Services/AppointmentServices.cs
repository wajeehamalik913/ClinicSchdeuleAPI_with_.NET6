using ClinicApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApi.Services
{
    public class AppointmentServices : IAppointments
    {
        public Task<IActionResult> Book()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Cancel()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Details()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> History(Guid patient_id)
        {
            throw new NotImplementedException();
        }
    }
}
