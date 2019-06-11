namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FunctionalityReplacedWithPermissionSystem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.IdentityRolesFunctionality", "FunctionalityName", "dbo.Functionality");
            DropForeignKey("dbo.IdentityRolesFunctionality", "RoleId", "dbo.IdentityRole");
            DropIndex("dbo.IdentityRolesFunctionality", new[] { "RoleId" });
            DropIndex("dbo.IdentityRolesFunctionality", new[] { "FunctionalityName" });
            CreateTable(
                "dbo.IdentityRolePermission",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        Permission = c.Int(nullable: false),
                        PermissionString = c.String(),
                    })
                .PrimaryKey(t => new { t.RoleId, t.Permission })
                .ForeignKey("dbo.IdentityRole", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            DropTable("dbo.Functionality");
            DropTable("dbo.IdentityRolesFunctionality");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.IdentityRolesFunctionality",
                c => new
                    {
                        RoleId = c.String(nullable: false, maxLength: 128),
                        FunctionalityName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => new { t.RoleId, t.FunctionalityName });
            
            CreateTable(
                "dbo.Functionality",
                c => new
                    {
                        FunctionalityName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.FunctionalityName);
            
            DropForeignKey("dbo.IdentityRolePermission", "RoleId", "dbo.IdentityRole");
            DropIndex("dbo.IdentityRolePermission", new[] { "RoleId" });
            DropTable("dbo.IdentityRolePermission");
            CreateIndex("dbo.IdentityRolesFunctionality", "FunctionalityName");
            CreateIndex("dbo.IdentityRolesFunctionality", "RoleId");
            AddForeignKey("dbo.IdentityRolesFunctionality", "RoleId", "dbo.IdentityRole", "Id", cascadeDelete: true);
            AddForeignKey("dbo.IdentityRolesFunctionality", "FunctionalityName", "dbo.Functionality", "FunctionalityName", cascadeDelete: true);
        }
    }
}
