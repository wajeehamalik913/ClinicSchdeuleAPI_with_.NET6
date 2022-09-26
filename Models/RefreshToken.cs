
#nullable disable
using Clinic.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicApi.Models
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string Token { get; set; }
        public string JWTId { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime DateAdded { get; set; }
        public double DateExpire { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User  User { get; set; }

    }
}
