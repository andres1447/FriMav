namespace FriMav.Application
{
    public class TicketItemCreate
    {
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }

        public decimal Total() => Quantity * Price;
    }
}
