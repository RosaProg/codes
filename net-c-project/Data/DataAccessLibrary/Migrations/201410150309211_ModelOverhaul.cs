namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelOverhaul : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionnaireElementText", "QuestionnaireElement_Id", "dbo.QuestionnaireElement");
            DropIndex("dbo.QuestionnaireElementText", new[] { "QuestionnaireElement_Id" });
            CreateTable(
                "dbo.QuestionnaireElementTextVersion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        SupportedPlatform = c.Int(nullable: false),
                        SupportedInstances = c.Int(nullable: false),
                        QuestionnaireElement_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionnaireElement", t => t.QuestionnaireElement_Id)
                .Index(t => t.QuestionnaireElement_Id);
            
            CreateTable(
                "dbo.QuestionnaireItemOptionGroupTextVersion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        SupportedPlatform = c.Int(nullable: false),
                        SupportedInstances = c.Int(nullable: false),
                        QuestionnaireItemOptionGroup_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionnaireItemOptionGroup", t => t.QuestionnaireItemOptionGroup_Id)
                .Index(t => t.QuestionnaireItemOptionGroup_Id);
            
            AddColumn("dbo.Episode", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.QuestionnaireItemInstruction", "SupportedPlatform", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireItemInstruction", "SupportedInstances", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireItemOption", "DisplayId", c => c.String());
            AddColumn("dbo.QuestionnaireSectionInstruction", "SupportedPlatform", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireSectionInstruction", "SupportedInstances", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionnaireItemOptionGroup", "RangeStep", c => c.Double(nullable: false));
            DropColumn("dbo.QuestionnaireElement", "IsMandatory");
            DropColumn("dbo.QuestionnaireElement", "IsInQuestionnaireDefinition");
            DropColumn("dbo.QuestionnaireItemOptionGroup", "Text");
            DropColumn("dbo.QuestionnaireItemOption", "OptionIdText");
            DropTable("dbo.QuestionnaireElementText");
        }
        
        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.QuestionnaireItemOption", "OptionIdText", c => c.String());
            AddColumn("dbo.QuestionnaireItemOptionGroup", "Text", c => c.String());
            AddColumn("dbo.QuestionnaireElement", "IsInQuestionnaireDefinition", c => c.Boolean());
            AddColumn("dbo.QuestionnaireElement", "IsMandatory", c => c.Boolean());
            DropForeignKey("dbo.QuestionnaireItemOptionGroupTextVersion", "QuestionnaireItemOptionGroup_Id", "dbo.QuestionnaireItemOptionGroup");
            DropForeignKey("dbo.QuestionnaireElementTextVersion", "QuestionnaireElement_Id", "dbo.QuestionnaireElement");
            DropIndex("dbo.QuestionnaireItemOptionGroupTextVersion", new[] { "QuestionnaireItemOptionGroup_Id" });
            DropIndex("dbo.QuestionnaireElementTextVersion", new[] { "QuestionnaireElement_Id" });
            AlterColumn("dbo.QuestionnaireItemOptionGroup", "RangeStep", c => c.Int(nullable: false));
            DropColumn("dbo.QuestionnaireSectionInstruction", "SupportedInstances");
            DropColumn("dbo.QuestionnaireSectionInstruction", "SupportedPlatform");
            DropColumn("dbo.QuestionnaireItemOption", "DisplayId");
            DropColumn("dbo.QuestionnaireItemInstruction", "SupportedInstances");
            DropColumn("dbo.QuestionnaireItemInstruction", "SupportedPlatform");
            DropColumn("dbo.Episode", "DateCreated");
            DropTable("dbo.QuestionnaireItemOptionGroupTextVersion");
            DropTable("dbo.QuestionnaireElementTextVersion");
            CreateIndex("dbo.QuestionnaireElementText", "QuestionnaireElement_Id");
            AddForeignKey("dbo.QuestionnaireElementText", "QuestionnaireElement_Id", "dbo.QuestionnaireElement", "Id");
        }
    }
}
