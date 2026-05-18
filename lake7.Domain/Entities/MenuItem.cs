using System;

namespace lake7.Domain.Entities
{
    public class MenuItem : CommonEntity
    {
        public Guid RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Availability { get; set; } = "available"; // available, out_of_stock
        public int StockLevel { get; set; } = 100;

        public Restaurant? Restaurant { get; set; }
    }
}
