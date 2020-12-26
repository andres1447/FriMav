namespace FriMav.Application
{
    public class InvoiceResult
    {
        public InvoiceResult(int number, decimal total, decimal balance)
        {
            Number = number;
            Total = total;
            Balance = balance;
        }

        public int Number { get; set; }
        public decimal Balance { get; set; }
        public decimal Total { get; set; }
    }
}