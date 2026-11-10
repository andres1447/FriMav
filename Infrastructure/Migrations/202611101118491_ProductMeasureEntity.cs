namespace FriMav.Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductMeasureEntity : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductMeasure",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128),
                        ShortName = c.String(nullable: false, maxLength: 2),
                        CreationDate = c.DateTime(nullable: false),
                        DeleteDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);

            Sql(@"
SET IDENTITY_INSERT dbo.ProductMeasure ON;
INSERT INTO dbo.ProductMeasure (Id, Name, ShortName, CreationDate) VALUES (1, N'Kg', N'Kg', GETUTCDATE());
INSERT INTO dbo.ProductMeasure (Id, Name, ShortName, CreationDate) VALUES (2, N'Unidades', N'Un', GETUTCDATE());
SET IDENTITY_INSERT dbo.ProductMeasure OFF;
");

            AddColumn("dbo.Product", "ProductMeasureId", c => c.Int());
            Sql("UPDATE dbo.Product SET ProductMeasureId = CASE Measure WHEN 0 THEN 1 WHEN 1 THEN 2 ELSE 1 END");
            AlterColumn("dbo.Product", "ProductMeasureId", c => c.Int(nullable: false));
            CreateIndex("dbo.Product", "ProductMeasureId");
            AddForeignKey("dbo.Product", "ProductMeasureId", "dbo.ProductMeasure", "Id");
            DropColumn("dbo.Product", "Measure");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "Measure", c => c.Int());
            Sql("UPDATE dbo.Product SET Measure = CASE ProductMeasureId WHEN 2 THEN 1 ELSE 0 END");
            AlterColumn("dbo.Product", "Measure", c => c.Int(nullable: false));
            DropForeignKey("dbo.Product", "ProductMeasureId", "dbo.ProductMeasure");
            DropIndex("dbo.Product", new[] { "ProductMeasureId" });
            DropColumn("dbo.Product", "ProductMeasureId");
            DropTable("dbo.ProductMeasure");
        }
    }
}
