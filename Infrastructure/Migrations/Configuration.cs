namespace FriMav.Infrastructure.Migrations
{
    using FriMav.Domain;
    using FriMav.Domain.Entities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FriMavDbContext>
    {
        public Configuration()
        {
        }

        protected override void Seed(FriMavDbContext context)
        {
            if (!context.Set<Customer>().Any(x => x.Code == "0"))
            {
                context.Set<Customer>().Add(new Customer
                {
                    Id = Customer.DefaultCustomerId,
                    Name = "Cliente Mostrador",
                    Code = "0",
                    CreationDate = new DateTime(2020, 12, 25).ToUniversalTime(),
                    PaymentMethod = PaymentMethod.Cash,
                    Shipping = Shipping.Self
                });
            }

            if (!context.Set<ConfigValue>().Any(x => x.Code == Constants.AttendBonusPercentageKey))
            {
                context.Set<ConfigValue>().Add(new ConfigValue
                {
                    Code = Constants.AttendBonusPercentageKey,
                    Description = "Presentismo - Porcentaje",
                    CreationDate = DateTime.Now.ToUniversalTime(),
                    Type = ConfigurationType.Decimal,
                    DecimalValue = 0.10m
                });
            }
        }
    }
}
