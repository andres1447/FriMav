namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InvoiceExternalReferenceNumber : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransactionDocument", "ExternalReferenceNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransactionDocument", "ExternalReferenceNumber");
        }
    }
}
