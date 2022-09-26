#nullable disable
namespace ClinicApi.Models.ViewModels
{
    public class AuthTokenVM
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; } 
        public string UserId { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
