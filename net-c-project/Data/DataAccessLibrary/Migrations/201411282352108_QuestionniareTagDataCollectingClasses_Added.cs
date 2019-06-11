namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionniareTagDataCollectingClasses_Added : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PatientTag",
                c => new
                    {
                        TagName = c.String(nullable: false, maxLength: 50),
                        PatientId = c.String(nullable: false, maxLength: 128),
                        TextValue = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => new { t.TagName, t.PatientId })
                .ForeignKey("dbo.Patient", t => t.PatientId, cascadeDelete: true)
                .Index(t => t.PatientId);
            
            CreateTable(
                "dbo.QuestionnaireUserResponseGroupTag",
                c => new
                    {
                        TagName = c.String(nullable: false, maxLength: 50),
                        GroupId = c.Int(nullable: false),
                        TextValue = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => new { t.TagName, t.GroupId })
                .ForeignKey("dbo.QuestionnaireUserResponseGroup", t => t.GroupId, cascadeDelete: true)
                .Index(t => t.GroupId);
            
            CreateTable(
                "dbo.QuestionnaireDataExtraction",
                c => new
                    {
                        QuestionnaireName = c.String(nullable: false, maxLength: 50),
                        TagName = c.String(nullable: false, maxLength: 50),
                        ItemActionId = c.String(),
                        OptionGroupActionId = c.String(),
                        OptionActionId = c.String(),
                    })
                .PrimaryKey(t => new { t.QuestionnaireName, t.TagName });
            
            AddColumn("dbo.QuestionnaireItemOptionGroup", "ActionId", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionnaireUserResponseGroupTag", "GroupId", "dbo.QuestionnaireUserResponseGroup");
            DropForeignKey("dbo.PatientTag", "PatientId", "dbo.Patient");
            DropIndex("dbo.QuestionnaireUserResponseGroupTag", new[] { "GroupId" });
            DropIndex("dbo.PatientTag", new[] { "PatientId" });
            DropColumn("dbo.QuestionnaireItemOptionGroup", "ActionId");
            DropTable("dbo.QuestionnaireDataExtraction");
            DropTable("dbo.QuestionnaireUserResponseGroupTag");
            DropTable("dbo.PatientTag");
        }
    }
}
