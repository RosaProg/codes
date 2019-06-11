namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserName_IsRequired_And_Unique : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "UserName", c => c.String(nullable: false, maxLength: 450));
            CreateIndex("dbo.User", "UserName", unique: true, name: "IX_User_UserName");
        }
        
        public override void Down()
        {
            DropIndex("dbo.User", "IX_User_UserName");
            AlterColumn("dbo.User", "UserName", c => c.String());
        }
    }
}
