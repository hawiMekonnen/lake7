using lake7.Application.Interface;
using lake7.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace lake7.Application.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly PasswordHasher<Driver> _passwordHasher = new PasswordHasher<Driver>();

        public DriverService(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<Driver> RegisterDriverAsync(Driver driver)
        {
            if (string.IsNullOrWhiteSpace(driver.Password))
                throw new ArgumentException("Password is required");

      
            driver.Password = _passwordHasher.HashPassword(driver, driver.Password);
            driver.CreatedAt = DateTime.UtcNow;
            return await _driverRepository.AddAsync(driver);
        }

        public async Task<Driver?> ValidateDriverAsync(string email, string password)
        {
           var driver = await _driverRepository.GetByEmailAsync(email);
            if (driver == null)
            {
                return null;
            }
            var result = _passwordHasher.VerifyHashedPassword(driver, driver.Password, password);


            if (result == PasswordVerificationResult.Success ||
                result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                return driver;
            }

            return null;
        }
        public async Task<List<Driver>> GetDriversAsync()
        {
            return (await _driverRepository.GetAllAsync()).ToList();
        }

        public async Task<Driver?> GetDriverByIdAsync(Guid id)
        {
            return await _driverRepository.GetByIdAsync(id);
        }

        public async Task<Driver?> UpdateDriverAsync(Driver driver, Guid id)
        {
            var existingDriver = await _driverRepository.GetByIdAsync(id);
            if (existingDriver == null) return null;

            existingDriver.Name = driver.Name;
            existingDriver.Email = driver.Email;
            existingDriver.UpdatedAt = DateTime.UtcNow;

            return await _driverRepository.UpdateAsync(existingDriver);
        }

        public async Task<bool> SetAvailabilityAsync(Guid id, bool isAvailable)
        {
            var driver = await _driverRepository.GetByIdAsync(id);
            if (driver == null) return false;

            driver.IsAvailable = isAvailable;
            driver.UpdatedAt = DateTime.UtcNow;
            await _driverRepository.UpdateAsync(driver);
            return true;
        }

    }
}
