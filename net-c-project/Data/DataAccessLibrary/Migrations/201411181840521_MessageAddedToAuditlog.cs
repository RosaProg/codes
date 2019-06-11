namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MessageAddedToAuditlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuditLog", "Message", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AuditLog", "Message");
        }
    }
}
