namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Functionalities_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Functionality",
                c => new
                    {
                        FunctionalityName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.FunctionalityName);
            
            CreateTable(
                "dbo.IdentityRolesFunctionality",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        FunctionalityName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.RoleId, t.FunctionalityName })
                .ForeignKey("dbo.Functionality", t => t.FunctionalityName, cascadeDelete: true)
                .ForeignKey("dbo.IdentityRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId)
                .Index(t => t.FunctionalityName);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.IdentityRolesFunctionality", "RoleId", "dbo.IdentityRole");
            DropForeignKey("dbo.IdentityRolesFunctionality", "FunctionalityName", "dbo.Functionality");
            DropIndex("dbo.IdentityRolesFunctionality", new[] { "FunctionalityName" });
            DropIndex("dbo.IdentityRolesFunctionality", new[] { "RoleId" });
            DropTable("dbo.IdentityRolesFunctionality");
            DropTable("dbo.Functionality");
        }
    }
}
