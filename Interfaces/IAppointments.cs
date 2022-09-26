using Microsoft.AspNetCore.Mvc;

namespace ClinicApi.Interfaces
{
    public interface IAppointments
    {
        public Task<IActionResult> Book();
        public Task<IActionResult> Cancel();
        public Task<IActionResult> Details();
        public Task<IActionResult> History(Guid patient_id);
    }
}
