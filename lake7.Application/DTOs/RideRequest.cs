namespace lake7.Application.DTOs
{
    public class RideRequestDto
    {
        public Guid UserId { get; set; }
        public double PickupLat { get; set; }
        public double PickupLng { get; set; }
        public double DropLat { get; set; }
        public double DropLng { get; set; }
    }
}
