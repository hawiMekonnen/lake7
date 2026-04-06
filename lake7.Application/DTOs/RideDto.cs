namespace lake7.Application.DTOs
{
    public class RideDto
    {
        public Guid Id { get; set; }
        public required string PickupLocation { get; set; }
        public required string DropoffLocation { get; set; }
        public string Status { get; set; }= "Pending";
    }
}
