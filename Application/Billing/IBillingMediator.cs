namespace FriMav.Application.Billing
{
    public interface IBillingMediator
    {
        BillingReportResponse Execute(BillingReportRequest request);
    }
}
