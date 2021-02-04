namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Payroll : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GoodsSoldItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodsSoldId = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        ProductName = c.String(),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.Id, t.GoodsSoldId })
                .ForeignKey("dbo.LiquidationDocument", t => t.GoodsSoldId, cascadeDelete: true)
                .Index(t => t.GoodsSoldId);
            
            CreateTable(
                "dbo.Payroll",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeleteDate = c.DateTime(),
                        PreviousBalance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EmployeeId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.LiquidationDocument",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeleteDate = c.DateTime(),
                        EmployeeId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LoanId = c.Int(),
                        Type = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Loan", t => t.LoanId)
                .Index(t => t.Date)
                .Index(t => t.EmployeeId)
                .Index(t => t.LoanId);
            
            CreateTable(
                "dbo.Loan",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DeleteDate = c.DateTime(),
                        EmployeeId = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Person", t => t.EmployeeId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.PayrollLiquidationDocument",
                c => new
                    {
                        PayrollId = c.Int(nullable: false),
                        LiquidationDocumentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PayrollId, t.LiquidationDocumentId })
                .ForeignKey("dbo.Payroll", t => t.PayrollId)
                .ForeignKey("dbo.LiquidationDocument", t => t.LiquidationDocumentId)
                .Index(t => t.PayrollId)
                .Index(t => t.LiquidationDocumentId);
            
            AddColumn("dbo.Person", "Salary", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Person", "JoinDate", c => c.DateTime());
            OnUp();
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LiquidationDocument", "LoanId", "dbo.Loan");
            DropForeignKey("dbo.Loan", "EmployeeId", "dbo.Person");
            DropForeignKey("dbo.PayrollLiquidationDocument", "LiquidationDocumentId", "dbo.LiquidationDocument");
            DropForeignKey("dbo.PayrollLiquidationDocument", "PayrollId", "dbo.Payroll");
            DropForeignKey("dbo.GoodsSoldItem", "GoodsSoldId", "dbo.LiquidationDocument");
            DropForeignKey("dbo.LiquidationDocument", "EmployeeId", "dbo.Person");
            DropForeignKey("dbo.Payroll", "EmployeeId", "dbo.Person");
            DropIndex("dbo.PayrollLiquidationDocument", new[] { "LiquidationDocumentId" });
            DropIndex("dbo.PayrollLiquidationDocument", new[] { "PayrollId" });
            DropIndex("dbo.Loan", new[] { "EmployeeId" });
            DropIndex("dbo.LiquidationDocument", new[] { "LoanId" });
            DropIndex("dbo.LiquidationDocument", new[] { "EmployeeId" });
            DropIndex("dbo.LiquidationDocument", new[] { "Date" });
            DropIndex("dbo.Payroll", new[] { "EmployeeId" });
            DropIndex("dbo.GoodsSoldItem", new[] { "GoodsSoldId" });
            DropColumn("dbo.Person", "JoinDate");
            DropColumn("dbo.Person", "Salary");
            DropTable("dbo.PayrollLiquidationDocument");
            DropTable("dbo.Loan");
            DropTable("dbo.LiquidationDocument");
            DropTable("dbo.Payroll");
            DropTable("dbo.GoodsSoldItem");
            OnDown();
        }
    }
}
