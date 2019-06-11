namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OriginalValue_RemovedFromAuditlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuditLog", "Value", c => c.String());
            DropColumn("dbo.AuditLog", "OriginalValue");
            DropColumn("dbo.AuditLog", "NewValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AuditLog", "NewValue", c => c.String());
            AddColumn("dbo.AuditLog", "OriginalValue", c => c.String());
            DropColumn("dbo.AuditLog", "Value");
        }
    }
}
