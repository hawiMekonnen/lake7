using System;
using System.Collections.Generic;
using System.Text;

namespace lake7.Domain.Entities
{
    public class DriverLocation: CommonEntity
    {
        public Guid DriverId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public Driver? Driver { get; set; }
    }

}
