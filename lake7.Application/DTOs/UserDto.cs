using lake7.Domain.Entities;

namespace lake7.Application.DTOs
{
    public class UserDto 
    {
        public Guid Id { get; set; }
        public string Email { get; set; }= string.Empty;
        public required string FullName { get; set; }
    }
}
