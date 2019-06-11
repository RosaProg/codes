namespace PCHI.DataAccessLibrary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionnaireUserResponseGroup_DateFields_Added : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionnaireUserResponseGroup", "StartDate", c => c.DateTime());
            AddColumn("dbo.QuestionnaireUserResponseGroup", "DateCompleted", c => c.DateTime());
            AddColumn("dbo.QuestionnaireUserResponseGroup", "DateCreated", c => c.DateTime(nullable: false));
            Sql("Update dbo.QuestionnaireUserResponseGroup set StartDate = DATEADD(dd, 0, DATEDIFF(dd, 0, StartTime)) where StartTime is not null");
            Sql("Update dbo.QuestionnaireUserResponseGroup set DateCompleted = DATEADD(dd, 0, DATEDIFF(dd, 0, DateTimeCompleted)) where DateTimeCompleted is not null");
            Sql("Update dbo.QuestionnaireUserResponseGroup set DateCreated = DATEADD(dd, 0, DATEDIFF(dd, 0, DatetimeCreated)) where DatetimeCreated is not null");
        }
        
        public override void Down()
        {
            DropColumn("dbo.QuestionnaireUserResponseGroup", "DateCreated");
            DropColumn("dbo.QuestionnaireUserResponseGroup", "DateCompleted");
            DropColumn("dbo.QuestionnaireUserResponseGroup", "StartDate");
        }
    }
}
