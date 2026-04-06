using System;
using System.Collections.Generic;
using System.Text;

namespace lake7.Domain.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? RideId { get; set; }
        public Guid? DeliveryId { get; set; }
        public required decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public DateTime TransactionDate { get; set; }= DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}

