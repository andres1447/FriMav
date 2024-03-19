using FriMav.Domain.Entities;
using System;

namespace FriMav.Application.Billing
{
    public interface IBillingService
    {
        void SaveBilling(BillingSource source, DateTime date, decimal total);
    }
}
