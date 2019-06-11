namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionnaireDescription_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionnaireDescription",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        SupportedPlatforms = c.Int(nullable: false),
                        SupportedInstances = c.Int(nullable: false),
                        Audience = c.Int(nullable: false, defaultValue: 0),
                        Questionnaire_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questionnaire", t => t.Questionnaire_Id)
                .Index(t => t.Questionnaire_Id);
            
            AddColumn("dbo.QuestionnaireIntroductionMessage", "Audience", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.QuestionnaireElementTextVersion", "Audience", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.QuestionnaireItemInstruction", "Audience", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.QuestionnaireItemOptionGroupTextVersion", "Audience", c => c.Int(nullable: false, defaultValue: 0));
            AddColumn("dbo.QuestionnaireSectionInstruction", "Audience", c => c.Int(nullable: false, defaultValue: 0));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionnaireDescription", "Questionnaire_Id", "dbo.Questionnaire");
            DropIndex("dbo.QuestionnaireDescription", new[] { "Questionnaire_Id" });
            DropColumn("dbo.QuestionnaireSectionInstruction", "Audience");
            DropColumn("dbo.QuestionnaireItemOptionGroupTextVersion", "Audience");
            DropColumn("dbo.QuestionnaireItemInstruction", "Audience");
            DropColumn("dbo.QuestionnaireElementTextVersion", "Audience");
            DropColumn("dbo.QuestionnaireIntroductionMessage", "Audience");
            DropTable("dbo.QuestionnaireDescription");
        }
    }
}
