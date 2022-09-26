using Clinic.Models;
using ClinicApi.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ClinicApi.Interfaces
{
    public interface IAuth
    {
        public Task<User> Register([FromBody] RegisterVM registerVM);
        public Task<AuthTokenVM> Login([FromBody] LoginVM loginVM);
        public Task<AuthTokenVM> RefreshToken([FromBody] RefreshTokenVM refreshTokenVM);

    }
}
