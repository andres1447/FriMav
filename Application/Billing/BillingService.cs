using FriMav.Domain;
using FriMav.Domain.Entities;
using System;
using System.Linq;

namespace FriMav.Application.Billing
{

    public class BillingService : IBillingService, IBillingMediator
    {
        private readonly IRepository<DailyBilling> _billingRepository;

        public BillingService(IRepository<DailyBilling> billingRepository)
        {
            _billingRepository = billingRepository;
        }

        public BillingReportResponse Execute(BillingReportRequest request)
        {
            var billings = GetAllInDateRange(request.From, request.To).ToList();
            var res = new BillingReportResponse();
            billings.ForEach(billing =>
            {
                if (billing.Source == BillingSource.Ticket)
                    res.TicketsTotal += billing.Total;
                else
                    res.InvoicesTotal += billing.Total;
            });
            return res;
        }

        public void SaveBilling(BillingSource source, DateTime date, decimal total)
        {
            var billing = GetAllInDateRange(date, date).FirstOrDefault(x => x.Source == source);
            if (billing == null)
            {
                billing = new DailyBilling
                {
                    CreationDate = date,
                    Source = source,
                    Total = total
                };
                _billingRepository.Add(billing);
            }
            else
                billing.Total += total;
        }

        private IQueryable<DailyBilling> GetAllInDateRange(DateTime from, DateTime to)
        {
            var start = from.Date;
            var finish = to.EndOfDay();
            return _billingRepository.Query().Where(x => x.CreationDate >= start && x.CreationDate <= finish);
        }
    }
}
