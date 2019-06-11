namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DobIsNowADate_Changed : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.User", "DateOfBirth", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.User", "DateOfBirth", c => c.DateTime());
        }
    }
}
