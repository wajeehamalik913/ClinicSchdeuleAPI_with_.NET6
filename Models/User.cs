using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Clinic.Models
{
    public partial class User : IdentityUser
    {
        public int User_Id { get; set; }
        public string Name { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int RoleId { get; set; }
        public string Custom { get; set; } = null!;
    }
}
