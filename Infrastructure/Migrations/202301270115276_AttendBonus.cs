namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttendBonus : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ConfigValue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        IntValue = c.Int(nullable: false),
                        StringValue = c.String(),
                        DecimalValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ConfigValue");
        }
    }
}
