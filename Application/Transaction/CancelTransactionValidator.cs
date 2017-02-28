using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FriMav.Application
{
    public class CancelTransactionValidator : AbstractValidator<CancelTransaction>
    {
        private IContainerContext _container;

        public CancelTransactionValidator(IContainerContext container)
        {
            _container = container;

            RuleFor(t => t.TransactionId).Must(TransactionNotRefunded).WithMessage("La transacción ya ha sido cancelada anteriormente.");
        }

        public bool TransactionNotRefunded(int transactionId)
        {
            return !_container.Resolve<ITransactionService>().IsReferenced(transactionId);
        }
    }
}