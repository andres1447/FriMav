namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceSurcharge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Person", "LastSurcharge", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Person", "LastSurcharge");
        }
    }
}
