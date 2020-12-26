namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransactionDocument",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IsRefunded = c.Boolean(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        PersonId = c.Int(nullable: false),
                        CustomerName = c.String(),
                        RefundDocumentId = c.Int(),
                        PreviousBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationDate = c.DateTime(nullable: false),
                        Shipping = c.Int(),
                        PaymentMethod = c.Int(),
                        DeliveryAddress = c.String(),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.PersonId, cascadeDelete: true)
                .ForeignKey("dbo.TransactionDocument", t => t.RefundDocumentId)
                .Index(t => t.PersonId)
                .Index(t => t.RefundDocumentId);
            
            CreateTable(
                "dbo.Person",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false),
                        Cuit = c.String(),
                        Address = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Shipping = c.Int(),
                        PaymentMethod = c.Int(),
                        ZoneId = c.Int(),
                        DeleteDate = c.DateTime(),
                        CreationDate = c.DateTime(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Zone", t => t.ZoneId)
                .Index(t => t.ZoneId);
            
            CreateTable(
                "dbo.Zone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CustomerPrice",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.CustomerId })
                .ForeignKey("dbo.Product", t => t.ProductId)
                .ForeignKey("dbo.Person", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 10),
                        Name = c.String(nullable: false, maxLength: 128),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PriceDate = c.DateTime(nullable: false),
                        DeleteDate = c.DateTime(),
                        ProductTypeId = c.Int(),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductType", t => t.ProductTypeId)
                .Index(t => t.ProductTypeId);
            
            CreateTable(
                "dbo.ProductType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InvoiceItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProductName = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.Id, t.InvoiceId })
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .ForeignKey("dbo.TransactionDocument", t => t.InvoiceId, cascadeDelete: true)
                .Index(t => t.InvoiceId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Delivery",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DeleteDate = c.DateTime(),
                        EmployeeId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.NumberSequence",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false),
                        CurrentId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeliveryInvoice",
                c => new
                    {
                        DeliveryId = c.Int(nullable: false),
                        InvoiceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DeliveryId, t.InvoiceId })
                .ForeignKey("dbo.Delivery", t => t.DeliveryId)
                .ForeignKey("dbo.TransactionDocument", t => t.InvoiceId)
                .Index(t => t.DeliveryId)
                .Index(t => t.InvoiceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeliveryInvoice", "InvoiceId", "dbo.TransactionDocument");
            DropForeignKey("dbo.DeliveryInvoice", "DeliveryId", "dbo.Delivery");
            DropForeignKey("dbo.Delivery", "EmployeeId", "dbo.Person");
            DropForeignKey("dbo.InvoiceItem", "InvoiceId", "dbo.TransactionDocument");
            DropForeignKey("dbo.InvoiceItem", "ProductId", "dbo.Product");
            DropForeignKey("dbo.TransactionDocument", "RefundDocumentId", "dbo.TransactionDocument");
            DropForeignKey("dbo.TransactionDocument", "PersonId", "dbo.Person");
            DropForeignKey("dbo.CustomerPrice", "CustomerId", "dbo.Person");
            DropForeignKey("dbo.CustomerPrice", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product", "ProductTypeId", "dbo.ProductType");
            DropForeignKey("dbo.Person", "ZoneId", "dbo.Zone");
            DropIndex("dbo.DeliveryInvoice", new[] { "InvoiceId" });
            DropIndex("dbo.DeliveryInvoice", new[] { "DeliveryId" });
            DropIndex("dbo.Delivery", new[] { "EmployeeId" });
            DropIndex("dbo.InvoiceItem", new[] { "ProductId" });
            DropIndex("dbo.InvoiceItem", new[] { "InvoiceId" });
            DropIndex("dbo.Product", new[] { "ProductTypeId" });
            DropIndex("dbo.CustomerPrice", new[] { "ProductId" });
            DropIndex("dbo.CustomerPrice", new[] { "CustomerId" });
            DropIndex("dbo.Person", new[] { "ZoneId" });
            DropIndex("dbo.TransactionDocument", new[] { "RefundDocumentId" });
            DropIndex("dbo.TransactionDocument", new[] { "PersonId" });
            DropTable("dbo.DeliveryInvoice");
            DropTable("dbo.NumberSequence");
            DropTable("dbo.Delivery");
            DropTable("dbo.InvoiceItem");
            DropTable("dbo.ProductType");
            DropTable("dbo.Product");
            DropTable("dbo.CustomerPrice");
            DropTable("dbo.Zone");
            DropTable("dbo.Person");
            DropTable("dbo.TransactionDocument");
        }
    }
}
