using System;

namespace lake7.Domain.Entities
{
    public class AdminAccount : CommonEntity
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; } // Should be hashed in practice
        public string Role { get; set; } = "Admin";
        public DateTime LastLogin { get; set; }
    }
}
