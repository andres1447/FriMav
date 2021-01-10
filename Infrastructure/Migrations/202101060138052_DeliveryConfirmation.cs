namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeliveryConfirmation : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.TransactionDocument", name: "RefundedDocumentId", newName: "ReferencedDocumentId");
            RenameIndex(table: "dbo.TransactionDocument", name: "IX_RefundedDocumentId", newName: "IX_ReferencedDocumentId");
            CreateTable(
                "dbo.DeliveryPayment",
                c => new
                    {
                        DeliveryId = c.Int(nullable: false),
                        PaymentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DeliveryId, t.PaymentId })
                .ForeignKey("dbo.Delivery", t => t.DeliveryId)
                .ForeignKey("dbo.TransactionDocument", t => t.PaymentId)
                .Index(t => t.DeliveryId)
                .Index(t => t.PaymentId);
            
            AddColumn("dbo.Delivery", "CloseDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeliveryPayment", "PaymentId", "dbo.TransactionDocument");
            DropForeignKey("dbo.DeliveryPayment", "DeliveryId", "dbo.Delivery");
            DropIndex("dbo.DeliveryPayment", new[] { "PaymentId" });
            DropIndex("dbo.DeliveryPayment", new[] { "DeliveryId" });
            DropColumn("dbo.Delivery", "CloseDate");
            DropTable("dbo.DeliveryPayment");
            RenameIndex(table: "dbo.TransactionDocument", name: "IX_ReferencedDocumentId", newName: "IX_RefundedDocumentId");
            RenameColumn(table: "dbo.TransactionDocument", name: "ReferencedDocumentId", newName: "RefundedDocumentId");
        }
    }
}
