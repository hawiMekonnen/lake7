namespace lake7.Application.DTOs
{
    public class PaymentDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string? Method { get; set; }
        public string Status { get; set; }= "Pending";
    }
}
