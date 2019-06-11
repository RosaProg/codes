namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SupportedPlatformsNameChange : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireElementTextVersion", "SupportedPlatforms", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireItemInstruction", "SupportedPlatforms", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireItemOptionGroupTextVersion", "SupportedPlatforms", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireSectionInstruction", "SupportedPlatforms", c => c.Int(nullable: false));
            DropColumn("dbo.QuestionnaireElementTextVersion", "SupportedPlatform");
            DropColumn("dbo.QuestionnaireItemInstruction", "SupportedPlatform");
            DropColumn("dbo.QuestionnaireItemOptionGroupTextVersion", "SupportedPlatform");
            DropColumn("dbo.QuestionnaireSectionInstruction", "SupportedPlatform");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionnaireSectionInstruction", "SupportedPlatform", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireItemOptionGroupTextVersion", "SupportedPlatform", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireItemInstruction", "SupportedPlatform", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireElementTextVersion", "SupportedPlatform", c => c.Int(nullable: false));
            DropColumn("dbo.QuestionnaireSectionInstruction", "SupportedPlatforms");
            DropColumn("dbo.QuestionnaireItemOptionGroupTextVersion", "SupportedPlatforms");
            DropColumn("dbo.QuestionnaireItemInstruction", "SupportedPlatforms");
            DropColumn("dbo.QuestionnaireElementTextVersion", "SupportedPlatforms");
        }
    }
}
