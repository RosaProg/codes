namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateOfBirth_added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "DateOfBirth", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "DateOfBirth");
        }
    }
}
