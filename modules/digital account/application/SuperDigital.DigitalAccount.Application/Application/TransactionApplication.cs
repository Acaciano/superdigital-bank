using SuperDigital.DigitalAccount.Application.Contract;
using SuperDigital.DigitalAccount.Application.Models;
using SuperDigital.DigitalAccount.Application.Validation.Request;
using SuperDigital.DigitalAccount.CrossCutting;
using SuperDigital.DigitalAccount.Domain.Entities;
using SuperDigital.DigitalAccount.Domain.Services.Contracts;
using System;
using System.Collections.Generic;

namespace SuperDigital.DigitalAccount.Application.Application
{
    public class TransactionApplication : ITransactionApplication
    {
        private readonly ITransactionService _transactionService;
        private readonly ICheckingAccountApplication _checkingAccountApplication;

        public TransactionApplication(ITransactionService transactionService, ICheckingAccountApplication checkingAccountApplication)
        {
            _transactionService = transactionService;
            _checkingAccountApplication = checkingAccountApplication;
        }

        public BaseResponse<TransferResponse> Transfer(string userId, TransferRequest transferRequest)
        {
            var response = new BaseResponse<TransferResponse>();

            try
            {
                transferRequest.UserId = userId;

                var checkingAccountFrom = _checkingAccountApplication.GetByAccountNumber(transferRequest.NumberFrom);
                var checkingAccountTo = _checkingAccountApplication.GetByAccountNumber(transferRequest.NumberTo);

                TransferValidator transferValidator = new TransferValidator(_checkingAccountApplication);
                FluentValidation.Results.ValidationResult validationResult = transferValidator.Validate(transferRequest);

                if (!validationResult.IsValid)
                {
                    List<ErrorResponse> erros = new List<ErrorResponse>();

                    foreach (var item in validationResult.Errors)
                    {
                        ErrorResponse errorResponse = new ErrorResponse { Message = item.ErrorMessage, Code = item.GetHashCode() };
                        erros.Add(errorResponse);
                    }

                    response.Errors.AddRange(erros);

                    return response;
                }

                decimal balanceFrom = checkingAccountFrom.Balance - transferRequest.Amount;
                decimal balanceTo = checkingAccountTo.Balance + transferRequest.Amount;

                var from = _checkingAccountApplication.UpdateBalance(transferRequest.NumberFrom, balanceFrom);
                var to = _checkingAccountApplication.UpdateBalance(transferRequest.NumberTo, balanceTo);

                if (from != null && to != null)
                {
                    _transactionService.Add(new Transaction
                    {
                        Amount = transferRequest.Amount,
                        FromCheckingAccountId = checkingAccountFrom.UserId,
                        ToCheckingAccountId = checkingAccountTo.UserId
                    });

                    response.Result = new TransferResponse { Balance = balanceFrom };
                }
            }
            catch (Exception ex)
            {
                response.Errors.Add(new ErrorResponse
                {
                    Code = ex.GetHashCode(),
                    Message = ex.Message
                });
            }
            return response;
        }
    }
}
