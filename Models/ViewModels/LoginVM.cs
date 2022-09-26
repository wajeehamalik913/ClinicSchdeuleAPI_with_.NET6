using Microsoft.Build.Framework;

namespace ClinicApi.Models.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string EmailAddress { get; set; } = null!;
        [Required]
        public string Password { get; set; } = null!;
    }
}
