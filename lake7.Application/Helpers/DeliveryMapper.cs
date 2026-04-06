using lake7.Application.DTOs;
using lake7.Domain.Entities;

namespace lake7.Application.Helpers
{
    public static class DeliveryMapper
    {
        public static DeliveryDto ToDto(Delivery delivery)
        {
            return new DeliveryDto
            {
                Id = delivery.Id,
                PickupLocation = delivery.PickupLocation,
                DropoffLocation = delivery.DropoffLocation,
                PackageDescription = delivery.PackageDetails,
                Status = delivery.Status
            };
        }
    }
}
