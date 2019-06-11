namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SurveyNameFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProDomain", "AppliesTo", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireResponse", "Questionnaire_Id", c => c.Int());
            CreateIndex("dbo.QuestionnaireResponse", "Questionnaire_Id");
            AddForeignKey("dbo.QuestionnaireResponse", "Questionnaire_Id", "dbo.Questionnaire", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionnaireResponse", "Questionnaire_Id", "dbo.Questionnaire");
            DropIndex("dbo.QuestionnaireResponse", new[] { "Questionnaire_Id" });
            DropColumn("dbo.QuestionnaireResponse", "Questionnaire_Id");
            DropColumn("dbo.ProDomain", "AppliesTo");
        }
    }
}
