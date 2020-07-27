namespace RoleProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mymig : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Car_propertiesCar", "Cars", "dbo.Cars");
            DropForeignKey("dbo.Car_propertiesCar", "Car_properties", "dbo.Car_properties");
            DropIndex("dbo.Cars", new[] { "CLIENT_Client_ID" });
            DropIndex("dbo.Car_propertiesCar", new[] { "Cars" });
            DropIndex("dbo.Car_propertiesCar", new[] { "Car_properties" });
            CreateTable(
                "dbo.Car_And_Properites",
                c => new
                    {
                        Car_Id = c.Int(nullable: false),
                        id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Car_Id, t.id })
                .ForeignKey("dbo.Cars", t => t.Car_Id, cascadeDelete: true)
                .ForeignKey("dbo.Car_properties", t => t.id, cascadeDelete: true)
                .Index(t => t.Car_Id)
                .Index(t => t.id);
            
            CreateTable(
                "dbo.ReciveDates",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Start_Recive_Date = c.DateTime(nullable: false),
                        End_Recive_Date = c.DateTime(nullable: false),
                        Total_Cost = c.Double(nullable: false),
                        cars_Car_Id = c.Int(),
                        client_Client_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Cars", t => t.cars_Car_Id)
                .Index(t => t.cars_Car_Id)
                .Index(t => t.client_Client_ID);
            
            DropColumn("dbo.Cars", "Start_Book_Date");
            DropColumn("dbo.Cars", "End_Book_Date");
            DropColumn("dbo.Cars", "price_Total");
            DropColumn("dbo.Cars", "Is_reseved");
            DropColumn("dbo.Cars", "CLIENT_Client_ID");
            DropTable("dbo.Car_propertiesCar");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Car_propertiesCar",
                c => new
                    {
                        Cars = c.Int(nullable: false),
                        Car_properties = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Cars, t.Car_properties });
            
            AddColumn("dbo.Cars", "CLIENT_Client_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.Cars", "Is_reseved", c => c.Boolean(nullable: false));
            AddColumn("dbo.Cars", "price_Total", c => c.Double());
            AddColumn("dbo.Cars", "End_Book_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Cars", "Start_Book_Date", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.ReciveDates", "cars_Car_Id", "dbo.Cars");
            DropForeignKey("dbo.Car_And_Properites", "id", "dbo.Car_properties");
            DropForeignKey("dbo.Car_And_Properites", "Car_Id", "dbo.Cars");
            DropIndex("dbo.ReciveDates", new[] { "client_Client_ID" });
            DropIndex("dbo.ReciveDates", new[] { "cars_Car_Id" });
            DropIndex("dbo.Car_And_Properites", new[] { "id" });
            DropIndex("dbo.Car_And_Properites", new[] { "Car_Id" });
            DropTable("dbo.ReciveDates");
            DropTable("dbo.Car_And_Properites");
            CreateIndex("dbo.Car_propertiesCar", "Car_properties");
            CreateIndex("dbo.Car_propertiesCar", "Cars");
            CreateIndex("dbo.Cars", "CLIENT_Client_ID");
            AddForeignKey("dbo.Car_propertiesCar", "Car_properties", "dbo.Car_properties", "id", cascadeDelete: true);
            AddForeignKey("dbo.Car_propertiesCar", "Cars", "dbo.Cars", "Car_Id", cascadeDelete: true);
        }
    }
}
