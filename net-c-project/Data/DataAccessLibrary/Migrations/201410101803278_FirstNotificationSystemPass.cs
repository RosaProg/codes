namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstNotificationSystemPass : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NotificationType = c.Int(nullable: false),
                        NotificationObjectId = c.String(),
                        NotificationSendTime = c.DateTime(nullable: false),
                        NotificationCompleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notification");
        }
    }
}
