namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecurityCode_UserAndPurpose_AreNowKey : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserSecurityCode", "User_Id", "dbo.User");
            DropIndex("dbo.UserSecurityCode", new[] { "User_Id" });
            RenameColumn(table: "dbo.UserSecurityCode", name: "User_Id", newName: "UserId");
            DropPrimaryKey("dbo.UserSecurityCode");
            AlterColumn("dbo.UserSecurityCode", "SecurityCodePurpose", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.UserSecurityCode", "UserId", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.UserSecurityCode", new[] { "UserId", "SecurityCodePurpose" });
            CreateIndex("dbo.UserSecurityCode", "UserId");
            AddForeignKey("dbo.UserSecurityCode", "UserId", "dbo.User", "Id", cascadeDelete: true);
            DropColumn("dbo.UserSecurityCode", "Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserSecurityCode", "Id", c => c.Int(nullable: false, identity: true));
            DropForeignKey("dbo.UserSecurityCode", "UserId", "dbo.User");
            DropIndex("dbo.UserSecurityCode", new[] { "UserId" });
            DropPrimaryKey("dbo.UserSecurityCode");
            AlterColumn("dbo.UserSecurityCode", "UserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.UserSecurityCode", "SecurityCodePurpose", c => c.String());
            AddPrimaryKey("dbo.UserSecurityCode", "Id");
            RenameColumn(table: "dbo.UserSecurityCode", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.UserSecurityCode", "User_Id");
            AddForeignKey("dbo.UserSecurityCode", "User_Id", "dbo.User", "Id");
        }
    }
}
