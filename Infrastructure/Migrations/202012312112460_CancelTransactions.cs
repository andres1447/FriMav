namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CancelTransactions : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TransactionDocument", name: "RefundDocumentId", newName: "RefundedDocumentId");
            RenameIndex(table: "dbo.TransactionDocument", name: "IX_RefundDocumentId", newName: "IX_RefundedDocumentId");
            AddColumn("dbo.TransactionDocument", "DeleteDate", c => c.DateTime());
            CreateIndex("dbo.Person", "Code");
            CreateIndex("dbo.TransactionDocument", "Number");
            CreateIndex("dbo.TransactionDocument", "Date");
            DropColumn("dbo.TransactionDocument", "PreviousBalance");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TransactionDocument", "PreviousBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropIndex("dbo.TransactionDocument", new[] { "Date" });
            DropIndex("dbo.TransactionDocument", new[] { "Number" });
            DropIndex("dbo.Person", new[] { "Code" });
            DropColumn("dbo.TransactionDocument", "DeleteDate");
            RenameIndex(table: "dbo.TransactionDocument", name: "IX_RefundedDocumentId", newName: "IX_RefundDocumentId");
            RenameColumn(table: "dbo.TransactionDocument", name: "RefundedDocumentId", newName: "RefundDocumentId");
        }
    }
}
