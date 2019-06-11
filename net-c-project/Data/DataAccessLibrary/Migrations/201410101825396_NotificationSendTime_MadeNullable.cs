namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationSendTime_MadeNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notification", "NotificationSendTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notification", "NotificationSendTime", c => c.DateTime(nullable: false));
        }
    }
}
