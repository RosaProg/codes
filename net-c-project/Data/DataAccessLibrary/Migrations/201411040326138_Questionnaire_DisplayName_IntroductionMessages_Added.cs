namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Questionnaire_DisplayName_IntroductionMessages_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionnaireIntroductionMessage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        SupportedPlatforms = c.Int(nullable: false),
                        SupportedInstances = c.Int(nullable: false),
                        Questionnaire_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questionnaire", t => t.Questionnaire_Id)
                .Index(t => t.Questionnaire_Id);
            
            AddColumn("dbo.Questionnaire", "DisplayName", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionnaireIntroductionMessage", "Questionnaire_Id", "dbo.Questionnaire");
            DropIndex("dbo.QuestionnaireIntroductionMessage", new[] { "Questionnaire_Id" });
            DropColumn("dbo.Questionnaire", "DisplayName");
            DropTable("dbo.QuestionnaireIntroductionMessage");
        }
    }
}
