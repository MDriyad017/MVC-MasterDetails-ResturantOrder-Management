namespace ev_01.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scriptR : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Foods",
                c => new
                    {
                        FoodId = c.Int(nullable: false, identity: true),
                        FoodName = c.String(),
                    })
                .PrimaryKey(t => t.FoodId);
            
            CreateTable(
                "dbo.Receipts",
                c => new
                    {
                        ReceiptId = c.Int(nullable: false, identity: true),
                        FoodId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReceiptId)
                .ForeignKey("dbo.Foods", t => t.FoodId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.FoodId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(),
                        Picture = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                        TableNumber = c.Int(nullable: false),
                        AmountPaid = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receipts", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Receipts", "FoodId", "dbo.Foods");
            DropIndex("dbo.Receipts", new[] { "OrderId" });
            DropIndex("dbo.Receipts", new[] { "FoodId" });
            DropTable("dbo.Orders");
            DropTable("dbo.Receipts");
            DropTable("dbo.Foods");
        }
    }
}
