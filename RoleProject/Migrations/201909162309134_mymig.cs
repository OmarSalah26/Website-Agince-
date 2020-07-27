namespace RoleProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mymig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Aginces",
                c => new
                    {
                        Agince_ID = c.String(nullable: false, maxLength: 128),
                        name = c.String(nullable: false),
                        phone_number = c.String(),
                        city = c.String(),
                        street = c.String(),
                        photo_Agince = c.String(),
                        userAccount_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Agince_ID)
                .ForeignKey("dbo.AspNetUsers", t => t.userAccount_Id)
                .Index(t => t.userAccount_Id);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        Car_Id = c.Int(nullable: false, identity: true),
                        Type_Of_Car = c.String(nullable: false),
                        Car_Brand = c.String(nullable: false),
                        Car_Model = c.String(nullable: false),
                        Chassis_No = c.String(nullable: false),
                        price_in_day = c.Double(nullable: false),
                        photo_Car = c.String(),
                        Agince_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Car_Id)
                .ForeignKey("dbo.Aginces", t => t.Agince_ID)
                .Index(t => t.Agince_ID);
            
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
                "dbo.Car_properties",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        proprity_Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
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
                .ForeignKey("dbo.Clients", t => t.client_Client_ID)
                .Index(t => t.cars_Car_Id)
                .Index(t => t.client_Client_ID);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Client_ID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                        phone_Number = c.String(nullable: false),
                        city = c.String(),
                        street = c.String(),
                        age = c.Int(),
                        number_of_licience = c.Int(),
                        date_of_licience_expiry = c.DateTime(),
                        photo_Client = c.String(),
                    })
                .PrimaryKey(t => t.Client_ID);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        city = c.String(),
                        street = c.String(),
                        photoAdmin = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Aginces", "userAccount_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ReciveDates", "client_Client_ID", "dbo.Clients");
            DropForeignKey("dbo.ReciveDates", "cars_Car_Id", "dbo.Cars");
            DropForeignKey("dbo.Cars", "Agince_ID", "dbo.Aginces");
            DropForeignKey("dbo.Car_And_Properites", "id", "dbo.Car_properties");
            DropForeignKey("dbo.Car_And_Properites", "Car_Id", "dbo.Cars");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ReciveDates", new[] { "client_Client_ID" });
            DropIndex("dbo.ReciveDates", new[] { "cars_Car_Id" });
            DropIndex("dbo.Car_And_Properites", new[] { "id" });
            DropIndex("dbo.Car_And_Properites", new[] { "Car_Id" });
            DropIndex("dbo.Cars", new[] { "Agince_ID" });
            DropIndex("dbo.Aginces", new[] { "userAccount_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Clients");
            DropTable("dbo.ReciveDates");
            DropTable("dbo.Car_properties");
            DropTable("dbo.Car_And_Properites");
            DropTable("dbo.Cars");
            DropTable("dbo.Aginces");
        }
    }
}
