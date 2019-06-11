namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TextDefinitionsAndSupport_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TextDefinition",
                c => new
                    {
                        DefinitionCode = c.String(nullable: false, maxLength: 50),
                        Text = c.String(),
                        Html = c.String(),
                    })
                .PrimaryKey(t => t.DefinitionCode);
            
            CreateTable(
                "dbo.TextReplacementCode",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReplacementCode = c.String(maxLength: 50),
                        ReplacementValue = c.String(),
                        UseReplacementValue = c.Boolean(nullable: false),
                        ObjectKey = c.Int(nullable: false),
                        ObjectVariablePath = c.String(),
                        ToStringParameter = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ReplacementCode);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.TextReplacementCode", new[] { "ReplacementCode" });
            DropTable("dbo.TextReplacementCode");
            DropTable("dbo.TextDefinition");
        }
    }
}
