namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuditLogs_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditLog",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.String(maxLength: 128),
                        UserIp = c.String(),
                        ClientIps = c.String(),
                        ClientName = c.String(),
                        EventDateUTC = c.DateTime(nullable: false),
                        EventType = c.String(maxLength: 250),
                        Action = c.String(),
                        TableName = c.String(maxLength: 128),
                        RecordId = c.String(maxLength: 250),
                        ColumnName = c.String(maxLength: 250),
                        OriginalValue = c.String(),
                        NewValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AuditLog");
        }
    }
}
