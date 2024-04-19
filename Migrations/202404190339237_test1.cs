namespace latihan2.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        InventoryId = c.Guid(nullable: false),
                        InventoryCode = c.String(),
                        InventoryName = c.String(),
                        InventoryQty = c.Double(nullable: false),
                        BackOrderSalesOrder = c.Double(nullable: false),
                        InventoryQtyOutgoing = c.Double(nullable: false),
                        InventoryPrice = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.InventoryId);
            
            CreateTable(
                "dbo.SalesOrderItems",
                c => new
                    {
                        SalesOrderItemId = c.Guid(nullable: false),
                        SalesOrderId = c.Guid(nullable: false),
                        NomorBaris = c.Int(nullable: false),
                        KodeBarang = c.String(),
                        NamaBarang = c.String(),
                        ItemQty = c.Int(nullable: false),
                        ItemQtyDelivered = c.Int(nullable: false),
                        HargaJual = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SalesOrderItemId)
                .ForeignKey("dbo.SalesOrders", t => t.SalesOrderId, cascadeDelete: true)
                .Index(t => t.SalesOrderId);
            
            CreateTable(
                "dbo.SalesOrders",
                c => new
                    {
                        SalesOrderId = c.Guid(nullable: false),
                        NomorSalesOrder = c.String(),
                        TanggalSalesOrder = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SalesOrderId);
            
            CreateTable(
                "dbo.SuratJalanItems",
                c => new
                    {
                        SuratJalanItemId = c.Guid(nullable: false),
                        SuratJalanId = c.Guid(nullable: false),
                        SalesOrderId = c.Guid(nullable: false),
                        SalesOrderItemId = c.Guid(nullable: false),
                        NomorBaris = c.Int(nullable: false),
                        KodeBarang = c.String(),
                        NamaBarang = c.String(),
                        ItemQtyOrder = c.Int(nullable: false),
                        ItemQtyDelivered = c.Int(nullable: false),
                        HargaJual = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SuratJalanItemId)
                .ForeignKey("dbo.SuratJalans", t => t.SuratJalanId, cascadeDelete: true)
                .Index(t => t.SuratJalanId);
            
            CreateTable(
                "dbo.SuratJalans",
                c => new
                    {
                        SuratJalanId = c.Guid(nullable: false),
                        SalesOrderId = c.Guid(nullable: false),
                        NomorSalesOrder = c.String(),
                        TanggalSuratJalan = c.DateTime(nullable: false),
                        NamaSuratJalan = c.String(),
                        NamaCustomer = c.String(),
                        AlamatCustomer = c.String(),
                    })
                .PrimaryKey(t => t.SuratJalanId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SuratJalanItems", "SuratJalanId", "dbo.SuratJalans");
            DropForeignKey("dbo.SalesOrderItems", "SalesOrderId", "dbo.SalesOrders");
            DropIndex("dbo.SuratJalanItems", new[] { "SuratJalanId" });
            DropIndex("dbo.SalesOrderItems", new[] { "SalesOrderId" });
            DropTable("dbo.SuratJalans");
            DropTable("dbo.SuratJalanItems");
            DropTable("dbo.SalesOrders");
            DropTable("dbo.SalesOrderItems");
            DropTable("dbo.Inventories");
        }
    }
}
