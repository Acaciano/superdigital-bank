using FluentValidation;
using SuperDigital.DigitalAccount.Application.Contract;
using SuperDigital.DigitalAccount.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SuperDigital.DigitalAccount.Application.Validation.Request
{
    public class TransferValidator : AbstractValidator<TransferRequest>
    {
        public TransferValidator(ICheckingAccountApplication checkingAccountApplication)
        {
            RuleFor(transfer => transfer.NumberFrom)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .NotNull()
                .Must(number => number > 0)
                .WithMessage("Número de conta origem inválido");

            RuleFor(transfer => transfer.NumberTo)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .NotNull()
                .Must(number => number > 0)
                .WithMessage("Número de conta destino inválido");

            RuleFor(transfer => transfer.NumberFrom)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEqual(t => t.NumberTo)
                .WithMessage("Conta origem e conta destino não podem ser iguais");

            RuleFor(transfer => transfer.Amount)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .NotNull()
                .Must(number => number > 0)
                .WithMessage("Valor de transferência não pode ser negativo.");

            RuleFor(transfer => transfer)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Custom((transfer, context) =>
                {
                    var checkingAccountFrom = checkingAccountApplication.GetByAccountNumber(transfer.NumberFrom);

                    if (checkingAccountFrom == null) context.AddFailure($"Não foi encontrado a conta corrente (ORIGEM) {transfer.NumberFrom}.");
                    else if (checkingAccountFrom.Balance < transfer.Amount) context.AddFailure($"Saldo da conta corrente (ORIGEM) {transfer.NumberFrom}, insuficiente.");

                    if (checkingAccountFrom.UserId.ToString() != transfer.UserId) context.AddFailure("Conta origem não pertence ao usuário informado");
                });

            RuleFor(transfer => transfer)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .Custom((transfer, context) =>
                {
                    var checkingAccountFrom = checkingAccountApplication.GetByAccountNumber(transfer.NumberTo);

                    if (checkingAccountFrom == null) context.AddFailure($"Não foi encontrado a conta corrente (DESTINO) {transfer.NumberTo}.");
                });
        }
    }
}
