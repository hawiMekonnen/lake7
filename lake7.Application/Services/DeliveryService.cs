using lake7.Domain.Entities;
using lake7.Domain.Enums; // import DeliveryStatus
using lake7.Application.Interface;

namespace lake7.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;

        public DeliveryService(IDeliveryRepository deliveryRepository)
        {
            _deliveryRepository = deliveryRepository;
        }

        public async Task<Delivery> RequestDeliveryAsync(Delivery delivery)
        {
            delivery.Status = RideStatus.Pending;
            delivery.RequestedAt = DateTime.UtcNow;
            return await _deliveryRepository.AddAsync(delivery);
        }

        public async Task<List<Delivery>> GetAllDeliveriesAsync()
        {
            return (await _deliveryRepository.GetAllAsync()).ToList();
        }

        public async Task<Delivery?> GetDeliveryByIdAsync(Guid id)
        {
            return await _deliveryRepository.GetByIdAsync(id);
        }

        public async Task<Delivery?> UpdateDeliveryStatusAsync(Guid id, RideStatus status)
        {
            var delivery = await _deliveryRepository.GetByIdAsync(id);
            if (delivery == null) return null;

            delivery.Status = status;
            delivery.UpdatedAt = DateTime.UtcNow;
            return await _deliveryRepository.UpdateAsync(delivery);
        }
    }
}
