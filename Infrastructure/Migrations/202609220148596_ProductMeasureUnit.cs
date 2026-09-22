namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductMeasureUnit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "Measure", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "Measure");
        }
    }
}
