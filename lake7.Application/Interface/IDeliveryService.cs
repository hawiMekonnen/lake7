using lake7.Domain.Entities;

namespace lake7.Application.Interface
{
    public interface IDeliveryService
    {
        Task<Delivery> RequestDeliveryAsync(Delivery delivery);
        Task<List<Delivery>> GetAllDeliveriesAsync();
        Task<Delivery?> GetDeliveryByIdAsync(Guid id);
        Task<Delivery?> UpdateDeliveryStatusAsync(Guid id, string status);
    }
}
