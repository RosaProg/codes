namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FormatSupportForDifferentPlatformsAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FormatContainer", "QuestionnaireFormat_Name", "dbo.Format");
            DropForeignKey("dbo.FormatContainer", new[] { "QuestionnaireFormat_Name", "QuestionnaireFormat_SupportedPlatform" }, "dbo.Format");
            DropIndex("dbo.FormatContainer", new[] { "QuestionnaireFormat_Name" });
            DropPrimaryKey("dbo.Format");
            CreateTable(
                "dbo.QuestionnaireElementText",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Platform = c.Int(nullable: false),
                        Instance = c.Int(nullable: false),
                        QuestionnaireElement_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionnaireElement", t => t.QuestionnaireElement_Id)
                .Index(t => t.QuestionnaireElement_Id);
            
            AddColumn("dbo.FormatContainer", "QuestionnaireFormat_SupportedPlatform", c => c.Int());
            AddColumn("dbo.Format", "SupportedPlatform", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Format", new[] { "Name", "SupportedPlatform" });
            CreateIndex("dbo.FormatContainer", new[] { "QuestionnaireFormat_Name", "QuestionnaireFormat_SupportedPlatform" });
            AddForeignKey("dbo.FormatContainer", new[] { "QuestionnaireFormat_Name", "QuestionnaireFormat_SupportedPlatform" }, "dbo.Format", new[] { "Name", "SupportedPlatform" });
            DropColumn("dbo.QuestionnaireElement", "Text");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionnaireElement", "Text", c => c.String());
            DropForeignKey("dbo.FormatContainer", new[] { "QuestionnaireFormat_Name", "QuestionnaireFormat_SupportedPlatform" }, "dbo.Format");
            DropForeignKey("dbo.QuestionnaireElementText", "QuestionnaireElement_Id", "dbo.QuestionnaireElement");
            DropIndex("dbo.QuestionnaireElementText", new[] { "QuestionnaireElement_Id" });
            DropIndex("dbo.FormatContainer", new[] { "QuestionnaireFormat_Name", "QuestionnaireFormat_SupportedPlatform" });
            DropPrimaryKey("dbo.Format");
            DropColumn("dbo.Format", "SupportedPlatform");
            DropColumn("dbo.FormatContainer", "QuestionnaireFormat_SupportedPlatform");
            DropTable("dbo.QuestionnaireElementText");
            AddPrimaryKey("dbo.Format", "Name");
            CreateIndex("dbo.FormatContainer", "QuestionnaireFormat_Name");
            AddForeignKey("dbo.FormatContainer", new[] { "QuestionnaireFormat_Name", "QuestionnaireFormat_SupportedPlatform" }, "dbo.Format", new[] { "Name", "SupportedPlatform" });
            AddForeignKey("dbo.FormatContainer", "QuestionnaireFormat_Name", "dbo.Format", "Name");
        }
    }
}
