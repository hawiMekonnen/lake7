using lake7.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lake7.Application.Interface
{
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);
        Task <List<User>> GetUserAsync();
        Task<User?> GetUserByIdAsync(Guid ID);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> UpdateUserAsync(User user, Guid ID);
        Task<User?> ValidateUserAsync(string email, string password);

    }
}
