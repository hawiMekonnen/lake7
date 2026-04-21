using lake7.Domain.Entities;
using lake7.Domain.Enums;

namespace lake7.Application.Interface
{
    public interface IDeliveryService
    {
        Task<Delivery> RequestDeliveryAsync(Delivery delivery);
        Task<List<Delivery>> GetAllDeliveriesAsync();
        Task<Delivery?> GetDeliveryByIdAsync(Guid id);
        Task<Delivery?> UpdateDeliveryStatusAsync(Guid id, RideStatus status);
    }
}
