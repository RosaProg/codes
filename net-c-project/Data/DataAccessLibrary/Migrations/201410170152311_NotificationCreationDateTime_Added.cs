namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationCreationDateTime_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notification", "DateTimeCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notification", "DateTimeCreated");
        }
    }
}
