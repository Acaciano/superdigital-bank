namespace SuperDigital.DigitalAccount.Application.Models
{
    public class TransferRequest
    {
        public string UserId { get; set; }
        public long NumberFrom { get; set; }
        public long NumberTo { get; set; }
        public decimal Amount { get; set; }
    }
}
