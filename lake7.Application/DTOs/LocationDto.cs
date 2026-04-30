namespace lake7.Application.DTOs
{
    public class LocationDto
    {
        public Guid DriverId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public required string LicensePlate { get; set; }
        public required string VehicleType { get; set; }
    }
}
