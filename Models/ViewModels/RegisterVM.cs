using Microsoft.Build.Framework;

namespace ClinicApi.Models.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Email { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
        [Required]
        public string Role { get; set; } = null!;
        [Required]
        public int RoleId { get; set; }
        public string Custom { get; set; } = null!;
    }
}
