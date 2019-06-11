namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredRestrictionRemovedOn_ScheduleQuestionniareDate_In_QuestionnaireUserResponseGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ScheduledQuestionnaireDate", "AssignedQuestionnaire_Id", "dbo.AssignedQuestionnaire");
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id", "dbo.ScheduledQuestionnaireDate");
            DropIndex("dbo.ScheduledQuestionnaireDate", new[] { "AssignedQuestionnaire_Id" });
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "ScheduledQuestionnaireDate_Id" });
            AlterColumn("dbo.ScheduledQuestionnaireDate", "AssignedQuestionnaire_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id", c => c.Int());
            CreateIndex("dbo.ScheduledQuestionnaireDate", "AssignedQuestionnaire_Id");
            CreateIndex("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id");
            AddForeignKey("dbo.ScheduledQuestionnaireDate", "AssignedQuestionnaire_Id", "dbo.AssignedQuestionnaire", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id", "dbo.ScheduledQuestionnaireDate", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id", "dbo.ScheduledQuestionnaireDate");
            DropForeignKey("dbo.ScheduledQuestionnaireDate", "AssignedQuestionnaire_Id", "dbo.AssignedQuestionnaire");
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "ScheduledQuestionnaireDate_Id" });
            DropIndex("dbo.ScheduledQuestionnaireDate", new[] { "AssignedQuestionnaire_Id" });
            AlterColumn("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.ScheduledQuestionnaireDate", "AssignedQuestionnaire_Id", c => c.Int());
            CreateIndex("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id");
            CreateIndex("dbo.ScheduledQuestionnaireDate", "AssignedQuestionnaire_Id");
            AddForeignKey("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id", "dbo.ScheduledQuestionnaireDate", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ScheduledQuestionnaireDate", "AssignedQuestionnaire_Id", "dbo.AssignedQuestionnaire", "Id");
        }
    }
}
