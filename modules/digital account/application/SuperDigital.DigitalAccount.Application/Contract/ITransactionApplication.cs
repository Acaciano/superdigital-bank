using SuperDigital.DigitalAccount.Application.Models;
using SuperDigital.DigitalAccount.CrossCutting;

namespace SuperDigital.DigitalAccount.Application.Contract
{
    public interface ITransactionApplication
    {
        BaseResponse<TransferResponse> Transfer(string userId, TransferRequest transferRequest);
    }
}
