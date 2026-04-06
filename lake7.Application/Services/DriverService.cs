using System;
using System.Collections.Generic;
using System.Text;

using lake7.Domain.Entities;
using lake7.Application.Interface;

namespace lake7.Application.Services
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;

        public DriverService(IDriverRepository driverRepository)
        {
            _driverRepository = driverRepository;
        }

        public async Task<Driver> RegisterDriverAsync(Driver driver)
        {
            driver.CreatedAt = DateTime.UtcNow;
            return await _driverRepository.AddAsync(driver);
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
