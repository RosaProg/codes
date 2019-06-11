namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnonymousQuestionnaireLoading_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireUserResponseGroup", "QuestionnaireFormatName", c => c.String());
            AddColumn("dbo.QuestionnaireUserResponseGroup", "User_Id", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.QuestionnaireUserResponseGroup", "User_Id");
            AddForeignKey("dbo.QuestionnaireUserResponseGroup", "User_Id", "dbo.User", "Id", cascadeDelete: true);
            DropColumn("dbo.QuestionnaireUserResponseGroup", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QuestionnaireUserResponseGroup", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "User_Id", "dbo.User");
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "User_Id" });
            DropColumn("dbo.QuestionnaireUserResponseGroup", "User_Id");
            DropColumn("dbo.QuestionnaireUserResponseGroup", "QuestionnaireFormatName");
        }
    }
}
