public class RideRequestDto
{
    public string PickupLocation { get; set; } = string.Empty;
    public double PickupLatitude { get; set; }
    public double PickupLongitude { get; set; }

    public string DropoffLocation { get; set; } = string.Empty;
    public double DropLatitude { get; set; }
    public double DropLongitude { get; set; }
}