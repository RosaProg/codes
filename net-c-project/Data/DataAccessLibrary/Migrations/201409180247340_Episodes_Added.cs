namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Episodes_Added : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", "dbo.Questionnaire");
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "Questionnaire_Id" });
            CreateTable(
                "dbo.AssignedQuestionnaire",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionnaireName = c.String(),
                        ScheduleString = c.String(),
                        Episode_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Episode", t => t.Episode_Id)
                .Index(t => t.Episode_Id);
            
            CreateTable(
                "dbo.Episode",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Condition = c.String(),
                        IsCompletedStatus = c.Boolean(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.DiagnosisCode",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Episode_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Episode", t => t.Episode_Id)
                .Index(t => t.Episode_Id);
            
            CreateTable(
                "dbo.EpisodeHistory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StatusChanged = c.DateTime(nullable: false),
                        NewIsCompletedStatus = c.Boolean(nullable: false),
                        Episode_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Episode", t => t.Episode_Id)
                .Index(t => t.Episode_Id);
            
            CreateTable(
                "dbo.EpisodeMilestone",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MilestoneDate = c.DateTime(nullable: false),
                        Episode_Id = c.Int(),
                        Milestone_Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Episode", t => t.Episode_Id)
                .ForeignKey("dbo.Milestone", t => t.Milestone_Name)
                .Index(t => t.Episode_Id)
                .Index(t => t.Milestone_Name);
            
            CreateTable(
                "dbo.Milestone",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 50),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.TreatmentCode",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Episode_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Episode", t => t.Episode_Id)
                .Index(t => t.Episode_Id);
            
            CreateTable(
                "dbo.ScheduledQuestionnaireDate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScheduleString = c.String(),
                        CalculatedDate = c.DateTime(),
                        ScheduleHasBeenExecuted = c.Boolean(nullable: false),
                        AssignedQuestionnaire_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssignedQuestionnaire", t => t.AssignedQuestionnaire_Id)
                .Index(t => t.AssignedQuestionnaire_Id);
            
            AddColumn("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", c => c.Int());
            CreateIndex("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id");
            CreateIndex("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id");
            AddForeignKey("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id", "dbo.ScheduledQuestionnaireDate", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", "dbo.Questionnaire", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", "dbo.Questionnaire");
            DropForeignKey("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id", "dbo.ScheduledQuestionnaireDate");
            DropForeignKey("dbo.ScheduledQuestionnaireDate", "AssignedQuestionnaire_Id", "dbo.AssignedQuestionnaire");
            DropForeignKey("dbo.Episode", "User_Id", "dbo.User");
            DropForeignKey("dbo.TreatmentCode", "Episode_Id", "dbo.Episode");
            DropForeignKey("dbo.EpisodeMilestone", "Milestone_Name", "dbo.Milestone");
            DropForeignKey("dbo.EpisodeMilestone", "Episode_Id", "dbo.Episode");
            DropForeignKey("dbo.EpisodeHistory", "Episode_Id", "dbo.Episode");
            DropForeignKey("dbo.DiagnosisCode", "Episode_Id", "dbo.Episode");
            DropForeignKey("dbo.AssignedQuestionnaire", "Episode_Id", "dbo.Episode");
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "ScheduledQuestionnaireDate_Id" });
            DropIndex("dbo.QuestionnaireUserResponseGroup", new[] { "Questionnaire_Id" });
            DropIndex("dbo.ScheduledQuestionnaireDate", new[] { "AssignedQuestionnaire_Id" });
            DropIndex("dbo.TreatmentCode", new[] { "Episode_Id" });
            DropIndex("dbo.EpisodeMilestone", new[] { "Milestone_Name" });
            DropIndex("dbo.EpisodeMilestone", new[] { "Episode_Id" });
            DropIndex("dbo.EpisodeHistory", new[] { "Episode_Id" });
            DropIndex("dbo.DiagnosisCode", new[] { "Episode_Id" });
            DropIndex("dbo.Episode", new[] { "User_Id" });
            DropIndex("dbo.AssignedQuestionnaire", new[] { "Episode_Id" });
            AlterColumn("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", c => c.Int(nullable: false));
            DropColumn("dbo.QuestionnaireUserResponseGroup", "ScheduledQuestionnaireDate_Id");
            DropTable("dbo.ScheduledQuestionnaireDate");
            DropTable("dbo.TreatmentCode");
            DropTable("dbo.Milestone");
            DropTable("dbo.EpisodeMilestone");
            DropTable("dbo.EpisodeHistory");
            DropTable("dbo.DiagnosisCode");
            DropTable("dbo.Episode");
            DropTable("dbo.AssignedQuestionnaire");
            CreateIndex("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id");
            AddForeignKey("dbo.QuestionnaireUserResponseGroup", "Questionnaire_Id", "dbo.Questionnaire", "Id", cascadeDelete: true);
        }
    }
}
