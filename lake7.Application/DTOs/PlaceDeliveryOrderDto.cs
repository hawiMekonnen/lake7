using lake7.Domain.Entities;
using lake7.Domain.Enums;

namespace lake7.Application.DTOs
{
    public class PlaceDeliveryOrderDto
    {
        // Delivery Details
        public string SenderName { get; set; } = string.Empty;
        public string SenderPhone { get; set; } = string.Empty;
        public string ReceiverName { get; set; } = string.Empty;
        public string ReceiverPhone { get; set; } = string.Empty;
        public string PickupAddress { get; set; } = string.Empty;
        public string DropoffAddress { get; set; } = string.Empty;
        public double PickupLatitude { get; set; }
        public double PickupLongitude { get; set; }
        public double DropoffLatitude { get; set; }
        public double DropoffLongitude { get; set; }
        public string ItemDescription { get; set; } = string.Empty;
        public decimal EstimatedWeight { get; set; }
        public decimal EstimatedPrice { get; set; }

        // Payment Details
        public string PaymentMethod { get; set; } = "Cash"; // or "Wallet", "Card"
        public decimal PaymentAmount { get; set; }
    }
}
