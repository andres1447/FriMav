namespace FriMav.Infrastructure.Migrations
{
    using FriMav.Domain.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriMavDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FriMavDbContext context)
        {
            context.Set<Customer>().AddOrUpdate(x => x.Id, new Customer
            {
                Id = Customer.TicketCustomerId,
                Name = "Cliente Ticket",
                Code = "T",
                CreationDate = new DateTime(2020, 12, 25).ToUniversalTime(),
                PaymentMethod = PaymentMethod.Cash,
                Shipping = Shipping.Self
            });
            context.Set<Customer>().AddOrUpdate(x => x.Id, new Customer
            {
                Id = Customer.DefaultCustomerId,
                Name = "Cliente Mostrador",
                Code = "0",
                CreationDate = new DateTime(2020, 12, 25).ToUniversalTime(),
                PaymentMethod = PaymentMethod.Cash,
                Shipping = Shipping.Self
            });
        }
    }
}
