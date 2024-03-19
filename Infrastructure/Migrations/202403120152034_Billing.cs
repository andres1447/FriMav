namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Billing : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DailyBilling",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Source = c.Int(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DailyBilling");
        }
    }
}
