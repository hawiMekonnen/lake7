using lake7.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace lake7.Application.Interface
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(string email, string password, string name);
        Task<string> LoginAsync(string email, string password); // Generate a JWT token if valid
        Task<bool> ValidateTokenAsync(string token);
    }
}
