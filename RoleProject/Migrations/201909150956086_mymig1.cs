namespace RoleProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mymig1 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Cars", name: "Agince_Of_Car_Agince_ID", newName: "Agince_ID");
            RenameIndex(table: "dbo.Cars", name: "IX_Agince_Of_Car_Agince_ID", newName: "IX_Agince_ID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Cars", name: "IX_Agince_ID", newName: "IX_Agince_Of_Car_Agince_ID");
            RenameColumn(table: "dbo.Cars", name: "Agince_ID", newName: "Agince_Of_Car_Agince_ID");
        }
    }
}
