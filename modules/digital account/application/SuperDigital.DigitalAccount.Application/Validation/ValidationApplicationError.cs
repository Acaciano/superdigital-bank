
namespace SuperDigital.DigitalAccount.Application.Validation
{
    public class ValidationApplicationError
    {
        public string Message { get; set; }
        public ValidationApplicationError(string message)
        {
            this.Message = message;
        }
    }
}
