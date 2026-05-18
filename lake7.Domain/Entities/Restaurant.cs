using System;
using System.Collections.Generic;

namespace lake7.Domain.Entities
{
    public class Restaurant : CommonEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // e.g. Burger, Pizza, Ethiopian
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool IsActive { get; set; } = true;
        
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
