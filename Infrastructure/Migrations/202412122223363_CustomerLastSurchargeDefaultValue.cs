namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CustomerLastSurchargeDefaultValue : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Person", "LastSurcharge", c => c.Decimal(nullable: false, precision: 18, scale: 2, defaultValue: 0));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Person", "LastSurcharge", c => c.Decimal(precision: 18, scale: 2));
        }
    }
}
