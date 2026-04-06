using lake7.Application.DTOs;
using lake7.Domain.Entities;

namespace lake7.Application.Helpers
{
    public static class UserMapper
    {
        public static UserDto ToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.Name
            };
        }
    }
}
