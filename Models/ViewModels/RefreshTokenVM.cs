
#nullable disable
using System.ComponentModel.DataAnnotations;

namespace ClinicApi.Models.ViewModels
{
    public class RefreshTokenVM
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public string user_Id { get; set; }

    }
}
