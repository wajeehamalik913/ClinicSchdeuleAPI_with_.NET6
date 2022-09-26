#nullable disable
using Clinic.Data;
using Clinic.Models;
using ClinicApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClinicApi.Services
{
    public class DoctorServices : IDoctor
    {
        private readonly ClinicContext _context;
        public DoctorServices(ClinicContext context)
        {
            _context = context;
        }
        public async Task<List<User>> GetDoctors()
        {
            var val= await _context.Users.Where(u => u.RoleId.Equals(2)).ToListAsync();
            return val;
        }

        //public Task<IActionResult> GetDoctorAvailibility()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<User> GetDoctor(int id)
        {
            var val = await _context.Users.Where(u => u.RoleId.Equals(2)).SingleOrDefaultAsync(x => x.User_Id == id);
            if (val == null)
            {
                return null;
            }
            return new User
            {
                User_Id=val.User_Id,
                Name = val.Name,
                Email = val.Email,
                Password=val.Password,
                RoleId = val.RoleId,
            };
            
        }

        //public Task<IActionResult> GetDoctorHours(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IActionResult> GetDoctorMaxAppointments()
        //{
        //    throw new NotImplementedException();
        //}

    }
}
