using Microsoft.AspNetCore.Identity;
using lake7.Domain.Entities;
using lake7.Application.Interface;

namespace lake7.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password is required");

            // Hash the password here (this is the correct place)
            user.Password = _passwordHasher.HashPassword(user, user.Password);

            return await _userRepository.AddAsync(user);
        }

        public async Task<List<User>> GetUserAsync()
        {
            return (await _userRepository.GetAllAsync()).ToList();
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User?> UpdateUserAsync(User user, Guid id)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null) return null;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            if (!string.IsNullOrEmpty(user.Password))
            {
                existingUser.Password = _passwordHasher.HashPassword(existingUser, user.Password);
            }

            return await _userRepository.UpdateAsync(existingUser);
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return null;

            // Use the same PasswordHasher for verification
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

           
            if (result == PasswordVerificationResult.Success ||
                result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                return user;
            }

            return null;   // Wrong password
        }
    }
}
