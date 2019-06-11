namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PageTexts_Added_And_AuditLog_Enhanced : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PageText",
                c => new
                    {
                        Identifier = c.String(nullable: false, maxLength: 255),
                        Text = c.String(),
                    })
                .PrimaryKey(t => t.Identifier);
            
            AddColumn("dbo.AuditLog", "ObjectType", c => c.String(maxLength: 128));
            AddColumn("dbo.AuditLog", "Success", c => c.Boolean(nullable: false));
            AddColumn("dbo.AuditLog", "FieldName", c => c.String());
            DropColumn("dbo.AuditLog", "TableName");
            DropColumn("dbo.AuditLog", "ColumnName");
            DropColumn("dbo.AuditLog", "Value");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AuditLog", "Value", c => c.String());
            AddColumn("dbo.AuditLog", "ColumnName", c => c.String(maxLength: 250));
            AddColumn("dbo.AuditLog", "TableName", c => c.String(maxLength: 128));
            DropColumn("dbo.AuditLog", "FieldName");
            DropColumn("dbo.AuditLog", "Success");
            DropColumn("dbo.AuditLog", "ObjectType");
            DropTable("dbo.PageText");
        }
    }
}
