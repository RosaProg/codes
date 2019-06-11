namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModelUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireElement", "IsInQuestionnaireDefinition", c => c.Boolean());
            AddColumn("dbo.QuestionnaireResponse", "UserId", c => c.Int(nullable: false));
            DropColumn("dbo.QuestionnaireElement", "IsInProDefinition");
            DropColumn("dbo.QuestionnaireResponse", "usedId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionnaireResponse", "usedId", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionnaireElement", "IsInProDefinition", c => c.Boolean());
            DropColumn("dbo.QuestionnaireResponse", "UserId");
            DropColumn("dbo.QuestionnaireElement", "IsInQuestionnaireDefinition");
        }
    }
}
